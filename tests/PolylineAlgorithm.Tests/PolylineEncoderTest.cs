//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;

/// <summary>
/// Defines tests for the <see cref="PolylineEncoder"/> type.
/// </summary>
[TestClass]
public class PolylineEncoderTest {
    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    /// <summary>
    /// The instance of the <see cref="PolylineEncoder"/> used for testing.
    /// </summary>
    private readonly PolylineEncoder _encoder = new();

    public void Constructor_Options_Ok() {
        // Arrange
        var options = new PolylineEncodingOptions();

        // Act
        var encoder = new PolylineEncoder(options);

        // Assert
        Assert.IsNotNull(encoder);
        Assert.AreEqual(options, encoder.Options);
    }

    public void Constructor_NullOptions_Throws_ArgumentNullException() {
        // Arrange
        PolylineEncodingOptions options = null!;

        // Act
        PolylineEncoder New() => new(options);

        // Assert
        Assert.ThrowsExactly<ArgumentNullException>(New);
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with a null input, expecting an <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_NullInput_ThrowsException() {
        // Arrange
        List<Coordinate> @null = null!;

        // Act
        void EncodeNullCoordinates() {
            _encoder.Encode(@null);
        }

        // Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => EncodeNullCoordinates());
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with an empty input, expecting an <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_EmptyInput_ThrowsException() {
        // Arrange
        List<Coordinate> empty = [];

        // Act
        void EncodeEmptyCoordinates() => _encoder.Encode(empty);

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => EncodeEmptyCoordinates());
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result is that the encoded polyline matches <see cref="Values.Polyline.Valid"/>.</remarks>
    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Encode_RandomValue_ValidInput_Ok(int count) {
        // Arrange
        List<Coordinate> valid = [.. RandomValueProvider.GetCoordinates(count).Select(c => new Coordinate(c.Latitude, c.Longitude))];
        Polyline expected = Polyline.FromString(RandomValueProvider.GetPolyline(count));

        // Act
        var result = _encoder.Encode(valid);

        // Assert
        Assert.AreEqual(expected.IsEmpty, result.IsEmpty);
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result is that the encoded polyline matches <see cref="Values.Polyline.Valid"/>.</remarks>
    [TestMethod]
    public void Encode_StaticValue_ValidInput_Ok() {
        // Arrange
        List<Coordinate> valid = [.. StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude))];
        Polyline expected = Polyline.FromString(StaticValueProvider.Valid.GetPolyline());

        // Act
        var result = _encoder.Encode(valid);

        // Assert
        Assert.AreEqual(expected.Length == 0, result.IsEmpty);
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    /// <summary>
    /// Tests the round-trip encoding and decoding of coordinates.
    /// </summary>
    [TestMethod]
    public void Encode_Decode_RoundTrip_Ok() {
        var coordinates = new List<Coordinate>
        {
            new(10, 20),
            new(-10, -20),
            new(0, 0),
        };

        var encoder = new PolylineEncoder();
        var decoder = new PolylineDecoder();

        var polyline = encoder.Encode(coordinates);
        var decoded = decoder.Decode(polyline).ToList();

        Assert.HasCount(coordinates.Count, decoded);

        for (int i = 0; i < coordinates.Count; i++) {
            Assert.AreEqual(coordinates[i].Latitude, decoded[i].Latitude/*, 1e-6*/);
            Assert.AreEqual(coordinates[i].Longitude, decoded[i].Longitude/*, 1e-6*/);
        }
    }
}