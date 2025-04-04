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

/// <summary>
/// Benchmarks for the <see cref="PolylineDecoder"/> class.
/// </summary>
[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 25, 50, 100, 250, 500, 1_000, 5_000, 10_000, 25_000, 50_000, 100_000, 500_000, 1_000_000)]
    public int Count;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public Polyline Polyline { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline decoder instance.
    /// </summary>
    public DefaultPolylineDecoder Decoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Polyline = ValueProvider.GetPolyline(Count);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode() {
        Decoder
            .Decode(Polyline)
            .Consume(_consumer);
    }
}