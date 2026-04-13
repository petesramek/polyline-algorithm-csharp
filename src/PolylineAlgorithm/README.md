# PolylineAlgorithm for .NET

[![NuGet](https://img.shields.io/nuget/v/PolylineAlgorithm)](https://www.nuget.org/packages/PolylineAlgorithm)

Google's Encoded Polyline Algorithm compresses sequences of geographic coordinates into a compact ASCII string, widely used in mapping APIs. This library provides a fully compliant .NET implementation with extensible, type-safe encoding and decoding APIs.

## Features

- Google-compliant polyline encoding/decoding for geographic coordinates
- Fluent `FormatterBuilder<TValue, TPolyline>` for configuring custom coordinate and polyline types
- Immutable, sealed `PolylineFormatter<TValue, TPolyline>` built via `FormatterBuilder`
- `PolylineEncoder<TValue, TPolyline>` and `PolylineDecoder<TPolyline, TValue>` — configurable via `PolylineOptions`
- Extension methods for encoding from `List<T>` and arrays (`PolylineEncoderExtensions`)
- Robust input validation and descriptive exceptions
- Configurable stack-alloc buffer size and logging via `PolylineOptions`
- Thread-safe, stateless APIs
- Low-level utilities via static `PolylineEncoding` class (Normalize, Denormalize, TryReadValue, TryWriteValue, ValidateFormat, etc.)
- Benchmarks and unit tests for correctness and performance
- Auto-generated API docs ([API Reference](https://petesramek.github.io/polyline-algorithm-csharp/))
- Supports .NET Core, .NET 5+, Xamarin, Unity, Blazor via `netstandard2.1`

## Installation

```shell
dotnet add package PolylineAlgorithm
```

or via NuGet PMC:

```powershell
Install-Package PolylineAlgorithm
```

## Quick Start

Use `FormatterBuilder` to configure your coordinate and polyline types, then construct `PolylineEncoder` and `PolylineDecoder` from the resulting `PolylineOptions`.

### Build a formatter

```csharp
using PolylineAlgorithm;

PolylineFormatter<(double Lat, double Lon), string> formatter =
    FormatterBuilder<(double Lat, double Lon), string>.Create()
        .AddValue("lat", static c => c.Lat)
        .AddValue("lon", static c => c.Lon)
        .WithCreate(static v => (v[0], v[1]))
        .ForPolyline(static m => new string(m.Span), static s => s.AsMemory())
        .Build();
```

### Encode coordinates

```csharp
using PolylineAlgorithm.Extensions;

PolylineOptions<(double Lat, double Lon), string> options = new(formatter);
PolylineEncoder<(double Lat, double Lon), string> encoder = new(options);

var coordinates = new List<(double Lat, double Lon)>
{
    (48.858370, 2.294481),
    (51.500729, -0.124625)
};

string encoded = encoder.Encode(coordinates); // extension method for List<T>

Console.WriteLine(encoded);
// Output: "yseiHoc_MwacOjnwM"
```

### Decode polyline

```csharp
PolylineDecoder<string, (double Lat, double Lon)> decoder = new(options);

IEnumerable<(double Lat, double Lon)> decoded = decoder.Decode("yseiHoc_MwacOjnwM");
```

## Advanced Usage

Pass a `stackAllocLimit` and an `ILoggerFactory` to `PolylineOptions` to customize buffer sizing and logging:

```csharp
using Microsoft.Extensions.Logging;

PolylineOptions<(double Lat, double Lon), string> options = new(
    formatter,
    stackAllocLimit: 1024,
    loggerFactory: loggerFactory);

PolylineEncoder<(double Lat, double Lon), string> encoder = new(options);
PolylineDecoder<string, (double Lat, double Lon)> decoder = new(options);
```

Use static methods on `PolylineEncoding` for low-level normalization, validation, and bit-level read/write operations.

> See [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/) for full documentation.

## FAQ

- **What coordinate ranges are valid?**  
  Latitude: -90..90, Longitude: -180..180 (throws `ArgumentOutOfRangeException` for invalid input)
- **What happens if I pass malformed input to the decoder?**  
  The decoder throws `InvalidPolylineException` with a descriptive message. Wrap calls in a try/catch in your application.
- **What .NET versions are supported?**  
  Any environment supporting `netstandard2.1`
- **How do I customize encoder/decoder options?**  
  Create a `PolylineOptions<TValue, TPolyline>` with your `PolylineFormatter`, optional `stackAllocLimit`, and optional `ILoggerFactory`, then pass it to the `PolylineEncoder` or `PolylineDecoder` constructor.
- **Where can I get help?**  
  [GitHub issues](https://github.com/petesramek/polyline-algorithm-csharp/issues)

## Resources

- [GitHub Repository](https://github.com/petesramek/polyline-algorithm-csharp) — source code, issues, changelog, and samples
- [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/) — full auto-generated documentation

## License

MIT License © Pete Sramek
