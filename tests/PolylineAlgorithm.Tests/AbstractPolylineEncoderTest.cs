//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System;

[TestClass]
public class AbstractPolylineEncoderTest {
    private static readonly PolylineEncoder _encoder = new();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    public static IEnumerable<(double, double)> NotANumberAndInfinityCoordinates => StaticValueProvider.Invalid.GetNotANumberAndInfinityCoordinates();

    public static IEnumerable<(double, double)> MinAndMaxCoordinates => StaticValueProvider.Invalid.GetMinAndMaxCoordinates();


    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange && Act
        var encoder = new PolylineEncoder();

        // Assert
        Assert.IsNotNull(encoder);
        Assert.IsNotNull(encoder.Options);
    }

    [TestMethod]
    public void Constructor_ValidOptions_Ok() {
        // Arrange
        var options = new PolylineEncodingOptions();

        // Act
        var encoder = new PolylineEncoder(options);

        // Assert
        Assert.IsNotNull(encoder);
        Assert.AreSame(options, encoder.Options);
    }


    [TestMethod]
    public void Constructor_Null_Options_Throws_ArgumentNullException() {
        // Arrange
        static void New() => new PolylineEncoder(null!);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(New);

        // Assert
        Assert.AreEqual("options", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_Null_Coordinates_Throws_ArgumentException() {
        // Arrange
        static void Encode() => _encoder.Encode(null!);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_Empty_Coordinates_Throws_ArgumentException() {
        // Arrange
        static void Encode() => _encoder.Encode([]);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_Buffer_Too_Small_Throws_InternalBufferOverflowException() {
        // Arrange
        PolylineEncoder _encoder = new(new PolylineEncodingOptions { MaxBufferSize = 12 });
        List<(double Latitude, double Longitude)> coordinates = RandomValueProvider.GetCoordinates(2).ToList();

        // Act
        var exception = Assert.ThrowsExactly<InternalBufferOverflowException>(() => _encoder.Encode(coordinates));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(NotANumberAndInfinityCoordinates))]
    public void Encode_Not_A_Number_And_Infinity_Coordinate_Throws_ArgumentOutOfRangeException((double, double) coordinate) {
        // Arrange

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _encoder.Encode([coordinate]));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(MinAndMaxCoordinates))]
    public void Encode_Min_And_Max_Coordinate_Throws_ArgumentOutOfRangeException((double, double) coordinate) {
        // Arrange

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _encoder.Encode([coordinate]));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Encode_Random_Value_Valid_Input_Ok(int count) {
        // Arrange
        List<(double Latitude, double Longitude)> coordinates = [.. RandomValueProvider.GetCoordinates(count)];
        string expected = RandomValueProvider.GetPolyline(count);

        // Act
        var result = _encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    [TestMethod]
    public void Encode_Static_Value_Valid_Input_Ok() {
        // Arrange
        List<(double Latitude, double Longitude)> coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        var result = _encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    public class PolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
        public PolylineEncoder()
            : base() { }

        public PolylineEncoder(PolylineEncodingOptions options)
            : base(options) { }

        protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
        protected override double GetLatitude((double Latitude, double Longitude) coordinate) => coordinate.Latitude;
        protected override double GetLongitude((double Latitude, double Longitude) coordinate) => coordinate.Longitude;
    }
}
