namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;

internal class NetTopologyPolylineDecoder : AbstractPolylineDecoder<string, Point> {
    protected override Point CreateCoordinate(double latitude, double longitude) {
        return new Point(latitude, longitude);
    }

    protected override ReadOnlyMemory<char> GetReadOnlyMemory(string? polyline) {
        if (string.IsNullOrWhiteSpace(polyline)) {
            throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(polyline));
        }

        return polyline.AsMemory();
    }
}
