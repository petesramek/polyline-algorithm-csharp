//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using Newtonsoft.Json.Linq;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineReader"/> type.
/// </summary>
[TestClass]
public class PolylineReaderTest {
    public static IEnumerable<object[]> ValidConstructorParameters => [
        [ Values.Polyline.Empty ],
        [ Values.Polyline.Valid ],
        [ Values.Polyline.Invalid ]
    ];

    public static IEnumerable<object[]> ValidReadMethodParameters => [
        [ Values.Polyline.Valid, Values.Coordinates.Valid.Select(c => (c.Latitude, c.Longitude)).ToList() ]
    ];

    public static IEnumerable<object[]> InvalidReadMethodParameters => [
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
    [DynamicData(nameof(ValidConstructorParameters))]
    public void Constructor_Valid_Parameter_Ok(string value) {
        // Arrange
        int position = 0;
        Polyline polyline = Polyline.FromString(in value);
        bool canRead = !polyline.IsEmpty;

        // Act
        PolylineReader reader = new(in polyline);

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
    }

    [TestMethod]
    [DynamicData(nameof(ValidReadMethodParameters))]
    public void Read_Valid_Parameter_Ok(string value, IEnumerable<(double Latitude, double Longitude)> expected) {
        // Arrange
        int iterations = expected.Count();
        bool canRead = false;
        int position = value.Length;
        Polyline polyline = Polyline.FromString(in value);
        PolylineReader reader = new(in polyline);
        List<Coordinate> result = new(expected.Count());

        // Act
        for (int i = 0; i < iterations; i++) {
            var coordinate = reader.Read();
            result.Add(coordinate);
        }

        // Assert
        Assert.AreEqual(canRead, reader.CanRead);
        Assert.AreEqual(position, reader.Position);
        CollectionAssert.AreEqual(expected.Select(c => new Coordinate(c.Latitude, c.Longitude)).ToList(), result);
    }

    [TestMethod]
    [DynamicData(nameof(InvalidReadMethodParameters))]
    public void Read_Invalid_Parameter_InvalidReaderStateException(string value, IEnumerable<(double Latitude, double Longitude)> expected) {
        // Arrange
        int iterations = expected.Count() + 1;

        // Act
        static void Read(string value, int iterations) {
            Polyline polyline = Polyline.FromString(in value);
            PolylineReader reader = new(in polyline);

            for (int i = 0; i < iterations; i++) {
                _ = reader.Read();
            }
        };

        // Assert
        Assert.ThrowsException<InvalidReaderStateException>(() => Read(value, iterations));
    }

    [TestMethod]
    public void Read_Malformed_Parameter_PolylineMalformedException() {
        // Arrange
        string value = Values.Polyline.Malformed;

        // Act
        static void Read(string value) {
            Polyline polyline = Polyline.FromString(in value);
            PolylineReader reader = new(in polyline);
            _ = reader.Read();
        };

        // Assert
        Assert.ThrowsException<PolylineMalformedException>(() => Read(value));
    }
}
