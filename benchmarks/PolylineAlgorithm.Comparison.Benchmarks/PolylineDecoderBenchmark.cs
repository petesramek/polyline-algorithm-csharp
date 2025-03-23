//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Comparison.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Cloudikka.PolylineAlgorithm.Encoding;
using global::PolylineEncoder.Net.Utility;
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Comparison.Benchmarks.Internal;
using PolylinerNet;

/// <summary>
/// Benchmarks for the <see cref="PolylineAlgorithm.PolylineDecoder"/> class.
/// </summary>
[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int N;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public string StringValue { get; private set; }

    /// <summary>
    /// Gets the character array representing the encoded polyline.
    /// </summary>
    public Polyline PolylineValue { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline decoder instance.
    /// </summary>
    public PolylineDecoder<Coordinate> PolylineAlgorithm = new(new CoordinateFactory());

    public PolylineEncoding Cloudikka = new();

    public Polyliner PolylinerNet = new();

    public PolylineUtility PolylineUtility = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        StringValue = ValueProvider.GetPolyline(N).ToString();
        PolylineValue = ValueProvider.GetPolyline(N);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void PolylineAlgorithm_Decode() {
        PolylineAlgorithm
            .Decode(PolylineValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a character array.
    /// </summary>
    [Benchmark]
    public void Cloudikka_Decode() {
        Cloudikka
            .Decode(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylinerNet_Decode() {
        PolylinerNet
            .Decode(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void Polylines_Decode() {
        Polylines.Polyline
            .DecodePolyline(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylineUtility_Decode() {
        PolylineUtility
            .Decode(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylineReader_ReadToEnd() {
        PolylineReader reader = new(StringValue);

        var result = ReadToEnd(ref reader);

        result
            .Consume(_consumer);

        static IEnumerable<Coordinate> ReadToEnd(ref PolylineReader reader) {
            var result = new List<Coordinate>();

            while (reader.Read()) {
                result.Add(new(reader.Latitude, reader.Longitude));
            }

            return result;
        }
    }
}