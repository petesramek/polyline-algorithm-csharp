//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

/// <summary>
/// Provides functionality to validate geographic coordinates based on specified latitude and longitude ranges.
/// Implements the <see cref="ICoordinateValidator"/> interface.
/// </summary>
public sealed class CoordinateValidator : ICoordinateValidator {
    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateValidator"/> class with the specified latitude and longitude ranges.
    /// </summary>
    /// <param name="latitudeRange">The range within which the latitude value is considered valid.</param>
    /// <param name="longitudeRange">The range within which the longitude value is considered valid.</param>
    public CoordinateValidator(CoordinateRange latitudeRange, CoordinateRange longitudeRange) {
        Latitude = latitudeRange;
        Longitude = longitudeRange;
    }

    /// <summary>
    /// Gets the range within which the latitude value is considered valid.
    /// </summary>
    public CoordinateRange Latitude { get; }

    /// <summary>
    /// Gets the range within which the longitude value is considered valid.
    /// </summary>
    public CoordinateRange Longitude { get; }

    /// <summary>
    /// Determines whether the specified coordinate is valid based on the latitude and longitude ranges.
    /// </summary>
    /// <param name="coordinate">The <see cref="Coordinate"/> to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="coordinate"/> is within the valid latitude and longitude ranges;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid(Coordinate coordinate) {
        return
            Latitude.IsInRange(coordinate.Latitude)
            && Longitude.IsInRange(coordinate.Longitude);
    }
}
