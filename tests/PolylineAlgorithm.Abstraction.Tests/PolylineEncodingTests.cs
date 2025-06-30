namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

[TestClass]
public class PolylineEncodingTests {
    public static IEnumerable<(int variance, string polyline)> VariancePolylinePairs => [
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
        (-16777215,"|~~~^")
    ];

    public static IEnumerable<(double denormalized, int normalized, PolylineEncoding.ValueType)> DenormalizedNormalizedPairs => [
        (0,0, PolylineEncoding.ValueType.Latitude),
        (0,0, PolylineEncoding.ValueType.Longitude),
        (1.23456,123456, PolylineEncoding.ValueType.Latitude),
        (-1.23456,-123456, PolylineEncoding.ValueType.Latitude),
        (1.23456,123456, PolylineEncoding.ValueType.Longitude),
        (-1.23456,-123456, PolylineEncoding.ValueType.Longitude),
        (90,9000000, PolylineEncoding.ValueType.Latitude),
        (-90,-9000000, PolylineEncoding.ValueType.Latitude),
        (90,9000000, PolylineEncoding.ValueType.Longitude),
        (-90,-9000000, PolylineEncoding.ValueType.Longitude),
        (180,18000000, PolylineEncoding.ValueType.Longitude),
        (-180,-18000000, PolylineEncoding.ValueType.Longitude)
    ];

    public static IEnumerable<(double denormalized, PolylineEncoding.ValueType)> DenormalizedOutOfRangeValues => [
        (90.00001,PolylineEncoding.ValueType.Latitude),
        (-90.00001,PolylineEncoding.ValueType.Latitude),
        (180.00001,PolylineEncoding.ValueType.Longitude),
        (-180.00001,PolylineEncoding.ValueType.Longitude),
        (double.NaN,PolylineEncoding.ValueType.Latitude),
        (double.NaN,PolylineEncoding.ValueType.Longitude),
        (double.MinValue,PolylineEncoding.ValueType.Latitude),
        (double.MaxValue,PolylineEncoding.ValueType.Latitude),
        (double.MinValue,PolylineEncoding.ValueType.Longitude),
        (double.MaxValue,PolylineEncoding.ValueType.Longitude),
        (double.NegativeInfinity,PolylineEncoding.ValueType.Latitude),
        (double.PositiveInfinity,PolylineEncoding.ValueType.Latitude),
        (double.NegativeInfinity,PolylineEncoding.ValueType.Longitude),
        (double.PositiveInfinity,PolylineEncoding.ValueType.Longitude),
    ];

    public static IEnumerable<(int normalized, PolylineEncoding.ValueType)> NormalizedOutOfRangeValues => [
        (9000001,PolylineEncoding.ValueType.Latitude),
        (-9000001,PolylineEncoding.ValueType.Latitude),
        (18000001,PolylineEncoding.ValueType.Longitude),
        (-18000001,PolylineEncoding.ValueType.Longitude),
        (int.MinValue,PolylineEncoding.ValueType.Latitude),
        (int.MaxValue,PolylineEncoding.ValueType.Latitude),
        (int.MinValue,PolylineEncoding.ValueType.Longitude),
        (int.MaxValue,PolylineEncoding.ValueType.Longitude),
    ];

    [TestMethod]
    [DynamicData(nameof(DenormalizedNormalizedPairs), DynamicDataSourceType.Property)]
    public void Normalize_Ok(double denormalized, int expected, PolylineEncoding.ValueType type) {
        // Arrange & Act
        int result = PolylineEncoding.Normalize(denormalized, type);

        // Assert
        Assert.AreEqual(expected, result);
    }


    [TestMethod]
    [DynamicData(nameof(DenormalizedNormalizedPairs), DynamicDataSourceType.Property)]
    public void Denormalize_Ok(double expected, int normalized, PolylineEncoding.ValueType type) {
        // Arrange & Act
        double result = PolylineEncoding.Denormalize(normalized, type);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DynamicData(nameof(VariancePolylinePairs), DynamicDataSourceType.Property)]
    public void TryWriteValue_Ok(int variance, string expected) {
        // Arrange
        int position = 0;
        Span<char> buffer = stackalloc char[6];

        // Act
        bool result = PolylineEncoding.TryWriteValue(variance, ref buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected.Length, position);
        Assert.AreEqual(expected, buffer[..position].ToString());
    }

    [TestMethod]
    [DynamicData(nameof(VariancePolylinePairs), DynamicDataSourceType.Property)]
    public void TryReadValue_Ok(int expected, string polyline) {
        // Arrange
        int position = 0;
        int variance = 0;
        var buffer = polyline.AsMemory();

        // Act
        bool result = PolylineEncoding.TryReadValue(ref variance, ref buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(buffer.Length, position);
        Assert.AreEqual(expected, variance);
    }

    [TestMethod]
    [DynamicData(nameof(VariancePolylinePairs), DynamicDataSourceType.Property)]
    public void TryWriteValue_BufferExactlyRightSize_Ok(int variance, string _) {
        // Arrange
        int position = 0;
        int required = PolylineEncoding.GetCharCount(variance);
        Span<char> buffer = stackalloc char[required];

        // Act
        bool result = PolylineEncoding.TryWriteValue(variance, ref buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(required, position);
    }

    [TestMethod]
    [DynamicData(nameof(VariancePolylinePairs), DynamicDataSourceType.Property)]
    public void TryWriteValue_BufferTooSmall_ReturnsFalse(int variance, string _) {
        // Arrange
        int position = 0;
        int required = PolylineEncoding.GetCharCount(variance);
        Span<char> buffer = stackalloc char[required - 1];

        // Act
        bool result = PolylineEncoding.TryWriteValue(variance, ref buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryReadValue_EmptyBuffer_ReturnsFalse() {
        // Arrange
        int variance = 0;
        int position = 0;
        ReadOnlyMemory<char> buffer = Memory<char>.Empty;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref variance, ref buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TryReadValue_MalformedBuffer_ReturnsFalseAndDoesNotChangeVariance() {
        //Arrange
        int position = 0;
        int variance = 42;
        int expected = variance;
        // Buffer with a char that will never finish a value (simulate incomplete encoding)
        char[] chars = [(char)(127)]; // 127 - 63 = 64, which is >= 32, so loop never breaks
        ReadOnlyMemory<char> buffer = chars.AsMemory();

        // Act
        bool result = PolylineEncoding.TryReadValue(ref variance, ref buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(expected, variance);
    }

    [TestMethod]
    [DynamicData(nameof(DenormalizedOutOfRangeValues), DynamicDataSourceType.Property)]
    public void Normalize_Throws_ArgumentOutOfRangeException(double value, PolylineEncoding.ValueType type) {
        // Arrange
        static int Normalize(double value, PolylineEncoding.ValueType type) => PolylineEncoding.Normalize(value, type);
       
        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Normalize(value, type));
    }

    [TestMethod]
    [DynamicData(nameof(NormalizedOutOfRangeValues), DynamicDataSourceType.Property)]
    public void Denormalize_Throws_ArgumentOutOfRangeException(int value, PolylineEncoding.ValueType type) {
        // Arrange
        static double Denormalize(int value, PolylineEncoding.ValueType type) => PolylineEncoding.Denormalize(value, type);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => Denormalize(value, type));
    }

    [TestMethod]
    public void GetCharCount_Covers_All_Cases() {
        Assert.AreEqual(1, PolylineEncoding.GetCharCount(0));
        Assert.AreEqual(1, PolylineEncoding.GetCharCount(15));
        Assert.AreEqual(1, PolylineEncoding.GetCharCount(-16));
        Assert.AreEqual(2, PolylineEncoding.GetCharCount(16));
        Assert.AreEqual(2, PolylineEncoding.GetCharCount(-17));
        Assert.AreEqual(2, PolylineEncoding.GetCharCount(511));
        Assert.AreEqual(2, PolylineEncoding.GetCharCount(-512));
        Assert.AreEqual(3, PolylineEncoding.GetCharCount(512));
        Assert.AreEqual(3, PolylineEncoding.GetCharCount(-513));
        Assert.AreEqual(3, PolylineEncoding.GetCharCount(16383));
        Assert.AreEqual(3, PolylineEncoding.GetCharCount(-16384));
        Assert.AreEqual(4, PolylineEncoding.GetCharCount(16384));
        Assert.AreEqual(4, PolylineEncoding.GetCharCount(-16385));
        Assert.AreEqual(4, PolylineEncoding.GetCharCount(524287));
        Assert.AreEqual(4, PolylineEncoding.GetCharCount(-524288));
        Assert.AreEqual(5, PolylineEncoding.GetCharCount(524288));
        Assert.AreEqual(5, PolylineEncoding.GetCharCount(-524289));
        Assert.AreEqual(5, PolylineEncoding.GetCharCount(16777215));
        Assert.AreEqual(5, PolylineEncoding.GetCharCount(-16777216));
        Assert.AreEqual(6, PolylineEncoding.GetCharCount(16777216));
        Assert.AreEqual(6, PolylineEncoding.GetCharCount(-16777217));
        Assert.AreEqual(6, PolylineEncoding.GetCharCount(int.MaxValue));
        Assert.AreEqual(6, PolylineEncoding.GetCharCount(int.MinValue));
    }
}
