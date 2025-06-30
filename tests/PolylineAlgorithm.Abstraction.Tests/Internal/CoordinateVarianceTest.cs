namespace PolylineAlgorithm.Abstraction.Tests.Internal;

using PolylineAlgorithm.Abstraction.Internal;

[TestClass]
public class CoordinateVarianceTests {
    public static IEnumerable<(int Latitude, int Longitude)> Coordinates => [
        (0, 0),
        (-10, -10),
        (10, -10),
        (-10, 10),
        (10, 10)
    ];

    [TestMethod]
    public void Constructor_Sets_Defaults() {
        // Arrange & Act
        CoordinateVariance variance = new();
        // Assert
        Assert.AreEqual(0, variance.Latitude);
        Assert.AreEqual(0, variance.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Coordinates), DynamicDataSourceType.Property)]
    public void Next_Calculates_Correct_Variance(int latitude, int longitude) {
        // Arrange
        CoordinateVariance variance = new();
        var expected = (latitude, longitude);

        // Act
        variance.Next(latitude, longitude);

        // Assert
        Assert.AreEqual(expected.latitude, variance.Latitude);
        Assert.AreEqual(expected.longitude, variance.Longitude);
    }
}
