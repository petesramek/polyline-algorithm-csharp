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
        [double.MinValue, 0],
        [double.MaxValue, 0],
        [double.NaN, 0],
        [double.PositiveInfinity, 0],
        [double.NegativeInfinity, 0],
        [0, double.MinValue],
        [0, double.MaxValue],
        [0, double.NaN],
        [0, double.PositiveInfinity],
        [0, double.NegativeInfinity]
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
        // Arrange & Act
        Coordinate result = new(latitude, longitude);

        // Assert
        Assert.IsFalse(result.IsDefault());
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
        // Act
        static void New(double latitude, double longitude) => new Coordinate(latitude, longitude);

        // Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => New(latitude, longitude));
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

    /// <summary>
    /// Tests the <see cref="Coordinate"/> constructor with latitude out of range.
    /// </summary>
    [TestMethod]
    public void Constructor_Latitude_OutOfRange_Throws() {
        // Arrange & Act
        void OverMaxLatitude() => new Coordinate(91, 0);
        void UnderMinLatitude() => new Coordinate(-91, 0);

        // Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(UnderMinLatitude);
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(OverMaxLatitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate"/> constructor with longitude out of range.
    /// </summary>
    [TestMethod]
    public void Constructor_Longitude_OutOfRange_Throws() {
        // Arrange & Act
        void UnderMinLongitude() => new Coordinate(0, -181);
        void OverMaxLongitude() => new Coordinate(0, 181);


        // Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(UnderMinLongitude);
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(OverMaxLongitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate"/> constructor with boundary values.
    /// </summary>
    [TestMethod]
    public void Constructor_Boundary_Values_Ok() {
        // Arrange
        const int MinLatitude = -90;
        const int MaxLatitude = 90;
        const int MinLongitude = -180;
        const int MaxLongitude = 180;

        // Act
        var coordinate1 = new Coordinate(MaxLatitude, MaxLongitude);
        var coordinate2 = new Coordinate(MinLatitude, MinLongitude);

        // Assert
        Assert.AreEqual(MaxLatitude, coordinate1.Latitude);
        Assert.AreEqual(MaxLongitude, coordinate1.Longitude);
        Assert.AreEqual(MinLatitude, coordinate2.Latitude);
        Assert.AreEqual(-MaxLongitude, coordinate2.Longitude);
    }

    /// <summary>
    /// Tests the <see cref="Coordinate.Equals(object)"/> method with various cases.
    /// </summary>
    [TestMethod]
    public void Equals_Object_True_And_False() {
        // Arrange
        var coordinate = new Coordinate(10, 20);
        object equalCoordinate = new Coordinate(10, 20);
        object notEqualCoordinate = new Coordinate(0, 0);
        object notCoordinate = "not a coordinate";
        object @null = null!;

        // Act & Assert
        Assert.IsTrue(coordinate.Equals(equalCoordinate));
        Assert.IsFalse(coordinate.Equals(notEqualCoordinate));
        Assert.IsFalse(coordinate.Equals(notCoordinate));
        Assert.IsFalse(coordinate.Equals(@null));
    }

    /// <summary>
    /// Tests the <see cref="Coordinate.GetHashCode"/> method for equal coordinates.
    /// </summary>
    [TestMethod]
    public void GetHashCode_Equal_For_Equal_Coordinates() {
        var coordinate1 = new Coordinate(10, 20);
        var coordinate2 = new Coordinate(10, 20);
        Assert.AreEqual(coordinate1.GetHashCode(), coordinate2.GetHashCode());
    }

    /// <summary>
    /// Tests the <see cref="Coordinate.ToString"/> method for correct formatting.
    /// </summary>
    [TestMethod]
    public void ToString_Format_Ok() {
        var c = new Coordinate(12.34, 56.78);
        StringAssert.Contains(c.ToString(), "Latitude: 12.34");
        StringAssert.Contains(c.ToString(), "Longitude: 56.78");
    }

    /// <summary>
    /// Tests the equality operators for the <see cref="Coordinate"/> type.
    /// </summary>
    [TestMethod]
    public void Equality_Operators_Ok() {
        var coordinate = new Coordinate(10, 20);
        var equalCoordinate = new Coordinate(10, 20);
        var notEqualCoordinate = new Coordinate(0, 0);

        Assert.IsTrue(coordinate == equalCoordinate);
        Assert.IsFalse(coordinate != equalCoordinate);
        Assert.IsTrue(coordinate != notEqualCoordinate);
        Assert.IsFalse(coordinate == notEqualCoordinate);
    }
}