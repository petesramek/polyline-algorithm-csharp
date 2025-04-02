//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;
using PolylineAlgorithm.Benchmarks.Internal;
using PolylineAlgorithm.Extensions;

/// <summary>
/// Benchmarks for the <see cref="PolylineDecoder"/> class.
/// </summary>
[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 10, 100, 250, 500, 1_000, 2_500, 5_000, 7_500, 10_000, 15_000, 20_000, 25_000, 50_000, 75_000, 100_000, 250_000, 500_000, 750_000, 1_000_000)]
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
    public DefaultPolylineDecoder Decoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        StringValue = ValueProvider.GetPolyline(N).ToString();
        CharArray = StringValue.ToArray();
        Memory = CharArray.AsMemory();
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_FromString() {
        Polyline polyline = Polyline.FromString(StringValue);

        Decoder
            .Decode(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a character array.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_FromCharArray() {
        Polyline polyline = Polyline.FromCharArray(CharArray);

        Decoder
            .Decode(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylineDecoder_Decode_FromMemory() {
        Polyline polyline = Polyline.FromMemory(Memory);

        Decoder
            .Decode(StringValue)
            .Consume(_consumer);
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from read-only memory.
    /// </summary>
    [Benchmark]
    public void PolylineReader_ReadToEnd_Local() {
        PolylineReader reader = new(StringValue);

        var result = ReadToEnd(reader);

        result
            .Consume(_consumer);

        static IEnumerable<Coordinate> ReadToEnd(PolylineReader reader) {
            var result = new List<Coordinate>();

            while (reader.Read()) {
                result.Add(new(reader.Latitude, reader.Longitude));
            }

            return result;
        }
    }
}