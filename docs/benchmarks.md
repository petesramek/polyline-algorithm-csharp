# Benchmarks

This guide explains the benchmark project structure and how to write and run performance benchmarks.

## Project Structure

All benchmarks live in the `benchmarks/` directory:

```
benchmarks/
└── PolylineAlgorithm.Benchmarks/
    ├── PolylineEncoderBenchmark.cs    # Benchmarks for AbstractPolylineEncoder
    ├── PolylineDecoderBenchmark.cs    # Benchmarks for PolylineDecoder
    ├── PolylineEncodingBenchmark.cs   # Benchmarks for PolylineEncoding helpers
    ├── Program.cs                     # BenchmarkSwitcher entry point
    └── PolylineAlgorithm.Benchmarks.csproj
```

The project targets `net8.0`, `net9.0`, and `net10.0` and references the main `PolylineAlgorithm` library along with the `PolylineAlgorithm.Utility` helper project.

## Framework

Benchmarks use [BenchmarkDotNet](https://benchmarkdotnet.org/). Key packages:

| Package | Purpose |
|---|---|
| `BenchmarkDotNet` | Core benchmarking framework |
| `BenchmarkDotNet.Diagnostics.Windows` | Windows-specific diagnostics (ETW) |

## Writing a New Benchmark

1. Create a new `.cs` file in `benchmarks/PolylineAlgorithm.Benchmarks/`.
2. Add the standard copyright header.
3. Annotate the class with `[MemoryDiagnoser]` if you want allocation tracking.
4. Use `[Params]` to parameterize input sizes.
5. Mark benchmark methods with `[Benchmark]`. Mark one with `[Benchmark(Baseline = true)]` when comparing variants.
6. Use `[GlobalSetup]` to prepare shared data once per parameter combination.

Example:

```csharp
//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

/// <summary>
/// Benchmarks for <see cref="MyEncoder"/>.
/// </summary>
[MemoryDiagnoser]
public class MyEncoderBenchmark {
    private readonly Consumer _consumer = new();

    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

    private (double Latitude, double Longitude)[] _data = [];

    [GlobalSetup]
    public void Setup() {
        _data = [.. RandomValueProvider.GetCoordinates(CoordinatesCount)];
    }

    [Benchmark(Baseline = true)]
    public void EncodeArray() => new MyEncoder().Encode(_data).Consume(_consumer);
}
```

## Running Benchmarks Locally

Benchmarks **must** run in Release configuration to produce meaningful results:

```bash
dotnet run \
  --project ./benchmarks/PolylineAlgorithm.Benchmarks/PolylineAlgorithm.Benchmarks.csproj \
  --configuration Release \
  --framework net10.0 \
  -- --filter '*'
```

### Useful CLI flags

| Flag | Description |
|---|---|
| `--filter '*'` | Run all benchmarks |
| `--filter '*Encoder*'` | Run benchmarks whose name contains `Encoder` |
| `--runtimes net8.0 net9.0 net10.0` | Run on multiple runtimes |
| `--exporters GitHub` | Export results as GitHub Flavored Markdown |
| `--memory` | Enable memory diagnoser output |
| `--iterationTime 100` | Iteration time in milliseconds |
| `--join` | Merge results from multiple runs |
| `--artifacts <path>` | Output directory for results |

### Example: multi-runtime run with GitHub export

```bash
dotnet run \
  --project ./benchmarks/PolylineAlgorithm.Benchmarks/PolylineAlgorithm.Benchmarks.csproj \
  --configuration Release \
  --framework net10.0 \
  -- --runtimes net8.0 net9.0 net10.0 \
     --filter '*' \
     --exporters GitHub \
     --memory \
     --iterationTime 100 \
     --join \
     --artifacts /tmp/benchmarks
```

## Benchmarks in CI

The `pull-request` workflow runs benchmarks on Ubuntu, Windows, and macOS when `vars.BENCHMARKDOTNET_RUN_OVERRIDE == 'true'` or when building a release branch. Results are uploaded as artifacts (`benchmark-<os>`) and written to the workflow step summary as a Markdown table.

Relevant workflow variables:

| Variable | Description |
|---|---|
| `BENCHMARKDOTNET_WORKING_DIRECTORY` | Working directory for `dotnet run` |
| `BENCHMARKDOTNET_RUNTIMES` | Space-separated runtimes to bench |
| `BENCHMARKDOTNET_FILTER` | Filter expression passed to `--filter` |
| `DEFAULT_BUILD_FRAMEWORK` | Framework used with `--framework` |
| `BENCHMARKDOTNET_RUN_OVERRIDE` | Set to `true` to force benchmark run on PRs |

## When to Add or Update Benchmarks

- Add a new benchmark file when introducing a new encoding/decoding code path.
- Update an existing benchmark when changing the algorithmic implementation of an existing path.
- Attach benchmark results to pull requests that affect performance-sensitive code (see [CONTRIBUTING.md](../CONTRIBUTING.md)).
