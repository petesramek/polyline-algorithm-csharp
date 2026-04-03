//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for <see cref="PolylineEncoding"/>.
/// </summary>
public class PolylineEncodingBenchmark {
    /// <summary>
    /// A fixed floating-point coordinate value used for normalization benchmarks.
    /// </summary>
    private const double CoordinateValue = 37.78903;

    /// <summary>
    /// A fixed normalized integer coordinate value used for denormalization, buffer-size, and write benchmarks.
    /// </summary>
    private const int NormalizedValue = 3778903;

    /// <summary>
    /// A pre-allocated write buffer large enough to hold any single encoded delta value.
    /// </summary>
    private readonly char[] _writeBuffer = new char[16];

    private string _polyline = default!;
    private ReadOnlyMemory<char> _polylineMemory;

    /// <summary>
    /// Number of coordinates used to build the test polyline. Set by BenchmarkDotNet.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

    /// <summary>
    /// Sets up benchmark data.
    /// </summary>
    [GlobalSetup]
    public void Setup() {
        _polyline = RandomValueProvider.GetPolyline(CoordinatesCount);
        _polylineMemory = _polyline.AsMemory();
    }

    /// <summary>
    /// Benchmark: normalize a floating-point coordinate to its integer representation.
    /// </summary>
    [Benchmark]
    public int PolylineEncoding_Normalize() => PolylineEncoding.Normalize(CoordinateValue);

    /// <summary>
    /// Benchmark: denormalize an integer coordinate back to its floating-point representation.
    /// </summary>
    [Benchmark]
    public double PolylineEncoding_Denormalize() => PolylineEncoding.Denormalize(NormalizedValue);

    /// <summary>
    /// Benchmark: compute the number of characters required to encode a single integer delta.
    /// </summary>
    [Benchmark]
    public int PolylineEncoding_GetRequiredBufferSize() => PolylineEncoding.GetRequiredBufferSize(NormalizedValue);

    /// <summary>
    /// Benchmark: decode a single encoded value from the beginning of a polyline memory buffer.
    /// </summary>
    [Benchmark]
    public bool PolylineEncoding_TryReadValue() {
        int delta = 0;
        int position = 0;
        return PolylineEncoding.TryReadValue(ref delta, _polylineMemory, ref position);
    }

    /// <summary>
    /// Benchmark: encode a single integer delta value into a pre-allocated character buffer.
    /// </summary>
    [Benchmark]
    public bool PolylineEncoding_TryWriteValue() {
        int position = 0;
        return PolylineEncoding.TryWriteValue(NormalizedValue, _writeBuffer, ref position);
    }

    /// <summary>
    /// Benchmark: validate that all characters in the polyline are within the allowed ASCII range.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void PolylineEncoding_ValidateCharRange() => PolylineEncoding.ValidateCharRange(_polyline);

    /// <summary>
    /// Benchmark: validate the block structure of the polyline.
    /// </summary>
    [Benchmark]
    public void PolylineEncoding_ValidateBlockLength() => PolylineEncoding.ValidateBlockLength(_polyline);

    /// <summary>
    /// Benchmark: validate both character range and block structure of the polyline.
    /// </summary>
    [Benchmark]
    public void PolylineEncoding_ValidateFormat() => PolylineEncoding.ValidateFormat(_polyline);
}