namespace PolylineAlgorithm;

public interface ICoordinateValidator {
    bool IsValid((double Latitude, double Longitude) coordinate);
}