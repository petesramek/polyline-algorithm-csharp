//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="InvalidPolylineException"/> type.
/// </summary>
[TestClass]
public class InvalidPolylineExceptionTest {
    [TestMethod]
    public void Throw_Method_Invalid_Coordinate_Parameter_PolylineMalformedException_Throw() {
        // Arrange
        var position = Values.MalformedPolylineException.Position;

        // Act
        static void Throw(int position) => InvalidPolylineException.Throw(position);


        // Assert
        var exception = Assert.ThrowsExactly<InvalidPolylineException>(() => Throw(position));
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}