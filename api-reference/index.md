# PolylineAlgorithm for .NET

Welcome to **PolylineAlgorithm for .NET**, a modern library for Google-compliant polyline encoding and decoding with strong input validation, extensible API design, and robust performance.

## What is PolylineAlgorithm?

PolylineAlgorithm provides tools for encoding a sequence of geographic coordinates into a compact string format used by Google Maps and other mapping platforms, and for decoding those strings back into coordinates. Its simple API and thorough documentation make it suitable for .NET Core, .NET 5+, and any framework supporting `netstandard2.1`.

## Key Benefits

- **Standards compliance**: Implements Google's Encoded Polyline Algorithm as specified
- **Immutable, strongly-typed objects**: Guarantees reliability and thread safety
- **Extensible**: Custom coordinate/polyline support via abstract base classes and interfaces
- **Robust input validation**: Throws descriptive exceptions for out-of-range or malformed input
- **Configuration and logging**: Advanced options and integration with `ILoggerFactory`
- **Benchmarked and tested**: Includes unit and performance tests

## Getting Started

Install via the .NET CLI:

```shell
dotnet add package PolylineAlgorithm
```

Then explore the versioned guide and API reference below to learn more.

{versions_section}

## Links

- [Contributing Guidelines](https://github.com/petesramek/polyline-algorithm-csharp/blob/main/CONTRIBUTING.md)
- [Report an Issue](https://github.com/petesramek/polyline-algorithm-csharp/issues)
