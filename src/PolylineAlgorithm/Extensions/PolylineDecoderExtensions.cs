namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

/// <summary>
/// Provides extension methods for the <see cref="IPolylineDecoder"/> interface to simplify decoding operations.
/// </summary>
public static class PolylineDecoderExtensions {
    /// <summary>
    /// Decodes an encoded polyline string into a collection of geographic coordinates.
    /// </summary>
    /// <param name="decoder">The <see cref="IPolylineDecoder"/> instance used to decode the polyline.</param>
    /// <param name="polyline">The encoded polyline string to decode.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Coordinate"/> objects representing the decoded geographic coordinates.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<Coordinate> Decode(this IPolylineDecoder decoder, string polyline) {
        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromString(polyline));
    }

    /// <summary>
    /// Decodes an encoded polyline represented as a character array into a collection of geographic coordinates.
    /// </summary>
    /// <param name="decoder">The <see cref="IPolylineDecoder"/> instance used to decode the polyline.</param>
    /// <param name="polyline">The encoded polyline represented as a character array to decode.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Coordinate"/> objects representing the decoded geographic coordinates.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<Coordinate> Decode(this IPolylineDecoder decoder, char[] polyline) {
        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromCharArray(polyline));
    }
}
