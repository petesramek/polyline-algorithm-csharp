//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Comparison.Benchmarks;

using BenchmarkDotNet.Attributes;
using global::PolylineEncoder.Net.Utility;
using PolylineAlgorithm;
using PolylineAlgorithm.Comparison.Benchmarks.Internal;
using PolylinerNet;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolylineEncoding = Cloudikka.PolylineAlgorithm.Encoding.PolylineEncoding;

/// <summary>
/// Benchmarks for the <see cref="PolylineEncoder"/> class.
/// </summary>
[RankColumn]
public class PolylineEncoderBenchmark {
    [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int N;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the enumeration of coordinates to be encoded.
    /// </summary>
    public static IEnumerable<Coordinate> Enumeration { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public static List<Coordinate> List { get; private set; }

    public static IAsyncEnumerable<Coordinate> AsyncEnumeration { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline encoder instance.
    /// </summary>
    public PolylineEncoder PolylineAlgorithm = new();

    public PolylineEncoding Cloudikka = new();

    public Polyliner PolylinerNet = new();

    public PolylineUtility PolylineUtility = new();

    /// <summary>
    /// The async polyline encoder instance.
    /// </summary>
    //public AsyncPolylineEncoder AsyncEncoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Enumeration = ValueProvider.GetCoordinates(N);
        List = [.. Enumeration];
        AsyncEnumeration = GetAsyncEnumeration(Enumeration!);
    }

    private async IAsyncEnumerable<Coordinate> GetAsyncEnumeration(IEnumerable<Coordinate> enumerable) {
        foreach (var item in enumerable) {
            yield return await new ValueTask<Coordinate>(item);
        }
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark(Baseline = true)]
    public Polyline PolylineAlgorithm_Encode() {
        return PolylineAlgorithm
            .Encode(Enumeration);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a character array.
    /// </summary>
    [Benchmark]
    public string Cloudikka_Encode() {
        return Cloudikka
            .Encode(Enumeration.Select(c => (c.Latitude, c.Longitude)));
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylinerNet_Encode() {
        PolylinerNet
            .Encode([.. Enumeration.Select(c => new PolylinePoint(c.Latitude, c.Longitude))]);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string Polylines_Encode() {
        return Polylines.Polyline
            .EncodePoints(Enumeration.Select(c => new Polylines.PolylineCoordinate { Latitude = c.Latitude, Longitude = c.Longitude }));
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string PolylineUtility_Encode() {
        return PolylineUtility
            .Encode(Enumeration.Select(c => new Tuple<double, double>(c.Latitude, c.Longitude)));
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    //[Benchmark]
    //public void PolylineReader_ReadToEnd() {
    //    PolylineReader reader = new(StringValue);

    //    var result = ReadToEnd(ref reader);

    //    result
    //        .Consume(_consumer);

    //    static IEnumerable<Coordinate> ReadToEnd(ref PolylineReader reader) {
    //        var result = new List<Coordinate>();

    //        while (reader.Read()) {
    //            result.Add(new(reader.Latitude, reader.Longitude));
    //        }

    //        return result;
    //    }
    //}
}