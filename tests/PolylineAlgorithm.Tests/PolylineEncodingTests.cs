//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Tests.Properties;

/// <summary>
/// Tests for <see cref="PolylineEncoding"/>.
/// </summary>
[TestClass]
public sealed class PolylineEncodingTests {
    #region Normalize Tests

    /// <summary>
    /// Tests that Normalize returns zero when value is zero.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_ZeroValue_ReturnsZero() {
        // Arrange
        double value = 0.0;
        uint precision = 5;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Tests that Normalize throws when value is NaN.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_NaNValue_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double value = double.NaN;
        uint precision = 5;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value, precision));
    }

    /// <summary>
    /// Tests that Normalize throws when value is positive infinity.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_PositiveInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double value = double.PositiveInfinity;
        uint precision = 5;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value, precision));
    }

    /// <summary>
    /// Tests that Normalize throws when value is negative infinity.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_NegativeInfinity_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double value = double.NegativeInfinity;
        uint precision = 5;

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value, precision));
    }

    /// <summary>
    /// Tests that Normalize with zero precision returns truncated value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_ZeroPrecision_ReturnsTruncatedValue() {
        // Arrange
        double value = 37.78903;
        uint precision = 0;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(37, result);
    }

    /// <summary>
    /// Tests that Normalize with zero precision truncates negative values correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_ZeroPrecisionNegative_ReturnsTruncatedValue() {
        // Arrange
        double value = -122.4123;
        uint precision = 0;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(-122, result);
    }

    /// <summary>
    /// Tests that Normalize with default precision normalizes positive value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_DefaultPrecisionPositive_ReturnsNormalizedValue() {
        // Arrange
        double value = 37.78903;
        uint precision = 5;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(3778903, result);
    }

    /// <summary>
    /// Tests that Normalize with default precision normalizes negative value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_DefaultPrecisionNegative_ReturnsNormalizedValue() {
        // Arrange
        double value = -122.4123;
        uint precision = 5;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(-12241230, result);
    }

    /// <summary>
    /// Tests that Normalize with precision 1 works correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_Precision1_ReturnsNormalizedValue() {
        // Arrange
        double value = 37.78903;
        uint precision = 1;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(377, result);
    }

    /// <summary>
    /// Tests that Normalize with precision 6 works correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_Precision6_ReturnsNormalizedValue() {
        // Arrange
        double value = 37.789034;
        uint precision = 6;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(37789034, result);
    }

    /// <summary>
    /// Tests that Normalize truncates fractional parts.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_ValueWithFractionalPart_TruncatesFractionalPart() {
        // Arrange
        double value = 37.789999;
        uint precision = 5;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(3778999, result);
    }

    /// <summary>
    /// Tests that Normalize handles very small values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_VerySmallValue_ReturnsNormalizedValue() {
        // Arrange
        double value = 0.00001;
        uint precision = 5;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(1, result);
    }

    /// <summary>
    /// Tests that Normalize handles negative very small values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Normalize_NegativeVerySmallValue_ReturnsNormalizedValue() {
        // Arrange
        double value = -0.00001;
        uint precision = 5;

        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(-1, result);
    }

    #endregion

    #region Denormalize Tests

    /// <summary>
    /// Tests that Denormalize returns zero when value is zero.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_ZeroValue_ReturnsZero() {
        // Arrange
        int value = 0;
        uint precision = 5;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(0.0, result);
    }

    /// <summary>
    /// Tests that Denormalize with zero precision returns same value as double.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_ZeroPrecision_ReturnsSameValue() {
        // Arrange
        int value = 37;
        uint precision = 0;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(37.0, result);
    }

    /// <summary>
    /// Tests that Denormalize with zero precision handles negative values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_ZeroPrecisionNegative_ReturnsSameValue() {
        // Arrange
        int value = -122;
        uint precision = 0;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(-122.0, result);
    }

    /// <summary>
    /// Tests that Denormalize with default precision denormalizes positive value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_DefaultPrecisionPositive_ReturnsDenormalizedValue() {
        // Arrange
        int value = 3778903;
        uint precision = 5;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(37.78903, result, 0.0000001);
    }

    /// <summary>
    /// Tests that Denormalize with default precision denormalizes negative value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_DefaultPrecisionNegative_ReturnsDenormalizedValue() {
        // Arrange
        int value = -12241230;
        uint precision = 5;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(-122.4123, result, 0.0000001);
    }

    /// <summary>
    /// Tests that Denormalize with precision 1 works correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_Precision1_ReturnsDenormalizedValue() {
        // Arrange
        int value = 377;
        uint precision = 1;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(37.7, result, 0.0000001);
    }

    /// <summary>
    /// Tests that Denormalize with precision 6 works correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_Precision6_ReturnsDenormalizedValue() {
        // Arrange
        int value = 37789034;
        uint precision = 6;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(37.789034, result, 0.0000001);
    }

    /// <summary>
    /// Tests that Denormalize handles very small values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_VerySmallValue_ReturnsDenormalizedValue() {
        // Arrange
        int value = 1;
        uint precision = 5;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(0.00001, result, 0.0000001);
    }

    /// <summary>
    /// Tests that Denormalize handles negative very small values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Denormalize_NegativeVerySmallValue_ReturnsDenormalizedValue() {
        // Arrange
        int value = -1;
        uint precision = 5;

        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(-0.00001, result, 0.0000001);
    }

    #endregion

    #region TryReadValue Tests

    /// <summary>
    /// Tests that TryReadValue returns false when position is at buffer length.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_PositionAtBufferLength_ReturnsFalse() {
        // Arrange
        ReadOnlyMemory<char> buffer = "_p~iF~ps|U".AsMemory();
        int delta = 0;
        int position = buffer.Length;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, delta);
    }

    /// <summary>
    /// Tests that TryReadValue returns false when position exceeds buffer length.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_PositionExceedsBufferLength_ReturnsFalse() {
        // Arrange
        ReadOnlyMemory<char> buffer = "_p~iF~ps|U".AsMemory();
        int delta = 0;
        int position = buffer.Length + 1;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, delta);
    }

    /// <summary>
    /// Tests that TryReadValue reads positive single-character value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_PositiveSingleChar_ReadsValueAndReturnsTrue() {
        // Arrange
        ReadOnlyMemory<char> buffer = "?".AsMemory();
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, delta);
        Assert.AreEqual(1, position);
    }

    /// <summary>
    /// Tests that TryReadValue reads multi-character positive value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_PositiveMultiChar_ReadsValueAndReturnsTrue() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int writePosition = 0;
        int expectedDelta = 3778903;
        
        // First write the value to get the correct encoding
        PolylineEncoding.TryWriteValue(expectedDelta, buffer, ref writePosition);
        ReadOnlyMemory<char> readBuffer = new string(buffer[..writePosition]).AsMemory();
        
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, readBuffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedDelta, delta);
        Assert.AreEqual(writePosition, position);
    }

    /// <summary>
    /// Tests that TryReadValue reads negative value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_NegativeValue_ReadsValueAndReturnsTrue() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int writePosition = 0;
        int expectedDelta = -12241230;
        
        // First write the value to get the correct encoding
        PolylineEncoding.TryWriteValue(expectedDelta, buffer, ref writePosition);
        ReadOnlyMemory<char> readBuffer = new string(buffer[..writePosition]).AsMemory();
        
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, readBuffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedDelta, delta);
        Assert.AreEqual(writePosition, position);
    }

    /// <summary>
    /// Tests that TryReadValue accumulates delta correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_WithExistingDelta_AccumulatesDelta() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int writePosition = 0;
        int valueDelta = 3778903;
        
        // First write the value to get the correct encoding
        PolylineEncoding.TryWriteValue(valueDelta, buffer, ref writePosition);
        ReadOnlyMemory<char> readBuffer = new string(buffer[..writePosition]).AsMemory();
        
        int delta = 100;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, readBuffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3779003, delta);
    }

    /// <summary>
    /// Tests that TryReadValue reads multiple values from buffer.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_MultipleValues_ReadsSequentially() {
        // Arrange
        Span<char> buffer = stackalloc char[20];
        int writePosition = 0;
        int expectedDelta1 = 3778903;
        int expectedDelta2 = -12241230;
        
        // Write both values
        PolylineEncoding.TryWriteValue(expectedDelta1, buffer, ref writePosition);
        PolylineEncoding.TryWriteValue(expectedDelta2, buffer, ref writePosition);
        ReadOnlyMemory<char> readBuffer = new string(buffer[..writePosition]).AsMemory();
        
        int delta1 = 0;
        int position = 0;

        // Act
        bool result1 = PolylineEncoding.TryReadValue(ref delta1, readBuffer, ref position);
        int delta2 = 0;
        bool result2 = PolylineEncoding.TryReadValue(ref delta2, readBuffer, ref position);

        // Assert
        Assert.IsTrue(result1);
        Assert.AreEqual(expectedDelta1, delta1);
        Assert.IsTrue(result2);
        Assert.AreEqual(expectedDelta2, delta2);
        Assert.AreEqual(writePosition, position);
    }

    /// <summary>
    /// Tests that TryReadValue returns false when buffer ends mid-value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_BufferEndsMidValue_ReturnsFalse() {
        // Arrange
        Span<char> fullBuffer = stackalloc char[10];
        int writePosition = 0;
        PolylineEncoding.TryWriteValue(3778903, fullBuffer, ref writePosition);
        
        // Create incomplete buffer (truncate last character)
        ReadOnlyMemory<char> buffer = new string(fullBuffer[..(writePosition - 1)]).AsMemory();
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(buffer.Length, position);
    }

    /// <summary>
    /// Tests that TryReadValue reads value from middle of buffer.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryReadValue_StartingFromMiddle_ReadsCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[20];
        int writePosition = 0;
        int expectedDelta1 = 3778903;
        int expectedDelta2 = -12241230;
        
        // Write both values
        PolylineEncoding.TryWriteValue(expectedDelta1, buffer, ref writePosition);
        int secondValuePosition = writePosition;
        PolylineEncoding.TryWriteValue(expectedDelta2, buffer, ref writePosition);
        ReadOnlyMemory<char> readBuffer = new string(buffer[..writePosition]).AsMemory();
        
        int delta = 0;
        int position = secondValuePosition; // Start from second value

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, readBuffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedDelta2, delta);
        Assert.AreEqual(writePosition, position);
    }

    #endregion

    #region TryWriteValue Tests

    /// <summary>
    /// Tests that TryWriteValue returns false when buffer is too small.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_BufferTooSmall_ReturnsFalse() {
        // Arrange
        Span<char> buffer = stackalloc char[2];
        int delta = 3778903;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, position);
    }

    /// <summary>
    /// Tests that TryWriteValue returns false when remaining buffer is too small.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_RemainingBufferTooSmall_ReturnsFalse() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = 3778903;
        int position = 8; // Only 2 chars remaining, need 5

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(8, position);
    }

    /// <summary>
    /// Tests that TryWriteValue writes zero correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_ZeroValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('?', buffer[0]);
    }

    /// <summary>
    /// Tests that TryWriteValue writes positive value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_PositiveValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = 3778903;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(position > 0);
        
        // Verify by reading back
        ReadOnlyMemory<char> readBuffer = new string(buffer[..position]).AsMemory();
        int readDelta = 0;
        int readPosition = 0;
        bool readResult = PolylineEncoding.TryReadValue(ref readDelta, readBuffer, ref readPosition);
        
        Assert.IsTrue(readResult);
        Assert.AreEqual(delta, readDelta);
    }

    /// <summary>
    /// Tests that TryWriteValue writes negative value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_NegativeValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = -12241230;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(position > 0);
        
        // Verify by reading back
        ReadOnlyMemory<char> readBuffer = new string(buffer[..position]).AsMemory();
        int readDelta = 0;
        int readPosition = 0;
        bool readResult = PolylineEncoding.TryReadValue(ref readDelta, readBuffer, ref readPosition);
        
        Assert.IsTrue(readResult);
        Assert.AreEqual(delta, readDelta);
    }

    /// <summary>
    /// Tests that TryWriteValue writes multiple values sequentially.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_MultipleValues_WritesSequentially() {
        // Arrange
        Span<char> buffer = stackalloc char[20];
        int position = 0;
        int delta1 = 3778903;
        int delta2 = -12241230;

        // Act
        bool result1 = PolylineEncoding.TryWriteValue(delta1, buffer, ref position);
        int midPosition = position;
        bool result2 = PolylineEncoding.TryWriteValue(delta2, buffer, ref position);

        // Assert
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        Assert.IsTrue(position > midPosition);
        
        // Verify by reading back both values
        ReadOnlyMemory<char> readBuffer = new string(buffer[..position]).AsMemory();
        int readDelta1 = 0;
        int readPosition = 0;
        PolylineEncoding.TryReadValue(ref readDelta1, readBuffer, ref readPosition);
        int readDelta2 = 0;
        PolylineEncoding.TryReadValue(ref readDelta2, readBuffer, ref readPosition);
        
        Assert.AreEqual(delta1, readDelta1);
        Assert.AreEqual(delta2, readDelta2);
    }

    /// <summary>
    /// Tests that TryWriteValue writes small positive value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_SmallPositiveValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = 1;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        
        // Verify by reading back
        ReadOnlyMemory<char> readBuffer = new string(buffer[..position]).AsMemory();
        int readDelta = 0;
        int readPosition = 0;
        PolylineEncoding.TryReadValue(ref readDelta, readBuffer, ref readPosition);
        Assert.AreEqual(delta, readDelta);
    }

    /// <summary>
    /// Tests that TryWriteValue writes small negative value correctly.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_SmallNegativeValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = -1;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        
        // Verify by reading back
        ReadOnlyMemory<char> readBuffer = new string(buffer[..position]).AsMemory();
        int readDelta = 0;
        int readPosition = 0;
        PolylineEncoding.TryReadValue(ref readDelta, readBuffer, ref readPosition);
        Assert.AreEqual(delta, readDelta);
    }

    /// <summary>
    /// Tests that TryWriteValue starts writing at specified position.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_NonZeroStartPosition_WritesAtCorrectPosition() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        buffer[0] = 'X';
        buffer[1] = 'Y';
        int delta = 0;
        int position = 2;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3, position);
        Assert.AreEqual('X', buffer[0]);
        Assert.AreEqual('Y', buffer[1]);
        Assert.AreEqual('?', buffer[2]);
    }

    /// <summary>
    /// Tests that TryWriteValue handles large positive values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_LargePositiveValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = int.MaxValue / 2;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(position > 0);
    }

    /// <summary>
    /// Tests that TryWriteValue handles large negative values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void TryWriteValue_LargeNegativeValue_WritesCorrectly() {
        // Arrange
        Span<char> buffer = stackalloc char[10];
        int delta = int.MinValue / 2;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(position > 0);
    }

    #endregion

    #region GetRequiredBufferSize Tests

    /// <summary>
    /// Tests that GetRequiredBufferSize returns 1 for zero value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_ZeroValue_ReturnsOne() {
        // Arrange
        int delta = 0;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns correct size for small positive value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_SmallPositiveValue_ReturnsOne() {
        // Arrange
        int delta = 1;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns correct size for small negative value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_SmallNegativeValue_ReturnsOne() {
        // Arrange
        int delta = -1;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns correct size for large positive value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_LargePositiveValue_ReturnsCorrectSize() {
        // Arrange
        int delta = 3778903;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(5, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns correct size for large negative value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_LargeNegativeValue_ReturnsCorrectSize() {
        // Arrange
        int delta = -12241230;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(5, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize handles maximum positive integer.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_MaxInt_ReturnsCorrectSize() {
        // Arrange
        int delta = int.MaxValue;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.IsTrue(size > 0);
        Assert.IsTrue(size <= 7); // Maximum size for int32
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize handles minimum negative integer.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_MinInt_ReturnsCorrectSize() {
        // Arrange
        int delta = int.MinValue;

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.IsTrue(size > 0);
        Assert.IsTrue(size <= 7); // Maximum size for int32
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns consistent size with TryWriteValue.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_ConsistentWithTryWriteValue_MatchesActualSize() {
        // Arrange
        int delta = 3778903;
        int expectedSize = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[expectedSize];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedSize, position);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize with undersized buffer causes TryWriteValue to fail.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_UndersizedBuffer_CausesTryWriteValueToFail() {
        // Arrange
        int delta = 3778903;
        int requiredSize = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[requiredSize - 1]; // One char too small
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, position);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns correct size for boundary value 15.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_BoundaryValue15_ReturnsOne() {
        // Arrange
        int delta = 15; // 15 << 1 = 30, which is less than 32 (Space)

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(1, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns correct size for boundary value 16.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetRequiredBufferSize_BoundaryValue16_ReturnsTwo() {
        // Arrange
        int delta = 16; // 16 << 1 = 32, which equals Space

        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(2, size);
    }

    #endregion

    #region ValidateFormat Tests

    /// <summary>
    /// Tests that ValidateFormat succeeds with a valid polyline.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateFormat_ValidPolyline_DoesNotThrow() {
        // Arrange
        string polyline = "_p~iF~ps|U_ulLnnqC_mqNvxq`@";

        // Act & Assert
        PolylineEncoding.ValidateFormat(polyline);
    }

    /// <summary>
    /// Tests that ValidateFormat throws when polyline contains invalid character.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateFormat_InvalidCharacter_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "_p~iF!ps|U";

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateFormat(polyline));
    }

    /// <summary>
    /// Tests that ValidateFormat throws when polyline has invalid block structure.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateFormat_InvalidBlockStructure_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "________"; // All continuation characters, no block terminator

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateFormat(polyline));
    }

    /// <summary>
    /// Tests that ValidateFormat succeeds with empty polyline ending with terminator.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateFormat_EmptyPolylineWithTerminator_DoesNotThrow() {
        // Arrange
        string polyline = "?"; // Single terminator character

        // Act & Assert
        PolylineEncoding.ValidateFormat(polyline);
    }

    /// <summary>
    /// Tests that ValidateFormat throws when block is too long.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateFormat_BlockTooLong_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "________?"; // 8 characters in block (max is 7)

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateFormat(polyline));
    }

    #endregion

    #region ValidateCharRange Tests

    /// <summary>
    /// Tests that ValidateCharRange succeeds with all valid characters.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_AllValidCharacters_DoesNotThrow() {
        // Arrange
        string polyline = "_p~iF~ps|U_ulLnnqC_mqNvxq`@";

        // Act & Assert
        PolylineEncoding.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that ValidateCharRange succeeds with minimum valid character.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_MinimumValidCharacter_DoesNotThrow() {
        // Arrange
        string polyline = "?"; // ASCII 63 (Min)

        // Act & Assert
        PolylineEncoding.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that ValidateCharRange succeeds with maximum valid character.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_MaximumValidCharacter_DoesNotThrow() {
        // Arrange
        string polyline = "~"; // ASCII 126 (Max)

        // Act & Assert
        PolylineEncoding.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that ValidateCharRange throws when character is below minimum.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_CharacterBelowMinimum_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = ">"; // ASCII 62 (below Min of 63)

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    /// <summary>
    /// Tests that ValidateCharRange throws when character is above maximum.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_CharacterAboveMaximum_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "\u007F"; // ASCII 127 (above Max of 126)

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    /// <summary>
    /// Tests that ValidateCharRange throws when invalid character is in middle of valid polyline.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_InvalidCharacterInMiddle_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "_p~iF!ps|U"; // '!' is ASCII 33 (below Min)

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    /// <summary>
    /// Tests that ValidateCharRange succeeds with empty polyline.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_EmptyPolyline_DoesNotThrow() {
        // Arrange
        string polyline = "";

        // Act & Assert
        PolylineEncoding.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that ValidateCharRange throws when invalid character is at end of polyline.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_InvalidCharacterAtEnd_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "_p~iF~ps|U!"; // '!' at end

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    /// <summary>
    /// Tests that ValidateCharRange succeeds with long polyline to trigger SIMD path.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_LongPolyline_DoesNotThrow() {
        // Arrange
        // Create a string long enough to trigger SIMD vectorization (typically 8-16 chars depending on platform)
        string polyline = "????????????????????????????????";

        // Act & Assert
        PolylineEncoding.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that ValidateCharRange throws when invalid character is in SIMD-processed section.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_InvalidCharacterInSimdSection_ThrowsInvalidPolylineException() {
        // Arrange
        // Create a long string with invalid character to trigger SIMD path detection
        string polyline = "????????!???????????????????????";

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    /// <summary>
    /// Tests that ValidateCharRange throws when invalid character is in scalar remainder section.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateCharRange_InvalidCharacterInScalarRemainder_ThrowsInvalidPolylineException() {
        // Arrange
        // Create a string that leaves remainder after SIMD processing
        string polyline = "????????????????\u007F"; // Valid chars + one invalid at end

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    #endregion

    #region ValidateBlockLength Tests

    /// <summary>
    /// Tests that ValidateBlockLength succeeds with single block.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_SingleBlock_DoesNotThrow() {
        // Arrange
        string polyline = "?"; // Single terminator

        // Act & Assert
        PolylineEncoding.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that ValidateBlockLength succeeds with multiple blocks.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_MultipleBlocks_DoesNotThrow() {
        // Arrange
        string polyline = "_p~iF~ps|U"; // Multiple blocks

        // Act & Assert
        PolylineEncoding.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that ValidateBlockLength succeeds with maximum length block.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_MaximumLengthBlock_DoesNotThrow() {
        // Arrange
        string polyline = "______?"; // 6 continuation chars + terminator (max length is 7 total)

        // Act & Assert
        PolylineEncoding.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that ValidateBlockLength throws when block exceeds maximum length.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_BlockExceedsMaximumLength_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "________?"; // 8 chars in block (exceeds max of 7)

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateBlockLength(polyline));
    }

    /// <summary>
    /// Tests that ValidateBlockLength throws when polyline does not end with block terminator.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_NoBlockTerminator_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "________"; // All continuation characters, no terminator

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateBlockLength(polyline));
    }

    /// <summary>
    /// Tests that ValidateBlockLength throws when empty polyline has no block terminator.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_EmptyPolyline_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "";

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateBlockLength(polyline));
    }

    /// <summary>
    /// Tests that ValidateBlockLength throws when block is too long in middle of polyline.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_TooLongBlockInMiddle_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "?________?"; // Valid block, then too-long block

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateBlockLength(polyline));
    }

    /// <summary>
    /// Tests that ValidateBlockLength succeeds with consecutive terminators.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_ConsecutiveTerminators_DoesNotThrow() {
        // Arrange
        string polyline = "??"; // Two consecutive terminators (two 1-char blocks)

        // Act & Assert
        PolylineEncoding.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that ValidateBlockLength succeeds with mixed block lengths.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_MixedBlockLengths_DoesNotThrow() {
        // Arrange
        string polyline = "?__?_____?"; // Blocks of length 1, 2, and 5

        // Act & Assert
        PolylineEncoding.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that ValidateBlockLength throws when second-to-last block is too long.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ValidateBlockLength_SecondToLastBlockTooLong_ThrowsInvalidPolylineException() {
        // Arrange
        string polyline = "________?_?"; // First block is 8 chars (too long)

        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateBlockLength(polyline));
    }

    #endregion
}
