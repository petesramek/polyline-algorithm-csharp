//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Benchmarks.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Benchmarks for the <see cref="PolylineEncoder"/> class.
/// </summary>
[RankColumn]
public class PolylineEncoderBenchmark {
    private static string _dir = "C:\\temp_benchmark";
    private static StringBuilderPooledObjectPolicy _policy = new();

    [Params(10, 100, 1_000, 10_000, 100_000)]
    public int N;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Gets the enumeration of coordinates to be encoded.
    /// </summary>
    public static IEnumerable<Coordinate> Enumeration { get; private set; }

    /// <summary>
    /// Gets the list of coordinates to be encoded.
    /// </summary>
    public static List<Coordinate> List { get; private set; }

    public static IAsyncEnumerable<Coordinate> AsyncEnumeration { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// The polyline encoder instance.
    /// </summary>
    public PolylineEncoder Encoder = new();

    /// <summary>
    /// The async polyline encoder instance.
    /// </summary>
    public AsyncPolylineEncoder AsyncEncoder = new();

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Directory.CreateDirectory(_dir);
        Enumeration = ValueProvider.GetCoordinates(N);
        List = [.. Enumeration];
        AsyncEnumeration = GetAsyncEnumeration(Enumeration!);
    }

    private async IAsyncEnumerable<Coordinate> GetAsyncEnumeration(IEnumerable<Coordinate> enumerable) {
        foreach (var item in enumerable) {
            yield return await new ValueTask<Coordinate>(item);
        }
    }

    /// <summary>
    /// Benchmarks the encoding of a list of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineEncoder_Encode_List() {
        var polyline = Encoder.Encode(List!);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public Polyline PolylineEncoder_Encode_Enumerator() {
        var polyline = Encoder.Encode(Enumeration!);

        return polyline;
    }

    /// <summary>
    /// Benchmarks the encoding of an enumeration of coordinates into a polyline.
    /// </summary>
    /// <returns>The encoded polyline.</returns>
    [Benchmark]
    public async Task PolylineEncoder_EncodeAsync_String() {
        var result = AsyncEncoder
            .EncodeAsync(AsyncEnumeration!);

        await foreach (var _ in result) { }
    }
}