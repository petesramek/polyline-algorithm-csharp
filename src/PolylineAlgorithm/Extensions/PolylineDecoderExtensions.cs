namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

public static class PolylineDecoderExtensions {
    public static IEnumerable<TCoordinate> Decode<TCoordinate>(this IPolylineDecoder<TCoordinate> decoder, string polyline) {
        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromString(polyline));
    }

    public static IEnumerable<TCoordinate> Decode<TCoordinate>(this IPolylineDecoder<TCoordinate> decoder, byte[] polyline) {
        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromByteArray(polyline));
    }
}
