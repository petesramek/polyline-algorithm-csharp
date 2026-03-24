//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines tests for <see cref="IAsyncPolylineDecoder{TPolyline, TCoordinate}"/> as implemented by
/// <see cref="PolylineDecoder"/>, and for <see cref="AsyncPolylineDecoderExtensions"/>.
/// </summary>
[TestClass]
public class AsyncPolylineDecoderTest {
    private static readonly PolylineDecoder _decoder = new();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    // ── DecodeAsync(Polyline, CancellationToken) ──────────────────────────────

    [TestMethod]
    public async Task DecodeAsync_Polyline_EmptyPolyline_Throws_ArgumentException() {
        // Arrange & Act
        Task Execute() => _decoder.DecodeAsync(new Polyline(), CancellationToken.None).ToListAsync().AsTask();

        // Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(Execute);
    }

    [TestMethod]
    public async Task DecodeAsync_Polyline_Cancelled_Throws_OperationCanceledException() {
        // Arrange
        string polyline = StaticValueProvider.Valid.GetPolyline();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        Task Execute() => _decoder.DecodeAsync(Polyline.FromString(polyline), cts.Token).ToListAsync().AsTask();

        // Assert
        await Assert.ThrowsExactlyAsync<OperationCanceledException>(Execute);
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public async Task DecodeAsync_Polyline_Random_ValidInput_Ok(int count) {
        // Arrange
        IEnumerable<Coordinate> expected = RandomValueProvider.GetCoordinates(count).Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline value = Polyline.FromString(RandomValueProvider.GetPolyline(count));

        // Act
        var result = await _decoder.DecodeAsync(value, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    [TestMethod]
    public async Task DecodeAsync_Polyline_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline value = Polyline.FromString(StaticValueProvider.Valid.GetPolyline());

        // Act
        var result = await _decoder.DecodeAsync(value, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    // ── DecodeAsync extension — string overload ───────────────────────────────

    [TestMethod]
    public async Task DecodeAsync_String_NullDecoder_Throws_ArgumentNullException() {
        // Arrange
        PolylineDecoder decoder = null!;

        // Act
        Task Execute() => AsyncPolylineDecoderExtensions.DecodeAsync(decoder, string.Empty, CancellationToken.None).ToListAsync().AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("decoder", exception.ParamName);
    }

    [TestMethod]
    public async Task DecodeAsync_String_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        string polyline = StaticValueProvider.Valid.GetPolyline();

        // Act
        var result = await _decoder.DecodeAsync(polyline, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    // ── DecodeAsync extension — char[] overload ───────────────────────────────

    [TestMethod]
    public async Task DecodeAsync_CharArray_NullDecoder_Throws_ArgumentNullException() {
        // Arrange
        PolylineDecoder decoder = null!;

        // Act
        Task Execute() => AsyncPolylineDecoderExtensions.DecodeAsync(decoder, Array.Empty<char>(), CancellationToken.None).ToListAsync().AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("decoder", exception.ParamName);
    }

    [TestMethod]
    public async Task DecodeAsync_CharArray_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        char[] polyline = StaticValueProvider.Valid.GetPolyline().ToCharArray();

        // Act
        var result = await _decoder.DecodeAsync(polyline, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }

    // ── DecodeAsync extension — ReadOnlyMemory<char> overload ────────────────

    [TestMethod]
    public async Task DecodeAsync_Memory_NullDecoder_Throws_ArgumentNullException() {
        // Arrange
        PolylineDecoder decoder = null!;

        // Act
        Task Execute() => AsyncPolylineDecoderExtensions.DecodeAsync(decoder, ReadOnlyMemory<char>.Empty, CancellationToken.None).ToListAsync().AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("decoder", exception.ParamName);
    }

    [TestMethod]
    public async Task DecodeAsync_Memory_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> expected = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        ReadOnlyMemory<char> polyline = StaticValueProvider.Valid.GetPolyline().AsMemory();

        // Act
        var result = await _decoder.DecodeAsync(polyline, CancellationToken.None).ToListAsync();

        // Assert
        CollectionAssert.AreEqual(expected.ToArray(), result.ToArray());
    }
}
