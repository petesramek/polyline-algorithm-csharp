//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

/// <summary>
/// Provides validation of a coordinate value.
/// </summary>
public interface ICoordinateValidator {
    /// <summary>
    /// Determines whether the value of a coordinate is valid.
    /// </summary>
    /// <param name="coordinate">The coordinate value to be validated.</param>
    /// <returns><see langword="true"/> if the <paramref name="coordinate"/> parameter is a valid; otherwise, <see langword="false"/>.</returns>
    bool IsValid((double Latitude, double Longitude) coordinate);
}