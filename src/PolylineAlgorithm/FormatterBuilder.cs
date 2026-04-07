//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides a fluent builder for constructing a <see cref="PolylineFormatter{T}"/>.
/// </summary>
/// <typeparam name="T">The source object type from which column values are extracted.</typeparam>
/// <remarks>
/// <para>
/// Use <see cref="Create"/> to obtain an instance, call <see cref="AddValue"/> once per column,
/// optionally chain <see cref="SetBaseline"/> to specify an epoch for the most-recently added column,
/// then call <see cref="Build"/> to produce the immutable <see cref="PolylineFormatter{T}"/>.
/// </para>
/// <para>
/// The builder is the <em>only</em> way to create a <see cref="PolylineFormatter{T}"/> — its
/// constructor is internal.
/// </para>
/// </remarks>
public sealed class FormatterBuilder<T> {
    private readonly List<FormatterRule<T>> _rules = [];
    private readonly HashSet<string> _names = new(StringComparer.Ordinal);

    private FormatterBuilder() { }

    /// <summary>
    /// Creates a new <see cref="FormatterBuilder{T}"/> instance.
    /// </summary>
    /// <returns>A fresh <see cref="FormatterBuilder{T}"/> with no rules.</returns>
    public static FormatterBuilder<T> Create() => new();

    /// <summary>
    /// Adds a column with the specified value selector and precision.
    /// </summary>
    /// <param name="name">
    /// A unique, non-null, non-empty name that identifies the column. Used for diagnostics only.
    /// </param>
    /// <param name="selector">
    /// A delegate that extracts the column's raw <see cref="double"/> value from an item of type
    /// <typeparamref name="T"/>.
    /// </param>
    /// <param name="precision">
    /// The number of decimal places to preserve. Each extracted value is multiplied by
    /// 10^<paramref name="precision"/> before encoding. Defaults to 5.
    /// </param>
    /// <returns>The current <see cref="FormatterBuilder{T}"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="name"/> or <paramref name="selector"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is empty, or a rule with the same name already exists.
    /// </exception>
    public FormatterBuilder<T> AddValue(string name, Func<T, double> selector, uint precision = 5) {
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

        _rules.Add(new FormatterRule<T>(name, (long)Pow10.GetFactor(precision), selector));

        return this;
    }

    /// <summary>
    /// Sets a baseline (epoch) on the most-recently added column.
    /// During encoding the baseline is subtracted from the first item's scaled column value,
    /// keeping the initial delta small when the absolute first value is large.
    /// </summary>
    /// <param name="baseline">The baseline value to apply to the first item's column value.</param>
    /// <returns>The current <see cref="FormatterBuilder{T}"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no rules have been added yet. Call <see cref="AddValue"/> before <see cref="SetBaseline"/>.
    /// </exception>
    public FormatterBuilder<T> SetBaseline(long baseline) {
        if (_rules.Count == 0) {
            throw new InvalidOperationException("Cannot set a baseline when no rules have been added. Call AddValue first.");
        }

        var last = _rules[^1];
        _rules[^1] = new FormatterRule<T>(last.Name, last.Factor, last.Select, baseline);

        return this;
    }

    /// <summary>
    /// Bakes all added rules into a sealed, immutable <see cref="PolylineFormatter{T}"/>.
    /// </summary>
    /// <returns>
    /// An immutable <see cref="PolylineFormatter{T}"/> whose rules can no longer be changed.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no rules have been added.
    /// </exception>
    public PolylineFormatter<T> Build() {
        if (_rules.Count == 0) {
            throw new InvalidOperationException("At least one rule must be added before calling Build.");
        }

        return new PolylineFormatter<T>(_rules.ToArray());
    }
}
