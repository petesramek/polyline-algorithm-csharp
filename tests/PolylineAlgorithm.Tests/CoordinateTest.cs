//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for the <see cref="Coordinate"/> type.
/// </summary>
[TestClass]
public class CoordinateTest {
    /// <summary>
    /// Provides test data for the <see cref="Constructor_Valid_Parameters_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> Constructor_Valid_Parameters => [
        [90, 180],
        [-90, -180],
        [90, -180],
        [-90, 180],
    ];

    /// <summary>
    /// Provides test data for the <see cref="Constructor_Invalid_Parameters_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> Constructor_Invalid_Parameters => [
        [double.MaxValue, double.MaxValue],
        [double.MinValue, double.MinValue],
        [double.MaxValue, double.MinValue],
        [double.MinValue, double.MaxValue],
    ];

    /// <summary>
    /// Tests the parameterless constructor of the <see cref="Coordinate"/> class.
    /// </summary>
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        bool @default = true;
        double latitude = 0d;
        double longitude = 0d;

        // Act
        Coordinate result = new();

        // Assert
        Assert.AreEqual(@default, result.IsDefault());
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate"/> constructor with valid parameters.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
    public void Constructor_Valid_Parameters_Ok(double latitude, double longitude) {
        // Arrange
        bool @default = false;

        // Act
        Coordinate result = new(latitude, longitude);

        // Assert
        Assert.AreEqual(@default, result.IsDefault());
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate"/> constructor with invalid parameters.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Invalid_Parameters))]
    public void Constructor_Invalid_Parameters_Ok(double latitude, double longitude) {
        // Arrange
        bool @default = false;

        // Act
        Coordinate result = new(latitude, longitude);

        // Assert
        Assert.AreEqual(@default, result.IsDefault());
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate.Deconstruct(out double, out double)"/> method.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
    public void Deconstruct_Equals_Parameters(double latitude, double longitude) {
        // Arrange
        // Act
        Coordinate coordinate = new(latitude, longitude);

        // Assert
        Assert.AreEqual(latitude, coordinate.Latitude);
        Assert.AreEqual(longitude, coordinate.Longitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate.Equals(Coordinate)"/> method with equal coordinates.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
    public void Equals_Coordinate_True(double latitude, double longitude) {
        // Arrange
        Coordinate @this = new(latitude, longitude);
        Coordinate other = new(latitude, longitude);

        // Act
        bool result = @this.Equals(other);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate.Equals(Coordinate)"/> method with unequal coordinates.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
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