//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm.Abstraction;
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
    private readonly StringPolylineDecoder _stringDecoder = new();

    /// <summary>
    /// Char array polyline decoder instance.
    /// </summary>
    private readonly CharArrayPolylineDecoder _charArrayDecoder = new();

    /// <summary>
    /// String polyline decoder instance.
    /// </summary>
    private readonly MemoryCharPolylineDecoder _memoryCharDecoder = new();

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

    private sealed class StringPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
        protected override int ValuesPerItem => 2;
        protected override (double Latitude, double Longitude) CreateItem(ReadOnlyMemory<double> values) {
            ReadOnlySpan<double> span = values.Span;
            return (span[0], span[1]);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) {
            return polyline?.AsMemory() ?? Memory<char>.Empty;
        }
    }

    private sealed class CharArrayPolylineDecoder : AbstractPolylineDecoder<char[], (double Latitude, double Longitude)> {
        protected override int ValuesPerItem => 2;
        protected override (double Latitude, double Longitude) CreateItem(ReadOnlyMemory<double> values) {
            ReadOnlySpan<double> span = values.Span;
            return (span[0], span[1]);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in char[] polyline) {
            return polyline?.AsMemory() ?? Memory<char>.Empty;
        }
    }

    private sealed class MemoryCharPolylineDecoder : AbstractPolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)> {
        protected override int ValuesPerItem => 2;
        protected override (double Latitude, double Longitude) CreateItem(ReadOnlyMemory<double> values) {
            ReadOnlySpan<double> span = values.Span;
            return (span[0], span[1]);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in ReadOnlyMemory<char> polyline) {
            return polyline;
        }
    }
}