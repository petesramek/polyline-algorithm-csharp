//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineEncoder"/> type.
/// </summary>
[TestClass]
public class PolylineEncoderTest
{
    /// <summary>
    /// Subject under test.
    /// </summary>
    public PolylineEncoder Encoder = new();

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. <see langword="null" /> is passed as parameter.
    /// Expected result is <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_NullInput_ThrowsException()
    {
        // Arrange
        IEnumerable<Coordinate> @null = null!;

        // Act
        void EncodeNullCoordinates()
        {
            Encoder.Encode(@null);
        }

        // Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => EncodeNullCoordinates());
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. Empty enumeration is passed as parameter.
    /// Expected result is <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_EmptyInput_ThrowsException()
    {
        // Arrange
        IEnumerable<Coordinate> empty = Values.Coordinates.Empty;

        // Act
        void EncodeEmptyCoordinates()
        {
            Encoder.Encode(empty);
        }

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => EncodeEmptyCoordinates());
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. Enumeration containing only invalid values is passed as parameter.
    /// Expected result is <see cref="InvalidCoordinateException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_InvalidInput_ThrowsException()
    {
        // Arrange
        IEnumerable<Coordinate> invalid = Values.Coordinates.Invalid;

        // Act
        void EncodeEmptyCoordinates()
        {
            Encoder.Encode(invalid);
        }

        // Assert
        Assert.ThrowsExactly<InvalidCoordinateException>(() => EncodeEmptyCoordinates());
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. Enumeration containing only valid values is passed as parameter.
    /// Expected result is result and <see cref="Values.Polyline.Valid"/> are equal.
    /// </summary>
    [TestMethod]
    public void Encode_ValidInput_Ok()
    {
        // Arrange
        IEnumerable<Coordinate> valid = Values.Coordinates.Valid;

        // Act
        var result = Encoder.Encode(valid);

        // Assert
        Assert.AreEqual(Values.Polyline.Valid, result.ToString());
    }
}
