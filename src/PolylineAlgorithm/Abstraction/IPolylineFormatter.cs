//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System;

/// <summary>
/// Defines how to extract and scale values from a <typeparamref name="TValue"/> for encoding,
/// reconstruct a <typeparamref name="TValue"/> from scaled values for decoding,
/// produce a <typeparamref name="TPolyline"/> from an encoded character buffer, and extract that buffer
/// back from a <typeparamref name="TPolyline"/>.
/// </summary>
/// <typeparam name="TValue">The value or item type. For example a struct with Latitude/Longitude.</typeparam>
/// <typeparam name="TPolyline">The polyline surface type. For example <see cref="string"/> or
/// <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.</typeparam>
/// <remarks>
/// Use <see cref="FormatterBuilder{TValue, TPolyline}"/> to build a
/// <see cref="PolylineFormatter{TValue, TPolyline}"/> that implements this interface.
/// </remarks>
public interface IPolylineFormatter<TValue, TPolyline> {
    /// <summary>
    /// Gets the number of values (columns) per encoded item.
    /// This is the required length of the <see cref="Span{T}"/> passed to <see cref="GetValues"/>
    /// and the length of the span received in <see cref="CreateItem"/>.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Returns the baseline for the column at <paramref name="index"/>, or <c>0</c> if none is configured.
    /// The encoder uses this as the starting point for the first item's delta computation: the initial
    /// delta for the column is <c>scaled_first_value − baseline</c> rather than <c>scaled_first_value</c>.
    /// </summary>
    /// <param name="index">The zero-based column index. Must be in the range <c>[0, <see cref="Width"/>)</c>.</param>
    /// <returns>The baseline value, or <c>0</c> when no baseline has been defined for the column.</returns>
    long GetBaseline(int index) => 0L;

    /// <summary>
    /// Extracts and scales all column values from <paramref name="item"/> into the <paramref name="values"/> span.
    /// Called once per item in the encoding loop.
    /// </summary>
    /// <param name="item">The source item from which column values are extracted.</param>
    /// <param name="values">
    /// Output buffer that receives the scaled integer values. Its length must equal <see cref="Width"/>.
    /// </param>
    void GetValues(TValue item, Span<long> values);

    /// <summary>
    /// Creates a <typeparamref name="TPolyline"/> from the encoded character buffer produced by the encoder.
    /// </summary>
    /// <param name="encoded">The encoded polyline as a read-only memory of characters.</param>
    /// <returns>A <typeparamref name="TPolyline"/> wrapping or derived from <paramref name="encoded"/>.</returns>
    TPolyline Write(ReadOnlyMemory<char> encoded);

    /// <summary>
    /// Extracts the character buffer from a <typeparamref name="TPolyline"/> for the decoder to read.
    /// </summary>
    /// <param name="polyline">The polyline to read from.</param>
    /// <returns>A <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> representing the encoded characters.</returns>
    ReadOnlyMemory<char> Read(TPolyline polyline);

    /// <summary>
    /// Reconstructs a <typeparamref name="TValue"/> from the given accumulated scaled integer values.
    /// Called once per decoded item in the decoding loop. Implementations are responsible for
    /// denormalizing the raw scaled integers (e.g. dividing by the precision factor and adding back
    /// any baseline) before constructing the item.
    /// </summary>
    /// <param name="values">
    /// The raw accumulated scaled integer values decoded from the polyline. Each element corresponds to
    /// the same column position as in <see cref="GetValues"/>. These are the direct output of the
    /// delta-accumulation loop in the decoder before any denormalization is applied.
    /// </param>
    /// <returns>A <typeparamref name="TValue"/> reconstructed from <paramref name="values"/>.</returns>
    TValue CreateItem(ReadOnlySpan<long> values);
}
