//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides extension methods for the <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> interface to facilitate encoding geographic coordinates into polylines.
/// </summary>
public static class PolylineEncoderExtensions {
    /// <summary>
    /// Encodes a collection of <see cref="Coordinate"/> instances into an encoded polyline.
    /// </summary>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> instance used to perform the encoding operation.
    /// </param>
    /// <param name="coordinates">
    /// The sequence of <see cref="Coordinate"/> objects to encode.
    /// </param>
    /// <returns>
    /// A <see cref="Polyline"/> representing the encoded polyline string for the provided coordinates.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoder"/> is <see langword="null"/>.
    /// </exception>
    public static Polyline Encode(this IPolylineEncoder<Coordinate, Polyline> encoder, ICollection<Coordinate> coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }

    /// <summary>
    /// Encodes an array of <see cref="Coordinate"/> instances into an encoded polyline.
    /// </summary>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> instance used to perform the encoding operation.
    /// </param>
    /// <param name="coordinates">
    /// The array of <see cref="Coordinate"/> objects to encode.
    /// </param>
    /// <returns>
    /// A <see cref="Polyline"/> representing the encoded polyline string for the provided coordinates.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoder"/> is <see langword="null"/>.
    /// </exception>
    public static Polyline Encode(this IPolylineEncoder<Coordinate, Polyline> encoder, Coordinate[] coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }
}
