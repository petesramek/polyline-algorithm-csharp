namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class PolylineDecoderExtensionsTest {
    private readonly PolylineDecoder decoder = new PolylineDecoder();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    [TestMethod]
    [DynamicData(nameof(CoordinateCount), DynamicDataSourceType.Property)]
    public void Decode_String_Returns_Expected_Coordinates(int count) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(count);
        var expected = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude))
            .ToList();

        // Act
        var result = PolylineDecoderExtensions.Decode(decoder, polyline).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount), DynamicDataSourceType.Property)]
    public void Decode_CharArray_Returns_Expected_Coordinates(int count) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(count).ToCharArray();
        var expected = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude))
            .ToList();

        // Act
        var result = PolylineDecoderExtensions.Decode(decoder, polyline).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount), DynamicDataSourceType.Property)]
    public void Decode_Memory_Returns_Expected_Coordinates(int count) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(count).AsMemory();
        var expected = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude))
            .ToList();

        // Act
        var result = PolylineDecoderExtensions.Decode(decoder, polyline).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

}
