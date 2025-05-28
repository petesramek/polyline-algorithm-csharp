namespace PolylineAlgorithm.Abstraction.Validation;

using PolylineAlgorithm.Abstraction.Validation.Abstraction;

public sealed class NullValidator<TCoordinate> : CoordinateValidator<TCoordinate> {
    public override bool IsValid(TCoordinate coordinate) {
        return true;
    }

    public override bool IsValidLatitude(double latitude) {
        return true;
    }

    public override bool IsValidLongitude(double longitude) {
        return true;
    }
}
