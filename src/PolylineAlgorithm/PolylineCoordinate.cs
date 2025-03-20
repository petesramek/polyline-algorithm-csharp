namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;

public struct PolylineCoordinate {
    public PolylineCoordinate() {
        Latitude = default;
        Longitude = default;
    }

    public PolylineCoordinate(int latitude, int longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    public int Latitude { get; }

    public int Longitude { get; }


    public static implicit operator Coordinate(PolylineCoordinate coordinate) {
        return coordinate.ToCoordinate();
    }

    public static implicit operator PolylineCoordinate(Coordinate coordinate) {
        return FromCoordinate(coordinate);
    }

    public static CoordinateVariance operator -(PolylineCoordinate initial, PolylineCoordinate next) {
        return CoordinateVariance.Calculate(initial, next);
    }
    public Coordinate ToCoordinate() {
        return new Coordinate(Latitude / Defaults.Algorithm.Precision, Longitude / Defaults.Algorithm.Precision);
    }

    public static PolylineCoordinate FromCoordinate(Coordinate coordinate) {
        return new PolylineCoordinate(Convert.ToInt32(coordinate.Latitude * Defaults.Algorithm.Precision), Convert.ToInt32(coordinate.Longitude * Defaults.Algorithm.Precision));
    }
}
