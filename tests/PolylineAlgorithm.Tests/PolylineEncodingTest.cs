//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm;
using System;
using System.Collections.Generic;

[TestClass]
public class PolylineEncodingTest {
    #region Dynamic Data Properties

    public static IEnumerable<(int delta, string polyline)> DeltaPolylinePairs => [
        (0,"?"),
        (1,"A"),
        (-1,"@"),
        (16,"_@"),
        (-16,"^"),
        (511,"}^"),
        (-511,"|^"),
        (512,"__@"),
        (-512,"~^"),
        (16383,"}~^"),
        (-16383,"|~^"),
        (16384,"___@"),
        (-16384,"~~^"),
        (524287,"}~~^"),
        (-524287,"|~~^"),
        (524288,"____@"),
        (-524288,"~~~^"),
        (16777215,"}~~~^"),
        (-16777215,"|~~~^"),
    ];

    public static IEnumerable<(double denormalized, int normalized, uint precision)> DenormalizedNormalizedPairs => [
        (0, 0, 5),
        (0, 0, 5),
        (1.23456, 123456, 5),
        (-1.23456789, -123456, 5),
        (1.23456789, 12, 2),
        (-1.23456789, -12, 2),
        (90, 9000000, 5),
        (-90, -900000, 5),
        (90, 900, 2),
        (-90, -900, 2),
    ];

    public static IEnumerable<(double denormalized, CoordinateValueType)> DenormalizedOutOfRangeValues => [
        (90.00001,CoordinateValueType.Latitude),
        (-90.00001,CoordinateValueType.Latitude),
        (180.00001,CoordinateValueType.Longitude),
        (-180.00001,CoordinateValueType.Longitude),
        (double.NaN,CoordinateValueType.Latitude),
        (double.NaN,CoordinateValueType.Longitude),
        (double.MinValue,CoordinateValueType.Latitude),
        (double.MaxValue,CoordinateValueType.Latitude),
        (double.MinValue,CoordinateValueType.Longitude),
        (double.MaxValue,CoordinateValueType.Longitude),
        (double.NegativeInfinity,CoordinateValueType.Latitude),
        (double.PositiveInfinity,CoordinateValueType.Latitude),
        (double.NegativeInfinity,CoordinateValueType.Longitude),
        (double.PositiveInfinity,CoordinateValueType.Longitude),
        (0, CoordinateValueType.Unspecified),
        (0, CoordinateValueType.Unspecified),
    ];

    public static IEnumerable<(int normalized, CoordinateValueType)> NormalizedOutOfRangeValues => [
        (9000001,CoordinateValueType.Latitude),
        (-9000001,CoordinateValueType.Latitude),
        (18000001,CoordinateValueType.Longitude),
        (-18000001,CoordinateValueType.Longitude),
        (int.MinValue,CoordinateValueType.Latitude),
        (int.MaxValue,CoordinateValueType.Latitude),
        (int.MinValue,CoordinateValueType.Longitude),
        (int.MaxValue,CoordinateValueType.Longitude),
        (0, CoordinateValueType.Unspecified),
        (0, CoordinateValueType.Unspecified),
    ];

    public static IEnumerable<(int delta, int bufferSize)> DeltaBufferSizePairs => [
        (0, 1),
        (15, 1),
        (-16, 1),
        (16, 2),
        (-17,2),
        (511,2),
        (-512,2),
        (512,3),
        (-513,3),
        (16383,3),
        (-16384,3),
        (16384,4),
        (-16385,4),
        (524287,4),
        (-524288,4),
        (524288,5),
        (-524289,5),
        (16777215,5),
        (-16777216,5),
        (16777216,6),
        (-16777217,6),
        (int.MaxValue,7),
        (int.MinValue,7),
    ];

    #endregion

    [TestMethod]
    [DynamicData(nameof(DenormalizedNormalizedPairs))]
    public void Normalize_Equals_Expected(double denormalized, int expected, uint precision) {
        // Arrange & Act
        int result = PolylineEncoding.Normalize(denormalized, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }


    [TestMethod]
    [DynamicData(nameof(DenormalizedNormalizedPairs))]
    public void Denormalize_Equals_Expected(double expected, int normalized, uint precision) {
        // Arrange & Act
        double result = PolylineEncoding.Denormalize(normalized, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DynamicData(nameof(DeltaPolylinePairs))]
    public void TryWriteValue_StaticBuffer_Returns_True_Equals_Expected(int delta, string expected) {
        // Arrange
        int position = 0;
        Span<char> buffer = stackalloc char[6];

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected.Length, position);
        Assert.AreEqual(expected, buffer[..position].ToString());
    }


    [TestMethod]
    [DynamicData(nameof(DeltaPolylinePairs))]
    public void TryWriteValue_DynamicBuffer_Returns_True_Equals_Expected(int delta, string expected) {
        // Arrange
        int position = 0;
        int required = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[required];

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(required, position);
        Assert.AreEqual(expected.Length, position);
        Assert.AreEqual(expected, buffer[..position].ToString());
    }

    [TestMethod]
    [DynamicData(nameof(DeltaPolylinePairs))]
    public void TryWriteValue_BufferTooSmall_Returns_False(int delta, string _) {
        // Arrange
        int position = 0;
        int required = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[required - 1];

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DynamicData(nameof(DeltaPolylinePairs))]
    public void TryReadValue_Ok(int expected, string polyline) {
        // Arrange
        int position = 0;
        int delta = 0;
        var buffer = polyline.AsMemory();

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(buffer.Length, position);
        Assert.AreEqual(expected, delta);
    }

    [TestMethod]
    public void TryReadValue_EmptyBuffer_Returns_False() {
        // Arrange
        int delta = 0;
        int position = 0;
        ReadOnlyMemory<char> buffer = Memory<char>.Empty;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryReadValue_MalformedBuffer_Returns_False() {
        //Arrange
        int position = 0;
        int delta = 42;
        int expected = delta;
        // Buffer with a char that will never finish a value (simulate incomplete encoding)
        char[] chars = [(char)127]; // 127 - 63 = 64, which is >= 32, so loop never breaks
        ReadOnlyMemory<char> buffer = chars.AsMemory();

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(expected, delta);
    }

    [TestMethod]
    [DynamicData(nameof(DenormalizedOutOfRangeValues))]
    public void Normalize_Throws_ArgumentOutOfRangeException(double value) {
        // Arrange
        static int Normalize(double value) => PolylineEncoding.Normalize(value);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Normalize(value));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(NormalizedOutOfRangeValues))]
    public void Denormalize_Throws_ArgumentOutOfRangeException(int value) {
        // Arrange
        static double Denormalize(int value) => PolylineEncoding.Denormalize(value);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Denormalize(value));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(DeltaBufferSizePairs))]
    public void GetCharCount_Equals_Expected(int delta, int expected) {
        // Arrange & Act
        var bufferSize = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(expected, bufferSize);
    }
}
