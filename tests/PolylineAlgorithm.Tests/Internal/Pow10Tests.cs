//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;

/// <summary>
/// Tests for <see cref="Pow10"/>.
/// </summary>
[TestClass]
public sealed class Pow10Tests {
    /// <summary>
    /// Tests that GetFactor returns the correct power-of-ten factor for the given precision.
    /// </summary>
    [TestMethod]
    [DataRow(0u, 1u)]
    [DataRow(1u, 10u)]
    [DataRow(2u, 100u)]
    [DataRow(3u, 1000u)]
    [DataRow(4u, 10000u)]
    [DataRow(5u, 100000u)]
    [DataRow(6u, 1000000u)]
    [DataRow(7u, 10000000u)]
    [DataRow(8u, 100000000u)]
    [DataRow(9u, 1000000000u)]
    public void GetFactor_With_Valid_Precision_Returns_Expected_Value(uint precision, uint expected) {
        // Act
        uint result = Pow10.GetFactor(precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that GetFactor throws <see cref="OverflowException"/> when precision causes overflow.
    /// </summary>
    [TestMethod]
    [DataRow(10u)]
    [DataRow(15u)]
    [DataRow(20u)]
    [DataRow(uint.MaxValue)]
    public void GetFactor_With_Overflowing_Precision_Throws_OverflowException(uint precision) {
        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(precision));
    }
}
