//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Extensions;
using PolylineAlgorithm.Utility;
using System.Collections.Generic;

/// <summary>
/// Benchmarks for <see cref="PolylineEncoder"/>.
/// </summary>
public class PolylineEncoderBenchmark {
    private readonly Consumer _consumer = new();

    /// <summary>
    /// Number of coordinates for benchmarks.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Coordinates as list.
    /// </summary>
    public List<Coordinate> List { get; private set; }

    /// <summary>
    /// Coordinates as array.
    /// </summary>
    public Coordinate[] Array { get; private set; }

    /// <summary>
    /// Coordinates as read-only memory.
    /// </summary>
    public ReadOnlyMemory<Coordinate> Memory { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Polyline encoder instance.
    /// </summary>
    private readonly PolylineEncoder _encoder = new();

    /// <summary>
    /// Sets up benchmark data.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        List = [.. RandomValueProvider.GetCoordinates(CoordinatesCount).Select(c => new Coordinate(c.Latitude, c.Longitude))];
        Array = [.. List];
        Memory = Array.AsMemory();
    }

    /// <summary>
    /// Benchmark: encode coordinates from span.
    /// </summary>
    /// <returns>Encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_Span() {
        var polyline = _encoder
            .Encode(Memory.Span!);

        _consumer.Consume(polyline);
    }

    /// <summary>
    /// Benchmark: encode coordinates from array.
    /// </summary>
    /// <returns>Encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_Array() {
        var polyline = _encoder
            .Encode(Array!);

        _consumer.Consume(polyline);
    }

    /// <summary>
    /// Benchmark: encode coordinates from list.
    /// </summary>
    /// <returns>Encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_List() {
        var polyline = _encoder
            .Encode(List!);

        _consumer.Consume(polyline);
    }
}