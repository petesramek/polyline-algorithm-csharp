//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for the <see cref="PolylineReader"/> type.
/// </summary>
[TestClass]
public class PolylineReaderTest {
    /// <summary>
    /// Provides test data for the <see cref="Constructor_Valid_Parameter_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> Valid_Constructor_Parameters => new List<object[]> {
        new object[] { Values.Polyline.Empty },
        new object[] { Values.Polyline.Valid },
        new object[] { Values.Polyline.Invalid }
    };

    /// <summary>
    /// Provides test data for the <see cref="Read_Empty_Polyline_InvalidReaderStateException_Thrown"/> and <see cref="Read_Index_Out_Of_Range_InvalidReaderStateException_Thrown"/> methods.
    /// </summary>
    public static IEnumerable<object[]> Invalid_Read_Method_Parameters => new List<object[]> {
        new object[] { Values.Polyline.Empty, Values.Coordinates.Empty.Select(c => (c.Latitude, c.Longitude)).ToList() },
        new object[] { Values.Polyline.Invalid, Values.Coordinates.Invalid.Select(c => (c.Latitude, c.Longitude)).ToList() }
    };

    /// <summary>
    /// Tests the parameterless constructor of the <see cref="PolylineReader"/> class.
    /// </summary>
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        bool canRead = false;
        int position = 0;

        // Act
        PolylineReader reader = new();

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
    }

    /// <summary>
    /// Tests the <see cref="PolylineReader"/> constructor with valid parameters.
    /// </summary>
    /// <param name="value">The encoded polyline string.</param>
    [TestMethod]
    [DynamicData(nameof(Valid_Constructor_Parameters))]
    public void Constructor_Valid_Parameter_Ok(string value) {
        // Arrange
        Polyline polyline = Polyline.FromString(value);
        bool canRead = !polyline.IsEmpty;
        int position = 0;

        // Act
        PolylineReader reader = new(in polyline);

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
    }

    /// <summary>
    /// Tests the <see cref="PolylineReader.Read"/> method with a valid parameter.
    /// </summary>
    [TestMethod]
    public void Read_Valid_Parameter_Ok() {
        // Arrange
        string value = Values.Polyline.Valid;
        bool canRead = false;
        int position = value.Length;
        Polyline polyline = Polyline.FromString(value);
        PolylineReader reader = new(in polyline);
        List<Coordinate> expected = new(Values.Coordinates.Valid);
        List<Coordinate> result = new(expected.Count);

        // Act
        for (int i = 0; i < expected.Count; i++) {
            var coordinate = reader.Read();
            result.Add(coordinate);
        }

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
        CollectionAssert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests the <see cref="PolylineReader.Read"/> method with an empty polyline, expecting an <see cref="InvalidReaderStateException"/>.
    /// </summary>
    [TestMethod]
    public void Read_Empty_Polyline_InvalidReaderStateException_Thrown() {
        // Arrange
        string value = string.Empty;

        // Act
        static void Read(string value) {
            Polyline polyline = Polyline.FromString(value);
            PolylineReader reader = new(in polyline);
            _ = reader.Read();
        }

        // Assert
        Assert.ThrowsExactly<InvalidReaderStateException>(() => Read(value));
    }

    /// <summary>
    /// Tests the <see cref="PolylineReader.Read"/> method with an index out of range, expecting an <see cref="InvalidReaderStateException"/>.
    /// </summary>
    [TestMethod]
    public void Read_Index_Out_Of_Range_InvalidReaderStateException_Thrown() {
        // Arrange
        string value = Values.Polyline.Valid;
        int iterations = Values.Coordinates.Valid.Count() + 1;

        // Act
        static void Read(string value, int iterations) {
            Polyline polyline = Polyline.FromString(value);
            PolylineReader reader = new(in polyline);

            for (int i = 0; i < iterations; i++) {
                _ = reader.Read();
            }
        }

        // Assert
        Assert.ThrowsExactly<InvalidReaderStateException>(() => Read(value, iterations));
    }

    /// <summary>
    /// Tests the <see cref="PolylineReader.Read"/> method with a malformed polyline, expecting an <see cref="InvalidPolylineException"/>.
    /// </summary>
    [TestMethod]
    public void Read_Malformed_Polyline_PolylineMalformedException() {
        // Arrange
        string value = Values.Polyline.Malformed;

        // Act
        static void Read(string value) {
            Polyline polyline = Polyline.FromString(value);
            PolylineReader reader = new(in polyline);
            _ = reader.Read();
        }

        // Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => Read(value));
    }
}