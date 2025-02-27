//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;
using PolylineAlgorithm.Benchmarks.Internal;
using System.Diagnostics;

/// <summary>
/// Benchmarks for the <see cref="PolylineDecoder"/> class.
/// </summary>
[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(10, 100, 1_000, 10_000, 100_000)]
    public int N;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public string StringValue { get; private set; }

    /// <summary>
    /// Gets the character array representing the encoded polyline.
    /// </summary>
    public char[] CharArray { get; private set; }

    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public ReadOnlyMemory<char> Memory { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline decoder instance.
    /// </summary>
    public PolylineDecoder Decoder = new();

    /// <summary>
    /// The async polyline decoder instance.
    /// </summary>
    public AsyncPolylineDecoder AsyncDecoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        StringValue = ValueProvider.GetPolyline(N).ToString();
        CharArray = StringValue.ToCharArray();
        Memory = StringValue.AsMemory();
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_FromString() {
        Polyline polyline = Polyline.FromString(StringValue);

        Decoder
            .Decode(polyline)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a character array.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_FromCharArray() {
        Polyline polyline = Polyline.FromCharArray(CharArray);

        Decoder
            .Decode(polyline)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_FromMemory() {
        Polyline polyline = Polyline.FromMemory(Memory);

        Decoder
            .Decode(polyline)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public async Task PolylineDecoder_DecodeAsync_FromString() {
        Polyline polyline = Polyline.FromString(StringValue);

        var result = AsyncDecoder
            .DecodeAsync(polyline)
            .ConfigureAwait(false);

        await foreach (var _ in result) { }
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public async Task PolylineDecoder_DecodeAsync_CharArray() {
        Polyline polyline = Polyline.FromCharArray(CharArray);

        var result = AsyncDecoder
            .DecodeAsync(polyline)
            .ConfigureAwait(false);

        await foreach (var _ in result) { }
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public async Task PolylineDecoder_DecodeAsync_FromMemory() {
        Polyline polyline = Polyline.FromMemory(Memory);

        var result = AsyncDecoder
            .DecodeAsync(polyline)
            .ConfigureAwait(false);

        await foreach (var _ in result) { }
    }
}