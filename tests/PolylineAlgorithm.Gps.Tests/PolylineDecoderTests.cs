//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Gps;

/// <summary>
/// Tests for <see cref="PolylineDecoder"/>.
/// </summary>
[TestClass]
public sealed class PolylineDecoderTests {
    /// <summary>
    /// Tests that default constructor creates decoder with default options.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_DefaultConstructor_CreatesDecoderWithDefaultOptions() {
        // Arrange & Act
        PolylineDecoder decoder = new PolylineDecoder();

        // Assert
        Assert.IsNotNull(decoder);
        Assert.IsNotNull(decoder.Options);
    }

    /// <summary>
    /// Tests that default constructor creates decoder with default precision.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_DefaultConstructor_CreatesDecoderWithDefaultPrecision() {
        // Arrange & Act
        PolylineDecoder decoder = new PolylineDecoder();

        // Assert
        Assert.AreEqual(5u, decoder.Options.Precision);
    }

    /// <summary>
    /// Tests that default constructor creates decoder with default stack alloc limit.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_DefaultConstructor_CreatesDecoderWithDefaultStackAllocLimit() {
        // Arrange & Act
        PolylineDecoder decoder = new PolylineDecoder();

        // Assert
        Assert.AreEqual(512, decoder.Options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that parameterized constructor creates decoder with specified options.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_WithOptions_CreatesDecoderWithSpecifiedOptions() {
        // Arrange
        PolylineEncodingOptions options = new PolylineEncodingOptions {
            Precision = 6,
            StackAllocLimit = 1024
        };

        // Act
        PolylineDecoder decoder = new PolylineDecoder(options);

        // Assert
        Assert.IsNotNull(decoder);
        Assert.AreSame(options, decoder.Options);
    }

    /// <summary>
    /// Tests that parameterized constructor preserves custom precision.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_WithCustomPrecision_PreservesCustomPrecision() {
        // Arrange
        PolylineEncodingOptions options = new PolylineEncodingOptions {
            Precision = 7
        };

        // Act
        PolylineDecoder decoder = new PolylineDecoder(options);

        // Assert
        Assert.AreEqual(7u, decoder.Options.Precision);
    }

    /// <summary>
    /// Tests that parameterized constructor preserves custom stack alloc limit.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_WithCustomStackAllocLimit_PreservesCustomStackAllocLimit() {
        // Arrange
        PolylineEncodingOptions options = new PolylineEncodingOptions {
            StackAllocLimit = 2048
        };

        // Act
        PolylineDecoder decoder = new PolylineDecoder(options);

        // Assert
        Assert.AreEqual(2048, decoder.Options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that parameterized constructor throws ArgumentNullException when options is null.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_WithNullOptions_ThrowsArgumentNullException() {
        // Arrange
        PolylineEncodingOptions? options = null;

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(
            () => new PolylineDecoder(options!));
        Assert.AreEqual("options", exception.ParamName);
    }

    /// <summary>
    /// Tests that <see cref="PolylineDecoder.Decode(Polyline)"/> decodes a valid polyline to expected coordinates.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_Decode_ReturnsExpectedCoordinates() {
        // Arrange
        PolylineDecoder decoder = new PolylineDecoder();
        Polyline polyline = Polyline.FromString("_p~iF~ps|U_ulLnnqC");
        Coordinate[] expected =
        [
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95)
        ];

        // Act
        var result = decoder.Decode(polyline).ToArray();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineDecoder.Decode(Polyline, CancellationToken)"/> decodes a valid polyline to expected coordinates.
    /// </summary>
    [TestMethod]

    public void PolylineDecoder_Decode_WithCancellationToken_ReturnsExpectedCoordinates() {
        // Arrange
        PolylineDecoder decoder = new PolylineDecoder();
        Polyline polyline = Polyline.FromString("_p~iF~ps|U_ulLnnqC");
        Coordinate[] expected =
        [
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95)
        ];
        CancellationToken token = CancellationToken.None;

        // Act
        var result = decoder.Decode(polyline, token).ToArray();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }
}
