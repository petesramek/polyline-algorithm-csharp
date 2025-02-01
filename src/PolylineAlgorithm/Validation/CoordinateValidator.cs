//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// 
/// </summary>
public sealed class CoordinateValidator {
    /// <summary>
    /// Represents default coordinate validator. This field is read-only.
    /// </summary>
    /// <remarks>Validates latitude between -90 and 90; longitude between -180 and 180.</remarks>
    [SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "We just want to deal with it this way.")]
    public static readonly CoordinateValidator Default = new(new CoordinateRange(Constants.Coordinate.MinLatitude, Constants.Coordinate.MaxLatitude), new CoordinateRange(Constants.Coordinate.MinLongitude, Constants.Coordinate.MaxLongitude));

    /// <summary>
    /// Initializes an instance of coordinate validator.
    /// </summary>
    /// <param name="latitudeRange">A latitude range.</param>
    /// <param name="longitudeRange">A longitude range.</param>
    public CoordinateValidator(CoordinateRange latitudeRange, CoordinateRange longitudeRange) {
        Latitude = latitudeRange;
        Longitude = longitudeRange;
    }

    /// <summary>
    /// A latitude validation range.
    /// </summary>
    public CoordinateRange Latitude { get; }

    /// <summary>
    /// A longitude validation range.
    /// </summary>
    public CoordinateRange Longitude { get; }
}
