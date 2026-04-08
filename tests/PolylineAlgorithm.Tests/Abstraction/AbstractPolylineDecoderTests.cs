//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Abstraction;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Utility;
using System;
using System.Collections.Generic;
using PolylineAlgorithm;

/// <summary>
/// Tests for <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/>.
/// </summary>
[TestClass]
public sealed class AbstractPolylineDecoderTests {
    private sealed class TestStringDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) => (latitude, longitude);
    }

    private sealed class TestStringDecoderWithOptions : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
        public TestStringDecoderWithOptions(PolylineEncodingOptions options)
            : base(options) { }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) => (latitude, longitude);
    }

    /// <summary>
    /// Tests that Decode with a null polyline throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Null_Polyline_Throws_ArgumentNullException() {
        // Arrange
        TestStringDecoder decoder = new();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(() => decoder.Decode((string?)null!).ToList());
        Assert.AreEqual("polyline", ex.ParamName);
    }

    /// <summary>
    /// Tests that Decode with an empty polyline throws <see cref="InvalidPolylineException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Empty_Polyline_Throws_InvalidPolylineException() {
        // Arrange
        TestStringDecoder decoder = new();

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => decoder.Decode(string.Empty).ToList());
    }

    /// <summary>
    /// Tests that Decode with a polyline containing an invalid character throws <see cref="InvalidPolylineException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Invalid_Character_Polyline_Throws_InvalidPolylineException() {
        // Arrange
        TestStringDecoder decoder = new();

        // '!' (33) is below allowed range ('?' == 63)
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => decoder.Decode("!").ToList());
    }

    /// <summary>
    /// Tests that Decode with a valid polyline returns the expected coordinates.
    /// </summary>
    [TestMethod]
    public void Decode_With_Valid_Polyline_Returns_Expected_Coordinates() {
        // Arrange
        TestStringDecoder decoder = new();
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

    /// <summary>
    /// Tests that the options constructor with null throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Null_Options_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(() => new TestStringDecoderWithOptions(null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    /// <summary>
    /// Tests that the Options property returns the configured options.
    /// </summary>
    [TestMethod]
    public void Options_With_Default_Returns_Default_Options() {
        // Arrange
        TestStringDecoder decoder = new();

        // Assert
        Assert.IsNotNull(decoder.Options);
        Assert.AreEqual(5u, decoder.Options.Precision);
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
        TestStringDecoderWithOptions decoder = new(options);

        // Assert
        Assert.AreSame(options, decoder.Options);
    }

    /// <summary>
    /// Tests that Decode with a pre-cancelled token throws <see cref="OperationCanceledException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        TestStringDecoder decoder = new();
        string polyline = StaticValueProvider.Valid.GetPolyline();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(() => decoder.Decode(polyline, cts.Token).ToList());
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
            () => new AbstractPolylineDecoder<string, (double, double)>((PolylineOptions<(double, double), string>)null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    /// <summary>
    /// Tests that the formatter-based decoder decodes the known polyline to the expected coordinates.
    /// </summary>
    [TestMethod]
    public void Decode_FormatterPath_With_Known_Polyline_Returns_Expected_Coordinates() {
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

        AbstractPolylineDecoder<string, (double Latitude, double Longitude)> decoder = new(options);

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

    /// <summary>
    /// Tests that the formatter-based decoder throws <see cref="ArgumentNullException"/> for a null polyline.
    /// </summary>
    [TestMethod]
    public void Decode_FormatterPath_With_Null_Polyline_Throws_ArgumentNullException() {
        // Arrange
        PolylineValueFormatter<(double, double)> valueFormatter =
            FormatterBuilder<(double, double)>.Create()
                .AddValue("lat", c => c.Item1)
                .AddValue("lon", c => c.Item2)
                .WithCreate(static values => (values[0] / 1e5, values[1] / 1e5))
                .Build();

        AbstractPolylineDecoder<string, (double, double)> decoder = new(
            new PolylineOptions<(double, double), string>(valueFormatter, PolylineFormatter.ForString));

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => decoder.Decode(null!).ToList());
        Assert.AreEqual("polyline", ex.ParamName);
    }

    /// <summary>
    /// Tests that the formatter-based decoder with a pre-cancelled token throws <see cref="OperationCanceledException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_FormatterPath_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        PolylineValueFormatter<(double, double)> valueFormatter =
            FormatterBuilder<(double, double)>.Create()
                .AddValue("lat", c => c.Item1)
                .AddValue("lon", c => c.Item2)
                .WithCreate(static values => (values[0] / 1e5, values[1] / 1e5))
                .Build();

        AbstractPolylineDecoder<string, (double, double)> decoder = new(
            new PolylineOptions<(double, double), string>(valueFormatter, PolylineFormatter.ForString));

        string polyline = StaticValueProvider.Valid.GetPolyline();
        using CancellationTokenSource cts = new();
        cts.Cancel();

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(
            () => decoder.Decode(polyline, cts.Token).ToList());
    }
}
