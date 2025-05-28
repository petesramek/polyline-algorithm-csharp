//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Utility;
using System.Buffers;

/// <summary>
/// Benchmarks for the <see cref="PolylineValue"/> struct.
/// </summary>
[RankColumn]
public class PolylineBuilderBenchmark {
    [Params(1, 10, 100, 500, 1_000)]
    public int Length;

    [Params(1, 5, 10, 50, 100)]
    public int SegmentsCount;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    internal ReadOnlyMemory<char> MemoryValue { get; private set; }

    internal PolylineBuilder Builder { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Builder = new PolylineBuilder();
        MemoryValue = RandomValueProvider.GetPolyline(Length).ToString().AsMemory();
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public ReadOnlySequence<char> PolylineBuilder_Append_Memory() {
        for (int i = 0; i < SegmentsCount; i++) {
            Builder
                .Append(MemoryValue);
        }

        return Builder.Build();
    }
}