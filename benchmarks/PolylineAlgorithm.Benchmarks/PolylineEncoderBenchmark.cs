//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;
using System.Collections.Generic;

/// <summary>
/// Benchmarks for the <see cref="PolylineEncoder"/> class.
/// </summary>
public class PolylineEncoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 100, 1_000)]
    public int Count;

    [Params(100)]
    public int Iterations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the enumeration of coordinates to be encoded.
    /// </summary>
    public IEnumerable<Coordinate> Enumeration { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public List<Coordinate> List { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public Coordinate[] Array { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public ReadOnlyMemory<Coordinate> Memory { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline encoder instance.
    /// </summary>
    public readonly PolylineEncoder Encoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Enumeration = RandomValueProvider.GetCoordinates(Count).Select(c => new Coordinate(c.Latitude, c.Longitude));
        List = [.. Enumeration];
        Array = [.. Enumeration];
        Memory =  Array.AsMemory();
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_Span() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Encoder
                .Encode(Memory.Span!);

            _consumer.Consume(polyline);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_Array() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Encoder
                .Encode(Array!);

            _consumer.Consume(polyline);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_List() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Encoder
                .Encode(List!);

            _consumer.Consume(polyline);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void PolylineEncoder_Encode_Enumerator() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Encoder
            .Encode(Enumeration!);

            _consumer.Consume(polyline);
        }
    }
}