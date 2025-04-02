namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;

public sealed class DefaultPolylineEncoder : PolylineEncoder<Coordinate> {
    protected override (double Latitude, double Longitude) Deconstruct(Coordinate source) {
        return (source.Latitude, source.Longitude);
    }
}
