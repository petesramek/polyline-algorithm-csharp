//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmarks for <see cref="PolylineEncodingOptionsBuilder"/>.
/// </summary>
public class PolylineEncodingOptionsBuilderBenchmark {
    /// <summary>
    /// Benchmark: create a new builder instance.
    /// </summary>
    [Benchmark(Baseline = true)]
    public PolylineEncodingOptionsBuilder PolylineEncodingOptionsBuilder_Create() {
        return PolylineEncodingOptionsBuilder.Create();
    }

    /// <summary>
    /// Benchmark: create a builder and build default options.
    /// </summary>
    [Benchmark]
    public PolylineEncodingOptions PolylineEncodingOptionsBuilder_Build() {
        return PolylineEncodingOptionsBuilder
            .Create()
            .Build();
    }

    /// <summary>
    /// Benchmark: configure precision and build options.
    /// </summary>
    [Benchmark]
    public PolylineEncodingOptions PolylineEncodingOptionsBuilder_WithPrecision() {
        return PolylineEncodingOptionsBuilder
            .Create()
            .WithPrecision(5)
            .Build();
    }

    /// <summary>
    /// Benchmark: configure the stack allocation limit and build options.
    /// </summary>
    [Benchmark]
    public PolylineEncodingOptions PolylineEncodingOptionsBuilder_WithStackAllocLimit() {
        return PolylineEncodingOptionsBuilder
            .Create()
            .WithStackAllocLimit(512)
            .Build();
    }

    /// <summary>
    /// Benchmark: configure all options using the full fluent chain and build.
    /// </summary>
    [Benchmark]
    public PolylineEncodingOptions PolylineEncodingOptionsBuilder_FullChain() {
        return PolylineEncodingOptionsBuilder
            .Create()
            .WithPrecision(5)
            .WithStackAllocLimit(512)
            .Build();
    }
}
