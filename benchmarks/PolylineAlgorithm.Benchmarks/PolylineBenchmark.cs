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
/// Benchmarks for the <see cref="PolylineValue"/> struct.
/// </summary>
[RankColumn]
[ShortRunJob]
public class PolylineBenchmark {
    private static readonly Consumer consumer = new();

    [Params(1, 100, 1_000)]
    public int Count;

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

    public char[] CopyToDestination { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        PolylineValue = Polyline.FromString(RandomValueProvider.GetPolyline(Count));
        PolylineNotEqualValue = Polyline.FromString(RandomValueProvider.GetPolyline(Count + Random.Shared.Next(1, 101)));
        StringValue = PolylineValue.ToString();
        CharArrayValue = [.. StringValue];
        MemoryValue = CharArrayValue.AsMemory();

        CopyToDestination = new char[PolylineValue.Length];
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline Polyline_FromString() {
        var polyline = Polyline
            .FromString(StringValue);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline Polyline_FromCharArray() {
        var polyline = Polyline
            .FromCharArray(CharArrayValue);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline Polyline_FromMemory() {
        var polyline = Polyline
            .FromMemory(MemoryValue);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public string Polyline_ToString() {
        var stringValue = PolylineValue
            .ToString();

        return stringValue;
    }


    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_CopyTo() {
        PolylineValue
            .CopyTo(CopyToDestination);

        CopyToDestination
             .Consume(consumer);
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public bool Polyline_Equals_SameValue() {
        var equals = PolylineValue
            .Equals(PolylineValue);

        return equals;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public bool Polyline_Equals_DifferentValue() {
        var equals = PolylineValue
            .Equals(PolylineNotEqualValue);

        return equals;
    }


    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public bool Polyline_Equals_DifferentType() {
        var equals = PolylineValue
            .Equals(StringValue);

        return equals;
    }
}