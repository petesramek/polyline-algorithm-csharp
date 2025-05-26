//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Tests.Data;
using PolylineAlgorithm.Utility;

/// <summary>
/// Defines tests for the <see cref="PolylineDecoder"/> type.
/// </summary>
[TestClass]
public class PolylineDecoderTest {
    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000], [10_000], [100_000], [1_000_000]];
    
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
        string empty = String.Empty;

        // Act
        void Execute(string value) => Decoder.Decode(value).ToList();

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => Execute(empty));
    }

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
    //[TestMethod]
    //public void Decode_Invalid_Input_ThrowsException() {
    //    // Arrange
    //    Polyline value = Polyline.FromString(Values.Polyline.Invalid);

    //    // Act
    //    void Execute(Polyline value) => Decoder.Decode(value).ToList();

    //    // Assert
    //    var exception = Assert.ThrowsExactly<InvalidCoordinateException>(() => Execute(value));
    //}

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Decode_Valid_Input_Ok(int count) {
        // Arrange
        IEnumerable<Coordinate> expected = RandomValueProvider.GetCoordinates(1);
        string value = RandomValueProvider.GetPolyline(1).ToString();

        // Act
        var result = Decoder.Decode(value);

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }
}