//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineDecoder"/> type.
/// </summary>
[TestClass]
public class PolylineDecoderTest {
    public PolylineDecoder Decoder = new();

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. Empty <see cref="ReadOnlyMemory{char}" /> is passed as an argument.
    /// </summary>
    /// <remarks>Expected to throw <see cref="ArgumentException"/>.</remarks>
    [TestMethod]
    public void Decode_Empty_Input_ThrowsException() {
        // Arrange
        Polyline empty = new();

        // Act
        void Execute(Polyline value) => Decoder.Decode(in value);

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => Execute(empty));
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. Empty <see cref="ReadOnlyMemory{char}" /> is passed as an argument.
    /// </summary>
    /// <remarks>Expected to throw <see cref="ArgumentException"/>.</remarks>
    [TestMethod]
    public void Decode_Invalid_Input_ThrowsException() {
        // Arrange
        Polyline value = new(Values.Polyline.Invalid);

        // Act
        void Execute(Polyline value) => Decoder.Decode(in value);

        // Assert
        var exception = Assert.ThrowsExactly<InvalidCoordinateException>(() => Execute(value));
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. <see cref="ReadOnlyMemory{char}" /> containing valid polyline is passed as an argument.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>..</remarks>
    [TestMethod]
    public void Decode_Valid_Input_Ok() {
        // Arrange
        Polyline value = new(Values.Polyline.Valid);

        // Act
        var result = Decoder.Decode(in value);

        // Assert
        CollectionAssert.AreEqual(Values.Coordinates.Valid.ToArray(), result.ToArray());
    }
}
