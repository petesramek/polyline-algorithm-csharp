//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;

/// <summary>
/// Defines a contract for validating geographic coordinates by checking their latitude and longitude values
/// against predefined valid ranges.
/// </summary>
public interface ICoordinateValidator {
    /// <summary>
    /// Determines whether the specified <see cref="Coordinate"/> is valid by verifying that its latitude and longitude
    /// are within the allowed ranges.
    /// </summary>
    /// <param name="coordinate">The <see cref="Coordinate"/> to validate.</param>
    /// <returns>
    /// <see langword="true"/> if both the latitude and longitude of <paramref name="coordinate"/> are within their respective valid ranges;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValid(Coordinate coordinate);

    static void SetDefault(ICoordinateValidator validator) => Default = validator ?? throw new ArgumentNullException(nameof(validator));

    /// <summary>
    /// Gets the default <see cref="ICoordinateValidator"/> instance used for coordinate validation.
    /// </summary>
    internal static ICoordinateValidator Default { get; private set; } = new CoordinateValidator(Defaults.Coordinate.Range.Latitude, Defaults.Coordinate.Range.Longitude);
}