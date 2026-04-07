//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides an immutable, sealed rule engine that describes how to extract and scale values from
/// an object of type <typeparamref name="T"/> for polyline encoding, and how to reconstruct an
/// object of type <typeparamref name="T"/> from scaled values during decoding.
/// </summary>
/// <typeparam name="T">The source object type from which column values are extracted or to which they are reconstructed.</typeparam>
/// <remarks>
/// <para>
/// Instances of this class are constructed exclusively through <see cref="FormatterBuilder{T}"/>.
/// </para>
/// <para>
/// The <see langword="sealed"/> modifier allows the JIT to devirtualise and inline calls to
/// <see cref="GetValues"/> and <see cref="CreateItem"/>, eliminating vtable dispatch in the
/// encoding/decoding hot loop.
/// </para>
/// </remarks>
public sealed class PolylineValueFormatter<T> : IPolylineValueFormatter<T> {
    private readonly FormatterRule<T>[] _rules;
    private readonly PolylineItemFactory<T>? _create;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineValueFormatter{T}"/> with the baked rules.
    /// This constructor is intentionally internal; use <see cref="FormatterBuilder{T}"/> to create instances.
    /// </summary>
    /// <param name="rules">The pre-calculated rules array produced by the builder.</param>
    /// <param name="create">
    /// An optional factory delegate that reconstructs a <typeparamref name="T"/> from scaled values.
    /// Required for the decoding direction; if <see langword="null"/>, <see cref="CreateItem"/> throws.
    /// </param>
    internal PolylineValueFormatter(FormatterRule<T>[] rules, PolylineItemFactory<T>? create = null) {
        _rules = rules;
        _create = create;
        Width = rules.Length;
        HasBaselines = Array.Exists(rules, static r => r.Baseline.HasValue);
    }

    /// <summary>
    /// Gets the number of columns (values per item).
    /// This is the required length of the <see cref="Span{T}"/> passed to <see cref="GetValues"/>
    /// and the length of the span received by <see cref="CreateItem"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets a value indicating whether any column has a baseline defined.
    /// When <see langword="false"/> the encoder can skip the baseline-subtraction branch entirely,
    /// keeping the common-case encoding path branch-free.
    /// </summary>
    public bool HasBaselines { get; }

    /// <summary>
    /// Gets a value indicating whether a factory delegate was supplied at build time.
    /// When <see langword="false"/>, calling <see cref="CreateItem"/> throws an
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    public bool CanCreateItem => _create is not null;

    /// <summary>
    /// Extracts and scales all column values from <paramref name="item"/> into the <paramref name="values"/> span.
    /// Called once per item in the encoding hot loop. This method performs no heap allocation;
    /// the caller is responsible for providing and owning the output buffer.
    /// </summary>
    /// <param name="item">The source item from which column values are extracted.</param>
    /// <param name="values">
    /// Output buffer that receives the scaled values.
    /// Its length must equal <see cref="Width"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="values"/>.Length does not equal <see cref="Width"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetValues(T item, Span<long> values) {
        if (values.Length != Width) {
            throw new ArgumentException(
                $"Buffer length {values.Length} does not match the formatter width {Width}.",
                nameof(values));
        }

        var rules = _rules; // local copy avoids repeated bounds check on the field
        for (var i = 0; i < rules.Length; i++) {
            ref var rule = ref rules[i];
            values[i] = (long)(rule.Select(item) * rule.Factor);
        }
    }

    /// <summary>
    /// Reconstructs a <typeparamref name="T"/> from the given scaled integer values.
    /// </summary>
    /// <param name="values">
    /// The accumulated scaled integer values decoded from the polyline.
    /// </param>
    /// <returns>A <typeparamref name="T"/> reconstructed from <paramref name="values"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no factory delegate was supplied via <see cref="FormatterBuilder{T}.WithCreate"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T CreateItem(ReadOnlySpan<long> values) {
        if (_create is null) {
            throw new InvalidOperationException(
                $"Cannot reconstruct an item because no factory was registered. " +
                $"Call {nameof(FormatterBuilder<T>)}.{nameof(FormatterBuilder<T>.WithCreate)} before building.");
        }

        return _create(values);
    }

    /// <summary>
    /// Returns the baseline for the column at <paramref name="index"/>, or <c>0</c> if none is configured.
    /// The encoder subtracts this value from the first item's scaled column value during encoding.
    /// </summary>
    /// <param name="index">
    /// The zero-based column index. Must be in the range <c>[0, <see cref="Width"/>)</c>.
    /// An <see cref="IndexOutOfRangeException"/> is thrown if the index is out of range.
    /// </param>
    /// <returns>The baseline value, or <c>0</c> when no baseline has been defined for the column.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long GetBaseline(int index) => _rules[index].Baseline ?? 0L;
}
