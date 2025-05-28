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
    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000], [10_000], [100_000], [1_000_000]];

    /// <summary>
    /// The instance of the <see cref="PolylineEncoder"/> used for testing.
    /// </summary>
    public PolylineEncoder Encoder = new();

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
        void EncodeEmptyCoordinates() {
            Encoder.Encode(empty);
        }

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
        IEnumerable<Coordinate> valid = RandomValueProvider.GetCoordinates(count);
        Polyline expected = RandomValueProvider.GetPolyline(count);

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
        IEnumerable<Coordinate> valid = StaticValueProvider.GetCoordinates();
        Polyline expected = StaticValueProvider.GetPolyline();

        // Act
        var result = Encoder.Encode(valid);

        // Assert
        Assert.AreEqual(expected.IsEmpty, result.IsEmpty);
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }
}