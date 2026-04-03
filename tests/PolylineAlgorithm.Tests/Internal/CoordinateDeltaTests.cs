//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using System.Globalization;

/// <summary>
/// Tests for <see cref="CoordinateDelta"/>.
/// </summary>
[TestClass]
public sealed class CoordinateDeltaTests {
    /// <summary>
    /// Tests that the default constructor initializes delta values to zero.
    /// </summary>
    [TestMethod]
    public void Constructor_Default_Initializes_Latitude_And_Longitude_To_Zero() {
        // Act
        CoordinateDelta delta = new();

        // Assert
        Assert.AreEqual(0, delta.Latitude);
        Assert.AreEqual(0, delta.Longitude);
    }

    /// <summary>
    /// Tests that a single call to Next computes the correct delta from the initial zero state.
    /// </summary>
    [TestMethod]
    [DataRow(10, 20, 10, 20)]
    [DataRow(-50, -100, -50, -100)]
    [DataRow(0, 0, 0, 0)]
    [DataRow(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    [DataRow(int.MinValue, int.MinValue, int.MinValue, int.MinValue)]
    public void Next_Single_Call_From_Zero_Computes_Expected_Delta(int latitude, int longitude, int expectedLatitude, int expectedLongitude) {
        // Arrange
        CoordinateDelta delta = new();

        // Act
        delta.Next(latitude, longitude);

        // Assert
        Assert.AreEqual(expectedLatitude, delta.Latitude);
        Assert.AreEqual(expectedLongitude, delta.Longitude);
    }

    /// <summary>
    /// Tests that two consecutive calls to Next compute the delta relative to the previous value.
    /// </summary>
    [TestMethod]
    [DataRow(10, 20, 15, 30, 5, 10)]
    [DataRow(100, 200, 50, 150, -50, -50)]
    [DataRow(42, 84, 42, 84, 0, 0)]
    [DataRow(-50, 100, 25, -75, 75, -175)]
    public void Next_Sequential_Calls_Compute_Delta_From_Previous_Value(
        int firstLatitude, int firstLongitude,
        int secondLatitude, int secondLongitude,
        int expectedLatitude, int expectedLongitude) {
        // Arrange
        CoordinateDelta delta = new();
        delta.Next(firstLatitude, firstLongitude);

        // Act
        delta.Next(secondLatitude, secondLongitude);

        // Assert
        Assert.AreEqual(expectedLatitude, delta.Latitude);
        Assert.AreEqual(expectedLongitude, delta.Longitude);
    }

    /// <summary>
    /// Tests that ToString on a default instance returns a string containing expected structural keywords and a zero value.
    /// </summary>
    [TestMethod]
    public void ToString_With_Default_Constructor_Returns_Formatted_String_With_Zeros() {
        // Arrange
        CoordinateDelta delta = new();

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("Coordinate", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Delta", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Latitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Longitude", StringComparison.Ordinal));
        Assert.Contains('0', result);
    }

    /// <summary>
    /// Tests that ToString reflects the delta values computed by Next.
    /// </summary>
    [TestMethod]
    [DataRow(42, 84)]
    [DataRow(-100, -200)]
    [DataRow(int.MaxValue, int.MaxValue)]
    [DataRow(int.MinValue, int.MinValue)]
    public void ToString_After_Next_Contains_Expected_Values(int latitude, int longitude) {
        // Arrange
        CoordinateDelta delta = new();
        delta.Next(latitude, longitude);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(latitude.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal));
        Assert.IsTrue(result.Contains(longitude.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString after multiple Next calls reflects the most recent delta values.
    /// </summary>
    [TestMethod]
    public void ToString_After_Multiple_Next_Calls_Returns_Formatted_String_With_Latest_Values() {
        // Arrange
        CoordinateDelta delta = new();
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

}
