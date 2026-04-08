//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Abstraction;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Utility;
using System;
using PolylineAlgorithm;

/// <summary>
/// Tests for <see cref="AbstractPolylineEncoder{TCoordinate, TPolyline}"/>.
/// </summary>
[TestClass]
public sealed class AbstractPolylineEncoderTests {
    private sealed class TestStringEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
        public TestStringEncoder()
            : base() { }

        public TestStringEncoder(PolylineEncodingOptions options)
            : base(options) { }

        protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
        protected override double GetLatitude((double Latitude, double Longitude) current) => current.Latitude;
        protected override double GetLongitude((double Latitude, double Longitude) current) => current.Longitude;
    }

    /// <summary>
    /// Tests that the default constructor creates an instance with default options.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Default_Options_Creates_Instance() {
        // Act
        TestStringEncoder encoder = new();

        // Assert
        Assert.IsNotNull(encoder);
        Assert.IsNotNull(encoder.Options);
        Assert.AreEqual(5u, encoder.Options.Precision);
        Assert.AreEqual(512, encoder.Options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that the options constructor with null throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Null_Options_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(() => new TestStringEncoder(null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    /// <summary>
    /// Tests that the options constructor stores the provided options.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Options_Stores_Options() {
        // Arrange
        PolylineEncodingOptions options = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(7)
            .Build();

        // Act
        TestStringEncoder encoder = new(options);

        // Assert
        Assert.AreSame(options, encoder.Options);
    }

    /// <summary>
    /// Tests that Encode with an empty span throws <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Empty_Span_Throws_ArgumentException() {
        // Arrange
        TestStringEncoder encoder = new();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => encoder.Encode(ReadOnlySpan<(double, double)>.Empty));
    }

    /// <summary>
    /// Tests that Encode with a single valid coordinate returns a non-empty string.
    /// </summary>
    [TestMethod]
    public void Encode_With_Single_Coordinate_Returns_Non_Empty_String() {
        // Arrange
        TestStringEncoder encoder = new();
        (double, double)[] coordinates = [(0.0, 0.0)];

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    /// <summary>
    /// Tests that Encode with known coordinates returns the expected polyline string.
    /// </summary>
    [TestMethod]
    public void Encode_With_Known_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        TestStringEncoder encoder = new();
        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that Encode with a pre-cancelled token throws <see cref="OperationCanceledException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        TestStringEncoder encoder = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();
        (double, double)[] coordinates = [(0.0, 0.0), (1.0, 1.0)];

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(() => encoder.Encode(coordinates.AsSpan(), cts.Token));
    }

    /// <summary>
    /// Tests that Encode still produces the correct result when the buffer exceeds the stack allocation
    /// limit, forcing heap allocation via <see cref="System.Buffers.ArrayPool{T}"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Small_Stack_Alloc_Limit_Uses_Heap_Allocation_And_Produces_Correct_Result() {
        // Arrange — force heap path by making stackAllocLimit smaller than any real encoding needs
        PolylineEncodingOptions options = PolylineEncodingOptionsBuilder.Create()
            .WithStackAllocLimit(1)
            .Build();
        TestStringEncoder encoder = new(options);
        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }

    // -------------------------------------------------------------------------
    // Formatter-based path (PolylineOptions<TValue, TPolyline> constructor)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Tests that the PolylineOptions constructor with null throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Null_PolylineOptions_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => new AbstractPolylineEncoder<(double, double), string>((PolylineOptions<(double, double), string>)null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    /// <summary>
    /// Tests that the formatter-based encoder round-trips known coordinates through the polyline wire format.
    /// </summary>
    [TestMethod]
    public void Encode_FormatterPath_With_Known_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        PolylineValueFormatter<(double Latitude, double Longitude)> valueFormatter =
            FormatterBuilder<(double Latitude, double Longitude)>.Create()
                .AddValue("lat", c => c.Latitude)
                .AddValue("lon", c => c.Longitude)
                .Build();

        PolylineOptions<(double Latitude, double Longitude), string> options = new(
            valueFormatter,
            PolylineFormatter.ForString);

        AbstractPolylineEncoder<(double Latitude, double Longitude), string> encoder = new(options);

        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that the formatter-based encoder respects a non-zero baseline by subtracting it
    /// from the first item's scaled value, producing a smaller first delta.
    /// </summary>
    [TestMethod]
    public void Encode_FormatterPath_With_Baseline_Produces_Correct_First_Delta() {
        // Arrange: a single coordinate at (1.0, 2.0) with precision 5.
        // Without baseline, scaled values are (100000, 200000).
        // With baseline lat=100000, the first lat delta must be 0 (100000 - 100000).
        PolylineValueFormatter<(double Lat, double Lon)> valueFormatter =
            FormatterBuilder<(double Lat, double Lon)>.Create()
                .AddValue("lat", c => c.Lat)
                .SetBaseline(100000L)   // scaled value of 1.0 at precision 5
                .AddValue("lon", c => c.Lon)
                .Build();

        PolylineOptions<(double Lat, double Lon), string> optionsWithBaseline = new(
            valueFormatter,
            PolylineFormatter.ForString);

        PolylineValueFormatter<(double Lat, double Lon)> valueFormatterNoBaseline =
            FormatterBuilder<(double Lat, double Lon)>.Create()
                .AddValue("lat", c => c.Lat)
                .AddValue("lon", c => c.Lon)
                .Build();

        PolylineOptions<(double Lat, double Lon), string> optionsNoBaseline = new(
            valueFormatterNoBaseline,
            PolylineFormatter.ForString);

        AbstractPolylineEncoder<(double Lat, double Lon), string> encoderWithBaseline = new(optionsWithBaseline);
        AbstractPolylineEncoder<(double Lat, double Lon), string> encoderNoBaseline = new(optionsNoBaseline);

        (double, double)[] coordinates = [(1.0, 2.0)];

        // Act
        string resultWithBaseline = encoderWithBaseline.Encode(coordinates.AsSpan());
        string resultNoBaseline = encoderNoBaseline.Encode(coordinates.AsSpan());

        // Assert: baseline shifts the first delta so the results differ, and the baseline result
        // encodes a smaller (zero) latitude delta.
        Assert.AreNotEqual(resultNoBaseline, resultWithBaseline);
    }

    /// <summary>
    /// Tests that the formatter-based encoder produces output that the formatter-based decoder can
    /// reconstruct exactly (full round-trip).
    /// </summary>
    [TestMethod]
    public void Encode_FormatterPath_RoundTrip_Produces_Original_Coordinates() {
        // Arrange
        PolylineValueFormatter<(double Latitude, double Longitude)> valueFormatter =
            FormatterBuilder<(double Latitude, double Longitude)>.Create()
                .AddValue("lat", c => c.Latitude)
                .AddValue("lon", c => c.Longitude)
                .WithCreate(static values => (values[0] / 1e5, values[1] / 1e5))
                .Build();

        PolylineOptions<(double Latitude, double Longitude), string> options = new(
            valueFormatter,
            PolylineFormatter.ForString);

        AbstractPolylineEncoder<(double Latitude, double Longitude), string> encoder = new(options);
        AbstractPolylineDecoder<string, (double Latitude, double Longitude)> decoder = new(options);

        (double Latitude, double Longitude)[] original = [.. StaticValueProvider.Valid.GetCoordinates()];

        // Act
        string encoded = encoder.Encode(original.AsSpan());
        (double Latitude, double Longitude)[] decoded = [.. decoder.Decode(encoded)];

        // Assert
        Assert.AreEqual(original.Length, decoded.Length);
        for (int i = 0; i < original.Length; i++) {
            Assert.AreEqual(original[i].Latitude, decoded[i].Latitude, 1e-5);
            Assert.AreEqual(original[i].Longitude, decoded[i].Longitude, 1e-5);
        }
    }
}
