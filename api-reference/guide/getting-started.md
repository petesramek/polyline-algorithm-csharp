# Getting Started

PolylineAlgorithm is a lightweight, Google-compliant polyline encoding/decoding library for .NET Standard 2.1 and above.  
Follow these simple steps to get started, encode and decode polylines, and configure advanced features.

---

## Installation

Install via the .NET CLI:

```shell
dotnet add package PolylineAlgorithm
```

Or via NuGet Package Manager:

```powershell
Install-Package PolylineAlgorithm
```

---

## Basic Usage

### Encoding Coordinates

```csharp
using PolylineAlgorithm;

var coordinates = new List<Coordinate>
{
    new Coordinate(48.858370, 2.294481),    // Eiffel Tower
    new Coordinate(51.500729, -0.124625)    // Big Ben
};

var encoder = new PolylineEncoder();
Polyline encoded = encoder.Encode(coordinates);

Console.WriteLine(encoded.ToString()); // Prints the encoded polyline string
```

### Decoding a Polyline

```csharp
using PolylineAlgorithm;

var decoder = new PolylineDecoder();
Polyline polyline = Polyline.FromString("yseiHoc_MwacOjnwM"); // Sample encoded string

IEnumerable<Coordinate> decoded = decoder.Decode(polyline);

// Show decoded coordinates
foreach(var coord in decoded)
{
    Console.WriteLine($"{coord.Latitude}, {coord.Longitude}");
}
```

---

## Customizing and Advanced Features

- Use `PolylineEncodingOptionsBuilder` to customize settings (buffer size, logging, etc.)
- Implement custom encoder/decoder types for advanced coordinate representations
- See [API reference](https://petesramek.github.io/polyline-algorithm-csharp/) for details

---

## Need More Help?

- [FAQ](./faq.md)
- [Examples](./examples.md)
- [Report an Issue](https://github.com/petesramek/polyline-algorithm-csharp/issues)

---
