//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Validation;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class CoordinateRangeTest {
    public static IEnumerable<object[]> Consructor_Valid_Parameters => [
        [ -90, 90 ],
        [ -180, 180 ],
    ];

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

    [TestMethod]
    public void Constructor_Invalid_Min_Parameter_Ok() {
        // Arrange
        double min = 0;
        double max = 0;


        // Act
        void New(double min, double max) {
            CoordinateRange range = new(min, max);

        }
        

        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => New(min, max));
    }

    [TestMethod]
    [DynamicData(nameof(Consructor_Valid_Parameters))]
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
        Assert.IsTrue(range.IsInRange(aboveMin));
        Assert.IsTrue(range.IsInRange(equalMin));
        Assert.IsTrue(range.IsInRange(equalMax));
        Assert.IsTrue(range.IsInRange(belowMax));

        Assert.IsFalse(range.IsInRange(belowMin));
        Assert.IsFalse(range.IsInRange(aboveMax));
    }
}
