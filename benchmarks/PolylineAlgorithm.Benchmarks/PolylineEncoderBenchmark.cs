//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm;
using System.Collections.Generic;

/// <summary>
/// Benchmarks for the <see cref="PolylineEncoder"/> class.
/// </summary>
[RankColumn]
public class PolylineEncoderBenchmark {
    private static readonly Random R = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the enumeration of coordinates to be encoded.
    /// </summary>
    public static IEnumerable<Coordinate> Enumeration { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public static List<Coordinate> List { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline encoder instance.
    /// </summary>
    public PolylineEncoder Encoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Enumeration = Enumerable.Range(0, 100).Select(i => new Coordinate(R.Next(-90, 90) + R.NextDouble(), R.Next(-180, 180) + R.NextDouble()));
        List = new List<Coordinate>(Enumeration);
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineEncoder_Encode_List() {
        return Encoder.Encode(List!);
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineEncoder_Encode_Enumerator() {
        return Encoder.Encode(Enumeration!);
    }
}