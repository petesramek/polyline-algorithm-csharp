namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

public static class PolylineEncoderExtensions {
    public static Polyline Encode<TCoordinate>(this IPolylineEncoder<TCoordinate> encoder, ICollection<TCoordinate> coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }

    public static Polyline Encode<TCoordinate>(this IPolylineEncoder<TCoordinate> encoder, TCoordinate[] coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }
}
