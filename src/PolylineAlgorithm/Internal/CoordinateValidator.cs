//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm;
using PolylineAlgorithm.Validation;

/// <summary>
/// Validates geographic coordinates by checking if their latitude and longitude values fall within specified valid ranges.
/// Implements the <see cref="ICoordinateValidator"/> interface.
/// </summary>
internal sealed class CoordinateValidator : ICoordinateValidator {
    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateValidator"/> class using the provided latitude and longitude ranges.
    /// </summary>
    /// <param name="latitudeRange">
    /// The <see cref="CoordinateRange"/> that defines the valid range for latitude values.
    /// </param>
    /// <param name="longitudeRange">
    /// The <see cref="CoordinateRange"/> that defines the valid range for longitude values.
    /// </param>
    public CoordinateValidator(CoordinateRange latitudeRange, CoordinateRange longitudeRange) {
        LatitudeRange = latitudeRange;
        LongitudeRange = longitudeRange;
    }

    /// <summary>
    /// Gets the <see cref="CoordinateRange"/> that defines the valid range for latitude values.
    /// </summary>
    public CoordinateRange LatitudeRange { get; }

    /// <summary>
    /// Gets the <see cref="CoordinateRange"/> that defines the valid range for longitude values.
    /// </summary>
    public CoordinateRange LongitudeRange { get; }

    /// <summary>
    /// Determines whether the specified <see cref="Coordinate"/> is valid by checking if its latitude and longitude
    /// are within the configured valid ranges.
    /// </summary>
    /// <param name="coordinate">The <see cref="Coordinate"/> to validate.</param>
    /// <returns>
    /// <see langword="true"/> if both the latitude and longitude of <paramref name="coordinate"/> are within their respective valid ranges;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid(Coordinate coordinate) =>
        LatitudeRange.IsInRange(coordinate.Latitude)
            && LongitudeRange.IsInRange(coordinate.Longitude);
}
