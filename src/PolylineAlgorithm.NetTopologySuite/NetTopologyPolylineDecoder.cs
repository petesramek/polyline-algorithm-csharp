namespace PolylineAlgorithm.NetTopologySuite;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;

/// <inheritdoc cref="PolylineDecoder{MultiPoint, Point}"/>
public sealed class NetTopologyPolylineDecoder : PolylineDecoder<Point> {
    protected override Point CreateCoordinate(ref readonly double latitude, ref readonly double longitude) {
        return new Point(latitude, longitude);
    }
}
