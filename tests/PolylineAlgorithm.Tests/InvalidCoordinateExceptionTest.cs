//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Performs tests for <see cref="InvalidCoordinateException"/> type.
/// </summary>
[TestClass]
public class InvalidCoordinateExceptionTest {

    /// <summary>
    /// Method is testing <see cref="InvalidCoordinateException" /> constructor. <see cref="Coordinate" /> and <see cref="Exception"/> are passed as arguments.
    /// </summary>
    /// <remarks>Expected: <see cref="InvalidCoordinateException.Coordinate"/> equals passed argument,
    /// <see cref="Exception.InnerException" /> equals passed argument,
    /// and <see cref="Exception.Message"/> in not <see langword="null"/>, empty -or- whitespace.</remarks>
    [TestMethod]
    public void Constructor_Ok() {
        // Arrange
        var coordinate = Values.InvalidCoordinateException.Coordinate;
        var innerException = new Exception();

        // Act
        InvalidCoordinateException result = new(coordinate, innerException);

        // Assert
        Assert.AreEqual(coordinate, result.Coordinate);
        Assert.IsNotNull(innerException);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.Message));
    }
}
