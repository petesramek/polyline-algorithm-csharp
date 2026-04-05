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
    /// Tests that the constructor with count 2 initializes delta values to zero.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Count_Two_Initializes_Deltas_To_Zero() {
        // Act
        CoordinateDelta delta = new(2);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(2, deltas.Length);
        Assert.AreEqual(0, deltas[0]);
        Assert.AreEqual(0, deltas[1]);
    }

    /// <summary>
    /// Tests that the constructor with count 3 initializes delta values to zero.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Count_Three_Initializes_Deltas_To_Zero() {
        // Act
        CoordinateDelta delta = new(3);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(3, deltas.Length);
        for (int i = 0; i < deltas.Length; i++) {
            Assert.AreEqual(0, deltas[i]);
        }
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
    public void Next_Single_Call_From_Zero_Computes_Expected_Delta(int first, int second, int expectedFirst, int expectedSecond) {
        // Arrange
        CoordinateDelta delta = new(2);

        // Act
        delta.Next([first, second]);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(expectedFirst, deltas[0]);
        Assert.AreEqual(expectedSecond, deltas[1]);
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
        int firstA, int firstB,
        int secondA, int secondB,
        int expectedA, int expectedB) {
        // Arrange
        CoordinateDelta delta = new(2);
        delta.Next([firstA, firstB]);

        // Act
        delta.Next([secondA, secondB]);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(expectedA, deltas[0]);
        Assert.AreEqual(expectedB, deltas[1]);
    }

    /// <summary>
    /// Tests that Next works correctly for N-value items (N > 2).
    /// </summary>
    [TestMethod]
    public void Next_With_Three_Values_Computes_Expected_Deltas() {
        // Arrange
        CoordinateDelta delta = new(3);

        // Act
        delta.Next([10, 20, 30]);
        delta.Next([15, 25, 35]);

        // Assert
        ReadOnlySpan<int> deltas = delta.Deltas;
        Assert.AreEqual(5, deltas[0]);
        Assert.AreEqual(5, deltas[1]);
        Assert.AreEqual(5, deltas[2]);
    }

    /// <summary>
    /// Tests that ToString on a new instance returns a string containing expected structural keywords and zero values.
    /// </summary>
    [TestMethod]
    public void ToString_With_New_Instance_Returns_Formatted_String_With_Zeros() {
        // Arrange
        CoordinateDelta delta = new(2);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("Coordinate", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("Delta", StringComparison.Ordinal));
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
    public void ToString_After_Next_Contains_Expected_Values(int first, int second) {
        // Arrange
        CoordinateDelta delta = new(2);
        delta.Next([first, second]);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(first.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal));
        Assert.IsTrue(result.Contains(second.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal));
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
