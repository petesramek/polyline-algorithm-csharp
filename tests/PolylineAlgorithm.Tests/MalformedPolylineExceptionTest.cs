//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineMalformedException"/> type.
/// </summary>
[TestClass]
public class MalformedPolylineExceptionTest {
    [TestMethod]
    public void Constructor_Ok() {
        // Arrange
        var position = Values.MalformedPolylineException.Position;
        var innerException = new Exception();

        // Act
        PolylineMalformedException exception = new(position, innerException);

        // Assert
        Assert.AreEqual(Values.MalformedPolylineException.Position, exception.Position);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
        Assert.IsNotNull(exception.InnerException);
    }
}
