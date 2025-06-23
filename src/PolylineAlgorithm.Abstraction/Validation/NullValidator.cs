namespace PolylineAlgorithm.Abstraction.Validation;

using PolylineAlgorithm.Abstraction.Validation.Abstraction;


/// <summary>
/// A validator that always returns <see langword="true"/> for any coordinate, latitude, or longitude.
/// </summary>
/// <typeparam name="TCoordinate">The type representing a coordinate.</typeparam>
public sealed class NullValidator<TCoordinate> : Validator<TCoordinate> {
    /// <inheritdoc/>
    public override bool IsValid(TCoordinate coordinate) {
        return true;
    }

    /// <inheritdoc/>
    public override bool IsValidLatitude(double latitude) {
        return true;
    }

    /// <inheritdoc/>
    public override bool IsValidLongitude(double longitude) {
        return true;
    }
}
