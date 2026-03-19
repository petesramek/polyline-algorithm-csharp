//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Diagnostics;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Tests.Properties;

/// <summary>
/// Tests for <see cref="InvalidPolylineException"/>.
/// </summary>
[TestClass]
public sealed class InvalidPolylineExceptionTests {
    /// <summary>
    /// Tests that the default constructor creates an exception with default properties.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_Default_CreatesExceptionWithDefaultProperties() {
        // Arrange & Act
        var exception = new InvalidPolylineException();

        // Assert
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType<InvalidPolylineException>(exception);
        Assert.IsInstanceOfType<Exception>(exception);
    }

    /// <summary>
    /// Tests that the default constructor creates an exception with null message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_Default_CreatesExceptionWithNullMessage() {
        // Arrange & Act
        var exception = new InvalidPolylineException();

        // Assert
        Assert.IsNull(exception.Message);
    }

    /// <summary>
    /// Tests that the default constructor creates an exception with null inner exception.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_Default_CreatesExceptionWithNullInnerException() {
        // Arrange & Act
        var exception = new InvalidPolylineException();

        // Assert
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Tests that the message constructor creates an exception with the specified message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithMessage_CreatesExceptionWithSpecifiedMessage() {
        // Arrange
        const string expectedMessage = "Test error message";

        // Act
        var exception = CreateInvalidPolylineExceptionWithMessage(expectedMessage);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual(expectedMessage, exception.Message);
    }

    /// <summary>
    /// Tests that the message constructor creates an exception with null inner exception.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithMessage_CreatesExceptionWithNullInnerException() {
        // Arrange
        const string message = "Test error message";

        // Act
        var exception = CreateInvalidPolylineExceptionWithMessage(message);

        // Assert
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Tests that the message constructor handles null message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithNullMessage_CreatesExceptionWithNullMessage() {
        // Arrange
        const string? message = null;

        // Act
        var exception = CreateInvalidPolylineExceptionWithMessage(message!);

        // Assert
        Assert.IsNull(exception.Message);
    }

    /// <summary>
    /// Tests that the message constructor handles empty message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithEmptyMessage_CreatesExceptionWithEmptyMessage() {
        // Arrange
        var message = string.Empty;

        // Act
        var exception = CreateInvalidPolylineExceptionWithMessage(message);

        // Assert
        Assert.AreEqual(string.Empty, exception.Message);
    }

    /// <summary>
    /// Tests that the message and inner exception constructor creates an exception with specified message and inner exception.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithMessageAndInnerException_CreatesExceptionWithSpecifiedProperties() {
        // Arrange
        const string expectedMessage = "Outer exception message";
        var innerException = new InvalidOperationException("Inner exception message");

        // Act
        var exception = new InvalidPolylineException(expectedMessage, innerException);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual(expectedMessage, exception.Message);
        Assert.AreSame(innerException, exception.InnerException);
    }

    /// <summary>
    /// Tests that the message and inner exception constructor handles null message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithNullMessageAndInnerException_CreatesExceptionWithNullMessage() {
        // Arrange
        const string? message = null;
        var innerException = new InvalidOperationException("Inner exception message");

        // Act
        var exception = new InvalidPolylineException(message!, innerException);

        // Assert
        Assert.IsNull(exception.Message);
        Assert.AreSame(innerException, exception.InnerException);
    }

    /// <summary>
    /// Tests that the message and inner exception constructor handles null inner exception.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithMessageAndNullInnerException_CreatesExceptionWithNullInnerException() {
        // Arrange
        const string message = "Test error message";
        const Exception? innerException = null;

        // Act
        var exception = new InvalidPolylineException(message, innerException!);

        // Assert
        Assert.AreEqual(message, exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Tests that the message and inner exception constructor handles empty message.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithEmptyMessageAndInnerException_CreatesExceptionWithEmptyMessage() {
        // Arrange
        var message = string.Empty;
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new InvalidPolylineException(message, innerException);

        // Assert
        Assert.AreEqual(string.Empty, exception.Message);
        Assert.AreSame(innerException, exception.InnerException);
    }

    /// <summary>
    /// Tests that the message and inner exception constructor handles both null values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Constructor_WithNullMessageAndNullInnerException_CreatesExceptionWithNullProperties() {
        // Arrange
        const string? message = null;
        const Exception? innerException = null;

        // Act
        var exception = new InvalidPolylineException(message!, innerException!);

        // Assert
        Assert.IsNull(exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    /// <summary>
    /// Helper method to create an InvalidPolylineException with a message using reflection.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <returns>An InvalidPolylineException instance.</returns>
    private static InvalidPolylineException CreateInvalidPolylineExceptionWithMessage(string message) {
        var constructor = typeof(InvalidPolylineException).GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            [typeof(string)],
            null);
        
        return (InvalidPolylineException)constructor!.Invoke([message]);
    }
}
