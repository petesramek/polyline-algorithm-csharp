namespace PolylineAlgorithm.NetTopologySuite;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;

/// <inheritdoc cref="PolylineEncoder{Coordinate}"/>
public sealed class NetTopologyPolylineEncoder : PolylineEncoder<Coordinate> {
    protected override double GetLatitude(ref readonly Coordinate coordinate) {
        ArgumentNullException.ThrowIfNull(coordinate);

        return coordinate.X;
    }

    protected override double GetLongitude(ref readonly Coordinate coordinate) {
        ArgumentNullException.ThrowIfNull(coordinate);

        return coordinate.Y;
    }
}
