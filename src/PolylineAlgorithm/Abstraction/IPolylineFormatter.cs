//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System;

/// <summary>
/// Defines how to produce a <typeparamref name="TPolyline"/> from an encoded character buffer (output/write
/// direction), and how to extract that buffer back from a <typeparamref name="TPolyline"/> (input/read
/// direction).
/// </summary>
/// <typeparam name="TPolyline">The polyline surface type — for example <see cref="string"/> or
/// <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.</typeparam>
/// <remarks>
/// <para>
/// This interface is the polyline-surface counterpart to <see cref="IPolylineValueFormatter{TValue}"/>.
/// The engine exclusively works with <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> internally.
/// The formatter is the only code that touches <typeparamref name="TPolyline"/>.
/// </para>
/// <para>
/// Use <see cref="PolylineFormatter.ForString"/>, <see cref="PolylineFormatter.ForMemory"/>, or
/// <see cref="PolylineFormatter.Create{T}"/> to obtain a ready-made implementation.
/// </para>
/// </remarks>
public interface IPolylineFormatter<TPolyline> {
    /// <summary>
    /// Creates a <typeparamref name="TPolyline"/> from the encoded character buffer produced by the encoder.
    /// </summary>
    /// <param name="encoded">The encoded polyline as a read-only span of characters.</param>
    /// <returns>A <typeparamref name="TPolyline"/> wrapping or derived from <paramref name="encoded"/>.</returns>
    TPolyline Write(ReadOnlyMemory<char> encoded);

    /// <summary>
    /// Extracts the character buffer from a <typeparamref name="TPolyline"/> for the decoder to read.
    /// </summary>
    /// <param name="polyline">The polyline to read from.</param>
    /// <returns>A <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> representing the encoded characters.</returns>
    ReadOnlyMemory<char> Read(TPolyline polyline);
}
