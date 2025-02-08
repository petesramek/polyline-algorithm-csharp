//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Initializes an instance of coordinate validator.
/// </summary>
/// <remarks>
/// Initializes an instance of coordinate validator.
/// </remarks>
/// <param name="latitudeRange">A latitude range.</param>
/// <param name="longitudeRange">A longitude range.</param>
public sealed class CoordinateValidator(CoordinateRange latitudeRange, CoordinateRange longitudeRange) : ICoordinateValidator {
    ///// <summary>
    ///// Represents default coordinate validator. This field is read-only.
    ///// </summary>
    ///// <remarks>Validates latitude between -90 and 90; longitude between -180 and 180.</remarks>
    //[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "We want to expose default instance of .")]

    /// <summary>
    /// A latitude validation range.
    /// </summary>
    public CoordinateRange Latitude { get; } = latitudeRange;

    /// <summary>
    /// A longitude validation range.
    /// </summary>
    public CoordinateRange Longitude { get; } = longitudeRange;

    public bool IsValid(Coordinate coordinate) {
        return
            Latitude.IsInRange(coordinate.Latitude)
            && Longitude.IsInRange(coordinate.Longitude);
    }
}
