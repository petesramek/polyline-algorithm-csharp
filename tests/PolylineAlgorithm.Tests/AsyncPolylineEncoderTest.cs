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
/// Defines tests for <see cref="IAsyncPolylineEncoder{TCoordinate, TPolyline}"/> as implemented by
/// <see cref="PolylineEncoder"/>, and for <see cref="AsyncPolylineEncoderExtensions"/>.
/// </summary>
[TestClass]
public class AsyncPolylineEncoderTest {
    private static readonly PolylineEncoder _encoder = new();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    // ── EncodeAsync(IAsyncEnumerable, CancellationToken) ─────────────────────

    [TestMethod]
    public async Task EncodeAsync_NullCoordinates_Throws_ArgumentNullException() {
        // Arrange
        IAsyncEnumerable<Coordinate> coordinates = null!;

        // Act
        Task Execute() => _encoder.EncodeAsync(coordinates, CancellationToken.None).AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("coordinates", exception.ParamName);
    }

    [TestMethod]
    public async Task EncodeAsync_EmptyCoordinates_Throws_ArgumentException() {
        // Arrange
        async IAsyncEnumerable<Coordinate> Empty() { await Task.CompletedTask; yield break; }

        // Act
        Task Execute() => _encoder.EncodeAsync(Empty(), CancellationToken.None).AsTask();

        // Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(Execute);
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public async Task EncodeAsync_Random_ValidInput_Ok(int count) {
        // Arrange
        IEnumerable<Coordinate> coordinates = RandomValueProvider.GetCoordinates(count).Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline expected = Polyline.FromString(RandomValueProvider.GetPolyline(count));

        // Act
        Polyline result = await _encoder.EncodeAsync(ToAsyncEnumerable(coordinates), CancellationToken.None);

        // Assert
        Assert.IsTrue(expected.Equals(result));
    }

    [TestMethod]
    public async Task EncodeAsync_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> coordinates = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline expected = Polyline.FromString(StaticValueProvider.Valid.GetPolyline());

        // Act
        Polyline result = await _encoder.EncodeAsync(ToAsyncEnumerable(coordinates), CancellationToken.None);

        // Assert
        Assert.IsTrue(expected.Equals(result));
    }

    // ── EncodeAsync extension — IEnumerable<Coordinate> overload ─────────────

    [TestMethod]
    public async Task EncodeAsync_Extension_NullEncoder_Throws_ArgumentNullException() {
        // Arrange
        PolylineEncoder encoder = null!;

        // Act
        Task Execute() => AsyncPolylineEncoderExtensions.EncodeAsync(encoder, Array.Empty<Coordinate>(), CancellationToken.None).AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("encoder", exception.ParamName);
    }

    [TestMethod]
    public async Task EncodeAsync_Extension_NullCoordinates_Throws_ArgumentNullException() {
        // Arrange
        IEnumerable<Coordinate> coordinates = null!;

        // Act
        Task Execute() => _encoder.EncodeAsync(coordinates, CancellationToken.None).AsTask();

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<ArgumentNullException>(Execute);
        Assert.AreEqual("coordinates", exception.ParamName);
    }

    [TestMethod]
    public async Task EncodeAsync_Extension_Static_ValidInput_Ok() {
        // Arrange
        IEnumerable<Coordinate> coordinates = StaticValueProvider.Valid.GetCoordinates().Select(c => new Coordinate(c.Latitude, c.Longitude));
        Polyline expected = Polyline.FromString(StaticValueProvider.Valid.GetPolyline());

        // Act
        Polyline result = await _encoder.EncodeAsync(coordinates, CancellationToken.None);

        // Assert
        Assert.IsTrue(expected.Equals(result));
    }

    // ── Round-trip ────────────────────────────────────────────────────────────

    [TestMethod]
    public async Task EncodeDecodeAsync_RoundTrip_Ok() {
        // Arrange
        var decoder = new PolylineDecoder();
        var coordinates = new List<Coordinate> { new(10, 20), new(-10, -20), new(0, 0) };

        // Act
        Polyline polyline = await _encoder.EncodeAsync(ToAsyncEnumerable(coordinates), CancellationToken.None);
        var decoded = await decoder.DecodeAsync(polyline, CancellationToken.None).ToListAsync();

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
