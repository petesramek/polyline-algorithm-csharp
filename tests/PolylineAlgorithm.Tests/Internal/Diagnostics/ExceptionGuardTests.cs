//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal.Diagnostics;

using PolylineAlgorithm.Internal.Diagnostics;
using System;

/// <summary>
/// Tests for <see cref="ExceptionGuard"/>.
/// </summary>
[TestClass]
public sealed class ExceptionGuardTests {
    /// <summary>
    /// Tests that ThrowNotFiniteNumber throws ArgumentOutOfRangeException with correct parameter name.
    /// </summary>
    [TestMethod]
    public void ThrowNotFiniteNumber_With_Param_Name_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const string paramName = "value";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => ExceptionGuard.ThrowNotFiniteNumber(paramName));
        Assert.AreEqual(paramName, ex.ParamName);
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowArgumentNull throws ArgumentNullException with correct parameter name.
    /// </summary>
    [TestMethod]
    public void ThrowArgumentNull_With_Param_Name_Throws_ArgumentNullException() {
        // Arrange
        const string paramName = "input";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ExceptionGuard.ThrowArgumentNull(paramName));
        Assert.AreEqual(paramName, ex.ParamName);
    }

    /// <summary>
    /// Tests that ThrowBufferOverflow throws OverflowException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowBufferOverflow_With_Message_Throws_OverflowException() {
        // Arrange
        const string message = "Buffer overflow occurred.";

        // Act & Assert
        var ex = Assert.ThrowsExactly<OverflowException>(() => ExceptionGuard.ThrowBufferOverflow(message));
        Assert.AreEqual(message, ex.Message);
    }

    /// <summary>
    /// Tests that ThrowCoordinateValueOutOfRange throws ArgumentOutOfRangeException with correct parameter name.
    /// </summary>
    [TestMethod]
    public void ThrowCoordinateValueOutOfRange_With_Parameters_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const double value = 100.0;
        const double min = -90.0;
        const double max = 90.0;
        const string paramName = "latitude";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => ExceptionGuard.ThrowCoordinateValueOutOfRange(value, min, max, paramName));
        Assert.AreEqual(paramName, ex.ParamName);
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that StackAllocLimitMustBeEqualOrGreaterThan throws ArgumentOutOfRangeException with correct parameter name.
    /// </summary>
    [TestMethod]
    public void StackAllocLimitMustBeEqualOrGreaterThan_With_Parameters_Throws_ArgumentOutOfRangeException() {
        // Arrange
        const int minValue = 10;
        const string paramName = "stackAllocLimit";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => ExceptionGuard.StackAllocLimitMustBeEqualOrGreaterThan(minValue, paramName));
        Assert.AreEqual(paramName, ex.ParamName);
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowArgumentCannotBeEmptyEnumerationMessage throws ArgumentException with correct parameter name.
    /// </summary>
    [TestMethod]
    public void ThrowArgumentCannotBeEmptyEnumerationMessage_With_Param_Name_Throws_ArgumentException() {
        // Arrange
        const string paramName = "collection";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(paramName));
        Assert.AreEqual(paramName, ex.ParamName);
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowCouldNotWriteEncodedValueToBuffer throws InvalidOperationException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowCouldNotWriteEncodedValueToBuffer_Throws_InvalidOperationException() {
        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer());
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength throws ArgumentException with correct parameter name.
    /// </summary>
    [TestMethod]
    public void ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_With_Parameters_Throws_ArgumentException() {
        // Arrange
        const int destinationLength = 5;
        const int polylineLength = 10;
        const string paramName = "destination";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => ExceptionGuard.ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength, paramName));
        Assert.AreEqual(paramName, ex.ParamName);
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineLength throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowInvalidPolylineLength_With_Parameters_Throws_InvalidPolylineException() {
        // Arrange
        const int length = 5;
        const int min = 10;

        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidPolylineException>(() => ExceptionGuard.ThrowInvalidPolylineLength(length, min));
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineCharacter throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowInvalidPolylineCharacter_With_Parameters_Throws_InvalidPolylineException() {
        // Arrange
        const char character = '!';
        const int position = 15;

        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidPolylineException>(() => ExceptionGuard.ThrowInvalidPolylineCharacter(character, position));
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowPolylineBlockTooLong throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowPolylineBlockTooLong_With_Position_Throws_InvalidPolylineException() {
        // Arrange
        const int position = 42;

        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidPolylineException>(() => ExceptionGuard.ThrowPolylineBlockTooLong(position));
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineFormat throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowInvalidPolylineFormat_With_Position_Throws_InvalidPolylineException() {
        // Arrange
        const long position = 100L;

        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidPolylineException>(() => ExceptionGuard.ThrowInvalidPolylineFormat(position));
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineBlockTerminator throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    public void ThrowInvalidPolylineBlockTerminator_Throws_InvalidPolylineException() {
        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidPolylineException>(() => ExceptionGuard.ThrowInvalidPolylineBlockTerminator());
        Assert.IsNotNull(ex.Message);
    }

    /// <summary>
    /// Tests that FormatStackAllocLimitMustBeEqualOrGreaterThan returns formatted message with specified value.
    /// </summary>
    [TestMethod]
    public void FormatStackAllocLimitMustBeEqualOrGreaterThan_With_Min_Value_Returns_Formatted_Message() {
        // Arrange
        const int minValue = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatStackAllocLimitMustBeEqualOrGreaterThan(minValue);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatPolylineCannotBeShorterThan returns formatted message with specified values.
    /// </summary>
    [TestMethod]
    public void FormatPolylineCannotBeShorterThan_With_Length_And_Min_Length_Returns_Formatted_Message() {
        // Arrange
        const int length = 5;
        const int minLength = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatPolylineCannotBeShorterThan(length, minLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('5', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatMalformedPolyline returns formatted message with position.
    /// </summary>
    [TestMethod]
    public void FormatMalformedPolyline_With_Position_Returns_Formatted_Message() {
        // Arrange
        const long position = 42L;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatMalformedPolyline(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("42", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatMalformedPolyline with zero position returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatMalformedPolyline_With_Zero_Position_Returns_Formatted_Message() {
        // Arrange
        const long position = 0L;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatMalformedPolyline(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatMalformedPolyline with negative position returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatMalformedPolyline_With_Negative_Position_Returns_Formatted_Message() {
        // Arrange
        const long position = -10L;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatMalformedPolyline(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("-10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatMalformedPolyline with large position returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatMalformedPolyline_With_Large_Position_Returns_Formatted_Message() {
        // Arrange
        const long position = long.MaxValue;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatMalformedPolyline(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(long.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatCoordinateValueMustBeBetween returns formatted message with all parameters.
    /// </summary>
    [TestMethod]
    public void FormatCoordinateValueMustBeBetween_With_Parameters_Returns_Formatted_Message() {
        // Arrange
        const string name = "latitude";
        const double min = -90.0;
        const double max = 90.0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatCoordinateValueMustBeBetween(name, min, max);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("latitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("-90", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("90", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatCoordinateValueMustBeBetween with positive values returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatCoordinateValueMustBeBetween_With_Positive_Values_Returns_Formatted_Message() {
        // Arrange
        const string name = "longitude";
        const double min = 0.0;
        const double max = 180.0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatCoordinateValueMustBeBetween(name, min, max);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("longitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("180", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatCoordinateValueMustBeBetween with fractional values returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatCoordinateValueMustBeBetween_With_Fractional_Values_Returns_Formatted_Message() {
        // Arrange
        const string name = "value";
        const double min = 1.5;
        const double max = 10.75;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatCoordinateValueMustBeBetween(name, min, max);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("value", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatPolylineBlockTooLong returns formatted message with position.
    /// </summary>
    [TestMethod]
    public void FormatPolylineBlockTooLong_With_Position_Returns_Formatted_Message() {
        // Arrange
        const int position = 15;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatPolylineBlockTooLong(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("15", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatPolylineBlockTooLong with zero position returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatPolylineBlockTooLong_With_Zero_Position_Returns_Formatted_Message() {
        // Arrange
        const int position = 0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatPolylineBlockTooLong(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatPolylineBlockTooLong with large position returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatPolylineBlockTooLong_With_Large_Position_Returns_Formatted_Message() {
        // Arrange
        const int position = int.MaxValue;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatPolylineBlockTooLong(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains(int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineCharacter returns formatted message with character and position.
    /// </summary>
    [TestMethod]
    public void FormatInvalidPolylineCharacter_With_Character_And_Position_Returns_Formatted_Message() {
        // Arrange
        const char character = '!';
        const int position = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineCharacter(character, position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('!', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineCharacter with letter character returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatInvalidPolylineCharacter_With_Letter_Character_Returns_Formatted_Message() {
        // Arrange
        const char character = 'Z';
        const int position = 5;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineCharacter(character, position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('Z', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('5', StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineCharacter with special character returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatInvalidPolylineCharacter_With_Special_Character_Returns_Formatted_Message() {
        // Arrange
        const char character = '@';
        const int position = 0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineCharacter(character, position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('@', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_With_Lengths_Returns_Formatted_Message() {
        // Arrange
        const int destinationLength = 5;
        const int polylineLength = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('5', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength with zero destination length returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_With_Zero_Destination_Length_Returns_Formatted_Message() {
        // Arrange
        const int destinationLength = 0;
        const int polylineLength = 100;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("100", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength with large values returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_With_Large_Values_Returns_Formatted_Message() {
        // Arrange
        const int destinationLength = 1000;
        const int polylineLength = 2000;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("1000", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("2000", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineLength returns formatted message with length and min values.
    /// </summary>
    [TestMethod]
    public void FormatInvalidPolylineLength_With_Length_And_Min_Returns_Formatted_Message() {
        // Arrange
        const int length = 5;
        const int min = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineLength(length, min);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('5', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineLength with zero length returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatInvalidPolylineLength_With_Zero_Length_Returns_Formatted_Message() {
        // Arrange
        const int length = 0;
        const int min = 1;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineLength(length, min);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('1', StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineLength with negative values returns formatted message.
    /// </summary>
    [TestMethod]
    public void FormatInvalidPolylineLength_With_Negative_Values_Returns_Formatted_Message() {
        // Arrange
        const int length = -5;
        const int min = 0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineLength(length, min);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("-5", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('0', StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that GetArgumentValueMustBeFiniteNumber returns non-null message.
    /// </summary>
    [TestMethod]
    public void GetArgumentValueMustBeFiniteNumber_Returns_Non_Null_Message() {
        // Act
        string result = ExceptionGuard.ExceptionMessage.GetArgumentValueMustBeFiniteNumber();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result));
    }

    /// <summary>
    /// Tests that GetCouldNotWriteEncodedValueToTheBuffer returns non-null message.
    /// </summary>
    [TestMethod]
    public void GetCouldNotWriteEncodedValueToTheBuffer_Returns_Non_Null_Message() {
        // Act
        string result = ExceptionGuard.ExceptionMessage.GetCouldNotWriteEncodedValueToTheBuffer();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result));
    }

    /// <summary>
    /// Tests that GetArgumentCannotBeEmpty returns non-null message.
    /// </summary>
    [TestMethod]
    public void GetArgumentCannotBeEmpty_Returns_Non_Null_Message() {
        // Act
        string result = ExceptionGuard.ExceptionMessage.GetArgumentCannotBeEmpty();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result));
    }

    /// <summary>
    /// Tests that GetInvalidPolylineBlockTerminator returns non-null message.
    /// </summary>
    [TestMethod]
    public void GetInvalidPolylineBlockTerminator_Returns_Non_Null_Message() {
        // Act
        string result = ExceptionGuard.ExceptionMessage.GetInvalidPolylineBlockTerminator();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result));
    }
}