//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for <see cref="PolylineReader"/> type.
/// </summary>
[TestClass]
public class PolylineWriterTest
{
    public static IEnumerable<object[]> Valid_Constructor_Parameter => [
        [ 0 ],
        [ 100 ]
    ];

    public static IEnumerable<object[]> Invalid_Coordinate_Parameter =>
        Values.Coordinates.Invalid.Select(c => new object[] { (c.Latitude, c.Longitude) });

    public static IEnumerable<object[]> Invalid_Buffer_Size_Parameter => [
        [0],
        [1],
        [2]
    ];


    [TestMethod]
    public void Constructor_Parameterless_Ok()
    {
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
    [DynamicData(nameof(Valid_Constructor_Parameter))]
    public void Constructor_Valid_Parameter_Ok(int length)
    {
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
    public void Write_Valid_Parameter_Ok()
    {
        // Arrange
        IEnumerable<Coordinate> coordinates = Values.Coordinates.Valid;
        Polyline expected = Polyline.FromString(Values.Polyline.Valid);
        Memory<char> buffer = new char[coordinates.Count() * 12];
        PolylineWriter writer = new(in buffer);
        bool canWrite = buffer.Length > expected.Length;
        int position = expected.Length;

        // Act
        foreach (var coordinate in coordinates)
        {
            writer.Write(in coordinate);
        }

        // Assert
        Assert.AreEqual(canWrite, writer.CanWrite);
        Assert.AreEqual(position, writer.Position);
        Assert.AreEqual(expected, writer.ToPolyline());
    }

    [TestMethod]
    [DynamicData(nameof(Invalid_Coordinate_Parameter))]
    public void Write_Invalid_Coordinate_Parameter_Ok((double Latitude, double Longitude) value)
    {
        // Arrange
        Coordinate coordinate = new(value.Latitude, value.Longitude);
        int bufferSize = 12;

        // Act
        static void Write(Coordinate coordinate, int bufferSize)
        {
            Memory<char> buffer = new char[bufferSize];
            PolylineWriter writer = new(in buffer);
            writer.Write(in coordinate);
        }

        // Assert
        Assert.ThrowsExactly<InvalidCoordinateException>(() => Write(coordinate, bufferSize));
    }

    [TestMethod]
    [DynamicData(nameof(Invalid_Buffer_Size_Parameter))]
    public void Write_Buffer_Overflow_InvalidWriterStateException_Thrown(int bufferSize)
    {
        // Arrange
        Coordinate coordinate = new();

        // Act
        static void Write(Coordinate coordinate, int bufferSize)
        {
            Memory<char> buffer = new char[bufferSize];
            PolylineWriter writer = new(in buffer);

            writer.Write(in coordinate);
            writer.Write(in coordinate);
        }
        ;

        // Assert
        var exception = Assert.ThrowsExactly<InvalidWriterStateException>(() => Write(coordinate, bufferSize));
    }
}