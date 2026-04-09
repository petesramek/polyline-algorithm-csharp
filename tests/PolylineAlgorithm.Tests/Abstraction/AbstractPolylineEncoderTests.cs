//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Abstraction;

using PolylineAlgorithm.Utility;
using System;
using System.Threading;

/// <summary>
/// Tests for <see cref="PolylineEncoder{TCoordinate, TPolyline}"/>.
/// </summary>
[TestClass]
public sealed class AbstractPolylineEncoderTests {
    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static readonly Func<ReadOnlyMemory<char>, string> _write = m => new string(m.Span);
    private static readonly Func<string, ReadOnlyMemory<char>> _read = s => s.AsMemory();

    private static PolylineEncoder<(double Lat, double Lon), string> CreateEncoder(int stackAllocLimit = 512) {
        PolylineFormatter<(double Lat, double Lon), string> formatter =
            FormatterBuilder<(double Lat, double Lon), string>.Create()
                .AddValue("lat", static c => c.Lat)
                .AddValue("lon", static c => c.Lon)
                .ForPolyline(_write, _read)
                .Build();

        return new PolylineEncoder<(double Lat, double Lon), string>(
            new PolylineOptions<(double Lat, double Lon), string>(formatter, stackAllocLimit));
    }

    // ------------------------------------------------------------------
    // Constructor
    // ------------------------------------------------------------------

    /// <summary>Tests that a null options argument throws <see cref="ArgumentNullException"/>.</summary>
    [TestMethod]
    public void Constructor_With_Null_Options_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => new PolylineEncoder<(double, double), string>(null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    /// <summary>Tests that a valid options argument creates the instance without throwing.</summary>
    [TestMethod]
    public void Constructor_With_Valid_Options_Creates_Instance() {
        // Act
        PolylineEncoder<(double Lat, double Lon), string> encoder = CreateEncoder();

        // Assert
        Assert.IsNotNull(encoder);
    }

    // ------------------------------------------------------------------
    // Encode — argument validation
    // ------------------------------------------------------------------

    /// <summary>Tests that encoding an empty span throws <see cref="ArgumentException"/>.</summary>
    [TestMethod]
    public void Encode_With_Empty_Span_Throws_ArgumentException() {
        // Arrange
        PolylineEncoder<(double Lat, double Lon), string> encoder = CreateEncoder();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(
            () => encoder.Encode(ReadOnlySpan<(double, double)>.Empty));
    }

    // ------------------------------------------------------------------
    // Encode — happy path
    // ------------------------------------------------------------------

    /// <summary>Tests that a single coordinate encodes to a non-empty string.</summary>
    [TestMethod]
    public void Encode_With_Single_Coordinate_Returns_Non_Empty_String() {
        // Arrange
        PolylineEncoder<(double Lat, double Lon), string> encoder = CreateEncoder();
        (double, double)[] coordinates = [(0.0, 0.0)];

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    /// <summary>Tests that known coordinates produce the expected polyline string.</summary>
    [TestMethod]
    public void Encode_With_Known_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        PolylineEncoder<(double Lat, double Lon), string> encoder = CreateEncoder();
        (double Lat, double Lon)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }

    // ------------------------------------------------------------------
    // Encode — cancellation
    // ------------------------------------------------------------------

    /// <summary>Tests that a pre-cancelled token throws <see cref="OperationCanceledException"/>.</summary>
    [TestMethod]
    public void Encode_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        PolylineEncoder<(double Lat, double Lon), string> encoder = CreateEncoder();
        using CancellationTokenSource cts = new();
        cts.Cancel();
        (double, double)[] coordinates = [(0.0, 0.0), (1.0, 1.0)];

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(
            () => encoder.Encode(coordinates.AsSpan(), cts.Token));
    }

    // ------------------------------------------------------------------
    // Encode — heap allocation path
    // ------------------------------------------------------------------

    /// <summary>
    /// Tests that a stack-alloc limit of 1 forces heap allocation via ArrayPool
    /// but still produces the correct result.
    /// </summary>
    [TestMethod]
    public void Encode_With_Small_Stack_Alloc_Limit_Uses_Heap_And_Produces_Correct_Result() {
        // Arrange — limit of 1 forces every encode to go through ArrayPool
        PolylineEncoder<(double Lat, double Lon), string> encoder = CreateEncoder(stackAllocLimit: 1);
        (double Lat, double Lon)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }

    // ------------------------------------------------------------------
    // Encode — baseline
    // ------------------------------------------------------------------

    /// <summary>
    /// Tests that a non-zero baseline shifts the first delta so the result differs
    /// from the same encode without a baseline.
    /// </summary>
    [TestMethod]
    public void Encode_With_Baseline_Produces_Different_First_Delta() {
        // Arrange
        PolylineFormatter<(double, double), string> formatterWithBaseline =
            FormatterBuilder<(double, double), string>.Create()
                .AddValue("lat", c => c.Item1).SetBaseline(100000L)   // scaled 1.0 at precision 5
                .AddValue("lon", c => c.Item2)
                .ForPolyline(_write, _read)
                .Build();

        PolylineFormatter<(double, double), string> formatterNoBaseline =
            FormatterBuilder<(double, double), string>.Create()
                .AddValue("lat", c => c.Item1)
                .AddValue("lon", c => c.Item2)
                .ForPolyline(_write, _read)
                .Build();

        PolylineEncoder<(double, double), string> encoderWithBaseline =
            new(new PolylineOptions<(double, double), string>(formatterWithBaseline));
        PolylineEncoder<(double, double), string> encoderNoBaseline =
            new(new PolylineOptions<(double, double), string>(formatterNoBaseline));

        (double, double)[] coordinates = [(1.0, 2.0)];

        // Act
        string resultWith = encoderWithBaseline.Encode(coordinates.AsSpan());
        string resultWithout = encoderNoBaseline.Encode(coordinates.AsSpan());

        // Assert — the two encodings must differ because the first lat delta changes
        Assert.AreNotEqual(resultWithout, resultWith);
    }

    // ------------------------------------------------------------------
    // Round-trip
    // ------------------------------------------------------------------

    /// <summary>
    /// Tests that encoding then decoding reproduces the original coordinates within
    /// the precision of the encoding.
    /// </summary>
    [TestMethod]
    public void Encode_RoundTrip_Produces_Original_Coordinates() {
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

        (double Lat, double Lon)[] original = [.. StaticValueProvider.Valid.GetCoordinates()];

        // Act
        string encoded = encoder.Encode(original.AsSpan());
        (double Lat, double Lon)[] decoded = [.. decoder.Decode(encoded)];

        // Assert
        Assert.AreEqual(original.Length, decoded.Length);
        for (int i = 0; i < original.Length; i++) {
            Assert.AreEqual(original[i].Lat, decoded[i].Lat, 1e-5);
            Assert.AreEqual(original[i].Lon, decoded[i].Lon, 1e-5);
        }
    }
}
