//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Utility;

/// <summary>
/// Defines tests for the <see cref="PolylineDecoder"/> type.
/// </summary>
[TestClass]
public class PolylineDecoderTest {
    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    /// <summary>
    /// The instance of the <see cref="PolylineDecoder"/> used for testing.
    /// </summary>
    public PolylineDecoder Decoder = new();

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an empty input, expecting an <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_Default_Polyline_Throws_ArgumentException() {
        // Arrange
        Polyline empty = new();

        // Act
#pragma warning disable IDE0305 // Simplify collection initialization
        void Execute(Polyline value) => Decoder.Decode(value).ToList();
#pragma warning restore IDE0305 // Simplify collection initialization

        // Assert
        Assert.ThrowsExactly<ArgumentException>(() => Execute(empty));
    }

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
    //[TestMethod]
    //public void Decode_Invalid_Input_ThrowsException() {
    //    // Arrange
    //    Polyline value = Polyline.FromString(StaticValueProvider.GetPolyline());

    //    // Act
    //    void Execute(Polyline value) => Decoder.Decode(value).ToList();

    //    // Assert
    //    var exception = Assert.ThrowsExactly<InvalidPolylineException>(() => Execute(value));
    //}

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Random_Value_Decode_Valid_Input_Ok(int count) {
        // Arrange
        IEnumerable<Coordinate> expected = RandomValueProvider.GetCoordinates(count).Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline value = Polyline.FromString(RandomValueProvider.GetPolyline(count));

        // Act
        var result = Decoder.Decode(value);

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    [TestMethod]
    public void Static_Value_Decode_Valid_Input_Ok() {
        // Arrange
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        string value = StaticValueProvider.Valid.GetPolyline();

        // Act
        var result = Decoder.Decode(Polyline.FromString(value));

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }
}