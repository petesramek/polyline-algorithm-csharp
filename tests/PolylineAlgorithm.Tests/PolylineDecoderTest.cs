//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for the <see cref="PolylineDecoder"/> type.
/// </summary>
[TestClass]
public class PolylineDecoderTest {
    /// <summary>
    /// The instance of the <see cref="PolylineDecoder"/> used for testing.
    /// </summary>
    public PolylineDecoder Decoder = new();

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an empty input, expecting an <see cref="ArgumentException"/>.
    /// </summary>
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
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
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
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
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





