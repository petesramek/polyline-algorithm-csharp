namespace PolylineAlgorithm.Abstraction.Validation.Abstraction;

public abstract class CoordinateValidator<TCoordinate> {
    public abstract bool IsValid(TCoordinate coordinate);

    public abstract bool IsValidLatitude(double latitude);

    public abstract bool IsValidLongitude(double longitude);
}
