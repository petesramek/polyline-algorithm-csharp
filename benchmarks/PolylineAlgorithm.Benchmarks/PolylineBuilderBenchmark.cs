//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for the <see cref="PolylineValue"/> struct.
/// </summary>
[RankColumn]
public class PolylineBuilderBenchmark {
    [Params(1, 10, 100, 500, 1_000)]
    public int Count;

    [Params(1, 5, 10, 50, 100)]
    public int SegmentsCount;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the character array representing the encoded polyline.
    /// </summary>
    public char[] CharArrayValue { get; private set; }

    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public ReadOnlyMemory<char> MemoryValue { get; private set; }


    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public string StringValue { get; private set; }

    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public Polyline PolylineNotEqualValue { get; private set; }

    public char[] CopyToDestination { get; private set; }

    internal PolylineBuilder Builder { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Builder = new PolylineBuilder();

        var polyline = ValueProvider.GetPolyline(Count);
        StringValue = polyline.ToString();
        CharArrayValue = [.. StringValue];
        MemoryValue = CharArrayValue.AsMemory();
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineBuilder_Append_Memory() {
        for(int i = 0; i < SegmentsCount; i++) {
            Builder
                .Append(MemoryValue);
        }

        return Builder.Build();
    }
}