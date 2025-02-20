//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

/// <inheritdoc cref="ICoordinateValidator" />
public sealed class CoordinateValidator : ICoordinateValidator {
    /// <summary>
    /// Creates a new <see cref="CoordinateValidator"/> class that uses
    /// specified <paramref name="latitudeRange"/> and <paramref name="longitudeRange"/> to determine if <see cref="Coordinate"/> value is valid.
    /// </summary>
    /// <param name="latitudeRange"></param>
    /// <param name="longitudeRange"></param>
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