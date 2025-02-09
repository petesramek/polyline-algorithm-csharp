//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using Newtonsoft.Json.Linq;
using PolylineAlgorithm.Tests.Internal;
using System;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class CoordinateTest {
    public static IEnumerable<object[]> ValidParamaters => [
        [ 90, 180 ],
        [ -90, -180 ],
        [ 90, -180 ],
        [ -90, 180 ],
    ];

    public static IEnumerable<object[]> InvalidParamaters => [
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
    [DynamicData(nameof(ValidParamaters))]
    public void Constructor_Valid_Paramaters_Ok(double latitude, double longitude) {
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
    [DynamicData(nameof(InvalidParamaters))]
    public void Constructor_Invalid_Paramaters_Ok(double latitude, double longitude) {
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
    [DynamicData(nameof(ValidParamaters))]
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
    [DynamicData(nameof(ValidParamaters))]
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
