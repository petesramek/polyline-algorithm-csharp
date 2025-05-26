//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Validation;

using PolylineAlgorithm.Validation;

/// <summary>
/// Tests for the <see cref="CoordinateRange"/> type.
/// </summary>
[TestClass]
public class CoordinateRangeTest {
    /// <summary>
    /// Provides test data for the <see cref="Constructor_Valid_Parameters_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> Constructor_Valid_Parameters => [
        [-90, 90],
        [-180, 180],
    ];

    /// <summary>
    /// Tests the parameterless constructor of the <see cref="CoordinateRange"/> class.
    /// </summary>
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        double min = double.MinValue;
        double max = double.MaxValue;
        double value = 0d;

        // Act
        CoordinateRange range = new();

        // Assert
        Assert.AreEqual(min, range.Min);
        Assert.AreEqual(max, range.Max);
        Assert.IsTrue(range.IsInRange(min));
        Assert.IsTrue(range.IsInRange(value));
        Assert.IsTrue(range.IsInRange(max));
    }

    /// <summary>
    /// Tests the <see cref="CoordinateRange"/> constructor with valid parameters.
    /// </summary>
    /// <param name="min">The minimum value for the range.</param>
    /// <param name="max">The maximum value for the range.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
    public void Constructor_Valid_Parameters_Ok(double min, double max) {
        // Arrange
        double belowMax = max - 1;
        double belowMin = min - 1;
        double equalMax = max;
        double equalMin = min;
        double aboveMax = max + 1;
        double aboveMin = min + 1;

        // Act
        CoordinateRange range = new(min, max);

        // Assert
        Assert.IsTrue(range.IsInRange(equalMin));
        Assert.IsTrue(range.IsInRange(aboveMin));
        Assert.IsTrue(range.IsInRange(equalMax));
        Assert.IsTrue(range.IsInRange(belowMax));

        Assert.IsFalse(range.IsInRange(belowMin));
        Assert.IsFalse(range.IsInRange(aboveMax));
    }

    /// <summary>
    /// Tests the <see cref="CoordinateRange"/> constructor with invalid minimum parameter.
    /// </summary>
    [TestMethod]
    public void Constructor_Invalid_Min_Parameter_ArgumentOutOfRangeException() {
        // Arrange
        double min = 0;
        double max = 0;

        // Act
        static void New(double min, double max) {
            CoordinateRange range = new(min, max);
        }

        // Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => New(min, max));
    }

    /// <summary>
    /// Tests the <see cref="CoordinateRange.Equals(CoordinateRange)"/> method with equal ranges.
    /// </summary>
    /// <param name="min">The minimum value for the range.</param>
    /// <param name="max">The maximum value for the range.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
    public void Equals_CoordinateRange_True(double min, double max) {
        // Arrange
        CoordinateRange @this = new(min, max);
        CoordinateRange other = new(min, max);

        // Act
        bool result = @this.Equals(other);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests the <see cref="CoordinateRange.Equals(CoordinateRange)"/> method with unequal ranges.
    /// </summary>
    /// <param name="min">The minimum value for the range.</param>
    /// <param name="max">The maximum value for the range.</param>
    [TestMethod]
    [DynamicData(nameof(Constructor_Valid_Parameters))]
    public void Equals_CoordinateRange_False(double min, double max) {
        // Arrange
        CoordinateRange @this = new(min, max);
        CoordinateRange other = new(0, 1);

        // Act
        bool result = @this.Equals(other);

        // Assert
        Assert.IsFalse(result);
    }
}