//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Properties;

/// <summary>
/// Tests for <see cref="Pow10"/>.
/// </summary>
[TestClass]
public sealed class Pow10Tests {
    /// <summary>
    /// Tests that GetFactor with precision 0 returns 1.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Zero_Returns_One() {
        // Act
        uint result = Pow10.GetFactor(0);

        // Assert
        Assert.AreEqual(1u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 1 returns 10.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_One_Returns_Ten() {
        // Act
        uint result = Pow10.GetFactor(1);

        // Assert
        Assert.AreEqual(10u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 2 returns 100.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Two_Returns_One_Hundred() {
        // Act
        uint result = Pow10.GetFactor(2);

        // Assert
        Assert.AreEqual(100u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 3 returns 1000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Three_Returns_One_Thousand() {
        // Act
        uint result = Pow10.GetFactor(3);

        // Assert
        Assert.AreEqual(1000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 4 returns 10000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Four_Returns_Ten_Thousand() {
        // Act
        uint result = Pow10.GetFactor(4);

        // Assert
        Assert.AreEqual(10000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 5 returns 100000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Five_Returns_One_Hundred_Thousand() {
        // Act
        uint result = Pow10.GetFactor(5);

        // Assert
        Assert.AreEqual(100000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 6 returns 1000000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Six_Returns_One_Million() {
        // Act
        uint result = Pow10.GetFactor(6);

        // Assert
        Assert.AreEqual(1000000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 7 returns 10000000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Seven_Returns_Ten_Million() {
        // Act
        uint result = Pow10.GetFactor(7);

        // Assert
        Assert.AreEqual(10000000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 8 returns 100000000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Eight_Returns_One_Hundred_Million() {
        // Act
        uint result = Pow10.GetFactor(8);

        // Assert
        Assert.AreEqual(100000000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision 9 returns 1000000000.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Nine_Returns_One_Billion() {
        // Act
        uint result = Pow10.GetFactor(9);

        // Assert
        Assert.AreEqual(1000000000u, result);
    }

    /// <summary>
    /// Tests that GetFactor with precision causing overflow throws OverflowException.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Precision_Causing_Overflow_Throws_OverflowException() {
        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(15));
    }

    /// <summary>
    /// Tests that GetFactor with large precision causing overflow throws OverflowException.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Large_Precision_Causing_Overflow_Throws_OverflowException() {
        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(20));
    }

    /// <summary>
    /// Tests that GetFactor with maximum uint precision throws OverflowException.
    /// </summary>
    [TestMethod]

    public void GetFactor_With_Maximum_Uint_Precision_Throws_OverflowException() {
        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => Pow10.GetFactor(uint.MaxValue));
    }
}
