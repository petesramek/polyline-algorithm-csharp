//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

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
    public CoordinateEncoder Encoder = new();

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with a null input, expecting an <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_NullInput_ThrowsException() {
        // Arrange
        IEnumerable<Coordinate> @null = null!;

        // Act
        void EncodeNullCoordinates() {
            Encoder.Encode(@null);
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
        IEnumerable<Coordinate> empty = [];

        // Act
        void EncodeEmptyCoordinates() => Encoder.Encode(empty);

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => EncodeEmptyCoordinates());
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result is that the encoded polyline matches <see cref="Values.Polyline.Valid"/>.</remarks>
    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Random_Value_Encode_ValidInput_Ok(int count) {
        // Arrange
        IEnumerable<Coordinate> valid = RandomValueProvider.GetCoordinates(count).Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline expected = Polyline.FromString(RandomValueProvider.GetPolyline(count));

        // Act
        var result = Encoder.Encode(valid);

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
    public void Static_Value_Encode_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> valid = StaticValueProvider.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline expected = Polyline.FromString(StaticValueProvider.GetPolyline());

        // Act
        var result = Encoder.Encode(valid);

        // Assert
        Assert.AreEqual(expected.Length == 0, result.IsEmpty);
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    /// <summary>
    /// Tests the round-trip encoding and decoding of coordinates.
    /// </summary>
    [TestMethod]
    public void EncodeDecode_RoundTrip_Ok() {
        var coordinates = new List<Coordinate>
        {
            new(10, 20),
            new(-10, -20),
            new(0, 0)
        };

        var encoder = new CoordinateEncoder();
        var decoder = new CoordinateDecoder();

        var polyline = encoder.Encode(coordinates);
        var decoded = decoder.Decode(polyline).ToList();

        Assert.AreEqual(coordinates.Count, decoded.Count);

        for (int i = 0; i < coordinates.Count; i++) {
            Assert.AreEqual(coordinates[i].Latitude, decoded[i].Latitude/*, 1e-6*/);
            Assert.AreEqual(coordinates[i].Longitude, decoded[i].Longitude/*, 1e-6*/);
        }
    }
}