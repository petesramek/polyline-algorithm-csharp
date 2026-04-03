//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm.Internal;

/// <summary>
/// Benchmarks for <see cref="Pow10"/>.
/// </summary>
public class Pow10Benchmark {
    /// <summary>
    /// Benchmark: retrieve the cached power of 10 for precision 1 (10^1 = 10).
    /// </summary>
    [Benchmark(Baseline = true)]
    public uint Pow10_GetFactor_Precision1() => Pow10.GetFactor(1);

    /// <summary>
    /// Benchmark: retrieve the cached power of 10 for the default precision 5 (10^5 = 100_000).
    /// </summary>
    [Benchmark]
    public uint Pow10_GetFactor_Precision5() => Pow10.GetFactor(5);

    /// <summary>
    /// Benchmark: retrieve the cached power of 10 for the maximum pre-computed precision 9 (10^9 = 1_000_000_000).
    /// </summary>
    [Benchmark]
    public uint Pow10_GetFactor_Precision9() => Pow10.GetFactor(9);
}
