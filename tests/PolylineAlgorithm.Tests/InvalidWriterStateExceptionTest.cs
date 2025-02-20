//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for the <see cref="InvalidWriterStateException"/> type.
/// </summary>
[TestClass]
public class InvalidWriterStateExceptionTest {
    /// <summary>
    /// Tests the <see cref="InvalidWriterStateException.ThrowIfCannotWrite(bool, int, int)"/> method with valid parameters, expecting no exception.
    /// </summary>
    [TestMethod]
    public void ThrowIfCannotWrite_Method_True_Random_Random_Parameter_NoException() {
        // Arrange
        bool canWrite = true;
        int position = Values.InvalidWriterStateException.Position;
        int length = Values.InvalidWriterStateException.Length;

        // Act
        InvalidWriterStateException.ThrowIfCannotWrite(canWrite, position, length);

        // Assert
        // We are asserting that no exception was thrown. If an exception is thrown, the test will fail.
    }

    /// <summary>
    /// Tests the <see cref="InvalidWriterStateException.ThrowIfCannotWrite(bool, int, int)"/> method with invalid parameters, expecting an <see cref="InvalidWriterStateException"/>.
    /// </summary>
    [TestMethod]
    public void ThrowIfCannotWrite_Method_False_Random_Random_Parameter_InvalidWriterStateException_Throw() {
        // Arrange
        bool canWrite = false;
        int position = Values.InvalidWriterStateException.Position;
        int length = Values.InvalidWriterStateException.Length;

        // Act
        static void Throw(bool canWrite, int position, int length) => InvalidWriterStateException.ThrowIfCannotWrite(canWrite, position, length);

        // Assert
        var exception = Assert.ThrowsExactly<InvalidWriterStateException>(() => Throw(canWrite, position, length));
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}