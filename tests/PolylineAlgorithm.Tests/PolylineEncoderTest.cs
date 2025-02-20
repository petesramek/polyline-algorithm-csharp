//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for the <see cref="PolylineEncoder"/> type.
/// </summary>
[TestClass]
public class PolylineEncoderTest {
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
        IEnumerable<Coordinate> empty = Values.Coordinates.Empty;

        // Act
        void EncodeEmptyCoordinates() {
            Encoder.Encode(empty);
        }

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => EncodeEmptyCoordinates());
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_InvalidInput_ThrowsException() {
        // Arrange
        IEnumerable<Coordinate> invalid = Values.Coordinates.Invalid;

        // Act
        void EncodeInvalidCoordinates() {
            Encoder.Encode(invalid);
        }

        // Assert
        Assert.ThrowsExactly<InvalidCoordinateException>(() => EncodeInvalidCoordinates());
    }

    /// <summary>
    /// Tests the <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result is that the encoded polyline matches <see cref="Values.Polyline.Valid"/>.</remarks>
    [TestMethod]
    public void Encode_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> valid = Values.Coordinates.Valid;

        // Act
        var result = Encoder.Encode(valid);

        // Assert
        Assert.AreEqual(Values.Polyline.Valid, result.ToString());
    }
}





