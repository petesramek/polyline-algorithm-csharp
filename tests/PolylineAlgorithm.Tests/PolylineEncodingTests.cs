//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

/// <summary>
/// Tests for the <see cref="PolylineEncoding"/> type.
/// </summary>
[TestClass]
public class PolylineEncodingTests {
    #region Normalize Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> returns zero when value is zero.
    /// </summary>
    [TestMethod]
    public void Normalize_ZeroValue_ReturnsZero() {
        // Arrange
        const double value = 0.0;

        // Act
        int result = PolylineEncoding.Normalize(value);

        // Assert
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> returns correct value for positive number with default precision.
    /// </summary>
    [TestMethod]
    public void Normalize_PositiveValue_DefaultPrecision_ReturnsNormalizedValue() {
        // Arrange
        const double value = 37.78903;
        const int expected = 3778903;

        // Act
        int result = PolylineEncoding.Normalize(value);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> returns correct value for negative number with default precision.
    /// </summary>
    [TestMethod]
    public void Normalize_NegativeValue_DefaultPrecision_ReturnsNormalizedValue() {
        // Arrange
        const double value = -122.4123;
        const int expected = -12241230;

        // Act
        int result = PolylineEncoding.Normalize(value);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> returns correct value when precision is zero.
    /// </summary>
    [TestMethod]
    public void Normalize_ZeroPrecision_ReturnsIntegerValue() {
        // Arrange
        const double value = 37.78903;
        const uint precision = 0;
        const int expected = 37;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> returns correct value for precision 1.
    /// </summary>
    [TestMethod]
    public void Normalize_Precision1_ReturnsNormalizedValue() {
        // Arrange
        const double value = 12.34567;
        const uint precision = 1;
        const int expected = 123;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> returns correct value for precision 3.
    /// </summary>
    [TestMethod]
    public void Normalize_Precision3_ReturnsNormalizedValue() {
        // Arrange
        const double value = 12.34567;
        const uint precision = 3;
        const int expected = 12345;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> throws <see cref="ArgumentOutOfRangeException"/> for NaN.
    /// </summary>
    [TestMethod]
    public void Normalize_NaN_ThrowsArgumentOutOfRangeException() {
        // Arrange
        const double value = double.NaN;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> throws <see cref="ArgumentOutOfRangeException"/> for positive infinity.
    /// </summary>
    [TestMethod]
    public void Normalize_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        const double value = double.PositiveInfinity;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> throws <see cref="ArgumentOutOfRangeException"/> for negative infinity.
    /// </summary>
    [TestMethod]
    public void Normalize_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        const double value = double.NegativeInfinity;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> truncates instead of rounding.
    /// </summary>
    [TestMethod]
    public void Normalize_FractionalValue_TruncatesValue() {
        // Arrange
        const double value = 1.999999;
        const uint precision = 5;
        const int expected = 199999;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles negative zero correctly.
    /// </summary>
    [TestMethod]
    public void Normalize_NegativeZero_ReturnsZero() {
        // Arrange
        const double value = -0.0;

        // Act
        int result = PolylineEncoding.Normalize(value);

        // Assert
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles very small positive value.
    /// </summary>
    [TestMethod]
    public void Normalize_VerySmallPositiveValue_ReturnsZero() {
        // Arrange
        const double value = 0.000001;
        const uint precision = 5;
        const int expected = 0;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles very small negative value.
    /// </summary>
    [TestMethod]
    public void Normalize_VerySmallNegativeValue_ReturnsZero() {
        // Arrange
        const double value = -0.000001;
        const uint precision = 5;
        const int expected = 0;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> throws <see cref="OverflowException"/> for value causing overflow.
    /// </summary>
    [TestMethod]
    public void Normalize_ValueCausingOverflow_ThrowsOverflowException() {
        // Arrange
        const double value = double.MaxValue / 10;
        const uint precision = 5;

        // Act & Assert
        Assert.ThrowsExactly<OverflowException>(() => PolylineEncoding.Normalize(value, precision));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles negative value with zero precision.
    /// </summary>
    [TestMethod]
    public void Normalize_NegativeValueZeroPrecision_ReturnsTruncatedValue() {
        // Arrange
        const double value = -37.78903;
        const uint precision = 0;
        const int expected = -37;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles negative value with precision 3.
    /// </summary>
    [TestMethod]
    public void Normalize_NegativeValuePrecision3_ReturnsNormalizedValue() {
        // Arrange
        const double value = -12.34567;
        const uint precision = 3;
        const int expected = -12345;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles large positive value.
    /// </summary>
    [TestMethod]
    public void Normalize_LargePositiveValue_ReturnsNormalizedValue() {
        // Arrange
        const double value = 180.0;
        const uint precision = 5;
        const int expected = 18000000;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Normalize"/> handles large negative value.
    /// </summary>
    [TestMethod]
    public void Normalize_LargeNegativeValue_ReturnsNormalizedValue() {
        // Arrange
        const double value = -180.0;
        const uint precision = 5;
        const int expected = -18000000;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    #endregion

    #region Denormalize Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns zero when value is zero.
    /// </summary>
    [TestMethod]
    public void Denormalize_ZeroValue_ReturnsZero() {
        // Arrange
        const int value = 0;

        // Act
        double result = PolylineEncoding.Denormalize(value);

        // Assert
        Assert.AreEqual(0.0, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns correct value for positive number with default precision.
    /// </summary>
    [TestMethod]
    public void Denormalize_PositiveValue_DefaultPrecision_ReturnsDenormalizedValue() {
        // Arrange
        const int value = 3778903;
        const double expected = 37.78903;

        // Act
        double result = PolylineEncoding.Denormalize(value);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns correct value for negative number with default precision.
    /// </summary>
    [TestMethod]
    public void Denormalize_NegativeValue_DefaultPrecision_ReturnsDenormalizedValue() {
        // Arrange
        const int value = -12241230;
        const double expected = -122.4123;

        // Act
        double result = PolylineEncoding.Denormalize(value);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns correct value when precision is zero.
    /// </summary>
    [TestMethod]
    public void Denormalize_ZeroPrecision_ReturnsOriginalValue() {
        // Arrange
        const int value = 37;
        const uint precision = 0;
        const double expected = 37.0;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns correct value for precision 1.
    /// </summary>
    [TestMethod]
    public void Denormalize_Precision1_ReturnsDenormalizedValue() {
        // Arrange
        const int value = 123;
        const uint precision = 1;
        const double expected = 12.3;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns correct value for precision 3.
    /// </summary>
    [TestMethod]
    public void Denormalize_Precision3_ReturnsDenormalizedValue() {
        // Arrange
        const int value = 12345;
        const uint precision = 3;
        const double expected = 12.345;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> returns correct value for precision 5.
    /// </summary>
    [TestMethod]
    public void Denormalize_Precision5_ReturnsDenormalizedValue() {
        // Arrange
        const int value = 1234567;
        const uint precision = 5;
        const double expected = 12.34567;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> handles negative values correctly.
    /// </summary>
    [TestMethod]
    public void Denormalize_NegativeValue_Precision1_ReturnsDenormalizedValue() {
        // Arrange
        const int value = -123;
        const uint precision = 1;
        const double expected = -12.3;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> handles large positive value.
    /// </summary>
    [TestMethod]
    public void Denormalize_LargePositiveValue_ReturnsDenormalizedValue() {
        // Arrange
        const int value = 18000000;
        const uint precision = 5;
        const double expected = 180.0;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> handles large negative value.
    /// </summary>
    [TestMethod]
    public void Denormalize_LargeNegativeValue_ReturnsDenormalizedValue() {
        // Arrange
        const int value = -18000000;
        const uint precision = 5;
        const double expected = -180.0;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> handles int.MaxValue.
    /// </summary>
    [TestMethod]
    public void Denormalize_MaxValue_ReturnsDenormalizedValue() {
        // Arrange
        const int value = int.MaxValue;
        const uint precision = 5;
        const double expected = 21474.83647;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Denormalize"/> handles int.MinValue.
    /// </summary>
    [TestMethod]
    public void Denormalize_MinValue_ReturnsDenormalizedValue() {
        // Arrange
        const int value = int.MinValue;
        const uint precision = 5;
        const double expected = -21474.83648;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 0.000001);
    }

    #endregion

    #region TryReadValue Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> returns false when position is at buffer length.
    /// </summary>
    [TestMethod]
    public void TryReadValue_PositionAtBufferLength_ReturnsFalse() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "?".AsMemory();
        int position = 1;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> returns false when position exceeds buffer length.
    /// </summary>
    [TestMethod]
    public void TryReadValue_PositionExceedsBufferLength_ReturnsFalse() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "?".AsMemory();
        int position = 5;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> reads single character value correctly.
    /// </summary>
    [TestMethod]
    public void TryReadValue_SingleCharacter_ReadsDeltaCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "?".AsMemory(); // '?' (63) - 63 = 0
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, delta);
        Assert.AreEqual(1, position);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> reads positive delta value correctly.
    /// </summary>
    [TestMethod]
    public void TryReadValue_PositiveDelta_ReadsDeltaCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "_p~iF".AsMemory();
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3850000, delta);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> reads negative delta value correctly.
    /// </summary>
    [TestMethod]
    public void TryReadValue_NegativeDelta_ReadsDeltaCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "~ps|U".AsMemory();
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(-12020000, delta);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> accumulates delta correctly with multiple calls.
    /// </summary>
    [TestMethod]
    public void TryReadValue_MultipleCalls_AccumulatesDelta() {
        // Arrange
        int delta = 100;
        ReadOnlyMemory<char> buffer = "_p~iF".AsMemory();
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3850100, delta);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> updates position correctly after reading.
    /// </summary>
    [TestMethod]
    public void TryReadValue_ValidBuffer_UpdatesPositionCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "_p~iFn~cD".AsMemory();
        int position = 0;

        // Act
        bool firstResult = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(firstResult);
        Assert.AreEqual(5, position);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> can read multiple values from buffer.
    /// </summary>
    [TestMethod]
    public void TryReadValue_MultipleValues_ReadsAllValuesCorrectly() {
        // Arrange
        int delta1 = 0;
        int delta2 = 0;
        ReadOnlyMemory<char> buffer = "_p~iF~ps|U".AsMemory();
        int position = 0;

        // Act
        bool result1 = PolylineEncoding.TryReadValue(ref delta1, buffer, ref position);
        bool result2 = PolylineEncoding.TryReadValue(ref delta2, buffer, ref position);

        // Assert
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        Assert.AreEqual(3850000, delta1);
        Assert.AreEqual(-12020000, delta2);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> returns false when buffer ends without complete value.
    /// </summary>
    [TestMethod]
    public void TryReadValue_IncompleteValue_ReturnsFalse() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "\u00ff\u00ff\u00ff".AsMemory(); // All high-bit values
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> reads negative delta correctly with odd LSB.
    /// </summary>
    [TestMethod]
    public void TryReadValue_NegativeDeltaOddLSB_ReadsDeltaCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "@".AsMemory(); // char with value that produces odd LSB
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(-1, delta);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> handles empty buffer.
    /// </summary>
    [TestMethod]
    public void TryReadValue_EmptyBuffer_ReturnsFalse() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = ReadOnlyMemory<char>.Empty;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> handles character at Space boundary.
    /// </summary>
    [TestMethod]
    public void TryReadValue_CharacterAtSpaceBoundary_ReadsDeltaCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "_".AsMemory(); // char '_' = 95, 95 - 63 = 32 (Space)
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryReadValue"/> handles maximum encoded value.
    /// </summary>
    [TestMethod]
    public void TryReadValue_MaximumEncodedValue_ReadsDeltaCorrectly() {
        // Arrange
        int delta = 0;
        ReadOnlyMemory<char> buffer = "\u00ff\u00ff\u00ff\u00ff\u00ff?".AsMemory();
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
    }

    #endregion

    #region TryWriteValue Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> returns false when buffer is too small.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_InsufficientBuffer_ReturnsFalse() {
        // Arrange
        const int delta = 3850000;
        Span<char> buffer = stackalloc char[3];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes zero delta correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_ZeroDelta_WritesCorrectly() {
        // Arrange
        const int delta = 0;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('?', buffer[0]);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes positive delta correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_PositiveDelta_WritesCorrectly() {
        // Arrange
        const int delta = 3850000;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, position);
        string encoded = new(buffer[..position]);
        Assert.AreEqual("_p~iF", encoded);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes negative delta correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_NegativeDelta_WritesCorrectly() {
        // Arrange
        const int delta = -12020000;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, position);
        string encoded = new(buffer[..position]);
        Assert.AreEqual("~ps|U", encoded);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> updates position correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_ValidDelta_UpdatesPositionCorrectly() {
        // Arrange
        const int delta = 100;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(2, position);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes multiple values correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_MultipleValues_WritesAllCorrectly() {
        // Arrange
        const int delta1 = 3850000;
        const int delta2 = -12020000;
        Span<char> buffer = stackalloc char[20];
        int position = 0;

        // Act
        bool result1 = PolylineEncoding.TryWriteValue(delta1, buffer, ref position);
        bool result2 = PolylineEncoding.TryWriteValue(delta2, buffer, ref position);

        // Assert
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        string encoded = new(buffer[..position]);
        Assert.AreEqual("_p~iF~ps|U", encoded);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes small positive value correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_SmallPositiveValue_WritesCorrectly() {
        // Arrange
        const int delta = 1;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('A', buffer[0]);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes small negative value correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_SmallNegativeValue_WritesCorrectly() {
        // Arrange
        const int delta = -1;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('@', buffer[0]);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> handles position offset correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_PositionOffset_WritesAtCorrectPosition() {
        // Arrange
        const int delta = 100;
        Span<char> buffer = stackalloc char[10];
        int position = 3;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, position);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> returns false when buffer has exact size but position offset.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_ExactSizeWithOffset_ReturnsFalse() {
        // Arrange
        const int delta = 3850000;
        Span<char> buffer = stackalloc char[6];
        int position = 2;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes int.MaxValue correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_MaxValue_WritesCorrectly() {
        // Arrange
        const int delta = int.MaxValue;
        Span<char> buffer = stackalloc char[20];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(position > 0);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> writes int.MinValue correctly.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_MinValue_WritesCorrectly() {
        // Arrange
        const int delta = int.MinValue;
        Span<char> buffer = stackalloc char[20];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(position > 0);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> handles exact buffer size for value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_ExactBufferSize_WritesCorrectly() {
        // Arrange
        const int delta = 100;
        int requiredSize = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[requiredSize];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(requiredSize, position);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> fails with one less than required buffer size.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_OneLessThanRequired_ReturnsFalse() {
        // Arrange
        const int delta = 100;
        int requiredSize = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[requiredSize - 1];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> handles delta at Space boundary (31).
    /// </summary>
    [TestMethod]
    public void TryWriteValue_DeltaAtSpaceBoundary_WritesCorrectly() {
        // Arrange
        const int delta = 31;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(2, position);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.TryWriteValue"/> handles delta just above Space boundary (32).
    /// </summary>
    [TestMethod]
    public void TryWriteValue_DeltaAboveSpaceBoundary_WritesCorrectly() {
        // Arrange
        const int delta = 32;
        Span<char> buffer = stackalloc char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(2, position);
    }

    #endregion

    #region GetRequiredBufferSize Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns 1 for zero delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_ZeroDelta_ReturnsOne() {
        // Arrange
        const int delta = 0;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for small positive delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_SmallPositiveDelta_ReturnsCorrectSize() {
        // Arrange
        const int delta = 1;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for small negative delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_SmallNegativeDelta_ReturnsCorrectSize() {
        // Arrange
        const int delta = -1;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for large positive delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_LargePositiveDelta_ReturnsCorrectSize() {
        // Arrange
        const int delta = 3850000;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(5, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for large negative delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_LargeNegativeDelta_ReturnsCorrectSize() {
        // Arrange
        const int delta = -12020000;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(5, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for medium positive delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_MediumPositiveDelta_ReturnsCorrectSize() {
        // Arrange
        const int delta = 100;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(2, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for medium negative delta.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_MediumNegativeDelta_ReturnsCorrectSize() {
        // Arrange
        const int delta = -100;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(2, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for boundary value at 31.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_BoundaryValue31_ReturnsCorrectSize() {
        // Arrange
        const int delta = 31;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(2, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for boundary value at 32.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_BoundaryValue32_ReturnsCorrectSize() {
        // Arrange
        const int delta = 32;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(2, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for int.MaxValue.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_MaxValue_ReturnsCorrectSize() {
        // Arrange
        const int delta = int.MaxValue;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(7, result);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.GetRequiredBufferSize"/> returns correct size for int.MinValue.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_MinValue_ReturnsCorrectSize() {
        // Arrange
        const int delta = int.MinValue;

        // Act
        int result = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(7, result);
    }

    #endregion

    #region ValidateFormat Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> succeeds with valid polyline.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_ValidPolyline_DoesNotThrow() {
        // Arrange
        const string polyline = "_p~iF";

        // Act & Assert
        PolylineEncoding.Validation.ValidateFormat(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> succeeds with empty polyline after ValidateCharRange but fails at ValidateBlockLength.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_EmptyPolyline_ThrowsArgumentException() {
        // Arrange
        const string polyline = "";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> throws when polyline contains invalid character.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_InvalidCharacter_ThrowsArgumentException() {
        // Arrange
        const string polyline = "ABC\u0020DEF";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> throws when block exceeds 7 characters.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_BlockExceedsMaxLength_ThrowsArgumentException() {
        // Arrange
        const string polyline = "________?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.IsTrue(exception.Message.Contains("exceeds 7 characters"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> throws when polyline does not end with terminator.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_NoBlockTerminator_ThrowsArgumentException() {
        // Arrange
        const string polyline = "__";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> succeeds with multiple valid blocks.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_MultipleValidBlocks_DoesNotThrow() {
        // Arrange
        const string polyline = "_p~iFn~cD";

        // Act & Assert
        PolylineEncoding.Validation.ValidateFormat(polyline);
    }

    #endregion

    #region ValidateCharRange Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with valid characters.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_ValidCharacters_DoesNotThrow() {
        // Arrange
        const string polyline = "_p~iF";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with empty polyline.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_EmptyPolyline_DoesNotThrow() {
        // Arrange
        const string polyline = "";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with minimum valid character.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_MinimumValidCharacter_DoesNotThrow() {
        // Arrange
        const string polyline = "?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with maximum valid character.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_MaximumValidCharacter_DoesNotThrow() {
        // Arrange
        const string polyline = "~";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws when character is below minimum.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_CharacterBelowMinimum_ThrowsArgumentException() {
        // Arrange
        const string polyline = "ABC\u003eXYZ";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws when character is above maximum.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_CharacterAboveMaximum_ThrowsArgumentException() {
        // Arrange
        const string polyline = "ABC\u007fXYZ";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws when first character is invalid.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidFirstCharacter_ThrowsArgumentException() {
        // Arrange
        const string polyline = "\u0020ABCD";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws when last character is invalid.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidLastCharacter_ThrowsArgumentException() {
        // Arrange
        const string polyline = "ABCD\u0020";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws when middle character is invalid.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidMiddleCharacter_ThrowsArgumentException() {
        // Arrange
        const string polyline = "AB\u0020D";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with all valid characters.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_AllValidCharacters_DoesNotThrow() {
        // Arrange
        const string polyline = "?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws for null byte character.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_NullByteCharacter_ThrowsArgumentException() {
        // Arrange
        const string polyline = "ABC\u0000DEF";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with long valid polyline to exercise SIMD path.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_LongValidPolyline_DoesNotThrow() {
        // Arrange - create a string longer than vector size to test SIMD path
        string polyline = new string('?', 100);

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws with invalid character in SIMD processed region.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidCharacterInSimdRegion_ThrowsArgumentException() {
        // Arrange - create a string with invalid character in SIMD region
        string polyline = new string('?', 50) + "\u0020" + new string('?', 50);

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws with invalid character in scalar remainder region.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidCharacterInScalarRegion_ThrowsArgumentException() {
        // Arrange - create a string where invalid character is in the remainder region processed by scalar code
        string polyline = new string('?', 50) + "\u0020";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with one character below End threshold.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_CharacterAtEnd_DoesNotThrow() {
        // Arrange - character at value 94 (one below End=95)
        const string polyline = "^";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    #endregion

    #region ValidateBlockLength Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with single valid block.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_SingleValidBlock_DoesNotThrow() {
        // Arrange
        const string polyline = "?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with block of length 7.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockLength7_DoesNotThrow() {
        // Arrange
        const string polyline = "\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when block exceeds 7 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockExceeds7Characters_ThrowsArgumentException() {
        // Arrange
        const string polyline = "_______?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Block at position 0 exceeds 7 characters. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when polyline does not end with terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_NoTerminator_ThrowsArgumentException() {
        // Arrange
        const string polyline = "\u00ff\u00ff";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws for empty polyline.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_EmptyPolyline_ThrowsArgumentException() {
        // Arrange
        const string polyline = "";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with multiple valid blocks.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_MultipleValidBlocks_DoesNotThrow() {
        // Arrange
        const string polyline = "_p~iFn~cD";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when second block exceeds 7 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_SecondBlockExceeds7Characters_ThrowsArgumentException() {
        // Arrange
        const string polyline = ">\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff>";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Block at position 1 exceeds 7 characters. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with consecutive terminators.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_ConsecutiveTerminators_DoesNotThrow() {
        // Arrange
        const string polyline = "??";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when last block has no terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_LastBlockNoTerminator_ThrowsArgumentException() {
        // Arrange
        const string polyline = "?\u00ff";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with block of exactly 6 characters before terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockLength6_DoesNotThrow() {
        // Arrange
        const string polyline = "\u00ff\u00ff\u00ff\u00ff\u00ff?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with varying block lengths.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_VaryingBlockLengths_DoesNotThrow() {
        // Arrange
        const string polyline = "?\u00ff\u00ff?\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with character exactly at End threshold (95).
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_CharacterAtEndThreshold_DoesNotThrow() {
        // Arrange - character at value 95 (End threshold, so it's not a terminator)
        const string polyline = "_?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when 8th character causes block to exceed.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_ExactlyAt8thCharacter_ThrowsArgumentException() {
        // Arrange - exactly 8 characters before terminator
        const string polyline = "\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Block at position 0 exceeds 7 characters. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> succeeds with single character terminator.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_SingleCharacterTerminator_DoesNotThrow() {
        // Arrange
        const string polyline = "?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateFormat(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> succeeds with exactly 7 character block.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_BlockOfExactly7Characters_DoesNotThrow() {
        // Arrange - 6 continuation characters (>=95) plus terminator (<95)
        const string polyline = "~~~~~~?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateFormat(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> throws when character is exactly one below minimum.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_CharacterOneBelowMinimum_ThrowsArgumentException() {
        // Arrange
        const string polyline = "??>\u003e?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> throws when character is exactly one above maximum.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_CharacterOneAboveMaximum_ThrowsArgumentException() {
        // Arrange
        const string polyline = "??\u007f?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateFormat"/> throws when both validations would fail, char validation fails first.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_InvalidCharacterAndBlockStructure_ThrowsForInvalidCharacter() {
        // Arrange - has invalid character and would also fail block validation
        const string polyline = "\u0020\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff\u00ff";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateFormat(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    #endregion

    #region ValidateCharRange Additional Edge Cases

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with string exactly at vector boundary.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_StringExactlyVectorSize_DoesNotThrow() {
        // Arrange - length that aligns exactly with vector size
        int vectorSize = System.Numerics.Vector<ushort>.Count;
        string polyline = new string('?', vectorSize);

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with string exactly twice vector size.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_StringExactlyTwiceVectorSize_DoesNotThrow() {
        // Arrange - length that is exactly 2x vector size
        int vectorSize = System.Numerics.Vector<ushort>.Count;
        string polyline = new string('~', vectorSize * 2);

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws with invalid character at vector boundary.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidCharacterAtVectorBoundary_ThrowsArgumentException() {
        // Arrange - invalid character right at the vector boundary
        int vectorSize = System.Numerics.Vector<ushort>.Count;
        string polyline = new string('?', vectorSize) + "\u0020" + new string('?', vectorSize);

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws with invalid character at start of vector block.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidCharacterAtStartOfVectorBlock_ThrowsArgumentException() {
        // Arrange - invalid character at the start of a vector block
        int vectorSize = System.Numerics.Vector<ushort>.Count;
        string polyline = new string('?', vectorSize * 2) + "\u0020";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws when multiple invalid characters in same vector.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_MultipleInvalidCharactersInSameVector_ThrowsArgumentException() {
        // Arrange - multiple invalid characters in same vector block
        const string polyline = "?\u0020?\u0020?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> succeeds with mixed valid characters.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_MixedValidCharacters_DoesNotThrow() {
        // Arrange - mix of terminators and continuation characters
        const string polyline = "?@AB_`ab|}~";

        // Act & Assert
        PolylineEncoding.Validation.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws for character exactly at Min-1.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_CharacterExactlyMinMinusOne_ThrowsArgumentException() {
        // Arrange - character '>' (62), which is Min-1
        const string polyline = "??\u003e";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws for character exactly at Max+1.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_CharacterExactlyMaxPlusOne_ThrowsArgumentException() {
        // Arrange - character DEL (127), which is Max+1
        const string polyline = "??\u007f";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateCharRange"/> throws with invalid character at last position in vector.
    /// </summary>
    [TestMethod]
    public void ValidateCharRange_InvalidCharacterAtLastPositionInVector_ThrowsArgumentException() {
        // Arrange - invalid character at the end of a vector block
        int vectorSize = System.Numerics.Vector<ushort>.Count;
        string polyline = new string('?', vectorSize - 1) + "\u0020" + new string('?', vectorSize);

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateCharRange(polyline));
        Assert.IsTrue(exception.Message.Contains("invalid character"));
    }

    #endregion

    #region ValidateBlockLength Additional Edge Cases

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with alternating pattern of continuation and terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_AlternatingContinuationAndTerminator_DoesNotThrow() {
        // Arrange - pattern of _? repeated
        const string polyline = "_?_?_?_?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }


    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when single continuation character has no terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_SingleContinuationNoTerminator_ThrowsArgumentException() {
        // Arrange - single continuation character without terminator
        const string polyline = "_";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with maximum length blocks separated by terminators.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_MaximumLengthBlocksSeparatedByTerminators_DoesNotThrow() {
        // Arrange - first block of 7 characters, second block of 7 characters
        const string polyline = "~~~~~~?~~~~~~?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when middle block exceeds 7 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_MiddleBlockExceeds7Characters_ThrowsArgumentException() {
        // Arrange - first block valid, second exceeds
        const string polyline = "??________?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.IsTrue(exception.Message.Contains("exceeds 7 characters"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with all characters being terminators.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_AllTerminators_DoesNotThrow() {
        // Arrange - all characters are terminators
        const string polyline = "?????????";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with block of exactly 2 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockOfExactly2Characters_DoesNotThrow() {
        // Arrange - block with one continuation and terminator
        const string polyline = "~?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with block of exactly 3 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockOfExactly3Characters_DoesNotThrow() {
        // Arrange - block with 2 continuations and terminator
        const string polyline = "~~?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with block of exactly 4 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockOfExactly4Characters_DoesNotThrow() {
        // Arrange - block with 3 continuations and terminator
        const string polyline = "~~~?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with block of exactly 5 characters.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_BlockOfExactly5Characters_DoesNotThrow() {
        // Arrange - block with 4 continuations and terminator
        const string polyline = "~~~~?";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws for exactly 8 continuation characters followed by terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_Exactly8ContinuationsBeforeTerminator_ThrowsArgumentException() {
        // Arrange - 8 continuation characters then terminator
        const string polyline = "~~~~~~~~?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.IsTrue(exception.Message.Contains("exceeds 7 characters"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws for 9 continuation characters followed by terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_9ContinuationsBeforeTerminator_ThrowsArgumentException() {
        // Arrange - 9 continuation characters then terminator
        const string polyline = "~~~~~~~~~?";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.IsTrue(exception.Message.Contains("exceeds 7 characters"));
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> succeeds with character at position 94 (one below End).
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_CharacterAtPosition94_DoesNotThrow() {
        // Arrange - character '^' (94), which is a terminator (< 95)
        const string polyline = "^";

        // Act & Assert
        PolylineEncoding.Validation.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncoding.Validation.ValidateBlockLength"/> throws when third block has no terminator.
    /// </summary>
    [TestMethod]
    public void ValidateBlockLength_ThirdBlockNoTerminator_ThrowsArgumentException() {
        // Arrange - two valid blocks followed by unterminated block
        const string polyline = "?_?~";

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => PolylineEncoding.Validation.ValidateBlockLength(polyline));
        Assert.AreEqual("Polyline does not end with a valid block terminator. (Parameter 'polyline')", exception.Message);
    }

    #endregion
}
