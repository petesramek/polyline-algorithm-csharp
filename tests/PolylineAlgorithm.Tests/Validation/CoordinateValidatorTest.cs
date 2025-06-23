//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Validation;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Validation;

/// <summary>
/// Tests for the <see cref="CoordinateValidator"/> type.
/// </summary>
[TestClass]
public class CoordinateValidatorTest {
    private static readonly CoordinateRange _latitude = new(-90, 90);
    private static readonly CoordinateRange _longitude = new(-180, 180);

    /// <summary>
    /// Provides test data for the <see cref="IsValid_Valid_Parameters_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> IsValid_Method_Parameters => [
        [0, 0, true],
        [_latitude.Min, _longitude.Max, true],
        [_latitude.Min - 1, _longitude.Max, false],
        [_latitude.Min, _longitude.Max + 1, false],
        [_latitude.Min - 1, _longitude.Max + 1, false]
    ];

    /// <summary>
    /// Tests the <see cref="CoordinateValidator.IsValid(Coordinate)"/> method with valid parameters.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    /// <param name="expected">The expected result.</param>
    [TestMethod]
    [DynamicData(nameof(IsValid_Method_Parameters))]
    public void IsValid_Valid_Parameters_Ok(double latitude, double longitude, bool expected) {
        // Arrange
        Coordinate coordinate = new(latitude, longitude);
        CoordinateValidator validator = new(_latitude, _longitude);

        // Act
        bool result = validator.IsValid(coordinate);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests the <see cref="CoordinateRange"/> constructor with invalid minimum parameter.
    /// </summary>
    [TestMethod]
    public void Constructor_Invalid_Min_Parameter_ArgumentOutOfRangeException() {
        // Arrange
        double min = 0;
        double max = 0;

        // Act
        static void New(double min, double max) {
            CoordinateRange range = new(min, max);
        }

        // Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => New(min, max));
    }
}