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
/// A sealed, immutable formatter that implements <see cref="IPolylineFormatter{TValue, TPolyline}"/>.
/// </summary>
/// <typeparam name="TValue">The value or item type.</typeparam>
/// <typeparam name="TPolyline">The polyline surface type.</typeparam>
/// <remarks>
/// Instances are constructed exclusively through <see cref="FormatterBuilder{TValue, TPolyline}"/>.
/// The <see langword="sealed"/> modifier allows the JIT to devirtualise and inline calls to the
/// interface methods in the encoding/decoding hot loop.
/// </remarks>
public sealed class PolylineFormatter<TValue, TPolyline> : IPolylineFormatter<TValue, TPolyline> {
    private readonly FormatterRule<TValue>[] _rules;
    private readonly PolylineItemFactory<TValue>? _create;
    private readonly Func<ReadOnlyMemory<char>, TPolyline> _write;
    private readonly Func<TPolyline, ReadOnlyMemory<char>> _read;

    /// <summary>
    /// Initializes a new instance. Intentionally internal — use
    /// <see cref="FormatterBuilder{TValue, TPolyline}"/> to create instances.
    /// </summary>
    internal PolylineFormatter(
        FormatterRule<TValue>[] rules,
        PolylineItemFactory<TValue>? create,
        Func<ReadOnlyMemory<char>, TPolyline> write,
        Func<TPolyline, ReadOnlyMemory<char>> read) {
        _rules = rules;
        _create = create;
        _write = write;
        _read = read;
        Width = rules.Length;
    }

    /// <inheritdoc/>
    public int Width { get; }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long GetBaseline(int index) => _rules[index].Baseline ?? 0L;

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="values"/>.Length does not equal <see cref="Width"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void GetValues(TValue item, Span<long> values) {
        if (values.Length != Width) {
            throw new ArgumentException(
                $"Buffer length {values.Length} does not match the formatter width {Width}.",
                nameof(values));
        }

        var rules = _rules;
        for (var i = 0; i < rules.Length; i++) {
            ref var rule = ref rules[i];
            values[i] = (long)(rule.Select(item) * rule.Factor);
        }
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TPolyline Write(ReadOnlyMemory<char> encoded) => _write(encoded);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlyMemory<char> Read(TPolyline polyline) => _read(polyline);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no factory delegate was supplied via
    /// <see cref="FormatterBuilder{TValue, TPolyline}.WithValueFactory"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TValue CreateItem(ReadOnlySpan<long> values) {
        if (_create is null) {
            throw new InvalidOperationException(
                $"Cannot reconstruct an item because no factory was registered. " +
                $"Call {nameof(FormatterBuilder<TValue, TPolyline>)}.{nameof(FormatterBuilder<TValue, TPolyline>.WithValueFactory)} before building.");
        }

        // Denormalize each accumulated scaled integer back to the original double:
        // add back the baseline that was subtracted during encoding, then divide by the precision factor.
        var rules = _rules;
        int width = rules.Length;
        double[] doubles = new double[width];
        for (var i = 0; i < width; i++) {
            ref var rule = ref rules[i];
            doubles[i] = (values[i] + (rule.Baseline ?? 0L)) / (double)rule.Factor;
        }

        return _create(doubles);
    }
}
