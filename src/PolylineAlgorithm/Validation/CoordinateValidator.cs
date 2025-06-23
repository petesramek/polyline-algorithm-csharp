//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Abstraction.Validation.Abstraction;
using PolylineAlgorithm.Internal;

/// <summary>
/// Provides functionality to validate geographic coordinates based on specified latitude and longitude ranges.
/// Implements the <see cref="Validator{TCoordinate}"/> abstract class.
/// </summary>
public sealed class CoordinateValidator : Validator<Coordinate> {
    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateValidator"/> class with the specified latitude and longitude ranges.
    /// </summary>
    /// <param name="latitudeRange">The range within which the latitude value is considered valid.</param>
    /// <param name="longitudeRange">The range within which the longitude value is considered valid.</param>
    internal CoordinateValidator(CoordinateRange latitudeRange, CoordinateRange longitudeRange) {
        Latitude = latitudeRange;
        Longitude = longitudeRange;
    }

    /// <summary>
    /// Gets the range within which the latitude value is considered valid.
    /// </summary>
    internal CoordinateRange Latitude { get; }

    /// <summary>
    /// Gets the range within which the longitude value is considered valid.
    /// </summary>
    internal CoordinateRange Longitude { get; }

    /// <inheritdoc cref="Validator{TCoordinate}.IsValid(TCoordinate)"/>
    public override bool IsValid(Coordinate coordinate) {
        return
            IsValidLatitude(coordinate.Latitude)
            && IsValidLongitude(coordinate.Longitude);
    }

    /// <inheritdoc cref="Validator{TCoordinate}.IsValidLatitude(double)"/>
    public override bool IsValidLatitude(double latitude) {
        return Latitude.IsInRange(latitude);
    }

    /// <inheritdoc cref="Validator{TCoordinate}.IsValidLongitude(double)"/>
    public override bool IsValidLongitude(double longitude) {
        return Longitude.IsInRange(longitude);
    }
}
