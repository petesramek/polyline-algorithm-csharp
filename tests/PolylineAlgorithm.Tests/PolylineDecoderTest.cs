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
    public void Decode_EmptyInput_ThrowsException() {
        // Arrange
        Polyline empty = new();

        // Act
        void Execute(Polyline value) => Decoder.Decode(in value);

        // Assert
        Assert.ThrowsException<ArgumentException>(() => Execute(empty));
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. <see cref="ReadOnlyMemory{char}" /> containing valid polyline is passed as an argument.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>..</remarks>
    [TestMethod]
    public void Decode_ValidInput_Ok() {
        // Arrange
        var value = Values.Polyline.Valid;
        Polyline valid = new(value);

        // Act
        var result = Decoder.Decode(in valid);

        // Assert
        CollectionAssert.AreEqual(Values.Coordinates.Valid.ToArray(), result.ToArray());
    }
}
