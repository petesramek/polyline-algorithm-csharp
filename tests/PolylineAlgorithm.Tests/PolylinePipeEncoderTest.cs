//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Utility;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines tests for <see cref="IPolylinePipeEncoder{TCoordinate}"/> as implemented by
/// <see cref="PolylineEncoder"/>, covering the zero-allocation <see cref="PipeWriter"/>-based encode path.
/// </summary>
[TestClass]
public class PolylinePipeEncoderTest {
    private static readonly PolylineEncoder _encoder = new();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    // ── Null / empty argument checks ─────────────────────────────────────────

    [TestMethod]
    public async Task EncodeAsync_NullCoordinates_Throws_ArgumentNullException() {
        // Arrange
        IAsyncEnumerable<Coordinate> coordinates = null!;
        PipeWriter writer = new Pipe().Writer;

        // Act
        Task Execute() => _encoder.EncodeAsync(coordinates, writer, CancellationToken.None).AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("coordinates", exception.ParamName);
    }

    [TestMethod]
    public async Task EncodeAsync_NullWriter_Throws_ArgumentNullException() {
        // Arrange
        PipeWriter writer = null!;

        // Act
        Task Execute() => _encoder.EncodeAsync(ToAsyncEnumerable([]), writer, CancellationToken.None).AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("writer", exception.ParamName);
    }

    // ── Correctness ───────────────────────────────────────────────────────────

    [TestMethod]
    public async Task EncodeAsync_Static_ValidInput_WritesExpectedBytes() {
        // Arrange
        IEnumerable<Coordinate> coordinates = StaticValueProvider.Valid.GetCoordinates()
            .Select(c => new Coordinate(c.Latitude, c.Longitude));
        string expected = StaticValueProvider.Valid.GetPolyline();

        var pipe = new Pipe();

        // Act
        await _encoder.EncodeAsync(ToAsyncEnumerable(coordinates), pipe.Writer, CancellationToken.None);
        await pipe.Writer.CompleteAsync();

        ReadResult readResult = await pipe.Reader.ReadAsync(CancellationToken.None);
        string actual = Encoding.ASCII.GetString(readResult.Buffer.IsSingleSegment
            ? readResult.Buffer.First.Span
            : readResult.Buffer.ToArray());
        pipe.Reader.AdvanceTo(readResult.Buffer.End);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public async Task EncodeAsync_Random_ValidInput_WritesExpectedBytes(int count) {
        // Arrange
        IEnumerable<Coordinate> coordinates = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude));
        string expected = RandomValueProvider.GetPolyline(count);

        var pipe = new Pipe();

        // Act
        await _encoder.EncodeAsync(ToAsyncEnumerable(coordinates), pipe.Writer, CancellationToken.None);
        await pipe.Writer.CompleteAsync();

        ReadResult readResult = await pipe.Reader.ReadAsync(CancellationToken.None);
        string actual = Encoding.ASCII.GetString(readResult.Buffer.IsSingleSegment
            ? readResult.Buffer.First.Span
            : readResult.Buffer.ToArray());
        pipe.Reader.AdvanceTo(readResult.Buffer.End);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    // ── Round-trip via pipes ──────────────────────────────────────────────────

    [TestMethod]
    public async Task EncodeDecode_RoundTrip_Via_Pipes_Ok() {
        // Arrange
        var decoder = new PolylineDecoder();
        var coordinates = new List<Coordinate> { new(10, 20), new(-10, -20), new(0, 0) };
        var pipe = new Pipe();

        // Act — encode to pipe
        await _encoder.EncodeAsync(ToAsyncEnumerable(coordinates), pipe.Writer, CancellationToken.None);
        await pipe.Writer.CompleteAsync();

        // Act — decode from pipe
        var decoded = await decoder.DecodeAsync(pipe.Reader, CancellationToken.None).ToListAsync();

        // Assert
        Assert.AreEqual(coordinates.Count, decoded.Count);
        for (int i = 0; i < coordinates.Count; i++) {
            Assert.AreEqual(coordinates[i].Latitude, decoded[i].Latitude);
            Assert.AreEqual(coordinates[i].Longitude, decoded[i].Longitude);
        }
    }

    private static async IAsyncEnumerable<Coordinate> ToAsyncEnumerable(IEnumerable<Coordinate> source) {
        foreach (var item in source) {
            yield return item;
        }
    }
}
