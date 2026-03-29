//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Extensions;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for <see cref="PolylineDecoder"/>.
/// </summary>
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Polyline instance for benchmarks.
    /// </summary>
    public Polyline Polyline { get; private set; }

    /// <summary>
    /// Encoded polyline as string.
    /// </summary>
    public string String { get; private set; }

    /// <summary>
    /// Encoded polyline as char array.
    /// </summary>
    public char[] CharArray { get; private set; }

    /// <summary>
    /// Encoded polyline as read-only memory.
    /// </summary>
    public ReadOnlyMemory<char> Memory { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Polyline decoder instance.
    /// </summary>
    private readonly PolylineDecoder _decoder = new();

    /// <summary>
    /// Sets up benchmark data.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Polyline = Polyline.FromString(RandomValueProvider.GetPolyline(CoordinatesCount));
        String = RandomValueProvider.GetPolyline(CoordinatesCount);
        CharArray = RandomValueProvider.GetPolyline(CoordinatesCount).ToCharArray();
        Memory = RandomValueProvider.GetPolyline(CoordinatesCount).AsMemory();
    }

    /// <summary>
    /// Benchmark: decode polyline instance.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_Polyline() {
        _decoder
            .Decode(Polyline)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode from string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_String() {
        _decoder
            .Decode(String)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode from char array.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_CharArray() {
        _decoder
            .Decode(CharArray)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode from memory.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_Memory() {
        _decoder
            .Decode(Memory)
            .Consume(_consumer);
    }
}