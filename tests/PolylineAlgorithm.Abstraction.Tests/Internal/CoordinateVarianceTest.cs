namespace PolylineAlgorithm.Abstraction.Tests.Internal;

using PolylineAlgorithm.Abstraction.Internal;

[TestClass]
public class CoordinateVarianceTests {
    public static IEnumerable<(int Latitude, int Longitude)> Coordinates => [
        (0, 0),
        (10, 20),
        (-10, -20),
        (100, 200),
        (-100, -200),
        (123456789, 987654321),
        (-123456789, -987654321)
    ];

    [TestMethod]
    public void Constructor_Ok() {
        // Arrange & Act
        CoordinateVariance variance = new();
        // Assert
        Assert.AreEqual(0, variance.Latitude);
        Assert.AreEqual(0, variance.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(Coordinates), DynamicDataSourceType.Property)]
    public void Next_Ok(int latitude, int longitude) {
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
