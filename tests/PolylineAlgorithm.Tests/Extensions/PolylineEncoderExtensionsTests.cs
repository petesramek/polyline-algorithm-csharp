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
/// Tests for <see cref="PolylineEncoderExtensions"/>.
/// </summary>
[TestClass]
public sealed class PolylineEncoderExtensionsTests {
    private static PolylineEncoder<(double Latitude, double Longitude), string> CreateTestEncoder() {
        PolylineFormatter<(double Latitude, double Longitude), string> formatter =
            FormatterBuilder<(double Latitude, double Longitude), string>.Create()
                .AddValue("lat", static c => c.Latitude)
                .AddValue("lon", static c => c.Longitude)
                .WithReaderWriter(static m => new string(m.Span), static s => s.AsMemory())
                .Build();

        return new PolylineEncoder<(double Latitude, double Longitude), string>(
            new PolylineOptions<(double Latitude, double Longitude), string>(formatter));
    }

    // ----- Encode(T[]) -----

    /// <summary>
    /// Tests that Encode with a null encoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Array_Null_Encoder_Throws_ArgumentNullException() {
        // Arrange — call the extension method explicitly to target the array overload.
        IPolylineEncoder<(double, double), string>? encoder = null;
        (double, double)[] coordinates = [(0.0, 0.0)];

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => PolylineEncoderExtensions.Encode<(double, double), string>(encoder!, coordinates));
        Assert.AreEqual("encoder", ex.ParamName);
    }

    /// <summary>
    /// Tests that Encode with a null array throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Array_Null_Coordinates_Throws_ArgumentNullException() {
        // Arrange — call the extension method explicitly (same reasoning as above).
        IPolylineEncoder<(double, double), string> encoder = CreateTestEncoder();
        (double, double)[]? coordinates = null;

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => PolylineEncoderExtensions.Encode<(double, double), string>(encoder, coordinates!));
        Assert.AreEqual("values", ex.ParamName);
    }

    /// <summary>
    /// Tests that Encode with a valid array returns the expected polyline.
    /// </summary>
    [TestMethod]
    public void Encode_With_Array_Valid_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        var encoder = CreateTestEncoder();
        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
