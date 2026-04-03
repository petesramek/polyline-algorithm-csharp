//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for the polyline encoding validation methods in <see cref="PolylineEncoding"/>.
/// </summary>
public class PolylineEncodingBenchmark {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private string polyline;
#pragma warning restore CS8618

    /// <summary>
    /// Number of coordinates for benchmarks. Set by BenchmarkDotNet.
    /// </summary>
    [Params(8, 64, 128, 1024, 4096, 20480, 102400)]
    public int CoordinatesCount { get; set; }

    [GlobalSetup]
    public void Setup() {
        polyline = RandomValueProvider.GetPolyline(CoordinatesCount);
    }

    [Benchmark(Baseline = true)]
    public void ValidateCharRange() => PolylineEncoding.ValidateCharRange(polyline);

    [Benchmark]
    public void ValidateBlockLength() => PolylineEncoding.ValidateBlockLength(polyline);

    [Benchmark]
    public void ValidateFormat() => PolylineEncoding.ValidateFormat(polyline);
}