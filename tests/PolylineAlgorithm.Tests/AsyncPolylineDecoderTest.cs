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
public class AsyncPolylineDecoderTest {
    /// <summary>
    /// The instance of the <see cref="PolylineDecoder"/> used for testing.
    /// </summary>
    public AsyncPolylineDecoder Decoder = new();

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an empty input, expecting an <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public async Task Decode_Empty_Input_ThrowsException() {
        // Arrange
        Polyline empty = new();

        // Act
        async Task<IEnumerable<Coordinate>> Execute(Polyline value) {
            var result = new List<Coordinate>();

            await foreach (var coordinate in Decoder.DecodeAsync(value)) {
                result.Add(coordinate);
            }

            return result;
        };

        // Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await Execute(empty));
    }

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
    [TestMethod]
    public async Task Decode_Invalid_Input_ThrowsException() {
        // Arrange
        Polyline invalid = new(Values.Polyline.Invalid);

        // Act
        async Task<IEnumerable<Coordinate>> Execute(Polyline value) {
            var result = new List<Coordinate>();

            await foreach (var coordinate in Decoder.DecodeAsync(value)) {
                result.Add(coordinate);
            }

            return result;
        };

        // Assert
        await Assert.ThrowsExactlyAsync<InvalidCoordinateException>(async () => await Execute(invalid));
    }

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
    [TestMethod]
    public async Task Decode_Valid_Input_Ok() {
        // Arrange
        Polyline value = new(Values.Polyline.Valid);

        // Act
        var result = new List<Coordinate>();

        await foreach (var coordinate in Decoder.DecodeAsync(value)) {
            result.Add(coordinate);
        }

        // Assert
        CollectionAssert.AreEqual(Values.Coordinates.Valid.ToArray(), result.ToArray());
    }
}