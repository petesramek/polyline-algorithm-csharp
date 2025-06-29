namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System.Buffers;
using System.Text;

internal class NetTopologyPolylineEncoder : PolylineEncoder<Point, string> {

    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return string.Empty;
        }

        return polyline.ToString();
    }

    protected override double GetLatitude(Point? current) {
        return current?.X ?? 0d;
    }

    protected override double GetLongitude(Point? current) {
        return current?.Y ?? 0d;
    }
}