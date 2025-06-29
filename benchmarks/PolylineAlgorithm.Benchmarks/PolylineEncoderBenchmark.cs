//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm;
using PolylineAlgorithm.Utility;
using System.Collections.Generic;

/// <summary>
/// Benchmarks for the <see cref="PolylineEncoder"/> class.
/// </summary>
[RankColumn]
public class PolylineEncoderBenchmark {
    [Params(1, 25, 50, 100, 250, 500, 1_000)]
    public int Count;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the enumeration of coordinates to be encoded.
    /// </summary>
    public IEnumerable<Coordinate> Enumeration { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public List<Coordinate> List { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline encoder instance.
    /// </summary>
    public CoordinateEncoder Encoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Enumeration = RandomValueProvider.GetCoordinates(Count).Select(c => new Coordinate(c.Latitude, c.Longitude));
        List = [.. Enumeration];
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineEncoder_Encode_List() {
        var polyline = Encoder
            .Encode(List!);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineEncoder_Encode_Enumerator() {
        var polyline = Encoder
            .Encode(Enumeration!);

        return polyline;
    }
}