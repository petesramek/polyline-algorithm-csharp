namespace PolylineAlgorithm;

public sealed class DefaultPolylineDecoder : PolylineDecoder<Coordinate> {
    public override Coordinate Construct(double latitude, double longitude) {
        return new Coordinate(latitude, longitude);
    }
}
