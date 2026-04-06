# PolylineAlgorithm for .NET

[![NuGet](https://img.shields.io/nuget/v/PolylineAlgorithm)](https://www.nuget.org/packages/PolylineAlgorithm)

Google's Encoded Polyline Algorithm compresses sequences of geographic coordinates into a compact ASCII string, widely used in mapping APIs. This library provides a fully compliant .NET implementation with extensible, type-safe encoding and decoding APIs.

## Features

- Google-compliant polyline encoding/decoding for geographic coordinates
- Extensible APIs for custom coordinate and polyline types (`AbstractPolylineEncoder<TCoordinate, TPolyline>`, `AbstractPolylineDecoder<TPolyline, TCoordinate>`)
- Extension methods for encoding from `List<T>` and arrays (`PolylineEncoderExtensions`)
- Robust input validation and descriptive exceptions
- Configurable with `PolylineEncodingOptions` (precision, buffer size, logging)
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

The library provides abstract base classes to build your own encoder and decoder for any coordinate and polyline type.

### Implement a custom encoder

```csharp
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;

public sealed class MyPolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
    protected override void Write((double Latitude, double Longitude) item, IPolylineWriter writer) {
        writer.Write(item.Latitude);   // field 0
        writer.Write(item.Longitude);  // field 1
    }
    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
}
```

### Encode coordinates

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
// Output: "yseiHoc_MwacOjnwM"
```

### Implement a custom decoder

```csharp
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;

public sealed class MyPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
    protected override (double Latitude, double Longitude) Read(IPolylineReader reader) =>
        (reader.Read(), reader.Read());  // field 0, field 1
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
}
```

### Decode polyline

```csharp
string encoded = "yseiHoc_MwacOjnwM";

var decoder = new MyPolylineDecoder();
IEnumerable<(double Latitude, double Longitude)> decoded = decoder.Decode(encoded);
```

## Advanced Usage

Use `PolylineEncodingOptionsBuilder` to customize precision, buffer size, and logging, then pass the built options to the encoder or decoder constructor:

```csharp
using Microsoft.Extensions.Logging;

PolylineEncodingOptions options = PolylineEncodingOptionsBuilder.Create()
    .WithPrecision(6)                        // 6 decimal places instead of the default 5
    .WithLoggerFactory(loggerFactory)        // plug in your ILoggerFactory
    .Build();

var encoder = new MyPolylineEncoder(options);
var decoder = new MyPolylineDecoder(options);
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
- **How do I customize encoder options?**  
  Use `PolylineEncodingOptionsBuilder` and pass the built options to the encoder or decoder constructor.
- **Where can I get help?**  
  [GitHub issues](https://github.com/petesramek/polyline-algorithm-csharp/issues)

## Resources

- [GitHub Repository](https://github.com/petesramek/polyline-algorithm-csharp) — source code, issues, changelog, and samples
- [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/) — full auto-generated documentation

## License

MIT License © Pete Sramek
