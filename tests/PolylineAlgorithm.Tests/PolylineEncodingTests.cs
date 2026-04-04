//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;

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
    public void Normalize_With_Zero_Value_Returns_Zero() {
        // Act
        int result = PolylineEncoding.Normalize(0.0);

        // Assert
        Assert.AreEqual(0, result);
    }

    /// <summary>
    /// Tests that Normalize throws <see cref="ArgumentOutOfRangeException"/> when value is not finite.
    /// </summary>
    [TestMethod]
    [DataRow(double.NaN)]
    [DataRow(double.PositiveInfinity)]
    [DataRow(double.NegativeInfinity)]
    public void Normalize_With_NonFinite_Value_Throws_ArgumentOutOfRangeException(double value) {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PolylineEncoding.Normalize(value, 5));
    }

    /// <summary>
    /// Tests that Normalize returns the expected normalized integer for the given value and precision.
    /// </summary>
    [TestMethod]
    [DataRow(37.78903, 0u, 37)]
    [DataRow(-122.4123, 0u, -122)]
    [DataRow(37.78903, 5u, 3778903)]
    [DataRow(-122.4123, 5u, -12241230)]
    [DataRow(37.78903, 1u, 377)]
    [DataRow(37.789034, 6u, 37789034)]
    [DataRow(37.789999, 5u, 3778999)]
    [DataRow(0.00001, 5u, 1)]
    [DataRow(-0.00001, 5u, -1)]
    public void Normalize_With_Value_And_Precision_Returns_Expected_Normalized_Value(double value, uint precision, int expected) {
        // Act
        int result = PolylineEncoding.Normalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result);
    }

    #endregion

    #region Denormalize Tests

    /// <summary>
    /// Tests that Denormalize returns zero when value is zero.
    /// </summary>
    [TestMethod]
    public void Denormalize_With_Zero_Value_Returns_Zero() {
        // Act
        double result = PolylineEncoding.Denormalize(0);

        // Assert
        Assert.AreEqual(0.0, result);
    }

    /// <summary>
    /// Tests that Denormalize returns the expected floating-point value for the given integer and precision.
    /// </summary>
    [TestMethod]
    [DataRow(37, 0u, 37.0)]
    [DataRow(-122, 0u, -122.0)]
    [DataRow(3778903, 5u, 37.78903)]
    [DataRow(-12241230, 5u, -122.4123)]
    [DataRow(377, 1u, 37.7)]
    [DataRow(37789034, 6u, 37.789034)]
    [DataRow(1, 5u, 0.00001)]
    [DataRow(-1, 5u, -0.00001)]
    public void Denormalize_With_Value_And_Precision_Returns_Expected_Denormalized_Value(int value, uint precision, double expected) {
        // Act
        double result = PolylineEncoding.Denormalize(value, precision);

        // Assert
        Assert.AreEqual(expected, result, 1e-7);
    }

    #endregion

    #region TryReadValue Tests

    /// <summary>
    /// Tests that TryReadValue returns false when position is at buffer length.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Position_At_Buffer_Length_Returns_False() {
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
    public void TryReadValue_With_Position_Exceeds_Buffer_Length_Returns_False() {
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
    /// Tests that TryReadValue reads a positive single-character encoded value.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Positive_Single_Char_Reads_Value_And_Returns_True() {
        // Arrange
        // Encode value 5: zigzag = 10 = 0x0A; char = 10 + 63 = 73 = 'I'
        ReadOnlyMemory<char> buffer = "I".AsMemory(); // Single char encoding of value 5
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, delta);
        Assert.AreEqual(1, position);
    }

    /// <summary>
    /// Tests that TryReadValue reads a positive multi-character encoded value.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Positive_Multi_Char_Reads_Value_And_Returns_True() {
        // Arrange
        // _p~iF encodes latitude 38.5 (normalized = 3850000, zigzag = 7700000)
        ReadOnlyMemory<char> buffer = "_p~iF".AsMemory();
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(3850000, delta);
        Assert.AreEqual(5, position);
    }

    /// <summary>
    /// Tests that TryReadValue reads a negative encoded value.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Negative_Value_Reads_Value_And_Returns_True() {
        // Arrange
        // ~ps|U encodes longitude -120.2 (normalized = -12020000, zigzag encodes negative)
        ReadOnlyMemory<char> buffer = "~ps|U".AsMemory();
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(-12020000, delta);
        Assert.AreEqual(5, position);
    }

    /// <summary>
    /// Tests that TryReadValue accumulates into existing delta.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Existing_Delta_Accumulates_Delta() {
        // Arrange
        ReadOnlyMemory<char> buffer = "I".AsMemory(); // encodes 5
        int delta = 10; // existing delta
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(15, delta); // 10 + 5 = 15
    }

    /// <summary>
    /// Tests that TryReadValue reads multiple values sequentially from the buffer.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Multiple_Values_Reads_Sequentially() {
        // Arrange - "_p~iF~ps|U" encodes lat 38.5 then delta lon -120.2
        ReadOnlyMemory<char> buffer = "_p~iF~ps|U".AsMemory();
        int delta = 0;
        int position = 0;

        // Act - read first value
        bool first = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);
        int firstDelta = delta;

        // read second value
        bool second = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(first);
        Assert.IsTrue(second);
        Assert.AreEqual(3850000, firstDelta);
        Assert.AreEqual(10, position); // consumed all 10 chars
    }

    /// <summary>
    /// Tests that TryReadValue returns false when buffer ends mid-value.
    /// </summary>
    [TestMethod]
    public void TryReadValue_With_Buffer_Ends_Mid_Value_Returns_False() {
        // Arrange - truncate a multi-char encoding
        ReadOnlyMemory<char> buffer = "_p~".AsMemory(); // incomplete multi-char encoding
        int delta = 0;
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that TryReadValue correctly reads from a non-zero starting position.
    /// </summary>
    [TestMethod]
    public void TryReadValue_Starting_From_Middle_Reads_Correctly() {
        // Arrange - "_p~iF~ps|U": start at position 5 to read the longitude value
        ReadOnlyMemory<char> buffer = "_p~iF~ps|U".AsMemory();
        int delta = 0;
        int position = 5;

        // Act
        bool result = PolylineEncoding.TryReadValue(ref delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(-12020000, delta);
        Assert.AreEqual(10, position);
    }

    #endregion

    #region TryWriteValue Tests

    /// <summary>
    /// Tests that TryWriteValue returns false when the buffer is too small.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Buffer_Too_Small_Returns_False() {
        // Arrange
        Span<char> buffer = [];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(3850000, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, position);
    }

    /// <summary>
    /// Tests that TryWriteValue returns false when the remaining buffer is too small.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Remaining_Buffer_Too_Small_Returns_False() {
        // Arrange - need 5 chars for 3850000, but only 3 remain
        Span<char> buffer = new char[3];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(3850000, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, position);
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes zero.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Zero_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(0, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('?', buffer[0]); // 0 + 63 = '?'
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes a positive value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Positive_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(3850000, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, position);
        Assert.AreEqual("_p~iF", new string(buffer[..5]));
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes a negative value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Negative_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(-12020000, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(5, position);
        Assert.AreEqual("~ps|U", new string(buffer[..5]));
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes multiple values sequentially.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Multiple_Values_Writes_Sequentially() {
        // Arrange
        Span<char> buffer = new char[20];
        int position = 0;

        // Act
        bool first = PolylineEncoding.TryWriteValue(3850000, buffer, ref position);
        bool second = PolylineEncoding.TryWriteValue(-12020000, buffer, ref position);

        // Assert
        Assert.IsTrue(first);
        Assert.IsTrue(second);
        Assert.AreEqual("_p~iF~ps|U", new string(buffer[..10]));
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes a small positive value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Small_Positive_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(5, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('I', buffer[0]); // zigzag(5) = 10; 10 + 63 = 73 = 'I'
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes a small negative value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Small_Negative_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(-5, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, position);
        Assert.AreEqual('H', buffer[0]); // zigzag(-5) = 9; 9 + 63 = 72 = 'H'
    }

    /// <summary>
    /// Tests that TryWriteValue writes at the correct non-zero starting position.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Non_Zero_Start_Position_Writes_At_Correct_Position() {
        // Arrange
        Span<char> buffer = new char[20];
        int position = 5;

        // Act
        bool result = PolylineEncoding.TryWriteValue(5, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(6, position);
        Assert.AreEqual('I', buffer[5]);
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes a large positive value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Large_Positive_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;
        const int delta = 3778903;
        int expectedSize = PolylineEncoding.GetRequiredBufferSize(delta);

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedSize, position);
    }

    /// <summary>
    /// Tests that TryWriteValue correctly encodes a large negative value.
    /// </summary>
    [TestMethod]
    public void TryWriteValue_With_Large_Negative_Value_Writes_Correctly() {
        // Arrange
        Span<char> buffer = new char[10];
        int position = 0;
        const int delta = -12241230;
        int expectedSize = PolylineEncoding.GetRequiredBufferSize(delta);

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedSize, position);
    }

    #endregion

    #region GetRequiredBufferSize Tests

    /// <summary>
    /// Tests that GetRequiredBufferSize returns the expected character count for the given delta.
    /// </summary>
    [TestMethod]
    [DataRow(0, 1)]
    [DataRow(1, 1)]
    [DataRow(-1, 1)]
    [DataRow(15, 1)]
    [DataRow(16, 2)]
    [DataRow(3778903, 5)]
    [DataRow(-12241230, 5)]
    public void GetRequiredBufferSize_Returns_Expected_Size(int delta, int expectedSize) {
        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(delta);

        // Assert
        Assert.AreEqual(expectedSize, size);
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns a valid size for the maximum positive integer.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_With_Max_Int_Returns_Correct_Size() {
        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(int.MaxValue);

        // Assert
        Assert.IsGreaterThan(0, size);
        Assert.IsLessThanOrEqualTo(7, size); // Maximum size for int32
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize returns a valid size for the minimum negative integer.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_With_Min_Int_Returns_Correct_Size() {
        // Act
        int size = PolylineEncoding.GetRequiredBufferSize(int.MinValue);

        // Assert
        Assert.IsGreaterThan(0, size);
        Assert.IsLessThanOrEqualTo(7, size); // Maximum size for int32
    }

    /// <summary>
    /// Tests that GetRequiredBufferSize is consistent with the actual bytes written by TryWriteValue.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_Consistent_With_TryWriteValue_Matches_Actual_Size() {
        // Arrange
        const int delta = 3778903;
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
    /// Tests that an undersized buffer causes TryWriteValue to fail.
    /// </summary>
    [TestMethod]
    public void GetRequiredBufferSize_With_Undersized_Buffer_Causes_TryWriteValue_To_Fail() {
        // Arrange
        const int delta = 3778903;
        int requiredSize = PolylineEncoding.GetRequiredBufferSize(delta);
        Span<char> buffer = stackalloc char[requiredSize - 1]; // One char too small
        int position = 0;

        // Act
        bool result = PolylineEncoding.TryWriteValue(delta, buffer, ref position);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, position);
    }

    #endregion

    #region ValidateFormat Tests

    /// <summary>
    /// Tests that ValidateFormat succeeds with a valid polyline.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_With_Valid_Polyline_Does_Not_Throw() {
        // Act & Assert
        PolylineEncoding.ValidateFormat("_p~iF~ps|U_ulLnnqC_mqNvxq`@");
    }

    /// <summary>
    /// Tests that ValidateFormat throws when polyline contains an invalid character.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_With_Invalid_Character_Throws_InvalidPolylineException() {
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateFormat("_p~iF!ps|U"));
    }

    /// <summary>
    /// Tests that ValidateFormat throws when polyline has invalid block structure.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_With_Invalid_Block_Structure_Throws_InvalidPolylineException() {
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateFormat("________")); // all continuation chars, no terminator
    }

    /// <summary>
    /// Tests that ValidateFormat succeeds with a single terminator character.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_With_Single_Terminator_Does_Not_Throw() {
        // Act & Assert
        PolylineEncoding.ValidateFormat("?");
    }

    /// <summary>
    /// Tests that ValidateFormat throws when a block exceeds maximum length.
    /// </summary>
    [TestMethod]
    public void ValidateFormat_With_Block_Too_Long_Throws_InvalidPolylineException() {
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateFormat("________?")); // 8-char block (max is 7)
    }

    #endregion

    #region ValidateCharRange Tests

    /// <summary>
    /// Tests that ValidateCharRange succeeds for valid polyline strings.
    /// </summary>
    [TestMethod]
    [DataRow("_p~iF~ps|U_ulLnnqC_mqNvxq`@")]
    [DataRow("?")]  // min valid char (ASCII 63)
    [DataRow("~")]  // max valid char (ASCII 126)
    [DataRow("")]   // empty is valid
    [DataRow("????????????????????????????????")] // long string to trigger SIMD path
    public void ValidateCharRange_With_Valid_Polyline_Does_Not_Throw(string polyline) {
        // Act & Assert
        PolylineEncoding.ValidateCharRange(polyline);
    }

    /// <summary>
    /// Tests that ValidateCharRange throws <see cref="InvalidPolylineException"/> for polylines containing invalid characters.
    /// </summary>
    [TestMethod]
    [DataRow(">")]                              // ASCII 62 (below min of 63)
    [DataRow("\u007F")]                        // ASCII 127 (above max of 126)
    [DataRow("_p~iF!ps|U")]                    // '!' in middle (ASCII 33)
    [DataRow("_p~iF~ps|U!")]                   // '!' at end
    [DataRow("????????!???????????????????????")]  // invalid in SIMD section
    [DataRow("????????????????\u007F")]           // invalid in scalar remainder
    public void ValidateCharRange_With_Invalid_Character_Throws_InvalidPolylineException(string polyline) {
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateCharRange(polyline));
    }

    #endregion

    #region ValidateBlockLength Tests

    /// <summary>
    /// Tests that ValidateBlockLength succeeds for valid block structures.
    /// </summary>
    [TestMethod]
    [DataRow("?")]          // single terminator
    [DataRow("_p~iF~ps|U")] // multiple blocks
    [DataRow("______?")]    // 6 continuation chars + terminator (maximum block length)
    [DataRow("??")]         // consecutive terminators
    [DataRow("?__?_____?")] // mixed block lengths
    public void ValidateBlockLength_With_Valid_Polyline_Does_Not_Throw(string polyline) {
        // Act & Assert
        PolylineEncoding.ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Tests that ValidateBlockLength throws <see cref="InvalidPolylineException"/> for invalid block structures.
    /// </summary>
    [TestMethod]
    [DataRow("________?")]  // 8-char block (exceeds max of 7)
    [DataRow("________")]   // all continuation chars, no terminator
    [DataRow("")]           // empty polyline has no terminator
    [DataRow("?________?")] // valid block then too-long block
    [DataRow("________?_?")] // first block too long
    public void ValidateBlockLength_With_Invalid_Polyline_Throws_InvalidPolylineException(string polyline) {
        // Act & Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => PolylineEncoding.ValidateBlockLength(polyline));
    }

    #endregion
}
