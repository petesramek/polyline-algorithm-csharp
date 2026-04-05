//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;

/// <summary>
/// Tests for <see cref="CoordinateDelta"/>.
/// </summary>
[TestClass]
public sealed class CoordinateDeltaTests {
    /// <summary>
    /// Tests that a newly constructed instance reports all-zero deltas.
    /// </summary>
    [TestMethod]
    public void Constructor_Default_Initializes_Deltas_To_Zero() {
        // Act
        CoordinateDelta delta = new(2);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(2, deltas.Length);
        Assert.AreEqual(0, deltas[0]);
        Assert.AreEqual(0, deltas[1]);
    }

    /// <summary>
    /// Tests that a single call to Next computes the correct delta from the initial zero state for 2 values.
    /// </summary>
    [TestMethod]
    [DataRow(10, 20, 10, 20)]
    [DataRow(-50, -100, -50, -100)]
    [DataRow(0, 0, 0, 0)]
    [DataRow(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    [DataRow(int.MinValue, int.MinValue, int.MinValue, int.MinValue)]
    public void Next_Single_Call_From_Zero_Computes_Expected_Delta_For_Two_Values(int v0, int v1, int expectedD0, int expectedD1) {
        // Arrange
        CoordinateDelta delta = new(2);

        // Act
        delta.Next([v0, v1]);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(expectedD0, deltas[0]);
        Assert.AreEqual(expectedD1, deltas[1]);
    }

    /// <summary>
    /// Tests that two consecutive calls to Next compute the delta relative to the previous value for 2 values.
    /// </summary>
    [TestMethod]
    [DataRow(10, 20, 15, 30, 5, 10)]
    [DataRow(100, 200, 50, 150, -50, -50)]
    [DataRow(42, 84, 42, 84, 0, 0)]
    [DataRow(-50, 100, 25, -75, 75, -175)]
    public void Next_Sequential_Calls_Compute_Delta_From_Previous_Value_For_Two_Values(
        int first0, int first1,
        int second0, int second1,
        int expectedD0, int expectedD1) {
        // Arrange
        CoordinateDelta delta = new(2);
        delta.Next([first0, first1]);

        // Act
        delta.Next([second0, second1]);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(expectedD0, deltas[0]);
        Assert.AreEqual(expectedD1, deltas[1]);
    }

    /// <summary>
    /// Tests that Next works correctly for a single value dimension.
    /// </summary>
    [TestMethod]
    public void Next_Single_Dimension_Computes_Correct_Delta() {
        // Arrange
        CoordinateDelta delta = new(1);
        delta.Next([100]);

        // Act
        delta.Next([150]);

        // Assert
        Assert.AreEqual(50, delta.Deltas[0]);
    }

    /// <summary>
    /// Tests that Next works correctly for three value dimensions.
    /// </summary>
    [TestMethod]
    public void Next_Three_Dimensions_Computes_Correct_Deltas() {
        // Arrange
        CoordinateDelta delta = new(3);
        delta.Next([10, 20, 30]);

        // Act
        delta.Next([15, 25, 25]);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(5, deltas[0]);
        Assert.AreEqual(5, deltas[1]);
        Assert.AreEqual(-5, deltas[2]);
    }

    /// <summary>
    /// Tests that ToString on a default instance returns a non-null string containing structural keywords.
    /// </summary>
    [TestMethod]
    public void ToString_With_Default_Constructor_Returns_Formatted_String_With_Zeros() {
        // Arrange
        CoordinateDelta delta = new(2);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("Values", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Deltas", StringComparison.Ordinal));
        Assert.Contains('0', result);
    }

    /// <summary>
    /// Tests that ToString reflects the delta values computed by Next.
    /// </summary>
    [TestMethod]
    [DataRow(42, 84)]
    [DataRow(-100, -200)]
    public void ToString_After_Next_Contains_Expected_Values(int v0, int v1) {
        // Arrange
        CoordinateDelta delta = new(2);
        delta.Next([v0, v1]);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(v0.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal));
        Assert.IsTrue(result.Contains(v1.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that ToString after multiple Next calls reflects the most recent delta values.
    /// </summary>
    [TestMethod]
    public void ToString_After_Multiple_Next_Calls_Returns_Formatted_String_With_Latest_Values() {
        // Arrange
        CoordinateDelta delta = new(2);
        delta.Next([10, 20]);
        delta.Next([30, 50]);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("30", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("50", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("20", StringComparison.Ordinal));
    }
}

