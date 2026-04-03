//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides extension methods for the <see cref="IPolylineDecoder{TPolyline, TValue}"/> interface to facilitate decoding encoded polylines.
/// </summary>
public static class PolylineDecoderExtensions {
    /// <summary>
    /// Decodes an encoded polyline represented as a character array into a sequence of geographic coordinates.
    /// </summary>
    /// <typeparam name="TValue">The coordinate type returned by the decoder.</typeparam>
    /// <param name="decoder">
    /// The <see cref="IPolylineDecoder{TPolyline, TValue}"/> instance used to perform the decoding operation.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline as a character array to decode. The array is converted to a string internally.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TValue"/> containing the decoded coordinate pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> or <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<TValue> Decode<TValue>(this IPolylineDecoder<string, TValue> decoder, char[] polyline) {
        if (decoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(decoder));
        }

        if (polyline is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(polyline));
        }

        return decoder.Decode(new string(polyline));
    }

    /// <summary>
    /// Decodes an encoded polyline represented as a read-only memory of characters into a sequence of geographic coordinates.
    /// </summary>
    /// <typeparam name="TValue">The coordinate type returned by the decoder.</typeparam>
    /// <param name="decoder">
    /// The <see cref="IPolylineDecoder{TPolyline, TValue}"/> instance used to perform the decoding operation.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline as a read-only memory of characters to decode. The memory is converted to a string internally.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TValue"/> containing the decoded coordinate pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<TValue> Decode<TValue>(this IPolylineDecoder<string, TValue> decoder, ReadOnlyMemory<char> polyline) {
        if (decoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(decoder));
        }

        return decoder.Decode(polyline.ToString());
    }

    /// <summary>
    /// Decodes an encoded polyline string into a sequence of geographic coordinates,
    /// using a decoder that accepts <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
    /// </summary>
    /// <typeparam name="TValue">The coordinate type returned by the decoder.</typeparam>
    /// <param name="decoder">
    /// The <see cref="IPolylineDecoder{TPolyline, TValue}"/> instance used to perform the decoding operation.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline string to decode. The string is converted to <see cref="ReadOnlyMemory{T}"/> internally.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TValue"/> containing the decoded coordinate pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> or <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<TValue> Decode<TValue>(this IPolylineDecoder<ReadOnlyMemory<char>, TValue> decoder, string polyline) {
        if (decoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(decoder));
        }

        if (polyline is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(polyline));
        }

        return decoder.Decode(polyline.AsMemory());
    }
}
