//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;
/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class CoordinateTest {
    public static IEnumerable<object[]> ValidParameters => [
        [ 90, 180 ],
        [ -90, -180 ],
        [ 90, -180 ],
        [ -90, 180 ],
    ];

    public static IEnumerable<object[]> InvalidParameters => [
        [ double.MaxValue, double.MaxValue ],
        [ double.MinValue, double.MinValue ],
        [ double.MaxValue, double.MinValue ],
        [ double.MinValue, double.MaxValue ],
    ];

    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        bool valid = true;
        bool @default = true;
        double latitude = 0d;
        double longitude = 0d;

        // Act
        Coordinate result = new();

        // Assert
        Assert.AreEqual(valid, result.IsValid);
        Assert.AreEqual(@default, result.IsDefault);
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(ValidParameters))]
    public void Constructor_Valid_Parameters_Ok(double latitude, double longitude) {
        // Arrange
        bool valid = true;
        bool @default = false;

        // Act
        Coordinate result = new(latitude, longitude);

        // Assert
        Assert.AreEqual(valid, result.IsValid);
        Assert.AreEqual(@default, result.IsDefault);
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(InvalidParameters))]
    public void Constructor_Invalid_Parameters_Ok(double latitude, double longitude) {
        // Arrange
        bool valid = false;
        bool @default = false;

        // Act
        Coordinate result = new(latitude, longitude);

        // Assert
        Assert.AreEqual(valid, result.IsValid);
        Assert.AreEqual(@default, result.IsDefault);
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }


    [TestMethod]
    [DynamicData(nameof(ValidParameters))]
    public void Equals_Coordinate_True(double latitude, double longitude) {
        // Arrange
        Coordinate @this = new(latitude, longitude);
        Coordinate other = new(latitude, longitude);

        // Act
        bool result = @this.Equals(other);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DynamicData(nameof(ValidParameters))]
    public void Equals_Coordinate_False(double latitude, double longitude) {
        // Arrange
        Coordinate @this = new(latitude, longitude);
        Coordinate other = new(0, 0);

        // Act
        bool result = @this.Equals(other);

        // Assert
        Assert.IsFalse(result);
    }
}
