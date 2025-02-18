//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineReader"/> type.
/// </summary>
[TestClass]
public class PolylineReaderTest {
    public static IEnumerable<object[]> Valid_Constructor_Parameters => [
        [ Values.Polyline.Empty ],
        [ Values.Polyline.Valid ],
        [ Values.Polyline.Invalid ]
    ];

    public static IEnumerable<object[]> Invalid_Read_Method_Parameters => [
        [ Values.Polyline.Empty, Values.Coordinates.Empty.Select(c => (c.Latitude, c.Longitude)).ToList() ],
        [ Values.Polyline.Invalid, Values.Coordinates.Invalid.Select(c => (c.Latitude, c.Longitude)).ToList() ]
    ];


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

    [TestMethod]
    [DynamicData(nameof(Valid_Constructor_Parameters))]
    public void Constructor_Valid_Parameter_Ok(string value) {
        // Arrange
        Polyline polyline = Polyline.FromString(in value);
        bool canRead = !polyline.IsEmpty;
        int position = 0;

        // Act
        PolylineReader reader = new(in polyline);

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
    }

    [TestMethod]
    public void Read_Valid_Parameter_Ok() {
        // Arrange
        string value = Values.Polyline.Valid;
        bool canRead = false;
        int position = value.Length;
        Polyline polyline = Polyline.FromString(in value);
        PolylineReader reader = new(in polyline);
        List<Coordinate> expected = [.. Values.Coordinates.Valid];
        List<Coordinate> result = new(expected.Count());

        // Act
        for (int i = 0; i < expected.Count(); i++) {
            var coordinate = reader.Read();
            result.Add(coordinate);
        }

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Read_Empty_Polyline_InvalidReaderStateException_Thrown() {
        // Arrange
        string value = string.Empty;

        // Act
        static void Read(string value) {
            Polyline polyline = Polyline.FromString(in value);
            PolylineReader reader = new(in polyline);
            _ = reader.Read();
        }

        // Assert
        var exception = Assert.ThrowsExactly<InvalidReaderStateException>(() => Read(value));
    }

    [TestMethod]
    public void Read_Index_Out_Of_Range_InvalidReaderStateException_Thrown() {
        // Arrange
        string value = Values.Polyline.Valid;
        int iterations = Values.Coordinates.Valid.Count() + 1;

        // Act
        static void Read(string value, int iterations) {
            Polyline polyline = Polyline.FromString(in value);
            PolylineReader reader = new(in polyline);

            for (int i = 0; i < iterations; i++) {
                _ = reader.Read();
            }
        }

        // Assert
        var exception = Assert.ThrowsExactly<InvalidReaderStateException>(() => Read(value, iterations));
    }

    [TestMethod]
    public void Read_Malformed_Polyline_PolylineMalformedException() {
        // Arrange
        string value = Values.Polyline.Malformed;

        // Act
        static void Read(string value) {
            Polyline polyline = Polyline.FromString(in value);
            PolylineReader reader = new(in polyline);
            _ = reader.Read();
        }
        ;

        // Assert
        Assert.ThrowsExactly<InvalidPolylineException>(() => Read(value));
    }
}
