//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Validation;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class CoordinateValidatorTest {
    private static readonly CoordinateRange _latitude = new(-90, 90);
    private static readonly CoordinateRange _longitude = new(-180, 180);

    public static IEnumerable<object[]> IsValid_Method_Parameters => [
        [ 0, 0, true ],
        [ _latitude.Min, _longitude.Max, true ],
        [ _latitude.Min - 1, _longitude.Max, false ],
        [ _latitude.Min, _longitude.Max + 1, false ],
        [ _latitude.Min - 1, _longitude.Max + 1, false ]
    ];

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

    [TestMethod]
    public void Constructor_Invalid_Min_Parameter_ArgumentOutOfRangeException() {
        // Arrange
        double min = 0;
        double max = 0;

        // Act
        void New(double min, double max) {
            CoordinateRange range = new(min, max);

        }

        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => New(min, max));
    }

    [TestMethod]
    public void SetDefault_Validator_Ok() {
        // Arrange
        CoordinateRange range = new(double.MinValue, double.MaxValue);
        CoordinateValidator validator = new(range, range);

        // Act
        ICoordinateValidator original = ICoordinateValidator.Default;

        ICoordinateValidator
            .SetDefault(validator);

        ICoordinateValidator @new = ICoordinateValidator.Default;

        ICoordinateValidator
            .SetDefault(original);

        // Assert
        Assert.AreEqual(validator, @new);
        Assert.AreNotEqual(original, @new);
    }
}
