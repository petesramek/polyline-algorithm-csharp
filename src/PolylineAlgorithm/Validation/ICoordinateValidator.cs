//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;

/// <summary>
/// Provides a mechanism for validating geographic coordinates, specifically their latitude and longitude values,
/// against predefined valid ranges.
/// </summary>
public interface ICoordinateValidator {
    /// <summary>
    /// Gets the range within which the latitude value is considered valid.
    /// </summary>
    CoordinateRange Latitude { get; }

    /// <summary>
    /// Gets the range within which the longitude value is considered valid.
    /// </summary>
    CoordinateRange Longitude { get; }

    /// <summary>
    /// Determines whether the specified coordinate is valid based on the latitude and longitude ranges.
    /// </summary>
    /// <param name="coordinate">The <see cref="Coordinate"/> to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="coordinate"/> is within the valid latitude and longitude ranges;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValid(Coordinate coordinate);

    /// <summary>
    /// Gets the default coordinate validator instance.
    /// </summary>
    internal static ICoordinateValidator Default { get; } = new CoordinateValidator(Defaults.Coordinate.Range.Latitude, Defaults.Coordinate.Range.Longitude);
}