//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Extensions;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System;
using System.Collections.Generic;

/// <summary>
/// Tests for <see cref="PolylineDecoderExtensions"/>.
/// </summary>
[TestClass]
public sealed class PolylineDecoderExtensionsTests {
    private sealed class TestStringDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) => (latitude, longitude);
    }

    private sealed class TestMemoryDecoder : AbstractPolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)> {
        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in ReadOnlyMemory<char> polyline) => polyline;
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) => (latitude, longitude);
    }

    // ----- Decode(char[]) for IPolylineDecoder<string, TValue> -----

    /// <summary>
    /// Tests that Decode with a null decoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_CharArray_NullDecoder_ThrowsArgumentNullException() {
        // Arrange
        IPolylineDecoder<string, (double, double)>? decoder = null;
        char[] polyline = StaticValueProvider.Valid.GetPolyline().ToCharArray();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder!.Decode(polyline).ToList());
        Assert.AreEqual("decoder", ex.ParamName);
    }

    /// <summary>
    /// Tests that Decode with a null char array throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_CharArray_NullPolyline_ThrowsArgumentNullException() {
        // Arrange
        TestStringDecoder decoder = new();
        char[]? polyline = null;

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder.Decode(polyline!).ToList());
        Assert.AreEqual("polyline", ex.ParamName);
    }

    /// <summary>
    /// Tests that Decode with a valid char array returns expected coordinates.
    /// </summary>
    [TestMethod]
    public void Decode_CharArray_ValidPolyline_ReturnsExpectedCoordinates() {
        // Arrange
        TestStringDecoder decoder = new();
        char[] polyline = StaticValueProvider.Valid.GetPolyline().ToCharArray();
        (double Latitude, double Longitude)[] expected = [.. StaticValueProvider.Valid.GetCoordinates()];

        // Act
        (double Latitude, double Longitude)[] result = [.. decoder.Decode(polyline)];

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        for (int i = 0; i < expected.Length; i++) {
            Assert.AreEqual(expected[i].Latitude, result[i].Latitude, 1e-5);
            Assert.AreEqual(expected[i].Longitude, result[i].Longitude, 1e-5);
        }
    }

    // ----- Decode(ReadOnlyMemory<char>) for IPolylineDecoder<string, TValue> -----

    /// <summary>
    /// Tests that Decode with a null decoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_Memory_NullDecoder_ThrowsArgumentNullException() {
        // Arrange
        IPolylineDecoder<string, (double, double)>? decoder = null;
        ReadOnlyMemory<char> polyline = StaticValueProvider.Valid.GetPolyline().AsMemory();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder!.Decode(polyline).ToList());
        Assert.AreEqual("decoder", ex.ParamName);
    }

    /// <summary>
    /// Tests that Decode with a valid memory returns expected coordinates.
    /// </summary>
    [TestMethod]
    public void Decode_Memory_ValidPolyline_ReturnsExpectedCoordinates() {
        // Arrange
        TestStringDecoder decoder = new();
        ReadOnlyMemory<char> polyline = StaticValueProvider.Valid.GetPolyline().AsMemory();
        (double Latitude, double Longitude)[] expected = [.. StaticValueProvider.Valid.GetCoordinates()];

        // Act
        (double Latitude, double Longitude)[] result = [.. decoder.Decode(polyline)];

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        for (int i = 0; i < expected.Length; i++) {
            Assert.AreEqual(expected[i].Latitude, result[i].Latitude, 1e-5);
            Assert.AreEqual(expected[i].Longitude, result[i].Longitude, 1e-5);
        }
    }

    // ----- Decode(string) for IPolylineDecoder<ReadOnlyMemory<char>, TValue> -----

    /// <summary>
    /// Tests that Decode with a null decoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_String_NullDecoder_ThrowsArgumentNullException() {
        // Arrange
        IPolylineDecoder<ReadOnlyMemory<char>, (double, double)>? decoder = null;
        string polyline = StaticValueProvider.Valid.GetPolyline();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => PolylineDecoderExtensions.Decode<(double, double)>(decoder!, polyline).ToList());
        Assert.AreEqual("decoder", ex.ParamName);
    }

    /// <summary>
    /// Tests that Decode with a null string throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_String_NullPolyline_ThrowsArgumentNullException() {
        // Arrange
        TestMemoryDecoder decoder = new();
        string? polyline = null;

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder.Decode(polyline!).ToList());
        Assert.AreEqual("polyline", ex.ParamName);
    }

    /// <summary>
    /// Tests that Decode with a valid string returns expected coordinates.
    /// </summary>
    [TestMethod]
    public void Decode_String_ValidPolyline_ReturnsExpectedCoordinates() {
        // Arrange
        TestMemoryDecoder decoder = new();
        string polyline = StaticValueProvider.Valid.GetPolyline();
        (double Latitude, double Longitude)[] expected = [.. StaticValueProvider.Valid.GetCoordinates()];

        // Act
        (double Latitude, double Longitude)[] result = [.. decoder.Decode(polyline)];

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        for (int i = 0; i < expected.Length; i++) {
            Assert.AreEqual(expected[i].Latitude, result[i].Latitude, 1e-5);
            Assert.AreEqual(expected[i].Longitude, result[i].Longitude, 1e-5);
        }
    }
}
