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
/// Benchmarks for <see cref="AbstractPolylineDecoder{TPolyline, TValue}"/>.
/// </summary>
public class AbstractPolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    /// <summary>
    /// Number of coordinates used to generate the test polyline.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
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
#pragma warning restore CS8618

    /// <summary>
    /// String polyline decoder instance.
    /// </summary>
    private readonly StringPolylineDecoder _stringDecoder = new();

    /// <summary>
    /// Char array polyline decoder instance.
    /// </summary>
    private readonly CharArrayPolylineDecoder _charArrayDecoder = new();

    /// <summary>
    /// Read-only memory polyline decoder instance.
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
    /// Benchmark: decode polyline from string.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void AbstractPolylineDecoder_Decode_String() {
        _stringDecoder
            .Decode(String)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode polyline from char array.
    /// </summary>
    [Benchmark]
    public void AbstractPolylineDecoder_Decode_CharArray() {
        _charArrayDecoder
            .Decode(CharArray)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: decode polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void AbstractPolylineDecoder_Decode_Memory() {
        _memoryCharDecoder
            .Decode(Memory)
            .Consume(_consumer);
    }

    private sealed class StringPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) {
            return (latitude, longitude);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) {
            return polyline?.AsMemory() ?? Memory<char>.Empty;
        }
    }

    private sealed class CharArrayPolylineDecoder : AbstractPolylineDecoder<char[], (double Latitude, double Longitude)> {
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) {
            return (latitude, longitude);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in char[] polyline) {
            return polyline?.AsMemory() ?? Memory<char>.Empty;
        }
    }

    private sealed class MemoryCharPolylineDecoder : AbstractPolylineDecoder<ReadOnlyMemory<char>, (double Latitude, double Longitude)> {
        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) {
            return (latitude, longitude);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(in ReadOnlyMemory<char> polyline) {
            return polyline;
        }
    }
}
