//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Comparison.Benchmarks;

using BenchmarkDotNet.Attributes;
using global::PolylineEncoder.Net.Utility;
using PolylineAlgorithm;
using PolylineAlgorithm.Utility;
using PolylinerNet;
using System.Collections.Generic;
using PolylineEncoding = Cloudikka.PolylineAlgorithm.Encoding.PolylineEncoding;

/// <summary>
/// Benchmarks for the <see cref="PolylineEncoder"/> class.
/// </summary>
[RankColumn]
public class PolylineEncoderBenchmark {
    [Params(1, 10, 50, 100, 250, 500, 1_000, 5_000, 10_000, 25_000, 50_000, 100_000, 500_000, 1_000_000)]
    public int Count;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public IEnumerable<Coordinate> PolylineAlgorithmEnumeration { get; private set; }
    public IEnumerable<(double, double)> CloudikkaEnumeration { get; private set; }
    public IEnumerable<PolylinePoint> PolylinerEnumeration { get; private set; }
    public IEnumerable<Polylines.PolylineCoordinate> PolylinesEnumeration { get; private set; }
    public IEnumerable<Tuple<double, double>> PolylineUtilityEnumeration { get; private set; }
    public List<Coordinate> PolylineAlgorithmList { get; private set; }
    public List<(double, double)> CloudikkaList { get; private set; }
    public List<PolylinePoint> PolylinerList { get; private set; }
    public List<Polylines.PolylineCoordinate> PolylinesList { get; private set; }
    public List<Tuple<double, double>> PolylineUtilityList { get; private set; }

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
        PolylineAlgorithmEnumeration = ValueProvider.GetCoordinates(Count);
        CloudikkaEnumeration = PolylineAlgorithmEnumeration.Select(c => (c.Latitude, c.Longitude));
        PolylinerEnumeration = PolylineAlgorithmEnumeration.Select(c => new PolylinePoint(c.Latitude, c.Longitude));
        PolylinesEnumeration = PolylineAlgorithmEnumeration.Select(c => new Polylines.PolylineCoordinate { Latitude = c.Latitude, Longitude = c.Longitude });
        PolylineUtilityEnumeration = PolylineAlgorithmEnumeration.Select(c => new Tuple<double, double>(c.Latitude, c.Longitude));
        PolylineAlgorithmList = [.. PolylineAlgorithmEnumeration];
        CloudikkaList = [.. CloudikkaEnumeration];
        PolylinerList = [.. PolylinerEnumeration];
        PolylinesList = [.. PolylinesEnumeration];
        PolylineUtilityList = [.. PolylineUtilityEnumeration];
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public Polyline PolylineAlgorithm_Encode_Enumeration() {
        return PolylineAlgorithm
            .Encode(PolylineAlgorithmEnumeration);
    }

    [Benchmark(Baseline = true)]
    public Polyline PolylineAlgorithm_Encode_List() {
        return PolylineAlgorithm
            .Encode(PolylineAlgorithmList);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a character array.
    /// </summary>
    [Benchmark]
    public string Cloudikka_Encode_Enumeration() {
        return Cloudikka
            .Encode(CloudikkaEnumeration);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a character array.
    /// </summary>
    [Benchmark]
    public string Cloudikka_Encode_List() {
        return Cloudikka
            .Encode(CloudikkaList);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string PolylinerNet_Encode_Enumeration() {
        return PolylinerNet
            .Encode([.. PolylinerEnumeration]);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string PolylinerNet_Encode_List() {
        return PolylinerNet
            .Encode(PolylinerList);
    }


    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string Polylines_Encode_Enumeration() {
        return Polylines.Polyline
            .EncodePoints([.. PolylinesEnumeration]);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string Polylines_Encode_List() {
        return Polylines.Polyline
            .EncodePoints(PolylinesList);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string PolylineUtility_Encode_Enumeration() {
        return PolylineUtility
            .Encode([.. PolylineUtilityEnumeration]);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public string PolylineUtility_Encode_List() {
        return PolylineUtility
            .Encode(PolylineUtilityList);
    }
}