//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Performs tests for the <see cref="InvalidCoordinateException"/> type.
/// </summary>
[TestClass]
public class InvalidCoordinateExceptionTest {

    /// <summary>
    /// Tests the <see cref="InvalidCoordinateException.ThrowIfNotValid(Coordinate)"/> method with a valid coordinate.
    /// </summary>
    /// <remarks>
    /// Expected: No exception is thrown.
    /// </remarks>
    [TestMethod]
    public void ThrowIfNotValid_Valid_Parameter_Ok() {
        // Arrange
        var coordinate = Values.InvalidCoordinateException.Valid;

        // Act
        InvalidCoordinateException.ThrowIfNotValid(coordinate);

        // Assert
        // We are asserting that no exception was thrown. If an exception is thrown, the test will fail.
    }

    /// <summary>
    /// Tests the <see cref="InvalidCoordinateException.ThrowIfNotValid(Coordinate)"/> method with an invalid coordinate, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
    [TestMethod]
    public void ThrowIfNotValid_Invalid_Parameter_InvalidCoordinateException_Thrown() {
        // Arrange
        var coordinate = Values.InvalidCoordinateException.Invalid;

        // Act
        static void ThrowIfNotValid(Coordinate coordinate) {
            InvalidCoordinateException.ThrowIfNotValid(coordinate);
        }

        // Assert
        var exception = Assert.ThrowsExactly<InvalidCoordinateException>(() => ThrowIfNotValid(coordinate));
        Assert.AreEqual(coordinate, exception.Coordinate);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}




