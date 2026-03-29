//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Extensions;

using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Tests.Properties;
using System;
using System.Collections.Generic;

/// <summary>
/// Tests for <see cref="PolylineEncoderExtensions"/>.
/// </summary>
[TestClass]
public sealed class PolylineEncoderExtensionsTests
{
    /// <summary>
    /// Tests that Encode with List parameter throws ArgumentNullException when encoder is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ListWithNullEncoder_ThrowsArgumentNullException()
    {
        // Arrange
        IPolylineEncoder<Coordinate, Polyline>? encoder = null;
        List<Coordinate> coordinates = [new Coordinate(38.5, -120.2)];

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineEncoderExtensions.Encode(encoder!, coordinates));
        Assert.AreEqual("encoder", exception.ParamName);
    }

    /// <summary>
    /// Tests that Encode with List parameter throws ArgumentNullException when coordinates is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ListWithNullCoordinates_ThrowsArgumentNullException()
    {
        // Arrange
        PolylineEncoder encoder = new();
        List<Coordinate>? coordinates = null;

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineEncoderExtensions.Encode(encoder, coordinates!));
        Assert.AreEqual("coordinates", exception.ParamName);
    }

    /// <summary>
    /// Tests that Encode with List parameter calls encoder with coordinates from list.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ListWithValidEncoder_CallsEncoderWithCoordinatesFromList()
    {
        // Arrange
        List<Coordinate> coordinates =
        [
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95),
            new Coordinate(43.252, -126.453)
        ];
        Polyline expectedPolyline = Polyline.FromString("_p~iF~ps|U_ulLnnqC_mqNvxq`@");
        PolylineEncoder encoder = new();

        // Act
        Polyline result = PolylineEncoderExtensions.Encode(encoder, coordinates);

        // Assert
        Assert.AreEqual(expectedPolyline, result);
    }

    /// <summary>
    /// Tests that Encode with List parameter handles empty list.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ListWithEmptyList_CallsEncoderWithEmptySpan()
    {
        // Arrange
        List<Coordinate> coordinates = [];
        PolylineEncoder encoder = new();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoderExtensions.Encode(encoder, coordinates));
    }

    /// <summary>
    /// Tests that Encode with array parameter throws ArgumentNullException when encoder is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ArrayWithNullEncoder_ThrowsArgumentNullException()
    {
        // Arrange
        IPolylineEncoder<Coordinate, Polyline>? encoder = null;
        Coordinate[] coordinates = [new Coordinate(38.5, -120.2)];

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineEncoderExtensions.Encode(encoder!, coordinates));
        Assert.AreEqual("encoder", exception.ParamName);
    }

    /// <summary>
    /// Tests that Encode with array parameter throws ArgumentNullException when coordinates is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ArrayWithNullCoordinates_ThrowsArgumentNullException()
    {
        // Arrange
        PolylineEncoder encoder = new();
        Coordinate[]? coordinates = null;

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineEncoderExtensions.Encode(encoder, coordinates!));
        Assert.AreEqual("coordinates", exception.ParamName);
    }

    /// <summary>
    /// Tests that Encode with array parameter calls encoder with coordinates from array.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ArrayWithValidEncoder_CallsEncoderWithCoordinatesFromArray()
    {
        // Arrange
        Coordinate[] coordinates =
        [
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95),
            new Coordinate(43.252, -126.453),
        ];
        Polyline expectedPolyline = Polyline.FromString("_p~iF~ps|U_ulLnnqC_mqNvxq`@");
        PolylineEncoder encoder = new();

        // Act
        Polyline result = PolylineEncoderExtensions.Encode(encoder, coordinates);

        // Assert
        Assert.AreEqual(expectedPolyline, result);
    }

    /// <summary>
    /// Tests that Encode with array parameter handles empty array.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Encode_ArrayWithEmptyArray_CallsEncoderWithEmptySpan()
    {
        // Arrange
        Coordinate[] coordinates = [];
        PolylineEncoder encoder = new();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoderExtensions.Encode(encoder, coordinates));
    }
}
