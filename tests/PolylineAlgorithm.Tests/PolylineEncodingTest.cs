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
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "In test we want to test invalid values.")]
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

    public static IEnumerable<(double denormalized, int normalized, CoordinateValueType)> DenormalizedNormalizedPairs => [
        (0,0, CoordinateValueType.Latitude),
        (0,0, CoordinateValueType.Longitude),
        (1.23456,123456, CoordinateValueType.Latitude),
        (-1.23456,-123456, CoordinateValueType.Latitude),
        (1.23456,123456, CoordinateValueType.Longitude),
        (-1.23456,-123456, CoordinateValueType.Longitude),
        (90,9000000, CoordinateValueType.Latitude),
        (-90,-9000000, CoordinateValueType.Latitude),
        (90,9000000, CoordinateValueType.Longitude),
        (-90,-9000000, CoordinateValueType.Longitude),
        (180,18000000, CoordinateValueType.Longitude),
        (-180,-18000000, CoordinateValueType.Longitude),
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

    public static IEnumerable<(int delta, int charCount)> DeltaCharCountPairs => [
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
    public void Normalize_Equals_Expected(double denormalized, int expected, CoordinateValueType type) {
        // Arrange & Act
        int result = PolylineEncoding.Normalize(denormalized, type);

        // Assert
        Assert.AreEqual(expected, result);
    }


    [TestMethod]
    [DynamicData(nameof(DenormalizedNormalizedPairs))]
    public void Denormalize_Equals_Expected(double expected, int normalized, CoordinateValueType type) {
        // Arrange & Act
        double result = PolylineEncoding.Denormalize(normalized, type);

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
        int required = PolylineEncoding.GetCharCount(delta);
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
        int required = PolylineEncoding.GetCharCount(delta);
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
    public void Normalize_Throws_ArgumentOutOfRangeException(double value, CoordinateValueType type) {
        // Arrange
        static int Normalize(double value, CoordinateValueType type) => PolylineEncoding.Normalize(value, type);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Normalize(value, type));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(NormalizedOutOfRangeValues))]
    public void Denormalize_Throws_ArgumentOutOfRangeException(int value, CoordinateValueType type) {
        // Arrange
        static double Denormalize(int value, CoordinateValueType type) => PolylineEncoding.Denormalize(value, type);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Denormalize(value, type));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(DeltaCharCountPairs))]
    public void GetCharCount_Equals_Expected(int delta, int expected) {
        // Arrange & Act
        var charCount = PolylineEncoding.GetCharCount(delta);

        // Assert
        Assert.AreEqual(expected, charCount);
    }
}
