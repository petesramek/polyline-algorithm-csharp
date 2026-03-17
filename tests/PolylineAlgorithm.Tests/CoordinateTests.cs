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
public class CoordinateTests {
    /// <summary>
    /// Tests that the default constructor initializes latitude to zero.
    /// </summary>
    [TestMethod]
    public void DefaultConstructor_Initialization_SetsLatitudeToZero() {
        // Act
        var coordinate = new Coordinate();

        // Assert
        Assert.AreEqual(0.0, coordinate.Latitude);
    }

    /// <summary>
    /// Tests that the default constructor initializes longitude to zero.
    /// </summary>
    [TestMethod]
    public void DefaultConstructor_Initialization_SetsLongitudeToZero() {
        // Act
        var coordinate = new Coordinate();

        // Assert
        Assert.AreEqual(0.0, coordinate.Longitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor sets latitude correctly with valid value.
    /// </summary>
    [TestMethod]
    public void Constructor_ValidLatitude_SetsLatitude() {
        // Arrange
        double latitude = 45.0;
        double longitude = 90.0;

        // Act
        var coordinate = new Coordinate(latitude, longitude);

        // Assert
        Assert.AreEqual(latitude, coordinate.Latitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor sets longitude correctly with valid value.
    /// </summary>
    [TestMethod]
    public void Constructor_ValidLongitude_SetsLongitude() {
        // Arrange
        double latitude = 45.0;
        double longitude = 90.0;

        // Act
        var coordinate = new Coordinate(latitude, longitude);

        // Assert
        Assert.AreEqual(longitude, coordinate.Longitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor accepts the minimum latitude value.
    /// </summary>
    [TestMethod]
    public void Constructor_MinimumLatitude_Succeeds() {
        // Arrange
        double latitude = -90.0;
        double longitude = 0.0;

        // Act
        var coordinate = new Coordinate(latitude, longitude);

        // Assert
        Assert.AreEqual(latitude, coordinate.Latitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor accepts the maximum latitude value.
    /// </summary>
    [TestMethod]
    public void Constructor_MaximumLatitude_Succeeds() {
        // Arrange
        double latitude = 90.0;
        double longitude = 0.0;

        // Act
        var coordinate = new Coordinate(latitude, longitude);

        // Assert
        Assert.AreEqual(latitude, coordinate.Latitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor accepts the minimum longitude value.
    /// </summary>
    [TestMethod]
    public void Constructor_MinimumLongitude_Succeeds() {
        // Arrange
        double latitude = 0.0;
        double longitude = -180.0;

        // Act
        var coordinate = new Coordinate(latitude, longitude);

        // Assert
        Assert.AreEqual(longitude, coordinate.Longitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor accepts the maximum longitude value.
    /// </summary>
    [TestMethod]
    public void Constructor_MaximumLongitude_Succeeds() {
        // Arrange
        double latitude = 0.0;
        double longitude = 180.0;

        // Act
        var coordinate = new Coordinate(latitude, longitude);

        // Assert
        Assert.AreEqual(longitude, coordinate.Longitude);
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when latitude is less than -90.
    /// </summary>
    [TestMethod]
    public void Constructor_LatitudeLessThanMinimum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = -90.1;
        double longitude = 0.0;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when latitude is greater than 90.
    /// </summary>
    [TestMethod]
    public void Constructor_LatitudeGreaterThanMaximum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 90.1;
        double longitude = 0.0;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when longitude is less than -180.
    /// </summary>
    [TestMethod]
    public void Constructor_LongitudeLessThanMinimum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 0.0;
        double longitude = -180.1;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when longitude is greater than 180.
    /// </summary>
    [TestMethod]
    public void Constructor_LongitudeGreaterThanMaximum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 0.0;
        double longitude = 180.1;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when latitude is NaN.
    /// </summary>
    [TestMethod]
    public void Constructor_LatitudeNaN_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = double.NaN;
        double longitude = 0.0;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when longitude is NaN.
    /// </summary>
    [TestMethod]
    public void Constructor_LongitudeNaN_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 0.0;
        double longitude = double.NaN;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when latitude is positive infinity.
    /// </summary>
    [TestMethod]
    public void Constructor_LatitudePositiveInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = double.PositiveInfinity;
        double longitude = 0.0;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when latitude is negative infinity.
    /// </summary>
    [TestMethod]
    public void Constructor_LatitudeNegativeInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = double.NegativeInfinity;
        double longitude = 0.0;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when longitude is positive infinity.
    /// </summary>
    [TestMethod]
    public void Constructor_LongitudePositiveInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 0.0;
        double longitude = double.PositiveInfinity;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that the parameterized constructor throws ArgumentOutOfRangeException when longitude is negative infinity.
    /// </summary>
    [TestMethod]
    public void Constructor_LongitudeNegativeInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 0.0;
        double longitude = double.NegativeInfinity;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Coordinate(latitude, longitude));
    }

    /// <summary>
    /// Tests that IsDefault returns true for default constructed coordinate.
    /// </summary>
    [TestMethod]
    public void IsDefault_DefaultConstructor_ReturnsTrue() {
        // Arrange
        var coordinate = new Coordinate();

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsDefault returns true for coordinate with zero latitude and longitude.
    /// </summary>
    [TestMethod]
    public void IsDefault_ZeroLatitudeAndLongitude_ReturnsTrue() {
        // Arrange
        var coordinate = new Coordinate(0.0, 0.0);

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that IsDefault returns false when latitude is non-zero.
    /// </summary>
    [TestMethod]
    public void IsDefault_NonZeroLatitude_ReturnsFalse() {
        // Arrange
        var coordinate = new Coordinate(1.0, 0.0);

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsDefault returns false when longitude is non-zero.
    /// </summary>
    [TestMethod]
    public void IsDefault_NonZeroLongitude_ReturnsFalse() {
        // Arrange
        var coordinate = new Coordinate(0.0, 1.0);

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that IsDefault returns false when both latitude and longitude are non-zero.
    /// </summary>
    [TestMethod]
    public void IsDefault_NonZeroLatitudeAndLongitude_ReturnsFalse() {
        // Arrange
        var coordinate = new Coordinate(45.0, 90.0);

        // Act
        bool result = coordinate.IsDefault();

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(object) returns true for equal coordinates.
    /// </summary>
    [TestMethod]
    public void EqualsObject_EqualCoordinates_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        object coordinate2 = new Coordinate(45.0, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals(object) returns false for coordinates with different latitude.
    /// </summary>
    [TestMethod]
    public void EqualsObject_DifferentLatitude_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        object coordinate2 = new Coordinate(46.0, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(object) returns false for coordinates with different longitude.
    /// </summary>
    [TestMethod]
    public void EqualsObject_DifferentLongitude_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        object coordinate2 = new Coordinate(45.0, 91.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(object) returns false when comparing with null.
    /// </summary>
    [TestMethod]
    public void EqualsObject_Null_ReturnsFalse() {
        // Arrange
        var coordinate = new Coordinate(45.0, 90.0);

        // Act
        bool result = coordinate.Equals(null);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(object) returns false when comparing with non-Coordinate object.
    /// </summary>
    [TestMethod]
    public void EqualsObject_DifferentType_ReturnsFalse() {
        // Arrange
        var coordinate = new Coordinate(45.0, 90.0);
        object other = "not a coordinate";

        // Act
        bool result = coordinate.Equals(other);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns true for equal coordinates.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_EqualCoordinates_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns true for default coordinates.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_DefaultCoordinates_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate();
        var coordinate2 = new Coordinate();

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns false when latitude differs.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_DifferentLatitude_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(46.0, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns false when longitude differs.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_DifferentLongitude_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 91.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns false when both latitude and longitude differ.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_DifferentLatitudeAndLongitude_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(46.0, 91.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns true for coordinates at minimum bounds.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_MinimumBounds_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(-90.0, -180.0);
        var coordinate2 = new Coordinate(-90.0, -180.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals(Coordinate) returns true for coordinates at maximum bounds.
    /// </summary>
    [TestMethod]
    public void EqualsCoordinate_MaximumBounds_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(90.0, 180.0);
        var coordinate2 = new Coordinate(90.0, 180.0);

        // Act
        bool result = coordinate1.Equals(coordinate2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns true when coordinates are identical.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_IdenticalCoordinates_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns true when coordinates are within tolerance.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_WithinTolerance_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0005, 90.0005);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns false when latitude difference exceeds tolerance.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_LatitudeDifferenceExceedsTolerance_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.002, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns false when longitude difference exceeds tolerance.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_LongitudeDifferenceExceedsTolerance_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 90.002);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns false when both latitude and longitude differences exceed tolerance.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_BothDifferencesExceedTolerance_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.002, 90.002);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns true when latitude difference is exactly at boundary.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_LatitudeDifferenceAtBoundary_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0009, 90.0);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns true when longitude difference is exactly at boundary.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_LongitudeDifferenceAtBoundary_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 90.0009);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns true when coordinates differ in negative direction within tolerance.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_NegativeDifferenceWithinTolerance_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(44.9995, 89.9995);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals with tolerance returns false when coordinates differ in negative direction exceeding tolerance.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_NegativeDifferenceExceedsTolerance_ReturnsFalse() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(44.998, 89.998);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals with zero tolerance requires difference to be less than zero (never true).
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_ZeroTolerance_RequiresDifferenceLessThanZero() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 90.0);
        var coordinate3 = new Coordinate(45.0000001, 90.0);

        // Act
        bool result1 = coordinate1.Equals(coordinate2, 0.0);
        bool result2 = coordinate1.Equals(coordinate3, 0.0);

        // Assert
        Assert.IsFalse(result1); // Even identical coordinates fail with 0 tolerance since 0 < 0 is false
        Assert.IsFalse(result2);
    }

    /// <summary>
    /// Tests that Equals with tolerance works correctly for negative coordinates.
    /// </summary>
    [TestMethod]
    public void EqualsTolerance_NegativeCoordinates_ReturnsTrue() {
        // Arrange
        var coordinate1 = new Coordinate(-45.0, -90.0);
        var coordinate2 = new Coordinate(-45.0005, -90.0005);

        // Act
        bool result = coordinate1.Equals(coordinate2, 0.001);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for equal coordinates.
    /// </summary>
    [TestMethod]
    public void GetHashCode_EqualCoordinates_ReturnsSameHashCode() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 90.0);

        // Act
        int hashCode1 = coordinate1.GetHashCode();
        int hashCode2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns different values for coordinates with different latitude.
    /// </summary>
    [TestMethod]
    public void GetHashCode_DifferentLatitude_ReturnsDifferentHashCode() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(46.0, 90.0);

        // Act
        int hashCode1 = coordinate1.GetHashCode();
        int hashCode2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns different values for coordinates with different longitude.
    /// </summary>
    [TestMethod]
    public void GetHashCode_DifferentLongitude_ReturnsDifferentHashCode() {
        // Arrange
        var coordinate1 = new Coordinate(45.0, 90.0);
        var coordinate2 = new Coordinate(45.0, 91.0);

        // Act
        int hashCode1 = coordinate1.GetHashCode();
        int hashCode2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for default coordinates.
    /// </summary>
    [TestMethod]
    public void GetHashCode_DefaultCoordinates_ReturnsSameHashCode() {
        // Arrange
        var coordinate1 = new Coordinate();
        var coordinate2 = new Coordinate(0.0, 0.0);

        // Act
        int hashCode1 = coordinate1.GetHashCode();
        int hashCode2 = coordinate2.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns consistent value when called multiple times.
    /// </summary>
    [TestMethod]
    public void GetHashCode_MultipleInvocations_ReturnsConsistentValue() {
        // Arrange
        var coordinate = new Coordinate(45.0, 90.0);

        // Act
        int hashCode1 = coordinate.GetHashCode();
        int hashCode2 = coordinate.GetHashCode();
        int hashCode3 = coordinate.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
        Assert.AreEqual(hashCode2, hashCode3);
    }

    /// <summary>
    /// Tests that ToString returns correct format for positive coordinates.
    /// </summary>
    [TestMethod]
    public void ToString_PositiveCoordinates_ReturnsCorrectFormat() {
        // Arrange
        var coordinate = new Coordinate(45.5, 90.75);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 45.5, Longitude: 90.75 }", result);
    }

    /// <summary>
    /// Tests that ToString returns correct format for negative coordinates.
    /// </summary>
    [TestMethod]
    public void ToString_NegativeCoordinates_ReturnsCorrectFormat() {
        // Arrange
        var coordinate = new Coordinate(-45.5, -90.75);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: -45.5, Longitude: -90.75 }", result);
    }

    /// <summary>
    /// Tests that ToString returns correct format for zero coordinates.
    /// </summary>
    [TestMethod]
    public void ToString_ZeroCoordinates_ReturnsCorrectFormat() {
        // Arrange
        var coordinate = new Coordinate(0.0, 0.0);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 0, Longitude: 0 }", result);
    }

    /// <summary>
    /// Tests that ToString returns correct format for minimum boundary values.
    /// </summary>
    [TestMethod]
    public void ToString_MinimumBoundaryValues_ReturnsCorrectFormat() {
        // Arrange
        var coordinate = new Coordinate(-90.0, -180.0);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: -90, Longitude: -180 }", result);
    }

    /// <summary>
    /// Tests that ToString returns correct format for maximum boundary values.
    /// </summary>
    [TestMethod]
    public void ToString_MaximumBoundaryValues_ReturnsCorrectFormat() {
        // Arrange
        var coordinate = new Coordinate(90.0, 180.0);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.AreEqual("{ Latitude: 90, Longitude: 180 }", result);
    }

    /// <summary>
    /// Tests that ToString uses invariant culture formatting.
    /// </summary>
    [TestMethod]
    public void ToString_InvariantCulture_UsesInvariantFormatting() {
        // Arrange
        var coordinate = new Coordinate(45.123456789, 90.987654321);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.IsTrue(result.Contains("45.123456789", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("90.987654321", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString handles very small decimal values correctly.
    /// </summary>
    [TestMethod]
    public void ToString_SmallDecimalValues_ReturnsCorrectFormat() {
        // Arrange
        var coordinate = new Coordinate(0.0000001, 0.0000001);

        // Act
        string result = coordinate.ToString();

        // Assert
        Assert.IsTrue(result.Contains("Latitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Longitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("1E-07", StringComparison.Ordinal) || result.Contains("0.0000001", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ValidateLatitude does not throw for valid latitude at minimum boundary.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_MinimumBoundary_DoesNotThrow() {
        // Arrange
        double latitude = -90.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(latitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude does not throw for valid latitude at maximum boundary.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_MaximumBoundary_DoesNotThrow() {
        // Arrange
        double latitude = 90.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(latitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude does not throw for valid latitude in middle range.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_ValidMiddleRange_DoesNotThrow() {
        // Arrange
        double latitude = 45.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(latitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude does not throw for zero latitude.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_Zero_DoesNotThrow() {
        // Arrange
        double latitude = 0.0;

        // Act & Assert
        Coordinate.Validator.ValidateLatitude(latitude);
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is less than minimum.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_BelowMinimum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = -90.1;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLatitude(latitude));
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is greater than maximum.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_AboveMaximum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = 90.1;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLatitude(latitude));
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is NaN.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_NaN_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = double.NaN;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLatitude(latitude));
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is positive infinity.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = double.PositiveInfinity;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLatitude(latitude));
    }

    /// <summary>
    /// Tests that ValidateLatitude throws ArgumentOutOfRangeException when latitude is negative infinity.
    /// </summary>
    [TestMethod]
    public void ValidateLatitude_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double latitude = double.NegativeInfinity;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLatitude(latitude));
    }

    /// <summary>
    /// Tests that ValidateLongitude does not throw for valid longitude at minimum boundary.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_MinimumBoundary_DoesNotThrow() {
        // Arrange
        double longitude = -180.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(longitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude does not throw for valid longitude at maximum boundary.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_MaximumBoundary_DoesNotThrow() {
        // Arrange
        double longitude = 180.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(longitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude does not throw for valid longitude in middle range.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_ValidMiddleRange_DoesNotThrow() {
        // Arrange
        double longitude = 90.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(longitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude does not throw for zero longitude.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_Zero_DoesNotThrow() {
        // Arrange
        double longitude = 0.0;

        // Act & Assert
        Coordinate.Validator.ValidateLongitude(longitude);
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is less than minimum.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_BelowMinimum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double longitude = -180.1;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLongitude(longitude));
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is greater than maximum.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_AboveMaximum_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double longitude = 180.1;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLongitude(longitude));
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is NaN.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_NaN_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double longitude = double.NaN;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLongitude(longitude));
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is positive infinity.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double longitude = double.PositiveInfinity;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLongitude(longitude));
    }

    /// <summary>
    /// Tests that ValidateLongitude throws ArgumentOutOfRangeException when longitude is negative infinity.
    /// </summary>
    [TestMethod]
    public void ValidateLongitude_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double longitude = double.NegativeInfinity;

        // Act & Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Coordinate.Validator.ValidateLongitude(longitude));
    }
}
