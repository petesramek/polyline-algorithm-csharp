//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Tests.Internal;

/// <summary>
/// Defines tests for <see cref="PolylineDecoder"/> type.
/// </summary>
[TestClass]
public class PolylineDecoderTest {
    public PolylineDecoder Decoder = new PolylineDecoder();

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. Empty <see cref="ReadOnlyMemory{char}" /> is passed as an argument.
    /// </summary>
    /// <remarks>Expected to throw <see cref="ArgumentException"/>.</remarks>
    [TestMethod]
    public void Decode_EmptyInput_ThrowsException() {
        // Arrange
        ReadOnlyMemory<char> empty = Defaults.Polyline.Empty.AsMemory();

        // Act
        void Execute(ReadOnlySpan<char> value) => Decoder.Decode(in value);

        // Assert
        Assert.ThrowsException<ArgumentException>(() => Execute(empty.Span));
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. <see cref="ReadOnlyMemory{char}" /> containing invalid polyline is passed as an argument.
    /// </summary>
    /// <remarks>Expected to throw <see cref="InvalidCoordinateException"/>.</remarks>
    [TestMethod]
    public void Decode_InvalidInput_ThrowsException() {
        // Arrange
        ReadOnlyMemory<char> invalid = Defaults.Polyline.Invalid.AsMemory();

        // Act
        void Execute(ReadOnlySpan<char> value) => Decoder.Decode(in value);

        // Assert
        Assert.ThrowsException<InvalidCoordinateException>(() => Execute(invalid.Span));
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{Coordinate})" /> method. <see cref="ReadOnlyMemory{char}" /> containing valid polyline is passed as an argument.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Defaults.Coordinates.Valid"/>..</remarks>
    [TestMethod]
    public void Decode_ValidInput_Ok() {
        // Arrange
        ReadOnlySpan<char> valid = Defaults.Polyline.Valid;

        // Act
        var result = Decoder.Decode(in valid);

        // Assert
        CollectionAssert.AreEqual(Defaults.Coordinates.Valid.ToArray(), result.ToArray());
    }
}
