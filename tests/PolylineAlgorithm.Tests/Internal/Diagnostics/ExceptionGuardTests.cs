//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal.Diagnostics;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Internal.Diagnostics;
using PolylineAlgorithm.Tests.Properties;
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
    [TestCategory(Category.Unit)]
    public void ThrowNotFiniteNumber_WithParamName_ThrowsArgumentOutOfRangeException() {
        // Arrange
        string paramName = "value";

        // Act & Assert
        try {
            ExceptionGuard.ThrowNotFiniteNumber(paramName);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        } catch (ArgumentOutOfRangeException ex) {
            Assert.AreEqual(paramName, ex.ParamName);
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowArgumentNull throws ArgumentNullException with correct parameter name.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowArgumentNull_WithParamName_ThrowsArgumentNullException() {
        // Arrange
        string paramName = "input";

        // Act & Assert
        try {
            ExceptionGuard.ThrowArgumentNull(paramName);
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        } catch (ArgumentNullException ex) {
            Assert.AreEqual(paramName, ex.ParamName);
        }
    }

    /// <summary>
    /// Tests that ThrowBufferOverflow throws OverflowException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowBufferOverflow_WithMessage_ThrowsOverflowException() {
        // Arrange
        string message = "Buffer overflow occurred.";

        // Act & Assert
        try {
            ExceptionGuard.ThrowBufferOverflow(message);
            Assert.Fail("Expected OverflowException was not thrown.");
        } catch (OverflowException ex) {
            Assert.AreEqual(message, ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowCoordinateValueOutOfRange throws ArgumentOutOfRangeException with correct parameter name.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowCoordinateValueOutOfRange_WithParameters_ThrowsArgumentOutOfRangeException() {
        // Arrange
        double value = 100.0;
        double min = -90.0;
        double max = 90.0;
        string paramName = "latitude";

        // Act & Assert
        try {
            ExceptionGuard.ThrowCoordinateValueOutOfRange(value, min, max, paramName);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        } catch (ArgumentOutOfRangeException ex) {
            Assert.AreEqual(paramName, ex.ParamName);
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that StackAllocLimitMustBeEqualOrGreaterThan throws ArgumentOutOfRangeException with correct parameter name.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void StackAllocLimitMustBeEqualOrGreaterThan_WithParameters_ThrowsArgumentOutOfRangeException() {
        // Arrange
        int minValue = 10;
        string paramName = "stackAllocLimit";

        // Act & Assert
        try {
            ExceptionGuard.StackAllocLimitMustBeEqualOrGreaterThan(minValue, paramName);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        } catch (ArgumentOutOfRangeException ex) {
            Assert.AreEqual(paramName, ex.ParamName);
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrwoArgumentCannotBeEmptyEnumerationMessage throws ArgumentException with correct parameter name.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrwoArgumentCannotBeEmptyEnumerationMessage_WithParamName_ThrowsArgumentException() {
        // Arrange
        string paramName = "collection";

        // Act & Assert
        try {
            ExceptionGuard.ThrwoArgumentCannotBeEmptyEnumerationMessage(paramName);
            Assert.Fail("Expected ArgumentException was not thrown.");
        } catch (ArgumentException ex) {
            Assert.AreEqual(paramName, ex.ParamName);
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowCouldNotWriteEncodedValueToBuffer throws InvalidOperationException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowCouldNotWriteEncodedValueToBuffer_ThrowsInvalidOperationException() {
        // Act & Assert
        try {
            ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
            Assert.Fail("Expected InvalidOperationException was not thrown.");
        } catch (InvalidOperationException ex) {
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength throws ArgumentException with correct parameter name.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_WithParameters_ThrowsArgumentException() {
        // Arrange
        int destinationLength = 5;
        int polylineLength = 10;
        string paramName = "destination";

        // Act & Assert
        try {
            ExceptionGuard.ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength, paramName);
            Assert.Fail("Expected ArgumentException was not thrown.");
        } catch (ArgumentException ex) {
            Assert.AreEqual(paramName, ex.ParamName);
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineLength throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowInvalidPolylineLength_WithParameters_ThrowsInvalidPolylineException() {
        // Arrange
        int length = 5;
        int min = 10;

        // Act & Assert
        try {
            ExceptionGuard.ThrowInvalidPolylineLength(length, min);
            Assert.Fail("Expected InvalidPolylineException was not thrown.");
        } catch (InvalidPolylineException ex) {
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineCharacter throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowInvalidPolylineCharacter_WithParameters_ThrowsInvalidPolylineException() {
        // Arrange
        char character = '!';
        int position = 15;

        // Act & Assert
        try {
            ExceptionGuard.ThrowInvalidPolylineCharacter(character, position);
            Assert.Fail("Expected InvalidPolylineException was not thrown.");
        } catch (InvalidPolylineException ex) {
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowPolylineBlockTooLong throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowPolylineBlockTooLong_WithPosition_ThrowsInvalidPolylineException() {
        // Arrange
        int position = 42;

        // Act & Assert
        try {
            ExceptionGuard.ThrowPolylineBlockTooLong(position);
            Assert.Fail("Expected InvalidPolylineException was not thrown.");
        } catch (InvalidPolylineException ex) {
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineFormat throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowInvalidPolylineFormat_WithPosition_ThrowsInvalidPolylineException() {
        // Arrange
        long position = 100L;

        // Act & Assert
        try {
            ExceptionGuard.ThrowInvalidPolylineFormat(position);
            Assert.Fail("Expected InvalidPolylineException was not thrown.");
        } catch (InvalidPolylineException ex) {
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that ThrowInvalidPolylineBlockTerminator throws InvalidPolylineException with correct message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void ThrowInvalidPolylineBlockTerminator_ThrowsInvalidPolylineException() {
        // Act & Assert
        try {
            ExceptionGuard.ThrowInvalidPolylineBlockTerminator();
            Assert.Fail("Expected InvalidPolylineException was not thrown.");
        } catch (InvalidPolylineException ex) {
            Assert.IsNotNull(ex.Message);
        }
    }

    /// <summary>
    /// Tests that FormatStackAllocLimitMustBeEqualOrGreaterThan returns formatted message with specified value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatStackAllocLimitMustBeEqualOrGreaterThan_WithMinValue_ReturnsFormattedMessage() {
        // Arrange
        int minValue = 10;

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
    [TestCategory(Category.Unit)]
    public void FormatPolylineCannotBeShorterThan_WithLengthAndMinLength_ReturnsFormattedMessage() {
        // Arrange
        int length = 5;
        int minLength = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatPolylineCannotBeShorterThan(length, minLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('5'));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatMalformedPolyline returns formatted message with position.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatMalformedPolyline_WithPosition_ReturnsFormattedMessage() {
        // Arrange
        long position = 42L;

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
    [TestCategory(Category.Unit)]
    public void FormatMalformedPolyline_WithZeroPosition_ReturnsFormattedMessage() {
        // Arrange
        long position = 0L;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatMalformedPolyline(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0'));
    }

    /// <summary>
    /// Tests that FormatMalformedPolyline with negative position returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatMalformedPolyline_WithNegativePosition_ReturnsFormattedMessage() {
        // Arrange
        long position = -10L;

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
    [TestCategory(Category.Unit)]
    public void FormatMalformedPolyline_WithLargePosition_ReturnsFormattedMessage() {
        // Arrange
        long position = long.MaxValue;

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
    [TestCategory(Category.Unit)]
    public void FormatCoordinateValueMustBeBetween_WithParameters_ReturnsFormattedMessage() {
        // Arrange
        string name = "latitude";
        double min = -90.0;
        double max = 90.0;

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
    [TestCategory(Category.Unit)]
    public void FormatCoordinateValueMustBeBetween_WithPositiveValues_ReturnsFormattedMessage() {
        // Arrange
        string name = "longitude";
        double min = 0.0;
        double max = 180.0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatCoordinateValueMustBeBetween(name, min, max);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("longitude", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('0'));
        Assert.IsTrue(result.Contains("180", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatCoordinateValueMustBeBetween with fractional values returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatCoordinateValueMustBeBetween_WithFractionalValues_ReturnsFormattedMessage() {
        // Arrange
        string name = "value";
        double min = 1.5;
        double max = 10.75;

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
    [TestCategory(Category.Unit)]
    public void FormatPolylineBlockTooLong_WithPosition_ReturnsFormattedMessage() {
        // Arrange
        int position = 15;

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
    [TestCategory(Category.Unit)]
    public void FormatPolylineBlockTooLong_WithZeroPosition_ReturnsFormattedMessage() {
        // Arrange
        int position = 0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatPolylineBlockTooLong(position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0'));
    }

    /// <summary>
    /// Tests that FormatPolylineBlockTooLong with large position returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatPolylineBlockTooLong_WithLargePosition_ReturnsFormattedMessage() {
        // Arrange
        int position = int.MaxValue;

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
    [TestCategory(Category.Unit)]
    public void FormatInvalidPolylineCharacter_WithCharacterAndPosition_ReturnsFormattedMessage() {
        // Arrange
        char character = '!';
        int position = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineCharacter(character, position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('!'));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineCharacter with letter character returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatInvalidPolylineCharacter_WithLetterCharacter_ReturnsFormattedMessage() {
        // Arrange
        char character = 'Z';
        int position = 5;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineCharacter(character, position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('Z'));
        Assert.IsTrue(result.Contains('5'));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineCharacter with special character returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatInvalidPolylineCharacter_WithSpecialCharacter_ReturnsFormattedMessage() {
        // Arrange
        char character = '@';
        int position = 0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineCharacter(character, position);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('@'));
        Assert.IsTrue(result.Contains('0'));
    }

    /// <summary>
    /// Tests that FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_WithLengths_ReturnsFormattedMessage() {
        // Arrange
        int destinationLength = 5;
        int polylineLength = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('5'));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength with zero destination length returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_WithZeroDestinationLength_ReturnsFormattedMessage() {
        // Arrange
        int destinationLength = 0;
        int polylineLength = 100;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destinationLength, polylineLength);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0'));
        Assert.IsTrue(result.Contains("100", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength with large values returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength_WithLargeValues_ReturnsFormattedMessage() {
        // Arrange
        int destinationLength = 1000;
        int polylineLength = 2000;

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
    [TestCategory(Category.Unit)]
    public void FormatInvalidPolylineLength_WithLengthAndMin_ReturnsFormattedMessage() {
        // Arrange
        int length = 5;
        int min = 10;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineLength(length, min);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('5'));
        Assert.IsTrue(result.Contains("10", StringComparison.Ordinal));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineLength with zero length returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatInvalidPolylineLength_WithZeroLength_ReturnsFormattedMessage() {
        // Arrange
        int length = 0;
        int min = 1;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineLength(length, min);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains('0'));
        Assert.IsTrue(result.Contains('1'));
    }

    /// <summary>
    /// Tests that FormatInvalidPolylineLength with negative values returns formatted message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void FormatInvalidPolylineLength_WithNegativeValues_ReturnsFormattedMessage() {
        // Arrange
        int length = -5;
        int min = 0;

        // Act
        string result = ExceptionGuard.ExceptionMessage.FormatInvalidPolylineLength(length, min);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("-5", StringComparison.Ordinal));
        Assert.IsTrue(result.Contains('0'));
    }

    /// <summary>
    /// Tests that GetArgumentValueMustBeFiniteNumber returns non-null message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void GetArgumentValueMustBeFiniteNumber_ReturnsNonNullMessage() {
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
    [TestCategory(Category.Unit)]
    public void GetCouldNotWriteEncodedValueToTheBuffer_ReturnsNonNullMessage() {
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
    [TestCategory(Category.Unit)]
    public void GetArgumentCannotBeEmpty_ReturnsNonNullMessage() {
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
    [TestCategory(Category.Unit)]
    public void GetInvalidPolylineBlockTerminator_ReturnsNonNullMessage() {
        // Act
        string result = ExceptionGuard.ExceptionMessage.GetInvalidPolylineBlockTerminator();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result));
    }
}
