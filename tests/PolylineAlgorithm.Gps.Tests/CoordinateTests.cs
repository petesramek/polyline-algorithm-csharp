//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Tests.Properties;
using System;

/// <summary>
/// Tests for <see cref="Coordinate"/>.
/// </summary>
[TestClass]
public sealed class CoordinateTests {
    /// <summary>
    /// Tests that default constructor creates coordinate with zero latitude and longitude.
    /// </summary>
    [TestMethod]
    public void Default_Constructor_Creates_Coordinate_With_Zero_Values() {
        // Arrange & Act
        Coordinate coordinate = new();

        // Assert
        Assert.AreEqual(0.0, coordinate.Latitude);
        Assert.AreEqual(0.0, coordinate.Longitude);
    }

    /// <summary>
    /// Tests that parameterized constructor creates coordinate with specified values.
    /// </summary>
    [TestMethod]
    [DataRow(0.0, 0.0)]
    [DataRow(90.0, 0.0)]
    [DataRow(-90.0, 0.0)]
    [DataRow(90.0, 180.0)]
    [DataRow(-90.0, 180.0)]
    [DataRow(90.0, -180.0)]
    [DataRow(-90.0, -180.0)]
    [DataRow(0.0, 180.0)]
    [DataRow(0.0, -180.0)]
    public void Constructor_With_Valid_Values_Creates_Instance_With_Specified_Values(double latitude, double longitude) {
        // Act
        Coordinate coordinate = new(latitude, longitude);

        // Assert
        Assert.AreEqual(latitude, coordinate.Latitude);
        Assert.AreEqual(longitude, coordinate.Longitude);
    }

    /// <summary>
    /// Tests that constructor throws ArgumentOutOfRangeException when latitude is greater than 90.
    /// </summary>
    [TestMethod]
    [DataRow(90.0000000001, 0.0, "latitude")]
    [DataRow(-90.0000000001, 0.0, "latitude")]
    [DataRow(double.NaN, 0.0, "latitude")]
    [DataRow(double.PositiveInfinity, 0.0, "latitude")]
    [DataRow(double.NegativeInfinity, 0.0, "latitude")]
    [DataRow(0.0, 180.0000000001, "longitude")]
    [DataRow(0.0, -180.0000000001, "longitude")]
    [DataRow(0.0, double.NaN, "longitude")]
    [DataRow(0.0, double.PositiveInfinity, "longitude")]
    [DataRow(0.0, double.NegativeInfinity, "longitude")]
    public void Constructor_With_Invalid_Values_Throws_ArgumentOutOfRangeException(double latitude, double longitude, string paramName) {
        // Act
        ArgumentOutOfRangeException exception =
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));

        // Assert
        Assert.AreEqual(paramName, exception.ParamName);
    }

    /// <summary>
    /// Tests that IsDefault returns true for default constructed coordinate.
    /// </summary>
    [TestMethod]
    public void IsDefault_With_Default_Coordinate_Instance_Returns_True() {
        // Arrange
        Coordinate coordinate = default;

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsDefault returns true for coordinate with zero values.
    /// </summary>
    [TestMethod]
    public void IsDefault_With_Zero_Values_Returns_True() {
        // Arrange
        Coordinate coordinate = new(0.0, 0.0);

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsDefault returns false for coordinate with non-zero latitude.
    /// </summary>
    [TestMethod]
    [DataRow(1.0, 0.0)]
    [DataRow(0.0, 1.0)]
    [DataRow(1.0, 1.0)]
    public void IsDefault_With_Non_Zero_Values_Returns_False(double latitude, double longitude) {
        // Arrange
        Coordinate coordinate = new(latitude, longitude);

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns true when object is same coordinate.
    /// </summary>
    [TestMethod]

    public void Equals_With_Identical_Coordinate_As_Object_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        object coordinate2 = new Coordinate(45.5, -122.5);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals returns false when object is null.
    /// </summary>
    [TestMethod]
    public void Equals_With_Null_Object_Returns_False() {
        // Arrange
        Coordinate coordinate = new(45.5, -122.5);

        // Act
        bool result = coordinate.Equals((object?)null);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns false when object is of different type.
    /// </summary>
    [TestMethod]

    public void Equals_With_Different_Type_Returns_False() {
        // Arrange
        Coordinate coordinate = new(45.5, -122.5);
        object otherObject = "not a coordinate";

        // Act
        bool result = coordinate.Equals(otherObject);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns false when object is coordinate with different values.
    /// </summary>
    [TestMethod]
    public void Equals_With_Different_Coordinate_As_Object_Returns_False() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        object coordinate2 = new Coordinate(45.5, -122.6);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns true when both coordinates have same values.
    /// </summary>
    [TestMethod]
    public void Equals_With_Identical_Coordinate_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5, -122.5);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals returns false when latitudes differ.
    /// </summary>
    [TestMethod]
    [DataRow(45.6, -122.5)]
    [DataRow(45.5, -122.6)]
    [DataRow(46.5, -121.5)]
    public void Equals_With_Different_Values_Returns_False(double latitude, double longitude) {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(latitude, longitude);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns true for default coordinates.
    /// </summary>
    [TestMethod]
    public void Equals_With_Default_Coordinates_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new();
        Coordinate coordinate2 = new();

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns true when coordinates are identical.
    /// </summary>
    [TestMethod]
    [DataRow(45.5, -122.5, 0.1)]
    [DataRow(45.5, -122.5, 0.001)]
    [DataRow(45.5, -122.5, 0.0000001)]
    [DataRow(45.5009, -122.5009, 0.001)]
    [DataRow(45.50099, -122.50099, 0.001)]
    [DataRow(45.4991, -122.4991, 0.001)]
    public void Equals_Within_Tolerance_Returns_True(double latitude, double longitude, double tolerance) {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(latitude, longitude);

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns false when latitude difference exceeds tolerance.
    /// </summary>
    [TestMethod]
    [DataRow(45.7, -122.5, 0.1)]
    [DataRow(45.5, -122.7, 0.1)]
    [DataRow(45.7, -122.7, 0.1)]
    [DataRow(45.502, -122.5, 0.001)]
    [DataRow(45.5, -122.502, 0.001)]
    [DataRow(45.5, -122.5000002, 0.0000001)]
    [DataRow(45.5000002, -122.5, 0.0000001)]
    public void Equals_With_Exceeding_Tolerance_Returns_False(double latitude, double longitude, double tolerance) {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(latitude, longitude);

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for identical coordinates.
    /// </summary>
    [TestMethod]

    public void GetHashCode_With_Identical_Coordinates_Returns_Identical_Hashcode() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5, -122.5);

        // Act
        int hash1 = coordinate1.GetHashCode();
        int hash2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that GetHashCode returns different values for different coordinates.
    /// </summary>
    [TestMethod]

    public void GetHashCode_With_Different_Coordinates_Returns_Different_Values() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.6, -122.6);

        // Act
        int hash1 = coordinate1.GetHashCode();
        int hash2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that GetHashCode returns consistent value for same coordinate.
    /// </summary>
    [TestMethod]

    public void GetHashCode_Returns_Identical_Value() {
        // Arrange
        Coordinate coordinate = new(45.5, -122.5);

        // Act
        int hash1 = coordinate.GetHashCode();
        int hash2 = coordinate.GetHashCode();

        // Assert
        Assert.AreEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for default coordinates.
    /// </summary>
    [TestMethod]

    public void GetHashCode_With_Default_Coordinates_Returns_Identical_Value() {
        // Arrange
        Coordinate coordinate1 = new();
        Coordinate coordinate2 = new(0.0, 0.0);

        // Act
        int hash1 = coordinate1.GetHashCode();
        int hash2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that ToString returns expected format with positive coordinates.
    /// </summary>
    [TestMethod]

    public void ToString_With_Positive_Coordinates_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(45.5, 122.5);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 45.5, Longitude: 122.5 }", result);
    }

    /// <summary>
    /// Tests that ToString returns expected format with negative coordinates.
    /// </summary>
    [TestMethod]

    public void ToString_With_Negative_Coordinates_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(-45.5, -122.5);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: -45.5, Longitude: -122.5 }", result);
    }

    /// <summary>
    /// Tests that ToString returns expected format for default coordinate.
    /// </summary>
    [TestMethod]

    public void ToString_With_Default_Coordinate_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new();

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 0, Longitude: 0 }", result);
    }

    /// <summary>
    /// Tests that ToString returns expected format with mixed sign coordinates.
    /// </summary>
    [TestMethod]

    public void ToString_With_Mixed_Sign_Coordinates_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(45.5, -122.5);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 45.5, Longitude: -122.5 }", result);
    }

    /// <summary>
    /// Tests that ToString uses invariant culture formatting.
    /// </summary>
    [TestMethod]

    public void ToString_Uses_Invariant_Culture_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(45.123456789, -122.987654321);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.IsTrue(result.Contains("45.123456789", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("-122.987654321", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString returns expected format with boundary values.
    /// </summary>
    [TestMethod]

    public void ToString_With_Boundary_Values_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(90.0, 180.0);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 90, Longitude: 180 }", result);
    }

    /// <summary>
    /// Tests that ValidateValue accepts valid value within specified range.
    /// </summary>
    [TestMethod]

    public void Validate_Value_With_Valid_Value_Does_Not_Throw() {
        // Arrange
        const double value = 50.0;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        Coordinate.Validator.ValidateValue(value, min, max, paramName);
    }

    /// <summary>
    /// Tests that ValidateValue accepts value at minimum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Minimum_Boundary_Does_Not_Throw() {
        // Arrange
        const double value = 0.0;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        Coordinate.Validator.ValidateValue(value, min, max, paramName);
    }

    /// <summary>
    /// Tests that ValidateValue accepts value at maximum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Maximum_Boundary_Does_Not_Throw() {
        // Arrange
        const double value = 100.0;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        Coordinate.Validator.ValidateValue(value, min, max, paramName);
    }

    /// <summary>
    /// Tests that ValidateValue throws ArgumentOutOfRangeException when value is below minimum.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Value_Below_Minimum_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double value = -0.1;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateValue(value, min, max, paramName));
        Assert.AreEqual(paramName, exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateValue throws ArgumentOutOfRangeException when value exceeds maximum.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Value_Above_Maximum_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double value = 100.1;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateValue(value, min, max, paramName));
        Assert.AreEqual(paramName, exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateValue throws ArgumentOutOfRangeException when value is NaN.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_NaN_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double value = double.NaN;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateValue(value, min, max, paramName));
        Assert.AreEqual(paramName, exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateValue throws ArgumentOutOfRangeException when value is positive infinity.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Positive_Infinity_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double value = double.PositiveInfinity;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateValue(value, min, max, paramName));
        Assert.AreEqual(paramName, exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateValue throws ArgumentOutOfRangeException when value is negative infinity.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Negative_Infinity_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double value = double.NegativeInfinity;
        const double min = 0.0;
        const double max = 100.0;
        const string paramName = "testParam";

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateValue(value, min, max, paramName));
        Assert.AreEqual(paramName, exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateValue accepts negative value within negative range.
    /// </summary>
    [TestMethod]

    public void ValidateValue_With_Negative_Value_In_Negative_Range_Does_Not_Throw() {
        // Arrange
        const double value = -50.0;
        const double min = -100.0;
        const double max = -10.0;
        const string paramName = "testParam";

        // Act & Assert
        Coordinate.Validator.ValidateValue(value, min, max, paramName);
    }

    /// <summary>
    /// Tests that Validate accepts valid latitude and longitude.
    /// </summary>
    [TestMethod]

    public void Validate_With_Valid_Latitude_And_Longitude_Does_Not_Throw() {
        // Arrange
        const double latitude = 45.5;
        const double longitude = -122.5;

        // Act & Assert
        Coordinate.Validator.Validate(latitude, longitude);
    }

    /// <summary>
    /// Tests that Validate accepts boundary values.
    /// </summary>
    [TestMethod]

    public void Validate_With_Boundary_Values_Does_Not_Throw() {
        // Arrange
        const double latitude = 90.0;
        const double longitude = 180.0;

        // Act & Assert
        Coordinate.Validator.Validate(latitude, longitude);
    }

    /// <summary>
    /// Tests that Validate accepts minimum boundary values.
    /// </summary>
    [TestMethod]

    public void Validate_With_Minimum_Boundary_Values_Does_Not_Throw() {
        // Arrange
        const double latitude = -90.0;
        const double longitude = -180.0;

        // Act & Assert
        Coordinate.Validator.Validate(latitude, longitude);
    }

    /// <summary>
    /// Tests that Validate throws ArgumentOutOfRangeException when latitude is invalid.
    /// </summary>
    [TestMethod]

    public void Validate_With_Invalid_Latitude_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = 91.0;
        const double validLongitude = 0.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.Validate(invalidLatitude, validLongitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that Validate throws ArgumentOutOfRangeException when longitude is invalid.
    /// </summary>
    [TestMethod]

    public void Validate_With_Invalid_Longitude_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double validLatitude = 0.0;
        const double invalidLongitude = 181.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.Validate(validLatitude, invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that Validate throws ArgumentOutOfRangeException when latitude is NaN.
    /// </summary>
    [TestMethod]

    public void Validate_With_Latitude_NaN_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = double.NaN;
        const double validLongitude = 0.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.Validate(invalidLatitude, validLongitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that Validate throws ArgumentOutOfRangeException when longitude is NaN.
    /// </summary>
    [TestMethod]

    public void Validate_With_Longitude_NaN_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double validLatitude = 0.0;
        const double invalidLongitude = double.NaN;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.Validate(validLatitude, invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLatitude accepts valid latitude within range.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Valid_Latitude_Does_Not_Throw() {
        // Arrange
        const double validLatitude = 45.5;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(validLatitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude accepts latitude at minimum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Minimum_Boundary_Does_Not_Throw() {
        // Arrange
        const double validLatitude = -90.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(validLatitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude accepts latitude at maximum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Maximum_Boundary_Does_Not_Throw() {
        // Arrange
        const double validLatitude = 90.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(validLatitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude exceeds maximum.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Latitude_Greater_Than_90_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = 91.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is below minimum.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Latitude_Less_Than_Negative_90_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = -91.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is NaN.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_NaN_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = double.NaN;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is positive infinity.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Positive_Infinity_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = double.PositiveInfinity;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is negative infinity.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Negative_Infinity_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = double.NegativeInfinity;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude accepts valid longitude within range.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Valid_Longitude_Does_Not_Throw() {
        // Arrange
        const double validLongitude = -122.5;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(validLongitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude accepts longitude at minimum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Minimum_Boundary_Does_Not_Throw() {
        // Arrange
        const double validLongitude = -180.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(validLongitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude accepts longitude at maximum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Maximum_Boundary_Does_Not_Throw() {
        // Arrange
        const double validLongitude = 180.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(validLongitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude exceeds maximum.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Longitude_Greater_Than_180_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = 181.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is below minimum.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Longitude_Less_Than_Negative_180_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = -181.0;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is NaN.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_NaN_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = double.NaN;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is positive infinity.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Positive_Infinity_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = double.PositiveInfinity;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is negative infinity.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Negative_Infinity_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = double.NegativeInfinity;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that Equals with zero tolerance returns false even for identical coordinates.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Zero_Tolerance_Returns_False() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5, -122.5);
        const double tolerance = 0.0;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with zero tolerance returns false for non-identical coordinates.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Zero_Tolerance_And_Different_Coordinates_Returns_False() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5000001, -122.5);
        const double tolerance = 0.0;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with difference exactly equal to tolerance returns true.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Difference_Exactly_Equal_To_Tolerance_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.501, -122.5);
        const double tolerance = 0.001;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with negative tolerance behaves like absolute tolerance.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Negative_Tolerance_Returns_False() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5, -122.5);
        const double tolerance = -0.001;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with large tolerance returns true for widely different coordinates.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Large_Tolerance_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(50.0, -120.0);
        const double tolerance = 10.0;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance works correctly at extreme latitude boundaries.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Extreme_Boundary_Latitudes_Returns_Expected_Result() {
        // Arrange
        Coordinate coordinate1 = new(90.0, 0.0);
        Coordinate coordinate2 = new(89.9991, 0.0);
        const double tolerance = 0.001;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance works correctly at extreme longitude boundaries.
    /// </summary>
    [TestMethod]

    public void Equals_With_Tolerance_With_Extreme_Boundary_Longitudes_Returns_Expected_Result() {
        // Arrange
        Coordinate coordinate1 = new(0.0, 180.0);
        Coordinate coordinate2 = new(0.0, 179.9991);
        const double tolerance = 0.001;

        // Act
        bool result = coordinate1.Equals(coordinate2, tolerance);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that GetHashCode returns different values for coordinates with slight differences.
    /// </summary>
    [TestMethod]

    public void GetHashCode_With_Slightly_Different_Coordinates_Returns_Different_Values() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5000001, -122.5);

        // Act
        int hash1 = coordinate1.GetHashCode();
        int hash2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that GetHashCode works with boundary latitude values.
    /// </summary>
    [TestMethod]

    public void GetHashCode_With_Boundary_Latitude_Values_Returns_Value() {
        // Arrange
        Coordinate coordinate1 = new(90.0, 0.0);
        Coordinate coordinate2 = new(-90.0, 0.0);

        // Act
        int hash1 = coordinate1.GetHashCode();
        int hash2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that GetHashCode works with boundary longitude values.
    /// </summary>
    [TestMethod]

    public void GetHashCode_With_Boundary_Longitude_Values_Returns_Value() {
        // Arrange
        Coordinate coordinate1 = new(0.0, 180.0);
        Coordinate coordinate2 = new(0.0, -180.0);

        // Act
        int hash1 = coordinate1.GetHashCode();
        int hash2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that ToString handles very small non-zero values correctly.
    /// </summary>
    [TestMethod]

    public void ToString_With_Very_Small_Values_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(0.0000001, -0.0000001);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.IsTrue(result.Contains("1E-07", StringComparison.Ordinal) || result.Contains("0.0000001", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("-1E-07", StringComparison.Ordinal) || result.Contains("-0.0000001", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString handles high precision decimal values correctly.
    /// </summary>
    [TestMethod]

    public void ToString_With_High_Precision_Values_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(45.123456789012345, -122.987654321098765);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.IsTrue(result.Contains("45.123456789", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("-122.98765432", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString formats negative zero as negative zero.
    /// </summary>
    [TestMethod]

    public void ToString_With_Negative_Zero_Returns_Expected_Format() {
        // Arrange
        Coordinate coordinate = new(-0.0, -0.0);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.IsTrue(result.Contains("Latitude: ") && result.Contains("Longitude: "));
    }

    /// <summary>
    /// Tests that ValidateLatitude accepts zero value.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Zero_Does_Not_Throw() {
        // Arrange
        const double validLatitude = 0.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(validLatitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws with value just beyond maximum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Value_Just_Beyond_Maximum_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = 90.000001;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws with value just beyond minimum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLatitude_With_Value_Just_Beyond_Minimum_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLatitude = -90.000001;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLatitude(invalidLatitude));
        Assert.AreEqual("latitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude accepts zero value.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Zero_Does_Not_Throw() {
        // Arrange
        const double validLongitude = 0.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(validLongitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws with value just beyond maximum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Value_Just_Beyond_Maximum_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = 180.000001;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws with value just beyond minimum boundary.
    /// </summary>
    [TestMethod]

    public void ValidateLongitude_With_Value_Just_Beyond_Minimum_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double invalidLongitude = -180.000001;

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            Coordinate.Validator.ValidateLongitude(invalidLongitude));
        Assert.AreEqual("longitude", exception.ParamName);
    }

    /// <summary>
    /// Tests the equality (==) operator returns true for identical coordinates.
    /// </summary>
    [TestMethod]

    public void Equality_Operator_With_Identical_Coordinates_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5, -122.5);

        // Act
        bool result = coordinate1 == coordinate2;

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests the equality (==) operator returns false for different coordinates.
    /// </summary>
    [TestMethod]

    public void Equality_Operator_With_Different_Coordinates_Returns_False() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.6, -122.5);

        // Act
        bool result = coordinate1 == coordinate2;

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests the inequality (!=) operator returns false for identical coordinates.
    /// </summary>
    [TestMethod]

    public void Inequality_Operator_With_Identical_Coordinates_Returns_False() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.5, -122.5);

        // Act
        bool result = coordinate1 != coordinate2;

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests the inequality (!=) operator returns true for different coordinates.
    /// </summary>
    [TestMethod]

    public void Inequality_Operator_With_Different_Coordinates_Returns_True() {
        // Arrange
        Coordinate coordinate1 = new(45.5, -122.5);
        Coordinate coordinate2 = new(45.6, -122.5);

        // Act
        bool result = coordinate1 != coordinate2;

        // Assert
        Assert.IsTrue(result);
    }
}
