//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides a fluent builder for constructing a <see cref="PolylineFormatter{TValue, TPolyline}"/>.
/// </summary>
/// <typeparam name="TValue">The value or item type from which column values are extracted.</typeparam>
/// <typeparam name="TPolyline">The polyline surface type produced and consumed by the formatter.</typeparam>
/// <remarks>
/// <para>
/// Use <see cref="Create"/> to obtain an instance, call <see cref="AddValue"/> once per column,
/// optionally chain <see cref="SetBaseline"/> to set a reference baseline for the most-recently added column,
/// optionally chain <see cref="WithValueFactory"/> to register a factory for the decoding direction,
/// call <see cref="WithReaderWriter"/> to supply the polyline surface delegates (required), then call
/// <see cref="Build"/> to produce the immutable <see cref="PolylineFormatter{TValue, TPolyline}"/>.
/// </para>
/// <para>
/// The builder is the <em>only</em> way to create a <see cref="PolylineFormatter{TValue, TPolyline}"/>
/// — its constructor is internal.
/// </para>
/// </remarks>
public sealed class FormatterBuilder<TValue, TPolyline> {
    private readonly List<FormatterRule<TValue>> _rules = [];
    private readonly HashSet<string> _names = new(StringComparer.Ordinal);
    private PolylineItemFactory<TValue>? _create;
    private Func<ReadOnlyMemory<char>, TPolyline>? _write;
    private Func<TPolyline, ReadOnlyMemory<char>>? _read;

    private FormatterBuilder() { }

    /// <summary>
    /// Creates a new <see cref="FormatterBuilder{TValue, TPolyline}"/> instance.
    /// </summary>
    /// <returns>A fresh builder with no rules and no polyline delegates.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Factory method on a generic builder intentionally lives on the type so callers write FormatterBuilder<T, U>.Create() without needing a separate non-generic factory class.")]
    public static FormatterBuilder<TValue, TPolyline> Create() => new();

    /// <summary>
    /// Adds a column with the specified value selector and precision.
    /// </summary>
    /// <param name="name">
    /// A unique, non-null, non-empty name that identifies the column. Used for diagnostics only.
    /// </param>
    /// <param name="selector">
    /// A delegate that extracts the column's raw <see cref="double"/> value from an item of type
    /// <typeparamref name="TValue"/>.
    /// </param>
    /// <param name="precision">
    /// The number of decimal places to preserve. Each extracted value is multiplied by
    /// 10^<paramref name="precision"/> before encoding. Defaults to 5.
    /// </param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="name"/> or <paramref name="selector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is empty, or a rule with the same name already exists.
    /// </exception>
    public FormatterBuilder<TValue, TPolyline> AddValue(string name, Func<TValue, double> selector, uint precision = 5) {
        if (name is null) {
            throw new ArgumentNullException(nameof(name));
        }

        if (name.Length == 0) {
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        }

        if (selector is null) {
            throw new ArgumentNullException(nameof(selector));
        }

        if (!_names.Add(name)) {
            throw new ArgumentException($"A rule with the name '{name}' has already been added.", nameof(name));
        }

        _rules.Add(new FormatterRule<TValue>(name, (long)Pow10.GetFactor(precision), selector));

        return this;
    }

    /// <summary>
    /// Sets a reference value (baseline) on the most-recently added column.
    /// During encoding, the baseline is subtracted from the first item's scaled column value so that
    /// the initial delta is <c>scaled_first_value − baseline</c> rather than <c>scaled_first_value</c>.
    /// Use this when the absolute scaled value of the first data point for a column would otherwise
    /// produce a very large initial encoded delta.
    /// </summary>
    /// <param name="baseline">
    /// The reference value to subtract from the first item's scaled column value during encoding.
    /// The decoder automatically adds this value back, so the reconstructed item matches the
    /// original input.
    /// </param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no rules have been added yet. Call <see cref="AddValue"/> before
    /// <see cref="SetBaseline"/>.
    /// </exception>
    public FormatterBuilder<TValue, TPolyline> SetBaseline(long baseline) {
        if (_rules.Count == 0) {
            throw new InvalidOperationException("Cannot set a baseline when no rules have been added. Call AddValue first.");
        }

        var last = _rules[^1];
        _rules[^1] = new FormatterRule<TValue>(last.Name, last.Factor, last.Select, baseline);

        return this;
    }

    /// <summary>
    /// Registers a factory delegate used to reconstruct a <typeparamref name="TValue"/> from
    /// denormalized values during decoding.
    /// </summary>
    /// <param name="create">
    /// A delegate that accepts the denormalized <see cref="double"/> values reconstructed from the
    /// polyline and returns a <typeparamref name="TValue"/>. The formatter automatically divides
    /// each accumulated scaled integer by its precision factor and adds back any baseline configured
    /// via <see cref="SetBaseline"/>, so the span values match the original values supplied to the
    /// encoder. The span length always equals the number of columns added via <see cref="AddValue"/>.
    /// </param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="create"/> is <see langword="null"/>.
    /// </exception>
    public FormatterBuilder<TValue, TPolyline> WithValueFactory(PolylineItemFactory<TValue> create) {
        if (create is null) {
            throw new ArgumentNullException(nameof(create));
        }

        _create = create;

        return this;
    }

    /// <summary>
    /// Supplies the polyline-surface delegates required to convert between the raw character buffer
    /// and a <typeparamref name="TPolyline"/>. This call is mandatory before <see cref="Build"/>.
    /// </summary>
    /// <param name="write">
    /// Converts the encoded <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> produced by the encoder
    /// into a <typeparamref name="TPolyline"/>.
    /// </param>
    /// <param name="read">
    /// Extracts the encoded character buffer from a <typeparamref name="TPolyline"/> for the decoder to
    /// consume.
    /// </param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="write"/> or <paramref name="read"/> is <see langword="null"/>.
    /// </exception>
    public FormatterBuilder<TValue, TPolyline> WithReaderWriter(
        Func<ReadOnlyMemory<char>, TPolyline> write,
        Func<TPolyline, ReadOnlyMemory<char>> read) {
        if (write is null) {
            throw new ArgumentNullException(nameof(write));
        }

        if (read is null) {
            throw new ArgumentNullException(nameof(read));
        }

        _write = write;
        _read = read;

        return this;
    }

    /// <summary>
    /// Bakes all added rules and delegates into a sealed, immutable
    /// <see cref="PolylineFormatter{TValue, TPolyline}"/>.
    /// </summary>
    /// <returns>
    /// An immutable <see cref="PolylineFormatter{TValue, TPolyline}"/> whose configuration can
    /// no longer be changed.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no rules have been added, or when <see cref="WithReaderWriter"/> has not been called.
    /// </exception>
    public PolylineFormatter<TValue, TPolyline> Build() {
        if (_rules.Count == 0) {
            throw new InvalidOperationException("At least one rule must be added before calling Build.");
        }

        if (_write is null || _read is null) {
            throw new InvalidOperationException(
                $"Polyline surface delegates must be supplied before calling Build. " +
                $"Call {nameof(WithReaderWriter)} first.");
        }

        return new PolylineFormatter<TValue, TPolyline>(_rules.ToArray(), _create, _write, _read);
    }
}
