//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Validation.Abstraction;

/// <summary>
/// Provides an abstract base class for validating coordinates and their latitude/longitude components.
/// </summary>
/// <typeparam name="TCoordinate">The type representing a coordinate.</typeparam>
public abstract class Validator<TCoordinate> {
    /// <summary>
    /// Determines whether the specified coordinate is valid.
    /// </summary>
    /// <param name="coordinate">The coordinate to validate.</param>
    /// <returns><see langword="true"/> if the coordinate is valid; otherwise, <see langword="false"/>.</returns>
    public abstract bool IsValid(TCoordinate coordinate);

    /// <summary>
    /// Determines whether the specified latitude value is valid.
    /// </summary>
    /// <param name="latitude">The latitude value to validate.</param>
    /// <returns><see langword="true"/> if the latitude is valid; otherwise, <see langword="false"/>.</returns>
    public abstract bool IsValidLatitude(double latitude);

    /// <summary>
    /// Determines whether the specified longitude value is valid.
    /// </summary>
    /// <param name="longitude">The longitude value to validate.</param>
    /// <returns><see langword="true"/> if the longitude is valid; otherwise, <see langword="false"/>.</returns>
    public abstract bool IsValidLongitude(double longitude);
}
