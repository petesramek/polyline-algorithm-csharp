//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="MalformedPolylineException"/> type.
/// </summary>
[TestClass]
public class InvalidReaderStateExceptionTest {
    [TestMethod]
    public void ThrowIfCannotwrite_Method_True_Random_Random_Parameter_InvalidReaderStateException_Throw() {
        // Arrange
        bool canRead = true;
        int position = Values.InvalidReaderStateException.Position;
        int length = Values.InvalidReaderStateException.Length;

        // Act
        InvalidReaderStateException.ThrowIfCannotRead(canRead, position, length);

        // Assert
        // We are assering exception was not thrown, if it was test won't pass
    }

    [TestMethod]
    public void ThrowIfCannotRead_Method_False_Random_Random_Parameter_InvalidReaderStateException_Throw() {
        // Arrange
        bool canRead = false;
        int position = Values.InvalidReaderStateException.Position;
        int length = Values.InvalidReaderStateException.Length;

        // Act
        static void Throw(bool canRead, int position, int length) => InvalidReaderStateException.ThrowIfCannotRead(canRead, position, length);

        // Assert
        var exception = Assert.ThrowsException<InvalidReaderStateException>(() => Throw(canRead, position, length));
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}
