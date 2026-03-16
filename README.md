# PolylineAlgorithm for .NET

Lightweight .NET Standard 2.1 library implementing Google Encoded Polyline Algorithm.

## Features

- Immutable types for encoded polylines and geographic coordinates
- Simple, extensible encoding interface (`IPolylineEncoder<TCoordinate, TPolyline>`)
- Unit tests and benchmarks included
- Internal logging and diagnostics (for agents and maintainers)

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

### PolylineEncoder and PolylineDecoder (predefined coordinate and polyline types)

#### Encoding

```csharp
using PolylineAlgorithm;

var coordinates = new List<Coordinate>
{
    new Coordinate(48.858370, 2.294481),
    new Coordinate(51.500729, -0.124625)
};

var encoder = new PolylineEncoder();
Polyline encoded = encoder.Encode(coordinates);

Console.WriteLine(encoded.ToString());
```

#### Decoding

```csharp
using PolylineAlgorithm;

var decoder = new PolylineDecoder();
IEnumerable<Coordinate> decoded = decoder.Decode(encoded.ToString());
```

### Custom encoder and decoder (user0defined coordinate and polyline types)

#### Encoding

Custom encoder implementation.

```csharp
public sealed class MyPolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
    public PolylineEncoder()
        : base() { }

    public PolylineEncoder(PolylineEncodingOptions options)
        : base(options) { }

    protected override double GetLatitude((double Latitude, double Longitude) coordinate) {
        return coordinate.Latitude;
    }

    protected override double GetLongitude((double Latitude, double Longitude) coordinate) {
        return coordinate.Longitude;
    }

    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
        return polyline.ToString();
    }
}
```

Custom encoder usage.

```csharp
using PolylineAlgorithm;

var coordinates = new List<(double Latitude, double Longitude)>
{
    (48.858370, 2.294481),
    (51.500729, -0.124625)
};

var encoder = new MyPolylineEncoder();
string encoded = encoder.Encode(coordinates);

Console.WriteLine(encoded.ToString());
```

#### Decoding

Custom decoder implementation.

```csharp
public sealed class MyPolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
    public PolylineDecoder()
        : base() { }

    public PolylineDecoder(PolylineEncodingOptions options)
        : base(options) { }

    protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) {
        return (latitude, longitude);
    }

    protected override ReadOnlyMemory<char> GetReadOnlyMemory(ref string polyline) {
        return polyline.AsMemory();
    }
}
```

Custom decoder usage.

```csharp
using PolylineAlgorithm;

string encoded = "oir~FxmzuOnhA|dHlyConJ"

var decoder = new MyPolylineDecoder();
IEnumerable<(double Latitude, double Longitude)> decoded = decoder.Decode(encoded);
```

> **Note:**
> If you need low-level utilities for normalization, validation, encoding and decoding, use static methods from the `PolylineEncoding` class.

## API Reference

Full API docs and guides (auto-generated from source) are available at [API Reference](https://petesramek.github.io/polyline-algorithm-csharp/)

## Benchmarks

- See `/benchmarks` in the repo for performance evaluation.
- Contributors: Update benchmarks and document results for performance-impacting PRs.

## FAQ

**Q: What coordinate ranges are valid?**  
A: Latitude must be -90..90; longitude -180..180. Out-of-range input throws `ArgumentOutOfRangeException`.

**Q: Which .NET versions are supported?**  
A: All platforms supporting `netstandard2.1` (including .NET Core and .NET 5+).

## Contributing

- Follow code style and PR instructions in [AGENTS.md](./AGENTS.md).
- Ensure all features are covered by tests and XML doc comments.
- For questions or suggestions, open an issue and use the provided templates.

---

## License

MIT License &copy; Pete Sramek
