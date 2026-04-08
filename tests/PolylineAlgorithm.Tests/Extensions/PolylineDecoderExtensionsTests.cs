//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Extensions;

using PolylineAlgorithm;
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
    private static PolylineDecoder<string, (double Latitude, double Longitude)> CreateStringDecoder() {
        PolylineFormatter<(double Latitude, double Longitude), string> formatter =
            FormatterBuilder<(double Latitude, double Longitude), string>.Create()
                .AddValue("lat", static c => c.Latitude)
                .AddValue("lon", static c => c.Longitude)
                .WithCreate(static v => (v[0], v[1]))
                .ForPolyline(static m => new string(m.Span), static s => s.AsMemory())
                .Build();

        return new PolylineDecoder<string, (double Latitude, double Longitude)>(
            new PolylineOptions<(double Latitude, double Longitude), string>(formatter));
    }

    private static PolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)> CreateMemoryDecoder() {
        PolylineFormatter<(double Latitude, double Longitude), ReadOnlyMemory<char>> formatter =
            FormatterBuilder<(double Latitude, double Longitude), ReadOnlyMemory<char>>.Create()
                .AddValue("lat", static c => c.Latitude)
                .AddValue("lon", static c => c.Longitude)
                .WithCreate(static v => (v[0], v[1]))
                .ForPolyline(static m => m, static m => m)
                .Build();

        return new PolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)>(
            new PolylineOptions<(double Latitude, double Longitude), ReadOnlyMemory<char>>(formatter));
    }

    // ----- Decode(char[]) for IPolylineDecoder<string, TValue> -----

    /// <summary>
    /// Tests that Decode with a null decoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Char_Array_Null_Decoder_Throws_ArgumentNullException() {
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
    public void Decode_With_Char_Array_Null_Polyline_Throws_ArgumentNullException() {
        // Arrange
        var decoder = CreateStringDecoder();
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
    public void Decode_With_Char_Array_Valid_Polyline_Returns_Expected_Coordinates() {
        // Arrange
        var decoder = CreateStringDecoder();
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
    public void Decode_With_Memory_Null_Decoder_Throws_ArgumentNullException() {
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
    public void Decode_With_Memory_Valid_Polyline_Returns_Expected_Coordinates() {
        // Arrange
        var decoder = CreateStringDecoder();
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
    public void Decode_With_String_Null_Decoder_Throws_ArgumentNullException() {
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
    public void Decode_With_String_Null_Polyline_Throws_ArgumentNullException() {
        // Arrange
        var decoder = CreateMemoryDecoder();
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
    public void Decode_With_String_Valid_Polyline_Returns_Expected_Coordinates() {
        // Arrange
        var decoder = CreateMemoryDecoder();
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
