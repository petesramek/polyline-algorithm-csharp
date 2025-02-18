//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="InvalidPolylineException"/> type.
/// </summary>
[TestClass]
public class InvalidWriterStateExceptionTest {
    [TestMethod]
    public void ThrowIfCannotWrite_Method_True_Random_Random_Parameter_InvalidReaderStateException_Throw() {
        // Arrange
        bool canRead = true;
        int position = Values.InvalidWriterStateException.Position;
        int length = Values.InvalidWriterStateException.Length;

        // Act
        InvalidWriterStateException.ThrowIfCannotWrite(canRead, position, length);

        // Assert
        // We are assering exception was not thrown, if it was test won't pass
    }

    [TestMethod]
    public void ThrowIfCannotWrite_Method_False_Random_Random_Parameter_InvalidReaderStateException_Throw() {
        // Arrange
        bool canRead = false;
        int position = Values.InvalidWriterStateException.Position;
        int length = Values.InvalidWriterStateException.Length;

        // Act
        static void Throw(bool canRead, int position, int length) => InvalidWriterStateException.ThrowIfCannotWrite(canRead, position, length);

        // Assert
        var exception = Assert.ThrowsExactly<InvalidWriterStateException>(() => Throw(canRead, position, length));
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}
