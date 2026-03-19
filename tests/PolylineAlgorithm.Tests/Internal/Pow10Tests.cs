//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm.Internal;

/// <summary>
/// Tests for the <see cref="Pow10"/> type.
/// </summary>
[TestClass]
public class Pow10Tests {
    /// <summary>
    /// Tests that GetFactor returns 1 when precision is 0.
    /// </summary>
    [TestMethod]
    public void GetFactor_PrecisionZero_ReturnsOne() {
        // Arrange
        uint precision = 0;

        // Act
        uint result = Pow10.GetFactor(precision);

        // Assert
        Assert.AreEqual(1u, result);
    }

    /// <summary>
    /// Tests that GetFactor returns 10 when precision is 1.
    /// </summary>
    [TestMethod]
    public void GetFactor_PrecisionOne_ReturnsTen() {
        // Arrange
        uint precision = 1;

        // Act
        uint result = Pow10.GetFactor(precision);

        // Assert
        Assert.AreEqual(10u, result);
    }

    /// <summary>
    /// Tests that GetFactor returns correct cached values for precision 0 through 9.
    /// </summary>
    [TestMethod]
    public void GetFactor_CachedValues_ReturnsCorrectPowerOfTen() {
        // Arrange & Act & Assert
        Assert.AreEqual(1u, Pow10.GetFactor(0));
        Assert.AreEqual(10u, Pow10.GetFactor(1));
        Assert.AreEqual(100u, Pow10.GetFactor(2));
        Assert.AreEqual(1000u, Pow10.GetFactor(3));
        Assert.AreEqual(10000u, Pow10.GetFactor(4));
        Assert.AreEqual(100000u, Pow10.GetFactor(5));
        Assert.AreEqual(1000000u, Pow10.GetFactor(6));
        Assert.AreEqual(10000000u, Pow10.GetFactor(7));
        Assert.AreEqual(100000000u, Pow10.GetFactor(8));
        Assert.AreEqual(1000000000u, Pow10.GetFactor(9));
    }

    /// <summary>
    /// Tests that GetFactor throws OverflowException for precision 10 as it exceeds uint.MaxValue.
    /// </summary>
    [TestMethod]
    public void GetFactor_PrecisionTen_ThrowsOverflowException() {
        // Arrange
        uint precision = 10;

        // Act & Assert
        _ = Assert.Throws<OverflowException>(() => Pow10.GetFactor(precision));
    }

    /// <summary>
    /// Tests that GetFactor throws OverflowException when result exceeds uint.MaxValue.
    /// </summary>
    [TestMethod]
    public void GetFactor_PrecisionCausesOverflow_ThrowsOverflowException() {
        // Arrange
        uint precision = 11;

        // Act & Assert
        _ = Assert.Throws<OverflowException>(() => Pow10.GetFactor(precision));
    }

    /// <summary>
    /// Tests that GetFactor throws OverflowException for very large precision values.
    /// </summary>
    [TestMethod]
    public void GetFactor_VeryLargePrecision_ThrowsOverflowException() {
        // Arrange
        uint precision = 100;

        // Act & Assert
        _ = Assert.Throws<OverflowException>(() => Pow10.GetFactor(precision));
    }

    /// <summary>
    /// Tests that GetFactor throws OverflowException for maximum uint precision value.
    /// </summary>
    [TestMethod]
    public void GetFactor_MaxUIntPrecision_ThrowsOverflowException() {
        // Arrange
        uint precision = uint.MaxValue;

        // Act & Assert
        _ = Assert.Throws<OverflowException>(() => Pow10.GetFactor(precision));
    }
}
