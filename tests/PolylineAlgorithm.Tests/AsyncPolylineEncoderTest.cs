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
public class AsyncPolylineEncoderTest {
    /// <summary>
    /// The instance of the <see cref="PolylineDecoder"/> used for testing.
    /// </summary>
    public AsyncPolylineEncoder Encoder = new();



    private async IAsyncEnumerable<Coordinate> GetAsyncEnumeration(IEnumerable<Coordinate> enumerable) {
        foreach (var item in enumerable) {
            yield return await new ValueTask<Coordinate>(item);
        }
    }

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an empty input, expecting an <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public async Task Decode_Null_Input_ThrowsException() {
        // Arrange
        IAsyncEnumerable<Coordinate> @null = null!;

        // Act
        async Task<Polyline> Execute(IAsyncEnumerable<Coordinate> value) {
            var result = new Polyline();

            await foreach (var polyline in Encoder.EncodeAsync(value)) {
                result = result
                    .Append(polyline);
            }

            return result;
        };

        // Assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => await Execute(@null));
    }

    ///// <summary>
    ///// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
    ///// </summary>
    //[TestMethod]
    //public async Task Decode_Invalid_Input_ThrowsException() {
    //    // Arrange
    //    Polyline invalid = new(Values.Polyline.Invalid);

    //    // Act
    //    async Task<IEnumerable<Coordinate>> Execute(Polyline value) {
    //        var result = new List<Coordinate>();

    //        await foreach (var coordinate in Decoder.DecodeAsync(value)) {
    //            result.Add(coordinate);
    //        }

    //        return result;
    //    }
    //    ;

    //    // Assert
    //    await Assert.ThrowsExactlyAsync<InvalidCoordinateException>(async () => await Execute(invalid));
    //}

    /// <summary>
    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
    /// </summary>
    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
    [TestMethod]
    public async Task Decode_Valid_Input_Ok() {
        // Arrange
        IAsyncEnumerable<Coordinate> valid = GetAsyncEnumeration(Values.Coordinates.Valid);

        // Act
        var collection = new List<Polyline>();

        var result = new Polyline();

        await foreach (var polyline in Encoder.EncodeAsync(valid)) {
            collection
                .Add(polyline);
            result = result
                .Append(polyline);
        }

        // Assert
        Assert.AreEqual(Values.Coordinates.Valid.Count(), collection.Count);
        Assert.AreEqual(Values.Polyline.Valid, result.ToString());
        Assert.AreEqual(Polyline.FromString(Values.Polyline.Valid), result);
    }
}