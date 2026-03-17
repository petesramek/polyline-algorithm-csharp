//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests for the <see cref="Pow10"/> type.
/// </summary>
[TestClass]
public class Pow10Tests {
    /// <summary>
    /// Provides test data for cached precision values (0-9).
    /// </summary>
    public static IEnumerable<object[]> CachedPrecisionValues => [
        [0u, 1u],
        [1u, 10u],
        [2u, 100u],
        [3u, 1000u],
        [4u, 10000u],
        [5u, 100000u],
        [6u, 1000000u],
        [7u, 10000000u],
        [8u, 100000000u],
        [9u, 1000000000u],
    ];

    /// <summary>
    /// Tests that <see cref="Pow10.GetFactor"/> returns correct cached values for all cached precision levels.
    /// </summary>
    /// <param name="precision">The precision level.</param>
    /// <param name="expected">The expected power of 10.</param>
    [TestMethod]
    [DynamicData(nameof(CachedPrecisionValues))]
    public void GetFactor_CachedPrecision_WithCache_ReturnsCorrectValue(uint precision, uint expected) {
        // Arrange
        Pow10.UseCache = true;

        // Act
        uint result = Pow10.GetFactor(precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="Pow10.GetFactor"/> computes correct values for all cached precision levels when cache is disabled.
    /// </summary>
    /// <param name="precision">The precision level.</param>
    /// <param name="expected">The expected power of 10.</param>
    [TestMethod]
    [DynamicData(nameof(CachedPrecisionValues))]
    public void GetFactor_CachedPrecision_WithoutCache_ReturnsCorrectValue(uint precision, uint expected) {
        // Arrange
        Pow10.UseCache = false;

        // Act
        uint result = Pow10.GetFactor(precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="Pow10.GetFactor"/> throws <see cref="OverflowException"/> for precision 10 with cache enabled.
    /// </summary>
    [TestMethod]
    public void GetFactor_Precision10_WithCache_ThrowsOverflowException() {
        // Arrange
        Pow10.UseCache = true;
        const uint precision = 10;

        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(precision));
    }

    /// <summary>
    /// Tests that <see cref="Pow10.GetFactor"/> throws <see cref="OverflowException"/> for precision 10 with cache disabled.
    /// </summary>
    [TestMethod]
    public void GetFactor_Precision10_WithoutCache_ThrowsOverflowException() {
        // Arrange
        Pow10.UseCache = false;
        const uint precision = 10;

        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(precision));
    }

    /// <summary>
    /// Tests that <see cref="Pow10.GetFactor"/> throws <see cref="OverflowException"/> for precision 11 with cache enabled.
    /// </summary>
    [TestMethod]
    public void GetFactor_Precision11_WithCache_ThrowsOverflowException() {
        // Arrange
        Pow10.UseCache = true;
        const uint precision = 11;

        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(precision));
    }

    /// <summary>
    /// Tests that <see cref="Pow10.GetFactor"/> throws <see cref="OverflowException"/> for precision 11 with cache disabled.
    /// </summary>
    [TestMethod]
    public void GetFactor_Precision11_WithoutCache_ThrowsOverflowException() {
        // Arrange
        Pow10.UseCache = false;
        const uint precision = 11;

        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(precision));
    }
}
