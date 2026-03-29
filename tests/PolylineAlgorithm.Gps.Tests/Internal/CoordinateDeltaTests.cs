//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Gps.Internal;

/// <summary>
/// Tests for <see cref="CoordinateDelta"/>.
/// </summary>
[TestClass]
public sealed class CoordinateDeltaTests {
    /// <summary>
    /// Tests that default constructor initializes delta values to zero.
    /// </summary>
    [TestMethod]

    public void Constructor_Default_Initializes_Latitude_And_Longitude_To_Zero() {
        // Act
        CoordinateDelta delta = new CoordinateDelta();

        // Assert
        Assert.AreEqual(0, delta.Latitude);
        Assert.AreEqual(0, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with positive values calculates correct delta from zero.
    /// </summary>
    [TestMethod]

    public void Next_With_Positive_Values_Calculates_Delta_From_Zero() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(10, 20);

        // Assert
        Assert.AreEqual(10, delta.Latitude);
        Assert.AreEqual(20, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with negative values calculates correct delta from zero.
    /// </summary>
    [TestMethod]

    public void Next_With_Negative_Values_Calculates_Delta_From_Zero() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(-50, -100);

        // Assert
        Assert.AreEqual(-50, delta.Latitude);
        Assert.AreEqual(-100, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with zero values keeps delta at zero.
    /// </summary>
    [TestMethod]

    public void Next_With_Zero_Values_Keeps_Delta_At_Zero() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(0, 0);

        // Assert
        Assert.AreEqual(0, delta.Latitude);
        Assert.AreEqual(0, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next called multiple times calculates delta from previous value.
    /// </summary>
    [TestMethod]

    public void Next_Called_Multiple_Times_Calculates_Delta_From_Previous_Value() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(10, 20);
        delta.Next(15, 30);

        // Assert
        Assert.AreEqual(5, delta.Latitude);
        Assert.AreEqual(10, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with decreasing values calculates negative delta.
    /// </summary>
    [TestMethod]

    public void Next_With_Decreasing_Values_Calculates_Negative_Delta() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(100, 200);
        delta.Next(50, 150);

        // Assert
        Assert.AreEqual(-50, delta.Latitude);
        Assert.AreEqual(-50, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with same values as previous calculates zero delta.
    /// </summary>
    [TestMethod]

    public void Next_With_Same_Values_As_Previous_Calculates_Zero_Delta() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(42, 84);
        delta.Next(42, 84);

        // Assert
        Assert.AreEqual(0, delta.Latitude);
        Assert.AreEqual(0, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with maximum integer values calculates correct delta.
    /// </summary>
    [TestMethod]

    public void Next_With_Maximum_Integer_Values_Calculates_Correct_Delta() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(int.MaxValue, int.MaxValue);

        // Assert
        Assert.AreEqual(int.MaxValue, delta.Latitude);
        Assert.AreEqual(int.MaxValue, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with minimum integer values calculates correct delta.
    /// </summary>
    [TestMethod]

    public void Next_With_Minimum_Integer_Values_Calculates_Correct_Delta() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(int.MinValue, int.MinValue);

        // Assert
        Assert.AreEqual(int.MinValue, delta.Latitude);
        Assert.AreEqual(int.MinValue, delta.Longitude);
    }

    /// <summary>
    /// Tests that Next with mixed positive and negative values calculates correct delta.
    /// </summary>
    [TestMethod]

    public void Next_With_Mixed_Positive_And_Negative_Values_Calculates_Correct_Delta() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        delta.Next(-50, 100);
        delta.Next(25, -75);

        // Assert
        Assert.AreEqual(75, delta.Latitude);
        Assert.AreEqual(-175, delta.Longitude);
    }

    /// <summary>
    /// Tests that ToString with default constructor returns formatted string with zeros.
    /// </summary>
    [TestMethod]

    public void ToString_With_Default_Constructor_Returns_Formatted_String_With_Zeros() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("Coordinate", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Delta", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Latitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Longitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('0'));
    }

    /// <summary>
    /// Tests that ToString after Next returns formatted string with correct values.
    /// </summary>
    [TestMethod]

    public void ToString_After_Next_Returns_Formatted_String_With_Correct_Values() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();
        delta.Next(42, 84);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("42", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("84", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString after multiple Next calls returns formatted string with latest values.
    /// </summary>
    [TestMethod]

    public void ToString_After_Multiple_Next_Calls_Returns_Formatted_String_With_Latest_Values() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();
        delta.Next(10, 20);
        delta.Next(30, 50);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("30", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("50", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("20", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString with negative values returns formatted string with negative signs.
    /// </summary>
    [TestMethod]

    public void ToString_With_Negative_Values_Returns_Formatted_String_With_Negative_Signs() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();
        delta.Next(-100, -200);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("-100", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("-200", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString with maximum integer values returns formatted string.
    /// </summary>
    [TestMethod]

    public void ToString_With_Maximum_Integer_Values_Returns_Formatted_String() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();
        delta.Next(int.MaxValue, int.MaxValue);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString with minimum integer values returns formatted string.
    /// </summary>
    [TestMethod]

    public void ToString_With_Minimum_Integer_Values_Returns_Formatted_String() {
        // Arrange
        CoordinateDelta delta = new CoordinateDelta();
        delta.Next(int.MinValue, int.MinValue);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(int.MinValue.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal));
    }
}