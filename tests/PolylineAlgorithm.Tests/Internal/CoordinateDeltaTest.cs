//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using System.Globalization;

[TestClass]
public class CoordinateDeltaTest {
    public static IEnumerable<(int Latitude, int Longitude)> Coordinates => [
        (0, 0),
        (-10, -10),
        (10, -10),
        (-10, 10),
        (10, 10),
    ];

    public static IEnumerable<((int Latitude, int Longitude) Initial, (int Latitude, int Longitude) Next, (int Latitude, int Longitude) Result)> Variances => [
        ((10, 10), (-20, -20), (-30, -30)),
        ((-10, -10), (20, 20), (30, 30)),
        ((0, 10), (10, -10), (10, -20)),
        ((0, -10), (10, 10), (10, 20)),
        ((10, 0), (10, -10), (0, -10)),
        ((-10, 0), (10, 10), (20, 10)),
        ((10, -10), (-10, 10), (-20, 20)),
        ((-10, 10), (10, 10), (20, 0)),
        ((10, 10), (10, 0), (0, -10)),
        ((-10, -10), (-10, 0), (0, 10)),
        ((10, 10), (0, 0), (-10, -10)),
        ((-10, -10), (0, 0), (10, 10)),
        ((10, -10), (0, 0), (-10, 10)),
        ((-10, 10), (0, 0), (10, -10)),
        ((0, 10), (0, 0), (0, -10)),
        ((0, -10), (0, 0), (0, 10)),
        ((10, 0), (0, 0), (-10, 0)),
        ((-10, 0), (0, 0), (10, 0)),
    ];

    [TestMethod]
    public void Constructor_Sets_Defaults() {
        // Arrange & Act
        CoordinateDelta variance = new();
        // Assert
        Assert.AreEqual(0, variance.Latitude);
        Assert.AreEqual(0, variance.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Coordinates))]
    public void Next_Calculates_Correct_Variance_From_Default_Variance(int latitude, int longitude) {
        // Arrange
        CoordinateDelta variance = new();
        var expected = (latitude, longitude);

        // Act
        variance.Next(latitude, longitude);

        // Assert
        Assert.AreEqual(expected.latitude, variance.Latitude);
        Assert.AreEqual(expected.longitude, variance.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Variances))]
    public void Next_Calculates_Correct_Variance_From_Previous_Variance((int Latitude, int Longitude) initial, (int Latitude, int Longitude) next, (int Latitude, int Longitude) expected) {
        // Arrange
        CoordinateDelta variance = new();
        variance.Next(initial.Latitude, initial.Longitude);

        // Act
        variance.Next(next.Latitude, next.Longitude);

        // Assert
        Assert.AreEqual(expected.Latitude, variance.Latitude);
        Assert.AreEqual(expected.Longitude, variance.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Coordinates))]
    public void ToString_Returns_Value_Containing_Variance(int latitude, int longitude) {
        // Arrange
        CoordinateDelta variance = new();
        variance.Next(latitude, longitude);

        // Act
        string result = variance.ToString();

        // Assert
        Assert.Contains($"Latitude: {latitude.ToString(CultureInfo.InvariantCulture)}", result, StringComparison.Ordinal);
        Assert.Contains($"Longitude: {longitude.ToString(CultureInfo.InvariantCulture)}", result, StringComparison.Ordinal);
    }
}
