//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Properties;
using PolylineAlgorithm.Utility;
using System;

[TestClass]
public class AbstractPolylineDecoderTest {
    private static readonly PolylineDecoder _decoder = new PolylineDecoder();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    public static IEnumerable<(double, double)> NotANumberAndInfinityCoordinates => StaticValueProvider.Invalid.GetNotANumberAndInfinityCoordinates();

    public static IEnumerable<(double, double)> MinAndMaxCoordinates => StaticValueProvider.Invalid.GetMinAndMaxCoordinates();

    public static IEnumerable<object[]> InvalidPolylines => StaticValueProvider.Invalid.GetInvalidPolylines().Select<string, object[]>(p => [p]);

    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange && Act
        var decoder = new PolylineDecoder();

        // Assert
        Assert.IsNotNull(decoder);
        Assert.IsNotNull(decoder.Options);
    }

    [TestMethod]
    public void Constructor_Options_Instance_Ok() {
        // Arrange
        var options = new PolylineEncodingOptions();

        // Act
        var decoder = new PolylineDecoder(options);

        // Assert
        Assert.IsNotNull(decoder);
        Assert.AreSame(options, decoder.Options);
    }

    [TestMethod]
    public void Constructor_Null_Options_Throws_ArgumentNullException() {
        // Arrange
        void New() => new PolylineDecoder(null!);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(New);

        // Assert
        Assert.AreEqual("options", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Decode_NullPolyline_Throws_ArgumentException() {
        // Arrange
        void Decode() => _decoder.Decode(null!).ToList();

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Decode);

        // Assert
        Assert.AreEqual("polyline", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Decode_EmptyPolyline_Throws_ArgumentException() {
        // Arrange
        void Decode() => _decoder.Decode(string.Empty).ToList();

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Decode);

        // Assert
        Assert.AreEqual("polyline", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Decode_WhitespacePolyline_Throws_ArgumentException() {
        // Arrange
        void Decode() => _decoder.Decode(" ").ToList();

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Decode);

        // Assert
        Assert.AreEqual("polyline", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(InvalidPolylines), DynamicDataSourceType.Property)]
    public void Decode_InvalidPolyline_Throws_InvalidPolylineException(string polyline) {
        // Arrange
        void Decode() => _decoder.Decode(polyline).ToList();

        // Act
        var exception = Assert.ThrowsExactly<InvalidPolylineException>(Decode);

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }


    [TestMethod]
    public void Decode_ShortPolyline_Throws_InvalidPolylineException() {
        // Arrange
        void Decode() => _decoder.Decode("?").ToList();

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Decode);

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Encode_RandomValue_ValidInput_Ok(int count) {
        // Arrange
        string polyline = RandomValueProvider.GetPolyline(count);
        IEnumerable<(double Latitude, double Longitude)> expected = RandomValueProvider.GetCoordinates(count);

        // Act
        var result = _decoder.Decode(polyline);

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    [TestMethod]
    public void Decode_StaticValue_ValidInput_Ok() {
        // Arrange
        string polyline = StaticValueProvider.Valid.GetPolyline();
        IEnumerable<(double Latitude, double Longitude)> expected = StaticValueProvider.Valid.GetCoordinates();

        // Act
        var result = _decoder.Decode(polyline);

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    public class PolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
        public PolylineDecoder()
            : base() { }

        public PolylineDecoder(PolylineEncodingOptions options)
            : base(options) { }

        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) {
            return (latitude, longitude);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(string? polyline) {
            if (string.IsNullOrWhiteSpace(polyline)) {
                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhiteSpaceMessage, nameof(polyline));
            }

            return polyline.AsMemory();
        }
    }
}
