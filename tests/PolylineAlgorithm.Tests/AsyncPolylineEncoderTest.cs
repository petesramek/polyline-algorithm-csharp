////
//// Copyright © Pete Sramek. All rights reserved.
//// Licensed under the MIT License. See LICENSE file in the project root for full license information.
////

//namespace PolylineAlgorithm.Tests;

//using PolylineAlgorithm;
//using PolylineAlgorithm.Tests.Data;
//using PolylineAlgorithm.Tests.Internal;

///// <summary>
///// Defines tests for the <see cref="PolylineDecoder"/> type.
///// </summary>
//[TestClass]
//public class AsyncPolylineEncoderTest {
//    /// <summary>
//    /// The instance of the <see cref="PolylineDecoder"/> used for testing.
//    /// </summary>
//    public AsyncPolylineEncoder Encoder = new();



//    private async IAsyncEnumerable<Coordinate> GetAsyncEnumeration(IEnumerable<Coordinate> enumerable) {
//        foreach (var item in enumerable) {
//            yield return await new ValueTask<Coordinate>(item);
//        }
//    }

//    /// <summary>
//    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an empty input, expecting an <see cref="ArgumentException"/>.
//    /// </summary>
//    [TestMethod]
//    public async Task Decode_Null_Input_ThrowsException() {
//        // Arrange
//        IAsyncEnumerable<Coordinate> @null = null!;

//        // Act
//        async Task<Polyline> Execute(IAsyncEnumerable<Coordinate> value) {
//            Polyline result = new();

//            await foreach (var item in Encoder.EncodeAsync(value)) {
//                result = result
//                .Append(item.Value);
//            }

//            return result;
//        }
//        ;

//        // Assert
//        await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => await Execute(@null));
//    }

//    ///// <summary>
//    ///// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with an invalid input, expecting an <see cref="InvalidCoordinateException"/>.
//    ///// </summary>
//    [TestMethod]
//    public async Task Decode_Invalid_Input_ThrowsException() {
//        // Arrange
//        IAsyncEnumerable<Coordinate> invalid = GetAsyncEnumeration(Values.Coordinates.Invalid);

//        // Act
//        async Task<Polyline> Execute(IAsyncEnumerable<Coordinate> value) {
//            Polyline result = new();

//            await foreach (var item in Encoder.EncodeAsync(value)) {
//                result = result
//                .Append(item.Value);
//            }

//            return result;
//        }

//        // Assert
//        await Assert.ThrowsExactlyAsync<InvalidCoordinateException>(async () => await Execute(invalid));
//    }

//    /// <summary>
//    /// Tests the <see cref="PolylineDecoder.Decode(ref readonly Polyline)"/> method with a valid input.
//    /// </summary>
//    /// <remarks>Expected result to equal <see cref="Values.Coordinates.Valid"/>.</remarks>
//    [TestMethod]
//    public async Task Decode_Valid_Input_Ok() {
//        // Arrange
//        IAsyncEnumerable<Coordinate> valid = GetAsyncEnumeration(ValueProvider.GetCoordinates(1_000_000));
//        Polyline result = new();

//        // Act
//        await foreach (var item in Encoder.EncodeAsync(valid).ConfigureAwait(false)) {
//            result = result
//                .Append(item.Value);
//        }

//        // Assert
//        //Assert.AreEqual(Values.Coordinates.Valid.Count(), result.Count);
//        Assert.AreEqual(Values.Polyline.Valid, result.ToString());
//        Assert.AreEqual(Polyline.FromString(Values.Polyline.Valid), result);
//    }
//}