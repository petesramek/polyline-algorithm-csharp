//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Abstraction;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Utility;
using System;

/// <summary>
/// Tests for <see cref="AbstractPolylineEncoder{TCoordinate, TPolyline}"/>.
/// </summary>
[TestClass]
public sealed class AbstractPolylineEncoderTests {
    private sealed class TestStringEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
        public TestStringEncoder()
            : base() { }

        public TestStringEncoder(PolylineEncodingOptions options)
            : base(options) { }

        protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
        protected override void Write((double Latitude, double Longitude) item, IPolylineWriter writer) {
            writer.Write(item.Latitude);
            writer.Write(item.Longitude);
        }
    }

    /// <summary>
    /// Tests that the default constructor creates an instance with default options.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Default_Options_Creates_Instance() {
        // Act
        TestStringEncoder encoder = new();

        // Assert
        Assert.IsNotNull(encoder);
        Assert.IsNotNull(encoder.Options);
        Assert.AreEqual(5u, encoder.Options.Precision);
        Assert.AreEqual(512, encoder.Options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that the options constructor with null throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Null_Options_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(() => new TestStringEncoder(null!));
        Assert.AreEqual("options", ex.ParamName);
    }

    /// <summary>
    /// Tests that the options constructor stores the provided options.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Options_Stores_Options() {
        // Arrange
        PolylineEncodingOptions options = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(7)
            .Build();

        // Act
        TestStringEncoder encoder = new(options);

        // Assert
        Assert.AreSame(options, encoder.Options);
    }

    /// <summary>
    /// Tests that Encode with an empty span throws <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Empty_Span_Throws_ArgumentException() {
        // Arrange
        TestStringEncoder encoder = new();

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => encoder.Encode(ReadOnlySpan<(double, double)>.Empty));
    }

    /// <summary>
    /// Tests that Encode with a single valid coordinate returns a non-empty string.
    /// </summary>
    [TestMethod]
    public void Encode_With_Single_Coordinate_Returns_Non_Empty_String() {
        // Arrange
        TestStringEncoder encoder = new();
        (double, double)[] coordinates = [(0.0, 0.0)];

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
    }

    /// <summary>
    /// Tests that Encode with known coordinates returns the expected polyline string.
    /// </summary>
    [TestMethod]
    public void Encode_With_Known_Coordinates_Returns_Expected_Polyline() {
        // Arrange
        TestStringEncoder encoder = new();
        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests that Encode with a pre-cancelled token throws <see cref="OperationCanceledException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Pre_Cancelled_Token_Throws_OperationCanceledException() {
        // Arrange
        TestStringEncoder encoder = new();
        using CancellationTokenSource cts = new();
        cts.Cancel();
        (double, double)[] coordinates = [(0.0, 0.0), (1.0, 1.0)];

        // Act & Assert
        Assert.ThrowsExactly<OperationCanceledException>(() => encoder.Encode(coordinates.AsSpan(), cts.Token));
    }

    /// <summary>
    /// Tests that Encode still produces the correct result when the buffer exceeds the stack allocation
    /// limit, forcing heap allocation via <see cref="System.Buffers.ArrayPool{T}"/>.
    /// </summary>
    [TestMethod]
    public void Encode_With_Small_Stack_Alloc_Limit_Uses_Heap_Allocation_And_Produces_Correct_Result() {
        // Arrange — force heap path by making stackAllocLimit smaller than any real encoding needs
        PolylineEncodingOptions options = PolylineEncodingOptionsBuilder.Create()
            .WithStackAllocLimit(1)
            .Build();
        TestStringEncoder encoder = new(options);
        (double Latitude, double Longitude)[] coordinates = [.. StaticValueProvider.Valid.GetCoordinates()];
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        string result = encoder.Encode(coordinates.AsSpan());

        // Assert
        Assert.AreEqual(expected, result);
    }
}
