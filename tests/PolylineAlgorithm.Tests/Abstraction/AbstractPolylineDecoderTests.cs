//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Abstraction;

using PolylineAlgorithm.Utility;
using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Tests for <see cref="PolylineDecoder{TPolyline, TCoordinate}"/>.
/// </summary>
[TestClass]
public sealed class AbstractPolylineDecoderTests {
    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static readonly Func<ReadOnlyMemory<char>, string> _write = m => new string(m.Span);
    private static readonly Func<string, ReadOnlyMemory<char>> _read = s => s.AsMemory();

    private static PolylineDecoder<string, (double Lat, double Lon)> CreateDecoder() {
        PolylineFormatter<(double Lat, double Lon), string> formatter =
            FormatterBuilder<(double Lat, double Lon), string>.Create()
                .AddValue("lat", c => c.Lat)
                .AddValue("lon", c => c.Lon)
                .WithCreate(static v => (v[0], v[1]))
                .ForPolyline(_write, _read)
                .Build();

        return new PolylineDecoder<string, (double Lat, double Lon)>(
            new PolylineOptions<(double Lat, double Lon), string>(formatter));
    }

    // ------------------------------------------------------------------
    // Constructor
    // ------------------------------------------------------------------

    /// <summary>Tests that a null options argument throws <see cref="ArgumentNullException"/>.</summary>
    [TestMethod]
    public void Constructor_With_Null_Options_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => new PolylineDecoder<string, (double, double)>(null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    // ------------------------------------------------------------------
    // Decode — argument validation
    // ------------------------------------------------------------------

    /// <summary>Tests that a null polyline throws <see cref="ArgumentNullException"/>.</summary>
    [TestMethod]
    public void Decode_With_Null_Polyline_Throws_ArgumentNullException() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder.Decode(null!).ToList());
        Assert.AreEqual("polyline", ex.ParamName);
    }

    /// <summary>Tests that an empty polyline throws <see cref="InvalidPolylineException"/>.</summary>
    [TestMethod]
    public void Decode_With_Empty_Polyline_Throws_InvalidPolylineException() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(
            () => decoder.Decode(string.Empty).ToList());
    }

    /// <summary>
    /// Tests that a polyline containing a character outside the valid range throws
    /// <see cref="InvalidPolylineException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Invalid_Character_Polyline_Throws_InvalidPolylineException() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();

        // '!' (33) is below the allowed minimum '?' (63)
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(
            () => decoder.Decode("!").ToList());
    }

    /// <summary>
    /// Tests that a polyline that contains only enough data for one of the required columns
    /// (i.e., a partial item) throws <see cref="InvalidPolylineException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Truncated_Polyline_Throws_InvalidPolylineException() {
        // Arrange — "?" encodes a single delta-0 value.  A 2-column formatter reads lat
        // successfully from "?" but then finds no bytes left for lon → invalid.
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(
            () => decoder.Decode("?").ToList());
    }

    // ------------------------------------------------------------------
    // Decode — happy path
    // ------------------------------------------------------------------

    /// <summary>Tests that a valid polyline produces the expected coordinates.</summary>
    [TestMethod]
    public void Decode_With_Valid_Polyline_Returns_Expected_Coordinates() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();
        string polyline = StaticValueProvider.Valid.GetPolyline();
        (double Latitude, double Longitude)[] expected = [.. StaticValueProvider.Valid.GetCoordinates()];

        // Act
        (double Lat, double Lon)[] result = [.. decoder.Decode(polyline)];

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        for (int i = 0; i < expected.Length; i++) {
            Assert.AreEqual(expected[i].Latitude, result[i].Lat, 1e-5);
            Assert.AreEqual(expected[i].Longitude, result[i].Lon, 1e-5);
        }
    }

    // ------------------------------------------------------------------
    // Decode — cancellation
    // ------------------------------------------------------------------

    /// <summary>Tests that a pre-cancelled token throws <see cref="OperationCanceledException"/>.</summary>
    [TestMethod]
    public void Decode_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();
        string polyline = StaticValueProvider.Valid.GetPolyline();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(
            () => decoder.Decode(polyline, cts.Token).ToList());
    }

    // ------------------------------------------------------------------
    // Decode — missing WithCreate factory
    // ------------------------------------------------------------------

    /// <summary>
    /// Tests that decoding with a formatter that has no <c>WithCreate</c> factory throws
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_Without_WithCreate_Throws_InvalidOperationException() {
        // Arrange — formatter built without WithCreate
        PolylineFormatter<(double Lat, double Lon), string> formatter =
            FormatterBuilder<(double Lat, double Lon), string>.Create()
                .AddValue("lat", c => c.Lat)
                .AddValue("lon", c => c.Lon)
                .ForPolyline(_write, _read)
                .Build();

        PolylineDecoder<string, (double Lat, double Lon)> decoder =
            new(new PolylineOptions<(double Lat, double Lon), string>(formatter));

        string polyline = StaticValueProvider.Valid.GetPolyline();

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(
            () => decoder.Decode(polyline).ToList());
    }
}
