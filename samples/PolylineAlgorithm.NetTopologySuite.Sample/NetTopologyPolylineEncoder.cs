namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;
using System.Buffers;
using System.Text;

internal class NetTopologyPolylineEncoder : PolylineEncoder<Point, string> {
    protected override string CreatePolyline(ReadOnlySequence<char> sequence) {
        if(sequence.IsEmpty) {
            return string.Empty;
        }

        if(sequence.IsSingleSegment) {
            return sequence.FirstSpan.ToString();
        }

        var enumerator = sequence.GetEnumerator();
        var sb = new StringBuilder();

        while (enumerator.MoveNext()) {
            sb.Append(enumerator.Current);
        }

        return sb.ToString();
    }

    protected override double GetLatitude(Point? current) {
        return current?.X ?? 0d;
    }

    protected override double GetLongitude(Point? current) {
        return current?.Y ?? 0d;
    }
}