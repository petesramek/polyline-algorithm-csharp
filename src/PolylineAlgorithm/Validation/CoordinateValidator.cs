//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;
/// <summary>
/// Initializes an instance of coordinate validator.
/// </summary>
/// <remarks>
/// Initializes an instance of coordinate validator.
/// </remarks>
/// <param name="latitudeRange">A latitude range.</param>
/// <param name="longitudeRange">A longitude range.</param>
public sealed class CoordinateValidator(CoordinateRange latitudeRange, CoordinateRange longitudeRange) : ICoordinateValidator {
    /// <summary>
    /// A latitude validation range.
    /// </summary>
    public CoordinateRange Latitude { get; } = latitudeRange;

    /// <summary>
    /// A longitude validation range.
    /// </summary>
    public CoordinateRange Longitude { get; } = longitudeRange;

    public bool IsValid(ref readonly Coordinate coordinate) {
        return
            Latitude.IsInRange(coordinate.Latitude)
            && Longitude.IsInRange(coordinate.Longitude);
    }
}
