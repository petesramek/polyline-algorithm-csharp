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

    public static IEnumerable<((int Latitude, int Longitude) Initial, (int Latitude, int Longitude) Next, (int Latitude, int Longitude) Result)> Deltas => [
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
        CoordinateDelta delta = new();
        // Assert
        Assert.AreEqual(0, delta.Latitude);
        Assert.AreEqual(0, delta.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Coordinates))]
    public void Next_Calculates_Correct_Delta_From_Default_Delta(int latitude, int longitude) {
        // Arrange
        CoordinateDelta delta = new();
        var expected = (latitude, longitude);

        // Act
        delta.Next(latitude, longitude);

        // Assert
        Assert.AreEqual(expected.latitude, delta.Latitude);
        Assert.AreEqual(expected.longitude, delta.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Deltas))]
    public void Next_Calculates_Correct_Delta_From_Previous_Delta((int Latitude, int Longitude) initial, (int Latitude, int Longitude) next, (int Latitude, int Longitude) expected) {
        // Arrange
        CoordinateDelta delta = new();
        delta.Next(initial.Latitude, initial.Longitude);

        // Act
        delta.Next(next.Latitude, next.Longitude);

        // Assert
        Assert.AreEqual(expected.Latitude, delta.Latitude);
        Assert.AreEqual(expected.Longitude, delta.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Coordinates))]
    public void ToString_Returns_Value_Containing_Delta(int latitude, int longitude) {
        // Arrange
        CoordinateDelta delta = new();
        delta.Next(latitude, longitude);

        // Act
        string result = delta.ToString();

        // Assert
        Assert.Contains($"Latitude: {latitude.ToString(CultureInfo.InvariantCulture)}", result, StringComparison.Ordinal);
        Assert.Contains($"Longitude: {longitude.ToString(CultureInfo.InvariantCulture)}", result, StringComparison.Ordinal);
    }
}
