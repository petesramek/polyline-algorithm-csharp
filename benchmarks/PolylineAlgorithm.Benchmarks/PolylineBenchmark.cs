//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;
using PolylineAlgorithm.Benchmarks.Internal;
using System.Text;

/// <summary>
/// Benchmarks for the <see cref="Polyline"/> struct.
/// </summary>
[RankColumn]
public class PolylineBenchmark {
    private static readonly Consumer consumer = new();

    [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int Length;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public string StringValue { get; private set; }

    /// <summary>
    /// Gets the character array representing the encoded polyline.
    /// </summary>
    public byte[] ByteArray { get; private set; }

    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public ReadOnlyMemory<byte> Memory { get; private set; }

    /// <summary>
    /// Gets the read-only memory representing the encoded polyline.
    /// </summary>
    public Polyline Polyline { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Polyline = ValueProvider.GetPolyline(Length);
        StringValue = Polyline.ToString();
        ByteArray = Encoding.UTF8.GetBytes(StringValue);
        Memory = ByteArray.AsMemory();
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
            .FromByteArray(ByteArray);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline Polyline_FromMemory() {
        var polyline = Polyline
            .FromMemory(Memory);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public string Polyline_ToString() {
        var stringValue = Polyline
            .ToString();

        return stringValue;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public long Polyline_GetCoordinateCount() {
        var coordinateCount = Polyline
            .GetCoordinateCount();

        return coordinateCount;
    }


    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_CopyTo() {
        var destination = new byte[Polyline.Length];

        Polyline
            .CopyTo(destination);

        destination
             .Consume(consumer);
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public bool Polyline_Equals() {
        var equals = Polyline
            .Equals(Polyline);

        return equals;
    }
}