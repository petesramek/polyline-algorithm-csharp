namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;
using System.Buffers;

internal class NetTopologyPolylineDecoder : PolylineDecoder<Point, string> {
    public override PolylineEncodingOptions<Point> Options { get; } = PolylineEncodingOptions<Point>.Default;

    protected override Point CreateCoordinate(double latitude, double longitude) {
        return new Point(latitude, longitude);
    }

    protected override ReadOnlySequence<char> GetReadOnlySequence(string? polyline) {
        if (string.IsNullOrWhiteSpace(polyline)) {
            throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(polyline));
        }

        return new(polyline.AsMemory());
    }
}
