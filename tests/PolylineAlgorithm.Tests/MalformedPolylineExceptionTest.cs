//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Internal;

/// <summary>
/// Defines tests for <see cref="MalformedPolylineException"/> type.
/// </summary>
[TestClass]
public class MalformedPolylineExceptionTest {
    [TestMethod]
    public void Constructor_Ok() {
        // Arrange
        var position = Defaults.MalformedPolylineException.Position;
        var innerException = new Exception();

        // Act
        MalformedPolylineException exception = new(position, innerException);

        // Assert
        Assert.AreEqual(Defaults.MalformedPolylineException.Position, exception.Position);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
        Assert.IsNotNull(exception.InnerException);
    }
}
