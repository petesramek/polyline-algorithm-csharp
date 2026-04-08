//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for <see cref="PolylineDecoder{TPolyline, TCoordinate}"/>.
/// </summary>
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
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
    /// String polyline decoder instance.
    /// </summary>
    private readonly PolylineDecoder<string, (double Latitude, double Longitude)> _stringDecoder = CreateStringDecoder();

    /// <summary>
    /// Char array polyline decoder instance.
    /// </summary>
    private readonly PolylineDecoder<char[], (double Latitude, double Longitude)> _charArrayDecoder = CreateCharArrayDecoder();

    /// <summary>
    /// Memory char polyline decoder instance.
    /// </summary>
    private readonly PolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)> _memoryCharDecoder = CreateMemoryDecoder();

    private static PolylineFormatter<(double Latitude, double Longitude), T> BuildFormatter<T>(
        Func<ReadOnlyMemory<char>, T> write, Func<T, ReadOnlyMemory<char>> read) =>
        FormatterBuilder<(double Latitude, double Longitude), T>.Create()
            .AddValue("lat", static c => c.Latitude)
            .AddValue("lon", static c => c.Longitude)
            .WithCreate(static v => (v[0] / 1e5, v[1] / 1e5))
            .ForPolyline(write, read)
            .Build();

    private static PolylineDecoder<string, (double Latitude, double Longitude)> CreateStringDecoder() {
        var fmt = BuildFormatter<string>(
            static m => new string(m.Span),
            static s => s?.AsMemory() ?? Memory<char>.Empty);
        return new PolylineDecoder<string, (double Latitude, double Longitude)>(
            new PolylineOptions<(double Latitude, double Longitude), string>(fmt));
    }

    private static PolylineDecoder<char[], (double Latitude, double Longitude)> CreateCharArrayDecoder() {
        var fmt = BuildFormatter<char[]>(
            static m => m.ToArray(),
            static a => a?.AsMemory() ?? Memory<char>.Empty);
        return new PolylineDecoder<char[], (double Latitude, double Longitude)>(
            new PolylineOptions<(double Latitude, double Longitude), char[]>(fmt));
    }

    private static PolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)> CreateMemoryDecoder() {
        var fmt = BuildFormatter<ReadOnlyMemory<char>>(
            static m => m,
            static m => m);
        return new PolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)>(
            new PolylineOptions<(double Latitude, double Longitude), ReadOnlyMemory<char>>(fmt));
    }

    /// <summary>
    /// Sets up benchmark data.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        String = RandomValueProvider.GetPolyline(CoordinatesCount);
        CharArray = RandomValueProvider.GetPolyline(CoordinatesCount).ToCharArray();
        Memory = RandomValueProvider.GetPolyline(CoordinatesCount).AsMemory();
    }

    /// <summary>
    /// Benchmark: decode from string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_String() {
        _stringDecoder
            .Decode(String)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode from char array.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_CharArray() {
        _charArrayDecoder
            .Decode(CharArray)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode from memory.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_Memory() {
        _memoryCharDecoder
            .Decode(Memory)
            .Consume(_consumer);
    }
}
