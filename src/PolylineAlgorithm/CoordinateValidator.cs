//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;

/// <inheritdoc cref="ICoordinateValidator" />
public sealed class CoordinateValidator : ICoordinateValidator {
    public bool IsValid((double Latitude, double Longitude) coordinate) {
        return IsValidLatitude(ref coordinate.Latitude) && IsValidLongitude(ref coordinate.Longitude);
    }

    /// <summary>
    /// Determines whether the value of a latitude is valid.
    /// </summary>
    /// <param name="latitude">The latitude to be validated.</param>
    /// <returns><see langword="true"/> if the <paramref name="latitude"/> parameter is a valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidLatitude(ref readonly double latitude) {
        return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
    }

    /// <summary>
    /// Determines whether the value of a latitude is valid.
    /// </summary>
    /// <param name="longitude">The longitude to be validated.</param>
    /// <returns><see langword="true"/> if the <paramref name="longitude"/> parameter is a valid; otherwise, <see langword="false"/>.</returns>
    private static bool IsValidLongitude(ref readonly double longitude) {
        return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
    }
}
