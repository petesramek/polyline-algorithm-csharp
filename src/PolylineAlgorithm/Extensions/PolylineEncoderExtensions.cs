//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Collections.Generic;
#if NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

/// <summary>
/// Provides extension methods for the <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> interface to facilitate encoding geographic coordinates into polylines.
/// </summary>
public static class PolylineEncoderExtensions {
    /// <summary>
    /// Encodes a <see cref="List{T}"/> of <typeparamref name="TCoordinate"/> instances into an encoded polyline.
    /// </summary>
    /// <typeparam name="TCoordinate">The type that represents a geographic coordinate to encode.</typeparam>
    /// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> instance used to perform the encoding operation.
    /// </param>
    /// <param name="coordinates">
    /// The list of <typeparamref name="TCoordinate"/> objects to encode.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TPolyline"/> instance representing the encoded polyline for the provided coordinates.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoder"/> or <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We need a list as we do need to marshal it as span.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0016:Prefer using collection abstraction instead of implementation", Justification = "We need a list as we do need to marshal it as span.")]
    public static TPolyline Encode<TCoordinate, TPolyline>(this IPolylineEncoder<TCoordinate, TPolyline> encoder, List<TCoordinate> coordinates) {
        if (encoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(encoder));
        }

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

#if NET5_0_OR_GREATER
        return encoder.Encode(CollectionsMarshal.AsSpan(coordinates));
#else
        return encoder.Encode([.. coordinates]);
#endif
    }


    /// <summary>
    /// Encodes an array of <typeparamref name="TCoordinate"/> instances into an encoded polyline.
    /// </summary>
    /// <typeparam name="TCoordinate">The type that represents a geographic coordinate to encode.</typeparam>
    /// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> instance used to perform the encoding operation.
    /// </param>
    /// <param name="coordinates">
    /// The array of <typeparamref name="TCoordinate"/> objects to encode.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TPolyline"/> instance representing the encoded polyline for the provided coordinates.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoder"/> or <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    public static TPolyline Encode<TCoordinate, TPolyline>(this IPolylineEncoder<TCoordinate, TPolyline> encoder, TCoordinate[] coordinates) {
        if (encoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(encoder));
        }

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

        return encoder.Encode(coordinates.AsSpan());
    }
}
