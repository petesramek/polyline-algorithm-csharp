//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Tests.Data;

/// <summary>
/// Defines tests for the <see cref="PolylineWriter"/> type.
/// </summary>
[TestClass]
public class PolylineWriterTest {
    /// <summary>
    /// Provides test data for the <see cref="Constructor_Valid_Parameter_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> Valid_Constructor_Parameter => new List<object[]> {
        new object[] { 0 },
        new object[] { 100 }
    };

    /// <summary>
    /// Provides test data for the <see cref="Write_Invalid_Coordinate_Parameter_Ok"/> method.
    /// </summary>
    public static IEnumerable<object[]> Invalid_Coordinate_Parameter =>
        Values.Coordinates.Invalid.Select(c => new object[] { (c.Latitude, c.Longitude) });

    /// <summary>
    /// Provides test data for the <see cref="Write_Buffer_Overflow_InvalidWriterStateException_Thrown"/> method.
    /// </summary>
    public static IEnumerable<object[]> Invalid_Buffer_Size_Parameter => new List<object[]> {
        new object[] { 0 },
        new object[] { 1 },
        new object[] { 2 }
    };

    /// <summary>
    /// Tests the parameterless constructor of the <see cref="PolylineWriter"/> class.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="PolylineWriter"/> constructor with valid parameters.
    /// </summary>
    /// <param name="length">The length of the buffer.</param>
    [TestMethod]
    [DynamicData(nameof(Valid_Constructor_Parameter))]
    public void Constructor_Valid_Parameter_Ok(int length) {
        // Arrange
        Memory<char> buffer = new char[length];
        bool canWrite = !buffer.IsEmpty;
        int position = 0;

        // Act
        PolylineWriter writer = new(buffer);

        // Assert
        Assert.AreEqual(canWrite, writer.CanWrite);
        Assert.AreEqual(position, writer.Position);
    }

    /// <summary>
    /// Tests the <see cref="PolylineWriter.Write"/> method with valid parameters.
    /// </summary>
    [TestMethod]
    public void Write_Valid_Parameter_Ok() {
        // Arrange
        IEnumerable<Coordinate> coordinates = Values.Coordinates.Valid;
        Polyline expected = Polyline.FromString(Values.Polyline.Valid);
        Memory<char> buffer = new char[coordinates.Count() * 12];
        PolylineWriter writer = new(buffer);
        bool canWrite = buffer.Length > expected.Length;
        int position = expected.Length;

        // Act
        foreach (var coordinate in coordinates) {
            writer.Write(coordinate);
        }

        // Assert
        Assert.AreEqual(canWrite, writer.CanWrite);
        Assert.AreEqual(position, writer.Position);
        Assert.AreEqual(expected, writer.ToPolyline());
    }

    /// <summary>
    /// Tests the <see cref="PolylineWriter.Write"/> method with invalid coordinate parameters, expecting an <see cref="InvalidCoordinateException"/>.
    /// </summary>
    /// <param name="value">The invalid coordinate value.</param>
    [TestMethod]
    [DynamicData(nameof(Invalid_Coordinate_Parameter))]
    public void Write_Invalid_Coordinate_Parameter_Ok((double Latitude, double Longitude) value) {
        // Arrange
        Coordinate coordinate = new(value.Latitude, value.Longitude);
        int bufferSize = 12;

        // Act
        static void Write(Coordinate coordinate, int bufferSize) {
            Memory<char> buffer = new char[bufferSize];
            PolylineWriter writer = new(buffer);
            writer.Write(coordinate);
        }

        // Assert
        Assert.ThrowsExactly<InvalidCoordinateException>(() => Write(coordinate, bufferSize));
    }

    /// <summary>
    /// Tests the <see cref="PolylineWriter.Write"/> method with buffer overflow, expecting an <see cref="InvalidWriterStateException"/>.
    /// </summary>
    /// <param name="bufferSize">The size of the buffer.</param>
    [TestMethod]
    [DynamicData(nameof(Invalid_Buffer_Size_Parameter))]
    public void Write_Buffer_Overflow_InvalidWriterStateException_Thrown(int bufferSize) {
        // Arrange
        Coordinate coordinate = new();

        // Act
        static void Write(Coordinate coordinate, int bufferSize) {
            Memory<char> buffer = new char[bufferSize];
            PolylineWriter writer = new(buffer);

            writer.Write(coordinate);
            writer.Write(coordinate);
        }

        // Assert
        Assert.ThrowsExactly<InvalidWriterStateException>(() => Write(coordinate, bufferSize));
    }
}