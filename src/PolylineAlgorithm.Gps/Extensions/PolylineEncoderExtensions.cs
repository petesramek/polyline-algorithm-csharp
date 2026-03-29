namespace PolylineAlgorithm.Gps.Extensions;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Gps.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

/// <summary>
/// Provides extension methods for the <see cref="IPolylineEncoder{TValue, TPolyline}"/> interface to facilitate encoding geographic coordinates into polylines.
/// </summary>
public static class PolylineEncoderExtensions {
    /// <summary>
    /// Encodes a collection of <see cref="Coordinate"/> instances into an encoded polyline.
    /// </summary>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TValue, TPolyline}"/> instance used to perform the encoding operation.
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
    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We need a list as we do need to marshal it as span.")]
    [SuppressMessage("Design", "MA0016:Prefer using collection abstraction instead of implementation", Justification = "We need a list as we do need to marshal it as span.")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Method ExceptionGuard.ThrowArgumentNull(string) throws ArgumentNullException.")]
    public static TPolyline Encode<TValue, TPolyline>(this IPolylineEncoder<TValue, TPolyline> encoder, List<TValue> coordinates) {
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
    /// Encodes an array of <see cref="Coordinate"/> instances into an encoded polyline.
    /// </summary>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TValue, TPolyline}"/> instance used to perform the encoding operation.
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

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Method ExceptionGuard.ThrowArgumentNull(string) throws ArgumentNullException.")]
    public static TPolyline Encode<TValue, TPolyline>(this IPolylineEncoder<TValue, TPolyline> encoder, TValue[] coordinates) {
        if (encoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(encoder));
        }

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

        return encoder.Encode(coordinates.AsSpan());
    }
}
