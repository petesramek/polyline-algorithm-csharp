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
/// Benchmarks for <see cref="Polyline"/>.
/// </summary>
public class PolylineBenchmark {
    private static readonly Consumer _consumer = new();

    /// <summary>
    /// Number of coordinates for benchmarks. Set by BenchmarkDotNet.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

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
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public Polyline PolylineValue { get; private set; }

    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public string StringValue { get; private set; }

    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public Polyline PolylineNotEqualValue { get; private set; }

    /// <summary>
    /// Gets the destination array used for benchmarking the <see cref="Polyline.CopyTo(char[])"/> method.
    /// This array is initialized in <see cref="SetupData"/> to match the length of the encoded polyline,
    /// and is used as the target buffer for copying polyline data during benchmark runs.
    /// </summary>
    public char[] CopyToDestination { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        PolylineValue = Polyline.FromString(RandomValueProvider.GetPolyline(CoordinatesCount));
        PolylineNotEqualValue = Polyline.FromString(RandomValueProvider.GetPolyline(CoordinatesCount + Random.Shared.Next(1, 101)));
        StringValue = PolylineValue.ToString();
        CharArrayValue = [.. StringValue];
        MemoryValue = CharArrayValue.AsMemory();

        CopyToDestination = new char[PolylineValue.Length];
    }

    /// <summary>
    /// Benchmark: create polyline from string.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_FromString() {
        var polyline = Polyline
        .FromString(StringValue);

        _consumer.Consume(polyline);
    }

    /// <summary>
    /// Benchmark: create polyline from char array.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_FromCharArray() {
        var polyline = Polyline
        .FromCharArray(CharArrayValue);

        _consumer.Consume(polyline);
    }

    /// <summary>
    /// Benchmark: create polyline from memory.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_FromMemory() {
        var polyline = Polyline
        .FromMemory(MemoryValue);

        _consumer.Consume(polyline);
    }

    /// <summary>
    /// Benchmark: convert polyline to string.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_ToString() {
        var stringValue = PolylineValue
        .ToString();

        _consumer.Consume(stringValue);
    }


    /// <summary>
    /// Benchmark: copy polyline to array.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_CopyTo() {
        PolylineValue
        .CopyTo(CopyToDestination);

        CopyToDestination
             .Consume(_consumer);
    }

    /// <summary>
    /// Benchmark: compare polyline with same value.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_Equals_SameValue() {
        var equals = PolylineValue
        .Equals(PolylineValue);

        _consumer.Consume(equals);
    }

    /// <summary>
    /// Benchmark: compare polyline with different value.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_Equals_DifferentValue() {
        var equals = PolylineValue
        .Equals(PolylineNotEqualValue);

        _consumer.Consume(equals);
    }


    /// <summary>
    /// Benchmark: compare polyline with different type.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_Equals_DifferentType() {
        var equals = PolylineValue
        .Equals(StringValue);

        _consumer.Consume(equals);
    }
}