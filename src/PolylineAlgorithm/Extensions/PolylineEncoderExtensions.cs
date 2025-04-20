namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides extension methods for the <see cref="IPolylineEncoder"/> interface to simplify encoding operations.
/// </summary>
public static class PolylineEncoderExtensions {
    /// <summary>
    /// Encodes a collection of geographic coordinates into an encoded polyline string.
    /// </summary>
    /// <typeparam name="Coordinate">The type representing a geographic coordinate.</typeparam>
    /// <param name="encoder">The <see cref="IPolylineEncoder"/> instance used to encode the coordinates.</param>
    /// <param name="coordinates">A collection of coordinates to encode.</param>
    /// <returns>
    /// A <see cref="Polyline"/> instance representing the encoded polyline string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="encoder"/> is <see langword="null"/>.
    /// </exception>
    public static Polyline Encode<Coordinate>(this IPolylineEncoder encoder, ICollection<Coordinate> coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }

    /// <summary>
    /// Encodes an array of geographic coordinates into an encoded polyline string.
    /// </summary>
    /// <param name="encoder">The <see cref="IPolylineEncoder"/> instance used to encode the coordinates.</param>
    /// <param name="coordinates">An array of coordinates to encode.</param>
    /// <returns>
    /// A <see cref="Polyline"/> instance representing the encoded polyline string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="encoder"/> is <see langword="null"/>.
    /// </exception>
    public static Polyline Encode(this IPolylineEncoder encoder, Coordinate[] coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }
}
