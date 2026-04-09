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
                .AddValue("lat", static c => c.Lat)
                .AddValue("lon", static c => c.Lon)
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

    // ------------------------------------------------------------------
    // Chunked Decode — options overload
    // ------------------------------------------------------------------

    /// <summary>
    /// Tests that the chunked overload with null options produces the same result as the standard
    /// overload.
    /// </summary>
    [TestMethod]
    public void Decode_Chunked_With_Null_Options_Produces_Same_Result_As_Standard() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();
        string polyline = StaticValueProvider.Valid.GetPolyline();

        // Act
        List<(double Lat, double Lon)> standard = decoder.Decode(polyline).ToList();
        List<(double Lat, double Lon)> chunked = decoder.Decode(polyline, null, CancellationToken.None).ToList();

        // Assert
        Assert.AreEqual(standard.Count, chunked.Count);
        for (int i = 0; i < standard.Count; i++) {
            Assert.AreEqual(standard[i].Lat, chunked[i].Lat, 1e-5);
            Assert.AreEqual(standard[i].Lon, chunked[i].Lon, 1e-5);
        }
    }

    /// <summary>
    /// Tests that chunked decoding with a Previous coordinate seeds the accumulated state
    /// correctly, producing a result different from standard decoding of the same polyline.
    /// </summary>
    [TestMethod]
    public void Decode_Chunked_With_Previous_Seeds_Accumulated_State() {
        // Arrange — build a chunked polyline where chunk B is relative to the last of chunk A
        PolylineFormatter<(double Lat, double Lon), string> formatter =
            FormatterBuilder<(double Lat, double Lon), string>.Create()
                .AddValue("lat", c => c.Lat)
                .AddValue("lon", c => c.Lon)
                .WithCreate(static v => (v[0], v[1]))
                .ForPolyline(_write, _read)
                .Build();

        PolylineOptions<(double Lat, double Lon), string> options = new(formatter);
        PolylineEncoder<(double Lat, double Lon), string> encoder = new(options);
        PolylineDecoder<string, (double Lat, double Lon)> decoder = new(options);

        (double Lat, double Lon)[] chunkA = [(38.5, -120.2), (40.7, -120.95)];
        (double Lat, double Lon)[] chunkB = [(43.252, -126.453)];

        string polylineB = encoder.Encode(
            chunkB.AsSpan(),
            new PolylineEncodingOptions<(double Lat, double Lon)>(chunkA[^1]),
            CancellationToken.None);

        // Act
        (double Lat, double Lon) decodedWithSeed = decoder.Decode(
            polylineB,
            new PolylineDecodingOptions<(double Lat, double Lon)>(chunkA[^1]),
            CancellationToken.None).First();

        // Assert — decoded value should match the original chunkB[0]
        Assert.AreEqual(chunkB[0].Lat, decodedWithSeed.Lat, 1e-5);
        Assert.AreEqual(chunkB[0].Lon, decodedWithSeed.Lon, 1e-5);
    }

    /// <summary>
    /// Tests chunked encode + chunked decode round-trip: splitting a sequence into two chunks,
    /// encoding with chaining, then decoding each chunk independently with Previous seed produces
    /// the original sequence.
    /// </summary>
    [TestMethod]
    public void Decode_Chunked_RoundTrip_Reproduces_Full_Sequence() {
        // Arrange
        PolylineFormatter<(double Lat, double Lon), string> formatter =
            FormatterBuilder<(double Lat, double Lon), string>.Create()
                .AddValue("lat", c => c.Lat)
                .AddValue("lon", c => c.Lon)
                .WithCreate(static v => (v[0], v[1]))
                .ForPolyline(_write, _read)
                .Build();

        PolylineOptions<(double Lat, double Lon), string> options = new(formatter);
        PolylineEncoder<(double Lat, double Lon), string> encoder = new(options);
        PolylineDecoder<string, (double Lat, double Lon)> decoder = new(options);

        (double Lat, double Lon)[] all = [
            (38.5, -120.2),
            (40.7, -120.95),
            (43.252, -126.453),
            (47.6, -122.3),
        ];

        (double Lat, double Lon)[] chunkA = all[..2];
        (double Lat, double Lon)[] chunkB = all[2..];

        // Encode chunked
        string polylineA = encoder.Encode(chunkA.AsSpan());
        string polylineB = encoder.Encode(
            chunkB.AsSpan(),
            new PolylineEncodingOptions<(double Lat, double Lon)>(chunkA[^1]),
            CancellationToken.None);

        // Decode chunked
        List<(double Lat, double Lon)> decodedA = decoder.Decode(polylineA).ToList();
        List<(double Lat, double Lon)> decodedB = decoder.Decode(
            polylineB,
            new PolylineDecodingOptions<(double Lat, double Lon)>(decodedA[^1]),
            CancellationToken.None).ToList();

        List<(double Lat, double Lon)> combined = [.. decodedA, .. decodedB];

        // Assert
        Assert.AreEqual(all.Length, combined.Count);
        for (int i = 0; i < all.Length; i++) {
            Assert.AreEqual(all[i].Lat, combined[i].Lat, 1e-5);
            Assert.AreEqual(all[i].Lon, combined[i].Lon, 1e-5);
        }
    }

    /// <summary>
    /// Tests that a null polyline throws <see cref="ArgumentNullException"/> when using the
    /// chunked overload.
    /// </summary>
    [TestMethod]
    public void Decode_Chunked_With_Null_Polyline_Throws_ArgumentNullException() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder.Decode(null!, null, CancellationToken.None).ToList());
        Assert.AreEqual("polyline", ex.ParamName);
    }

    /// <summary>
    /// Tests that a pre-cancelled token throws <see cref="OperationCanceledException"/> in the
    /// chunked overload.
    /// </summary>
    [TestMethod]
    public void Decode_Chunked_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        PolylineDecoder<string, (double Lat, double Lon)> decoder = CreateDecoder();
        string polyline = StaticValueProvider.Valid.GetPolyline();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(
            () => decoder.Decode(polyline, null, cts.Token).ToList());
    }
}
