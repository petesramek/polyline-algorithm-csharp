//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;

/// <summary>
/// Provides a mechanism for validating <see cref="Coordinate"/> structure and its values, Latitude and Longitude.
/// </summary>
public interface ICoordinateValidator {
    /// <summary>
    /// Gets latitude validation range.
    /// </summary>
    CoordinateRange Latitude { get; }

    /// <summary>
    /// Gets longitude validation range.
    /// </summary>
    CoordinateRange Longitude { get; }

    /// <summary>
    /// Returns a value indicating whether <see cref="Coordinate" /> is valid.
    /// </summary>
    /// <param name="coordinate">The coordinate to be validated.</param>
    /// <returns><see langword="true"/> if <paramref name="coordinate"/> is valid; otherwise, <see langword="false"/>.</returns>
    bool IsValid(Coordinate coordinate);

    /// <summary>
    /// Represents default coordinate validator instance.
    /// </summary>
    internal static ICoordinateValidator Default { get; } = new CoordinateValidator(Defaults.Coordinate.Range.Latitude, Defaults.Coordinate.Range.Longitude);
}