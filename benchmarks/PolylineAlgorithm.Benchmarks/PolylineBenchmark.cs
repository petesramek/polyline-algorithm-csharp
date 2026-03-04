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
public class PolylineBenchmark {
    private static readonly Consumer _consumer = new();

    [Params(1, 100, 1_000)]
    public int Count;

    [Params(100)]
    public int Iterations;

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
    public void Polyline_FromString() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Polyline
            .FromString(StringValue);

            _consumer.Consume(polyline);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_FromCharArray() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Polyline
            .FromCharArray(CharArrayValue);

            _consumer.Consume(polyline);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_FromMemory() {
        for (int i = 0; i < Iterations; i++) {
            var polyline = Polyline
            .FromMemory(MemoryValue);

            _consumer.Consume(polyline);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_ToString() {
        for (int i = 0; i < Iterations; i++) {
            var stringValue = PolylineValue
            .ToString();

            _consumer.Consume(stringValue);
        }
    }


    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_CopyTo() {
        for (int i = 0; i < Iterations; i++) {
            PolylineValue
            .CopyTo(CopyToDestination);

            CopyToDestination
                 .Consume(_consumer);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_Equals_SameValue() {
        for (int i = 0; i < Iterations; i++) {
            var equals = PolylineValue
            .Equals(PolylineValue);

            _consumer.Consume(equals);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_Equals_DifferentValue() {
        for (int i = 0; i < Iterations; i++) {
            var equals = PolylineValue
            .Equals(PolylineNotEqualValue);

            _consumer.Consume(equals);
        }
    }


    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public void Polyline_Equals_DifferentType() {
        for (int i = 0; i < Iterations; i++) {
            var equals = PolylineValue
            .Equals(StringValue);

            _consumer.Consume(equals);
        }
    }
}