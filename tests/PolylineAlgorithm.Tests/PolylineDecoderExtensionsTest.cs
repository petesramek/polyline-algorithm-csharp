//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class PolylineDecoderExtensionsTest {
    private readonly PolylineDecoder _decoder = new();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100]];

    [TestMethod]
    public void Decode_Null_Decoder_Null_String_Throws_ArgumentNullException() {
        // Arrange
#pragma warning disable IDE0305 // Simplify collection initialization
        static void Decode() => PolylineDecoderExtensions.Decode(null!, string.Empty).ToList();
#pragma warning restore IDE0305 // Simplify collection initialization

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Decode);

        // Assert
        Assert.AreEqual("decoder", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Value cannot be null.", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Decode_Null_Decoder_Null_CharArray_Throws_ArgumentNullException() {
        // Arrange
#pragma warning disable IDE0305 // Simplify collection initialization
        static void Decode() => PolylineDecoderExtensions.Decode(null!, []).ToList();
#pragma warning restore IDE0305 // Simplify collection initialization

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Decode);

        // Assert
        Assert.AreEqual("decoder", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Value cannot be null.", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Decode_Null_Decoder_Empty_Memory_Throws_ArgumentNullException() {
        // Arrange
#pragma warning disable IDE0305 // Simplify collection initialization
        static void Decode() => PolylineDecoderExtensions.Decode(null!, Memory<char>.Empty).ToList();
#pragma warning restore IDE0305 // Simplify collection initialization

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Decode);

        // Assert
        Assert.AreEqual("decoder", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Value cannot be null.", StringComparison.Ordinal));
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount), DynamicDataSourceType.Property)]
    public void Decode_String_Returns_Expected_Coordinates(int count) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(count);
        var expected = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude))
            .ToList();

        // Act
        var result = PolylineDecoderExtensions.Decode(_decoder, polyline).ToList();

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
        var result = PolylineDecoderExtensions.Decode(_decoder, polyline).ToList();

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
        var result = PolylineDecoderExtensions.Decode(_decoder, polyline).ToList();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

}
