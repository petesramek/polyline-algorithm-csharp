//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Extensions;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Utility;
using System;
using System.Collections.Generic;

/// <summary>
/// Tests for <see cref="PolylineEncoderExtensions"/>.
/// </summary>
[TestClass]
public sealed class PolylineEncoderExtensionsTests {
    private sealed class TestStringEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
        protected override string CreatePolyline(ReadOnlySpan<char> polyline) => polyline.ToString();
        protected override void Write((double Latitude, double Longitude) item, ref PolylineWriter writer) {
            writer.Write(item.Latitude);
            writer.Write(item.Longitude);
        }
    }

    // ----- Encode(List<T>) -----

    /// <summary>
    /// Tests that Encode with a null encoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_List_Null_Encoder_Throws_ArgumentNullException() {
        // Arrange — use interface type so the extension method is resolved
        IPolylineEncoder<(double, double), string>? encoder = null;
        List<(double, double)> coordinates = [(0.0, 0.0)];

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => encoder!.Encode(coordinates));
        Assert.AreEqual("encoder", ex.ParamName);
    }

    /// <summary>
    /// Tests that Encode with a null list throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_List_Null_Coordinates_Throws_ArgumentNullException() {
        // Arrange
        TestStringEncoder encoder = new();
        List<(double, double)>? coordinates = null;

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => encoder.Encode(coordinates!));
        Assert.AreEqual("coordinates", ex.ParamName);
    }

    /// <summary>
    /// Tests that Encode with a valid list returns the expected polyline.
    /// </summary>
    [TestMethod]
    public void Encode_With_List_Valid_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        TestStringEncoder encoder = new();
        List<(double Latitude, double Longitude)> coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected, result);
    }

    // ----- Encode(T[]) -----

    /// <summary>
    /// Tests that Encode with a null encoder throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Array_Null_Encoder_Throws_ArgumentNullException() {
        // Arrange — call the extension method explicitly because IPolylineEncoder.Encode(ReadOnlySpan<T>)
        // would be preferred over the extension when calling through method syntax with an array argument.
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
        IPolylineEncoder<(double, double), string> encoder = new TestStringEncoder();
        (double, double)[]? coordinates = null;

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => PolylineEncoderExtensions.Encode<(double, double), string>(encoder, coordinates!));
        Assert.AreEqual("coordinates", ex.ParamName);
    }

    /// <summary>
    /// Tests that Encode with a valid array returns the expected polyline.
    /// </summary>
    [TestMethod]
    public void Encode_With_Array_Valid_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        TestStringEncoder encoder = new();
        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
