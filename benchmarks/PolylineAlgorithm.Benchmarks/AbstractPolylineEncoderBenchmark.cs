//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for <see cref="AbstractPolylineEncoder{TValue, TPolyline}"/>.
/// </summary>
public class AbstractPolylineEncoderBenchmark {
    private readonly Consumer _consumer = new();

    /// <summary>
    /// Number of coordinates used to generate test data.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    /// <summary>
    /// Coordinates as array, used as a span source.
    /// </summary>
    public (double Latitude, double Longitude)[] Array { get; private set; }

    /// <summary>
    /// Coordinates as list.
    /// </summary>
    public List<(double Latitude, double Longitude)> List { get; private set; }
#pragma warning restore CS8618

    /// <summary>
    /// String polyline encoder instance.
    /// </summary>
    private readonly StringPolylineEncoder _encoder = new();

    /// <summary>
    /// Sets up benchmark data.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Array = [.. RandomValueProvider.GetCoordinates(CoordinatesCount)];
        List = [.. Array];
    }

    /// <summary>
    /// Benchmark: encode coordinates from a read-only span.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void AbstractPolylineEncoder_Encode_Span() {
        _consumer.Consume(_encoder.Encode(Array.AsSpan()));
    }

    /// <summary>
    /// Benchmark: encode coordinates from an array using the array extension method.
    /// </summary>
    [Benchmark]
    public void AbstractPolylineEncoder_Encode_Array() {
        _consumer.Consume(_encoder.Encode(Array));
    }

    /// <summary>
    /// Benchmark: encode coordinates from a list using the list extension method.
    /// </summary>
    [Benchmark]
    public void AbstractPolylineEncoder_Encode_List() {
        _consumer.Consume(_encoder.Encode(List));
    }

    private sealed class StringPolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
        protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
            return polyline.ToString();
        }

        protected override double GetLatitude((double Latitude, double Longitude) current) {
            return current.Latitude;
        }

        protected override double GetLongitude((double Latitude, double Longitude) current) {
            return current.Longitude;
        }
    }
}
