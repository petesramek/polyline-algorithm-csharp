# PolylineAlgorithm for .NET

[![NuGet](https://img.shields.io/nuget/v/PolylineAlgorithm)](https://www.nuget.org/packages/PolylineAlgorithm)
[![Build](https://github.com/petesramek/polyline-algorithm-csharp/actions/workflows/build.yml/badge.svg)](https://github.com/petesramek/polyline-algorithm-csharp/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/github/license/petesramek/polyline-algorithm-csharp)](./LICENSE)

Google's Encoded Polyline Algorithm compresses sequences of geographic coordinates into a compact ASCII string, widely used in mapping APIs. This library provides a fully compliant .NET implementation with extensible, type-safe encoding and decoding APIs.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [API Reference](#api-reference)
- [Benchmarks](#benchmarks)
- [FAQ](#faq)
- [Contributing](#contributing)
- [Support](#support)
- [License](#license)

## Features

- Fully compliant Google Encoded Polyline Algorithm for .NET Standard 2.1+
- Fully fluent `FormatterBuilder<TCoordinate, TPolyline>` — configure coordinate fields, factories, and polyline I/O in one chain
- Sealed, immutable `PolylineFormatter<TCoordinate, TPolyline>` produced by the builder
- Type-safe `PolylineEncoder<TCoordinate, TPolyline>` and `PolylineDecoder<TPolyline, TCoordinate>` with no inheritance required
- `PolylineOptions<TCoordinate, TPolyline>` for stack-alloc limits and optional logging
- Extension methods for encoding directly from `List<T>` and arrays
- Robust input validation with descriptive exceptions for malformed or out-of-range data
- Logging and diagnostic support via `Microsoft.Extensions.Logging`
- Low-level utilities for normalization, validation, and bit-level operations via static `PolylineEncoding` class
- Thread-safe, stateless APIs
- Thorough unit tests and benchmarks for correctness and performance
- Auto-generated API documentation ([API Reference](https://petesramek.github.io/polyline-algorithm-csharp/))
- Supports .NET Core, .NET 5+, Xamarin, Unity, Blazor, and any platform targeting `netstandard2.1`

## Installation

Using dotnet tool

```shell
dotnet add package PolylineAlgorithm
```

or via NuGet

```powershell
Install-Package PolylineAlgorithm
```

## Usage

The library uses a fluent `FormatterBuilder` to describe how to map between your coordinate type and a polyline type — no inheritance required. Build a `PolylineFormatter`, wrap it in `PolylineOptions`, then instantiate `PolylineEncoder` and `PolylineDecoder`.

### Quick Start

```csharp
using PolylineAlgorithm;

// 1. Build a formatter that maps (double Lat, double Lon) ↔ string polyline
PolylineFormatter<(double Lat, double Lon), string> formatter =
    FormatterBuilder<(double Lat, double Lon), string>.Create()
        .AddValue("lat", static c => c.Lat)
        .AddValue("lon", static c => c.Lon)
        .WithCreate(static v => (v[0], v[1]))
        .ForPolyline(static m => new string(m.Span), static s => s.AsMemory())
        .Build();

PolylineOptions<(double Lat, double Lon), string> options = new(formatter);

PolylineEncoder<(double Lat, double Lon), string> encoder = new(options);
PolylineDecoder<string, (double Lat, double Lon)> decoder = new(options);

// 2. Encode
var coordinates = new List<(double, double)> { (48.858370, 2.294481), (51.500729, -0.124625) };
string encoded = encoder.Encode(coordinates); // extension method for List<T>
// Output: "yseiHoc_MwacOjnwM"

// 3. Decode
IEnumerable<(double Lat, double Lon)> decoded = decoder.Decode(encoded);
```

### Building a formatter

`FormatterBuilder<TCoordinate, TPolyline>` configures how the library reads and writes your types:

| Method | Purpose |
|---|---|
| `FormatterBuilder<TC,TP>.Create()` | Static factory to start building |
| `.AddValue(name, selector, precision=5)` | Register a coordinate field (latitude, longitude, …) |
| `.SetBaseline(long)` | Override the encoding baseline (optional) |
| `.WithCreate(factory)` | Factory delegate `PolylineItemFactory<TC>`: `TC(ReadOnlySpan<double> values)` — required for decoding |
| `.ForPolyline(write, read)` | How to convert `ReadOnlyMemory<char>` → `TP` and `TP` → `ReadOnlyMemory<char>` |
| `.Build()` | Returns an immutable `PolylineFormatter<TC,TP>` |

```csharp
PolylineFormatter<(double Lat, double Lon), string> formatter =
    FormatterBuilder<(double Lat, double Lon), string>.Create()
        .AddValue("lat", static c => c.Lat)
        .AddValue("lon", static c => c.Lon)
        .WithCreate(static v => (v[0], v[1]))
        .ForPolyline(static m => new string(m.Span), static s => s.AsMemory())
        .Build();
```

### Encoding

```csharp
using PolylineAlgorithm;
using PolylineAlgorithm.Extensions;

PolylineOptions<(double Lat, double Lon), string> options = new(formatter);
PolylineEncoder<(double Lat, double Lon), string> encoder = new(options);

var coordinates = new List<(double Lat, double Lon)>
{
    (48.858370, 2.294481),
    (51.500729, -0.124625)
};

string encoded = encoder.Encode(coordinates); // extension method for List<T>
Console.WriteLine(encoded); // yseiHoc_MwacOjnwM
```

### Decoding

```csharp
using PolylineAlgorithm;

PolylineOptions<(double Lat, double Lon), string> options = new(formatter);
PolylineDecoder<string, (double Lat, double Lon)> decoder = new(options);

IEnumerable<(double Lat, double Lon)> decoded = decoder.Decode("yseiHoc_MwacOjnwM");
```

### Advanced options (logging, stack-alloc limit)

```csharp
using Microsoft.Extensions.Logging;

PolylineOptions<(double Lat, double Lon), string> options = new(
    formatter,
    stackAllocLimit: 1024,
    loggerFactory: loggerFactory);

var encoder = new PolylineEncoder<(double Lat, double Lon), string>(options);
var decoder = new PolylineDecoder<string, (double Lat, double Lon)>(options);
```

> **Note:**
> If you need low-level utilities for normalization, validation, encoding and decoding, use static methods from the `PolylineEncoding` class.

## API Reference

Full API docs and guides (auto-generated from source) are available at [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)

## Benchmarks

- See [`/benchmarks`](./benchmarks) in the repo for benchmark projects.
- Run benchmarks with:
  ```shell
  dotnet run --project benchmarks/PolylineAlgorithm.Benchmarks --configuration Release
  ```
- For guidance on writing and interpreting benchmarks, see [docs/benchmarks.md](./docs/benchmarks.md).
- Contributors: update benchmarks and document results for performance-impacting PRs.

## FAQ

**Q: What coordinate ranges are valid?**  
A: Latitude must be -90..90; longitude -180..180. Out-of-range input throws `ArgumentOutOfRangeException`.

**Q: Which .NET versions are supported?**  
A: All platforms supporting `netstandard2.1` (including .NET Core and .NET 5+).

**Q: What happens if I pass invalid or malformed input to the decoder?**
A: The decoder will throw descriptive exceptions (`InvalidPolylineException`) for malformed polyline strings. Check exception handling in your application.

**Q: How do I customize encoding options (e.g., precision, buffer size, logging)?**
A: Pass a `PolylineOptions<TC,TP>` to the encoder/decoder constructor. Set `stackAllocLimit` to control buffer size and `loggerFactory` for logging. Precision is set per-field via `.AddValue(name, selector, precision)` on the `FormatterBuilder`.

**Q: Is the library thread-safe?**
A: Yes, the main encoding and decoding APIs are stateless and thread-safe. If using mutable shared resources, manage synchronization in your code.

**Q: Can the library be used in Unity, Xamarin, Blazor, or other .NET-compatible platforms?**
A: Yes! Any environment supporting `netstandard2.1` can use this library.

**Q: Where can I report bugs or request features?**
A: Open a GitHub issue using the provided templates in the repository and tag @petesramek.

**Q: Is there support for elevation, time stamps, or third coordinate values?**
A: Not currently, not planned to be added, but you can extend by adding extra `.AddValue(...)` calls in your `FormatterBuilder` and using `PolylineEncoding` class methods for low-level operations.

**Q: How do I contribute documentation improvements?**
A: Update XML doc comments in the codebase and submit a PR; all public APIs require XML documentation. To improve guides, update the relevant markdown file in the `/api-reference/guide` folder.

**Q: Does the library support streaming or incremental decoding of polylines?**
A: Currently, only batch encode/decode is supported. For streaming scenarios, implement custom logic using `PolylineEncoding` utility functions.

## Contributing

- Follow code style and PR instructions in [CONTRIBUTING.md](./CONTRIBUTING.md).
- Ensure all features are covered by tests and XML doc comments.
- For questions or suggestions, open an issue and use the provided templates.

## Support

Have a question, bug, or feature request? [Open an issue](https://github.com/petesramek/polyline-algorithm-csharp/issues/new/choose) — bug report and feature request templates are available to guide you.

---

## License

MIT License &copy; Pete Sramek
