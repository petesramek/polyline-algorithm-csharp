//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Utility;
using System.Linq;

/// <summary>
/// Benchmarks for <see cref="CoordinateDelta"/>.
/// </summary>
public class CoordinateDeltaBenchmark {
    /// <summary>
    /// Number of coordinate pairs iterated per benchmark invocation.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

    private (int Latitude, int Longitude)[] _normalized = default!;

    /// <summary>
    /// Sets up pre-normalized integer coordinate pairs for the benchmark.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        _normalized = RandomValueProvider
            .GetCoordinates(CoordinatesCount)
            .Select(c => (PolylineEncoding.Normalize(c.Latitude), PolylineEncoding.Normalize(c.Longitude)))
            .ToArray();
    }

    /// <summary>
    /// Benchmark: compute successive deltas by advancing a <see cref="CoordinateDelta"/> through all coordinate pairs.
    /// </summary>
    [Benchmark]
    public void CoordinateDelta_Next() {
        var delta = new CoordinateDelta();

        foreach (var (lat, lon) in _normalized) {
            delta.Next(lat, lon);
        }
    }
}
