namespace PolylineAlgorithm.Utility;

using System.Collections.Generic;

internal static class StaticValueProvider {
    private static readonly Polyline _polyline = Polyline.FromString("???_gsia@_cidP??~fsia@?~fsia@~bidP?~bidP??_gsia@");
    private static readonly IEnumerable<Coordinate> _coordinates = [
        new (0, 0),
        new (0, 180),
        new (90, 180),
        new (90, 0),
        new (90, -180),
        new (0, -180),
        new (-90, -180),
        new (-90, 0)
    ];

    public static IEnumerable<Coordinate> GetCoordinates() {
        return _coordinates;
    }

    public static Polyline GetPolyline() {
        return _polyline;
    }
}
