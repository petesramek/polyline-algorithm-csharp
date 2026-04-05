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
- Extensible APIs — implement your own encoder/decoder for any coordinate or polyline type
- Robust input validation with descriptive exceptions for malformed or out-of-range data
- Advanced configuration via `PolylineEncodingOptions` (precision, buffer size, logging)
- Extension methods for encoding directly from `List<T>` and arrays
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

The library provides abstract base classes to implement your own encoder and decoder for any coordinate and polyline type. Inherit from `AbstractPolylineEncoder` or `AbstractPolylineDecoder`, override the coordinate accessors, then call `Encode` or `Decode`.

### Quick Start

```csharp
// 1. Implement a minimal encoder (see full example below)
var encoder = new MyPolylineEncoder();
string encoded = encoder.Encode(coordinates);    // e.g. "yseiHoc_MwacOjnwM"

// 2. Implement a minimal decoder (see full example below)
var decoder = new MyPolylineDecoder();
IEnumerable<(double Latitude, double Longitude)> decoded = decoder.Decode(encoded);
```

### Custom encoder and decoder

#### Encoding

Custom encoder implementation.

```csharp
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;

public sealed class MyPolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
    protected override double GetLatitude((double Latitude, double Longitude) coordinate) => coordinate.Latitude;
    protected override double GetLongitude((double Latitude, double Longitude) coordinate) => coordinate.Longitude;
    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
}
```

Custom encoder usage.

```csharp
using PolylineAlgorithm.Extensions;

var coordinates = new List<(double Latitude, double Longitude)>
{
    (48.858370, 2.294481),
    (51.500729, -0.124625)
};

var encoder = new MyPolylineEncoder();
string encoded = encoder.Encode(coordinates); // extension method for List<T>

Console.WriteLine(encoded);
```

#### Decoding

Custom decoder implementation.

```csharp
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;

public sealed class MyPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
    protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) => (latitude, longitude);
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
}
```

Custom decoder usage.

```csharp
string encoded = "yseiHoc_MwacOjnwM";

var decoder = new MyPolylineDecoder();
IEnumerable<(double Latitude, double Longitude)> decoded = decoder.Decode(encoded);
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
A: Use `PolylineEncodingOptionsBuilder` to set custom options and pass the built `PolylineEncodingOptions` to the encoder or decoder constructor.

**Q: Is the library thread-safe?**
A: Yes, the main encoding and decoding APIs are stateless and thread-safe. If using mutable shared resources, manage synchronization in your code.

**Q: Can the library be used in Unity, Xamarin, Blazor, or other .NET-compatible platforms?**
A: Yes! Any environment supporting `netstandard2.1` can use this library.

**Q: Where can I report bugs or request features?**
A: Open a GitHub issue using the provided templates in the repository and tag @petesramek.

**Q: Is there support for elevation, time stamps, or third coordinate values?**
A: Not currently, not planned to be added, but you can extend by implementing your own encoder/decoder using `PolylineEncoding` class methods.

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
