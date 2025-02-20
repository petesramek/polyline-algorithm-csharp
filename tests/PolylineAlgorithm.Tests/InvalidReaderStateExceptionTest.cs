//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for the <see cref="InvalidReaderStateException"/> type.
/// </summary>
[TestClass]
public class InvalidReaderStateExceptionTest {
    /// <summary>
    /// Tests the <see cref="InvalidReaderStateException.ThrowIfCannotRead(bool, int, int)"/> method with valid parameters, expecting no exception.
    /// </summary>
    [TestMethod]
    public void ThrowIfCannotRead_Method_True_Random_Random_Parameter_NoException() {
        // Arrange
        bool canRead = true;
        int position = Values.InvalidReaderStateException.Position;
        int length = Values.InvalidReaderStateException.Length;

        // Act
        InvalidReaderStateException.ThrowIfCannotRead(canRead, position, length);

        // Assert
        // We are asserting that no exception was thrown. If an exception is thrown, the test will fail.
    }

    /// <summary>
    /// Tests the <see cref="InvalidReaderStateException.ThrowIfCannotRead(bool, int, int)"/> method with invalid parameters, expecting an <see cref="InvalidReaderStateException"/>.
    /// </summary>
    [TestMethod]
    public void ThrowIfCannotRead_Method_False_Random_Random_Parameter_InvalidReaderStateException_Throw() {
        // Arrange
        bool canRead = false;
        int position = Values.InvalidReaderStateException.Position;
        int length = Values.InvalidReaderStateException.Length;

        // Act
        static void Throw(bool canRead, int position, int length) => InvalidReaderStateException.ThrowIfCannotRead(canRead, position, length);

        // Assert
        var exception = Assert.ThrowsExactly<InvalidReaderStateException>(() => Throw(canRead, position, length));
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}