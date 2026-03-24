//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Utility;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines tests for <see cref="IPolylinePipeDecoder{TCoordinate}"/> as implemented by
/// <see cref="PolylineDecoder"/>, covering the zero-allocation <see cref="PipeReader"/>-based decode path.
/// </summary>
[TestClass]
public class PolylinePipeDecoderTest {
    private static readonly PolylineDecoder _decoder = new();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    // ── Null / empty argument checks ─────────────────────────────────────────

    [TestMethod]
    public async Task DecodeAsync_NullReader_Throws_ArgumentNullException() {
        // Arrange
        PipeReader reader = null!;

        // Act
        Task Execute() => _decoder.DecodeAsync(reader, CancellationToken.None).ToListAsync().AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("reader", exception.ParamName);
    }

    [TestMethod]
    public async Task DecodeAsync_EmptyPipe_Throws_ArgumentException() {
        // Arrange
        PipeReader reader = PipeReader.Create(new System.IO.MemoryStream([]));

        // Act
        Task Execute() => _decoder.DecodeAsync(reader, CancellationToken.None).ToListAsync().AsTask();

        // Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(Execute);
    }

    [TestMethod]
    public async Task DecodeAsync_Cancelled_Throws_OperationCanceledException() {
        // Arrange
        byte[] bytes = Encoding.ASCII.GetBytes(StaticValueProvider.Valid.GetPolyline());
        PipeReader reader = PipeReader.Create(new System.IO.MemoryStream(bytes));
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act — PipeReader.ReadAsync with a cancelled token throws TaskCanceledException
        // (a subtype of OperationCanceledException)
        Task Execute() => _decoder.DecodeAsync(reader, cts.Token).ToListAsync().AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<TaskCanceledException>(Execute);
    }

    // ── Correctness ───────────────────────────────────────────────────────────

    [TestMethod]
    public async Task DecodeAsync_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates()
            .Select(c => new Coordinate(c.Latitude, c.Longitude));
        byte[] bytes = Encoding.ASCII.GetBytes(StaticValueProvider.Valid.GetPolyline());
        PipeReader reader = PipeReader.Create(new System.IO.MemoryStream(bytes));

        // Act
        var result = await _decoder.DecodeAsync(reader, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public async Task DecodeAsync_Random_ValidInput_Ok(int count) {
        // Arrange
        IEnumerable<Coordinate> expected = RandomValueProvider.GetCoordinates(count)
            .Select(c => new Coordinate(c.Latitude, c.Longitude));
        byte[] bytes = Encoding.ASCII.GetBytes(RandomValueProvider.GetPolyline(count));
        PipeReader reader = PipeReader.Create(new System.IO.MemoryStream(bytes));

        // Act
        var result = await _decoder.DecodeAsync(reader, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    [TestMethod]
    public async Task DecodeAsync_SmallBufferChunks_ValidInput_Ok() {
        // Arrange — deliver bytes 2 at a time to exercise multi-read code paths
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates()
            .Select(c => new Coordinate(c.Latitude, c.Longitude));
        byte[] bytes = Encoding.ASCII.GetBytes(StaticValueProvider.Valid.GetPolyline());

        var pipe = new Pipe(new PipeOptions(minimumSegmentSize: 2));

        // Write to pipe in 2-byte chunks
        _ = Task.Run(async () => {
            for (int i = 0; i < bytes.Length; i += 2) {
                int chunkSize = Math.Min(2, bytes.Length - i);
                await pipe.Writer.WriteAsync(new Memory<byte>(bytes, i, chunkSize));
                await pipe.Writer.FlushAsync();
            }

            await pipe.Writer.CompleteAsync();
        });

        // Act
        var result = await _decoder.DecodeAsync(pipe.Reader, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }
}
