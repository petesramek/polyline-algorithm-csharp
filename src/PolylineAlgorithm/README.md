# PolylineAlgorithm for .NET

A modern, fully compliant Google Encoded Polyline Algorithm library for .NET Standard 2.1+, supporting strong input validation, extensibility for custom coordinate types, and robust performance.

## Features

- Google-compliant polyline encoding/decoding for geographic coordinates
- Immutable, strongly-typed data structures: `Coordinate`, `Polyline`
- Predefined encoder/decoder types for easy usage
- Extensible APIs for custom coordinate and polyline types
- Robust input validation and descriptive exceptions
- Configurable with `PolylineEncodingOptions` (buffer, logging, etc.)
- Thread-safe, stateless APIs
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

### Encode coordinates

```csharp
using PolylineAlgorithm;

var coordinates = new List<Coordinate>
{
    new Coordinate(48.858370, 2.294481),
    new Coordinate(51.500729, -0.124625)
};

var encoder = new PolylineEncoder();
Polyline encoded = encoder.Encode(coordinates);

Console.WriteLine(encoded.ToString()); // Print encoded polyline string
```

### Decode polyline

```csharp
using PolylineAlgorithm;

var decoder = new PolylineDecoder();
Polyline polyline = Polyline.FromString("yseiHoc_MwacOjnwM");
IEnumerable<Coordinate> decoded = decoder.Decode(polyline);
```

## Advanced Usage

- Custom coordinate/polyline types are supported via `AbstractPolylineEncoder` and `AbstractPolylineDecoder`.
- Additional configuration via `PolylineEncodingOptionsBuilder`.

> See [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/) for full documentation.

## FAQ

- **What coordinate ranges are valid?**  
  Latitude: -90..90, Longitude: -180..180 (throws `ArgumentOutOfRangeException` for invalid input)
- **What .NET versions are supported?**  
  Any environment supporting `netstandard2.1`
- **How do I customize encoder options?**  
  Use `PolylineEncodingOptionsBuilder` and pass to the encoder constructor.
- **Where can I get help?**  
  [GitHub issues](https://github.com/petesramek/polyline-algorithm-csharp/issues)

## License

MIT License © Pete Sramek
