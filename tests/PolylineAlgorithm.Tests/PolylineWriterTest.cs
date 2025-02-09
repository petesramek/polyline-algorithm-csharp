//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineReader"/> type.
/// </summary>
[TestClass]
public class PolylineWriterTest {
    public static IEnumerable<object[]> Valid_Constructor_Parameters => [
        [ 0 ],
        [ 100 ]
    ];

    public static IEnumerable<object[]> Invalid_Write_Method_Parameters => [
        [ Values.Coordinates.Empty.Select(c => (c.Latitude, c.Longitude)).ToList(), Values.Polyline.Empty ],
        [ Values.Coordinates.Invalid.Select(c => (c.Latitude, c.Longitude)).ToList(), Values.Polyline.Invalid ]
    ];


    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        bool canWrite = false;
        int position = 0;

        // Act
        PolylineWriter writer = new();

        // Assert
        Assert.AreEqual(canWrite, writer.CanWrite);
        Assert.AreEqual(position, writer.Position);
    }

    [TestMethod]
    [DynamicData(nameof(Valid_Constructor_Parameters))]
    public void Constructor_Valid_Parameter_Ok(int length) {
        // Arrange
        Memory<char> buffer = new char[length];
        bool canWrite = !buffer.IsEmpty;
        int position = 0;

        // Act
        PolylineWriter writer = new(in buffer);

        // Assert
        Assert.AreEqual(canWrite, writer.CanWrite);
        Assert.AreEqual(position, writer.Position);
    }

    [TestMethod]
    public void Write_Valid_Parameter_Ok() {
        // Arrange
        IEnumerable<Coordinate> coordinates = Values.Coordinates.Valid;
        Polyline expected = Polyline.FromString(in Values.Polyline.Valid);
        Memory<char> buffer = new char[coordinates.Count() * 12];
        PolylineWriter writer = new(in buffer);
        bool canWrite = buffer.Length > expected.Length;
        int position = expected.Length;

        // Act
        foreach (var coordinate in coordinates) {
            writer.Write(in coordinate);
        }

        // Assert
        Assert.AreEqual(canWrite, writer.CanWrite);
        Assert.AreEqual(position, writer.Position);
        Assert.AreEqual(expected, writer.ToPolyline());
    }

    [TestMethod]
    public void Write_Empty_Buffer_InvalidOperationException() {
        // Arrange
        Coordinate coordinate = new();

        // Act
        static void Write(Coordinate coordinate) {
            Memory<char> buffer = Memory<char>.Empty;
            PolylineWriter writer = new(in buffer);
            
            writer.Write(in coordinate);
        };

        // Assert
        var exception = Assert.ThrowsException<InvalidOperationException>(() => Write(coordinate));
        Assert.IsInstanceOfType<InvalidWriterStateException>(exception.InnerException);
    }

    [TestMethod]
    public void Write_Full_Buffer_InvalidOperationException() {
        // Arrange
        Coordinate coordinate = new();

        // Act
        static void Write(Coordinate coordinate) {
            Memory<char> buffer = new char[1];
            PolylineWriter writer = new(in buffer);

            writer.Write(in coordinate);
        };

        // Assert
        var exception = Assert.ThrowsException<InvalidOperationException>(() => Write(coordinate));
        Assert.IsInstanceOfType<InvalidWriterStateException>(exception.InnerException);
    }
}