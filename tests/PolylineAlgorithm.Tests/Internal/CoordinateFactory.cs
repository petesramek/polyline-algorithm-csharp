namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Abstraction;

internal class CoordinateFactory : ICoordinateFactory<Coordinate> {
    public Coordinate Create(double latitude, double longitude) {
        return new Coordinate(latitude, longitude);
    }
}