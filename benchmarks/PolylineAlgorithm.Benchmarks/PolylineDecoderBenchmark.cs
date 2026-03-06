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
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 100, 1_000)]
    public int CoordinatesCount;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public Polyline Polyline { get; private set; }

    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public string String { get; private set; }

    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public char[] CharArray { get; private set; }

    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public ReadOnlyMemory<char> Memory { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline decoder instance.
    /// </summary>
    public readonly PolylineDecoder Decoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Polyline = Polyline.FromString(RandomValueProvider.GetPolyline(CoordinatesCount));
        String = RandomValueProvider.GetPolyline(CoordinatesCount);
        CharArray = RandomValueProvider.GetPolyline(CoordinatesCount).ToCharArray();
        Memory = RandomValueProvider.GetPolyline(CoordinatesCount).AsMemory();
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_Polyline() {
        Decoder
            .Decode(Polyline)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_String() {
        Decoder
            .Decode(String)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_CharArray() {
        Decoder
            .Decode(CharArray)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_Memory() {
        Decoder
            .Decode(Memory)
            .Consume(_consumer);
    }
}