//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

/// <inheritdoc cref="ICoordinateValidator" />
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

    /// <inheritdoc/>
    public CoordinateRange Latitude { get; }

    /// <inheritdoc/>
    public CoordinateRange Longitude { get; }

    /// <inheritdoc/>
    public bool IsValid(Coordinate coordinate) {
        return
            Latitude.IsInRange(coordinate.Latitude)
            && Longitude.IsInRange(coordinate.Longitude);
    }
}
