namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

public static class PolylineEncoderExtensions {
    public static Polyline Encode<Coordinate>(this IPolylineEncoder encoder, ICollection<Coordinate> coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }

    public static Polyline Encode(this IPolylineEncoder encoder, Coordinate[] coordinates) {
        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        return encoder.Encode(coordinates);
    }
}
