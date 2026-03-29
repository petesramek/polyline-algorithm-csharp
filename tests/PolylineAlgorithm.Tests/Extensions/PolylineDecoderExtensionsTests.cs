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
using System.Linq;

/// <summary>
/// Tests for <see cref="PolylineDecoderExtensions"/>.
/// </summary>
[TestClass]
public sealed class PolylineDecoderExtensionsTests
{
    private sealed class TestPolylineDecoder : IPolylineDecoder<Polyline, Coordinate>
    {
        public Polyline? LastPolyline { get; private set; }
        private readonly IEnumerable<Coordinate> _coordinatesToReturn;

        public TestPolylineDecoder(IEnumerable<Coordinate> coordinatesToReturn)
        {
            _coordinatesToReturn = coordinatesToReturn;
        }

        public IEnumerable<Coordinate> Decode(Polyline polyline)
        {
            LastPolyline = polyline;
            return _coordinatesToReturn;
        }
    }

    /// <summary>
    /// Tests that Decode with string parameter throws ArgumentNullException when decoder is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Decode_StringWithNullDecoder_ThrowsArgumentNullException()
    {
        // Arrange
        IPolylineDecoder<Polyline, Coordinate>? decoder = null;
        string polyline = "test";

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineDecoderExtensions.Decode(decoder!, polyline));
        Assert.AreEqual("decoder", exception.ParamName);
    }

    /// <summary>
    /// Tests that Decode with string parameter calls decoder with polyline from string.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Decode_StringWithValidDecoder_CallsDecoderWithPolylineFromString()
    {
        // Arrange
        string polylineString = "_p~iF~ps|U_ulLnnqC_mqNvxq`@";
        Coordinate[] expectedCoordinates =
        [
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95),
            new Coordinate(43.252, -126.453),
        ];
        var decoder = new TestPolylineDecoder(expectedCoordinates);

        // Act
        IEnumerable<Coordinate> result = PolylineDecoderExtensions.Decode(decoder, polylineString);

        // Assert
        Assert.IsNotNull(result);
        Coordinate[] coordinates = result.ToArray();
        Assert.HasCount(3, coordinates);
        Assert.AreEqual(decoder.LastPolyline, Polyline.FromString(polylineString));
    }

    /// <summary>
    /// Tests that Decode with char array parameter throws ArgumentNullException when decoder is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Decode_CharArrayWithNullDecoder_ThrowsArgumentNullException()
    {
        // Arrange
        IPolylineDecoder<Polyline, Coordinate>? decoder = null;
        char[] polyline = ['t', 'e', 's', 't'];

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineDecoderExtensions.Decode(decoder!, polyline));
        Assert.AreEqual("decoder", exception.ParamName);
    }

    /// <summary>
    /// Tests that Decode with char array parameter calls decoder with polyline from char array.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Decode_CharArrayWithValidDecoder_CallsDecoderWithPolylineFromCharArray()
    {
        // Arrange
        char[] polylineChars = ['_', 'p', '~', 'i', 'F', '~', 'p', 's', '|', 'U'];
        Coordinate[] expectedCoordinates =
        [
            new Coordinate(38.5, -120.2)
        ];
        var decoder = new TestPolylineDecoder(expectedCoordinates);

        // Act
        IEnumerable<Coordinate> result = PolylineDecoderExtensions.Decode(decoder, polylineChars);

        // Assert
        Assert.IsNotNull(result);
        Coordinate[] coordinates = result.ToArray();
        Assert.AreEqual(1, coordinates.Length);
        Assert.AreEqual(decoder.LastPolyline, Polyline.FromCharArray(polylineChars));
    }

    /// <summary>
    /// Tests that Decode with ReadOnlyMemory parameter throws ArgumentNullException when decoder is null.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Decode_ReadOnlyMemoryWithNullDecoder_ThrowsArgumentNullException()
    {
        // Arrange
        IPolylineDecoder<Polyline, Coordinate>? decoder = null;
        ReadOnlyMemory<char> polyline = "test".AsMemory();

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => PolylineDecoderExtensions.Decode(decoder!, polyline));
        Assert.AreEqual("decoder", exception.ParamName);
    }

    /// <summary>
    /// Tests that Decode with ReadOnlyMemory parameter calls decoder with polyline from memory.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Decode_ReadOnlyMemoryWithValidDecoder_CallsDecoderWithPolylineFromMemory()
    {
        // Arrange
        ReadOnlyMemory<char> polylineMemory = "_p~iF~ps|U".AsMemory();
        Coordinate[] expectedCoordinates =
        [
            new Coordinate(38.5, -120.2)
        ];
        var decoder = new TestPolylineDecoder(expectedCoordinates);

        // Act
        IEnumerable<Coordinate> result = PolylineDecoderExtensions.Decode(decoder, polylineMemory);

        // Assert
        Assert.IsNotNull(result);
        Coordinate[] coordinates = result.ToArray();
        Assert.AreEqual(1, coordinates.Length);
        Assert.AreEqual(decoder.LastPolyline, Polyline.FromMemory(polylineMemory));
    }
}
