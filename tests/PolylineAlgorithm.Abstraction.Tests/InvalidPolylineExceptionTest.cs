//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Abstraction;

/// <summary>
/// Defines tests for the <see cref="InvalidPolylineException"/> type.
/// </summary>
[TestClass]
public class InvalidPolylineExceptionTest {
    /// <summary>
    /// Tests the <see cref="InvalidPolylineException.Throw(int)"/> method with an invalid coordinate parameter, expecting an <see cref="InvalidPolylineException"/>.
    /// </summary>
    [TestMethod]
    public void Throw_Method_Invalid_Coordinate_Parameter_PolylineMalformedException_Throw() {
        // Arrange
        var position = Random.Shared.Next();
        static void ThrowAt(int position) => InvalidPolylineException.Throw(position);

        // Act
        var exception = Assert.ThrowsExactly<InvalidPolylineException>(() => ThrowAt(position));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
        Assert.IsTrue(exception.Message.Contains(position.ToString()));
    }
}