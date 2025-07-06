namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class PolylineEncoderExtensionsTest {
    private readonly PolylineEncoder Encoder = new PolylineEncoder();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    [TestMethod]
    public void Encode_Null_Encoder_Empty_List_Throws_ArgumentNullException() {
        // Arrange
        void Encode() => PolylineEncoderExtensions.Encode(null!, new List<Coordinate>());

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Encode);

        // Assert
        Assert.AreEqual("encoder", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Value cannot be null.", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Encode_Null_Encoder_Null_CharArray_Throws_ArgumentNullException() {
        // Arrange
        void Encode() => PolylineEncoderExtensions.Encode(null!, new Coordinate[0]);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Encode);

        // Assert
        Assert.AreEqual("encoder", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Value cannot be null.", StringComparison.Ordinal));
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount), DynamicDataSourceType.Property)]
    public void Encode_List_Returns_Expected_Coordinates(int count) {
        // Arrange
        var coordinates = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude))
            .ToList();
        var expected = RandomValueProvider.GetPolyline(count);

        // Act
        var result = PolylineEncoderExtensions.Encode(Encoder, coordinates);

        // Assert
        Assert.AreEqual(expected, result.ToString());
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount), DynamicDataSourceType.Property)]
    public void Encode_Array_Returns_Expected_Coordinates(int count) {
        // Arrange
        var coordinates = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude))
            .ToArray();
        var expected = RandomValueProvider.GetPolyline(count);

        // Act
        var result = PolylineEncoderExtensions.Encode(Encoder, coordinates);

        // Assert
        Assert.AreEqual(expected, result.ToString());
    }

}
