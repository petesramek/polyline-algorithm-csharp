//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;

/// <summary>
/// Defines proprties and methods used for validating coordinates.
/// </summary>
public interface ICoordinateValidator {
    /// <summary>
    /// Represents latitude validation range
    /// </summary>
    CoordinateRange Latitude { get; }

    /// <summary>
    /// Represents longitude validation range
    /// </summary>
    CoordinateRange Longitude { get; }

    /// <summary>
    /// Validates coordinate.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns><see langword="true"/></returns>
    bool IsValid(ref readonly Coordinate coordinate);

    /// <summary>
    /// Represents global coordinate validator instance.
    /// </summary>
    internal static ICoordinateValidator Default { get; private set; } = new CoordinateValidator(Defaults.Coordinate.Range.Latitude, Defaults.Coordinate.Range.Longitude);
}