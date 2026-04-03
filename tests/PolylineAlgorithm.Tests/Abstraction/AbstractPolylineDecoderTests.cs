//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Abstraction;

using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;

/// <summary>
/// Tests for <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/>.
/// </summary>
[TestClass]
public sealed class AbstractPolylineDecoderTests {
    private sealed class TestStringDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
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
}
