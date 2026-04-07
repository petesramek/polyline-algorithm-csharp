//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides an immutable, sealed rule engine that describes how to extract and scale values from
/// an object of type <typeparamref name="T"/> for polyline encoding.
/// </summary>
/// <typeparam name="T">The source object type from which column values are extracted.</typeparam>
/// <remarks>
/// <para>
/// Instances of this class are constructed exclusively through <see cref="FormatterBuilder{T}"/>.
/// </para>
/// <para>
/// The <see langword="sealed"/> modifier allows the JIT to devirtualise and inline calls to
/// <see cref="GetValues"/>, eliminating vtable dispatch in the encoding hot loop.
/// </para>
/// </remarks>
public sealed class PolylineFormatter<T> {
    private readonly FormatterRule<T>[] _rules;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineFormatter{T}"/> with the baked rules.
    /// This constructor is intentionally internal; use <see cref="FormatterBuilder{T}"/> to create instances.
    /// </summary>
    /// <param name="rules">The pre-calculated rules array produced by the builder.</param>
    internal PolylineFormatter(FormatterRule<T>[] rules) {
        _rules = rules;
        Width = rules.Length;
        HasBaselines = Array.Exists(rules, static r => r.Baseline.HasValue);
    }

    /// <summary>
    /// Gets the number of columns (values per item).
    /// This is the required length of the <see cref="Span{T}"/> passed to <see cref="GetValues"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets a value indicating whether any column has a baseline defined.
    /// When <see langword="false"/> the encoder can skip the baseline-subtraction branch entirely,
    /// keeping the common-case encoding path branch-free.
    /// </summary>
    public bool HasBaselines { get; }

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
