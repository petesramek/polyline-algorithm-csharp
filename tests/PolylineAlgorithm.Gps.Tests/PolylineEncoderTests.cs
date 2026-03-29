//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Gps;

/// <summary>
/// Tests for <see cref="PolylineEncoder"/>.
/// </summary>
[TestClass]
public sealed class PolylineEncoderTests {
    /// <summary>
    /// Tests that default constructor creates encoder with default options.
    /// </summary>
    [TestMethod]

    public void PolylineEncoder_DefaultConstructor_CreatesEncoderWithDefaultOptions() {
        // Arrange & Act
        PolylineEncoder encoder = new PolylineEncoder();

        // Assert
        Assert.IsNotNull(encoder);
        Assert.IsNotNull(encoder.Options);
    }

    /// <summary>
    /// Tests that default constructor creates encoder with default precision.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_DefaultConstructor_CreatesEncoderWithDefaultPrecision() {
        // Arrange & Act
        PolylineEncoder encoder = new PolylineEncoder();

        // Assert
        Assert.AreEqual(5u, encoder.Options.Precision);
    }

    /// <summary>
    /// Tests that default constructor creates encoder with default stack alloc limit.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_DefaultConstructor_CreatesEncoderWithDefaultStackAllocLimit() {
        // Arrange & Act
        PolylineEncoder encoder = new PolylineEncoder();

        // Assert
        Assert.AreEqual(512, encoder.Options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that parameterized constructor creates encoder with specified options.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_WithOptions_CreatesEncoderWithSpecifiedOptions() {
        // Arrange
        PolylineEncodingOptions options = new PolylineEncodingOptions {
            Precision = 6,
            StackAllocLimit = 1024
        };

        // Act
        PolylineEncoder encoder = new PolylineEncoder(options);

        // Assert
        Assert.IsNotNull(encoder);
        Assert.AreSame(options, encoder.Options);
    }

    /// <summary>
    /// Tests that parameterized constructor preserves custom precision.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_WithCustomPrecision_PreservesCustomPrecision() {
        // Arrange
        PolylineEncodingOptions options = new PolylineEncodingOptions {
            Precision = 7
        };

        // Act
        PolylineEncoder encoder = new PolylineEncoder(options);

        // Assert
        Assert.AreEqual(7u, encoder.Options.Precision);
    }

    /// <summary>
    /// Tests that parameterized constructor preserves custom stack alloc limit.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_WithCustomStackAllocLimit_PreservesCustomStackAllocLimit() {
        // Arrange
        PolylineEncodingOptions options = new PolylineEncodingOptions {
            StackAllocLimit = 2048
        };

        // Act
        PolylineEncoder encoder = new PolylineEncoder(options);

        // Assert
        Assert.AreEqual(2048, encoder.Options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that parameterized constructor throws ArgumentNullException when options is null.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_WithNullOptions_ThrowsArgumentNullException() {
        // Arrange
        PolylineEncodingOptions? options = null;

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(
            () => new PolylineEncoder(options!));
        Assert.AreEqual("options", exception.ParamName);
    }

    /// <summary>
    /// Tests that Encode encodes a collection of coordinates into a polyline string.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_Encode_EncodesCoordinatesToPolyline() {
        // Arrange
        var encoder = new PolylineEncoder();
        var coordinates = new[]
        {
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95),
            new Coordinate(43.252, -126.453)
        };

        // Act
        Polyline polyline = encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual("_p~iF~ps|U_ulLnnqC_mqNvxq`@", polyline.ToString());
    }

    /// <summary>
    /// Tests that Encode throws ArgumentNullException when coordinates is null.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_Encode_NullCoordinates_ThrowsArgumentException() {
        // Arrange
        var encoder = new PolylineEncoder();
        Coordinate[]? coordinates = null;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => encoder.Encode(coordinates));
    }

    /// <summary>
    /// Tests that Encode throws ArgumentException when coordinates is empty.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_Encode_EmptyCoordinates_ThrowsArgumentException() {
        // Arrange
        var encoder = new PolylineEncoder();
        var coordinates = Array.Empty<Coordinate>();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => encoder.Encode(coordinates));
    }

    /// <summary>
    /// Tests that Encode encodes a single coordinate correctly.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_Encode_SingleCoordinate_EncodesCorrectly() {
        // Arrange
        var encoder = new PolylineEncoder();
        var coordinates = new[] { new Coordinate(38.5, -120.2) };

        // Act
        Polyline polyline = encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual("_p~iF~ps|U", polyline.ToString());
    }

    /// <summary>
    /// Tests that Encode encodes two coordinates correctly.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_Encode_TwoCoordinates_EncodesCorrectly() {
        // Arrange
        var encoder = new PolylineEncoder();
        var coordinates = new[]
        {
            new Coordinate(38.5, -120.2),
            new Coordinate(40.7, -120.95)
        };

        // Act
        Polyline polyline = encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual("_p~iF~ps|U_ulLnnqC", polyline.ToString());
    }

    /// <summary>
    /// Tests that Encode works with negative and zero coordinates.
    /// </summary>
    [TestMethod]
    public void PolylineEncoder_Encode_NegativeAndZeroCoordinates_EncodesCorrectly() {
        // Arrange
        var encoder = new PolylineEncoder();
        var coordinates = new[]
        {
            new Coordinate(0, 0),
            new Coordinate(-45.12345, 179.99999)
        };

        // Act
        Polyline polyline = encoder.Encode(coordinates);

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(polyline.ToString()));
    }
}