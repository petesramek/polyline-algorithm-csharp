//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System;

/// <summary>
/// Defines how to extract scaled numeric values from a <typeparamref name="TValue"/> during encoding, and
/// how to reconstruct a <typeparamref name="TValue"/> from those values during decoding.
/// </summary>
/// <typeparam name="TValue">The coordinate or item type that the formatter understands.</typeparam>
/// <remarks>
/// <para>
/// This interface is the coordinate-side counterpart to <see cref="IPolylineFormatter{TPolyline}"/>.
/// Together they allow the engine base classes to be used directly — without subclassing — by supplying
/// both formatters via <see cref="PolylineOptions{TValue, TPolyline}"/>.
/// </para>
/// <para>
/// Use <see cref="FormatterBuilder{T}"/> to build a <see cref="PolylineValueFormatter{T}"/> that
/// already implements this interface.
/// </para>
/// </remarks>
public interface IPolylineValueFormatter<TValue> {
    /// <summary>
    /// Extracts and scales all column values from <paramref name="item"/> into the <paramref name="values"/> span.
    /// Called once per item in the encoding loop.
    /// </summary>
    /// <param name="item">The source item from which column values are extracted.</param>
    /// <param name="values">
    /// Output buffer that receives the scaled integer values. Its length must equal the number of columns
    /// defined by this formatter.
    /// </param>
    void GetValues(TValue item, Span<long> values);

    /// <summary>
    /// Reconstructs a <typeparamref name="TValue"/> from the given scaled integer values.
    /// Called once per decoded item in the decoding loop.
    /// </summary>
    /// <param name="values">
    /// The accumulated scaled integer values decoded from the polyline. Each element corresponds to
    /// the same column position as in <see cref="GetValues"/>.
    /// </param>
    /// <returns>A <typeparamref name="TValue"/> reconstructed from <paramref name="values"/>.</returns>
    TValue CreateItem(ReadOnlySpan<long> values);

    /// <summary>
    /// Gets the number of values (columns) per encoded item.
    /// This is the required length of the buffer passed to <see cref="GetValues"/> and
    /// the length of the span received in <see cref="CreateItem"/>.
    /// </summary>
    int Width { get; }
}
