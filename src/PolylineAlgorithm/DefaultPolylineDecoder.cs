namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;

public sealed class DefaultPolylineDecoder : PolylineDecoder<Coordinate> {
    public override Coordinate Construct(double latitude, double longitude) {
        return new Coordinate(latitude, longitude);
    }
}
