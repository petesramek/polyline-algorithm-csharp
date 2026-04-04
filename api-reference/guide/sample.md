# Sample Console Application: Using NetTopologySuite with PolylineAlgorithm

This sample demonstrates how to encode and decode polylines using custom implementations (`NetTopologyPolylineEncoder` and `NetTopologyPolylineDecoder`) based on NetTopologySuite's `Point` type.

---

## Prerequisites

- Install the following NuGet packages:
  - `PolylineAlgorithm`
  - `NetTopologySuite`

```shell
dotnet add package PolylineAlgorithm
dotnet add package NetTopologySuite
```

---

## Program.cs

```csharp
using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;

class Program
{
    static void Main()
    {
        // Create some sample points (latitude, longitude)
        var points = new List<Point>
        {
            new Point(48.858370, 2.294481),    // Eiffel Tower
            new Point(51.500729, -0.124625)    // Big Ben
        };

        // Instantiate the custom encoder
        var encoder = new NetTopologyPolylineEncoder();

        // Encode the list of points to a polyline string
        string encodedPolyline = encoder.Encode(points);

        Console.WriteLine("Encoded polyline string:");
        Console.WriteLine(encodedPolyline);

        // Instantiate the custom decoder
        var decoder = new NetTopologyPolylineDecoder();

        // Decode back to NetTopologySuite Point objects
        IEnumerable<Point> decodedPoints = decoder.Decode(encodedPolyline);

        Console.WriteLine("\nDecoded coordinates:");
        foreach (var point in decodedPoints)
        {
            Console.WriteLine($"Latitude: {point.X}, Longitude: {point.Y}");
        }
    }
}

public sealed class NetTopologyPolylineDecoder : AbstractPolylineDecoder<string, Point> {
    protected override Point CreateCoordinate(double latitude, double longitude) {
        return new Point(latitude, longitude);
    }

    protected override ReadOnlyMemory<char> GetReadOnlyMemory(ref string polyline) {
        return polyline.AsMemory();
    }
}

public sealed class NetTopologyPolylineEncoder : AbstractPolylineEncoder<Point, string> {
    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return string.Empty;
        }

        return polyline.ToString();
    }

    protected override double GetLatitude(Point current) {
        // Validate parameter

        return current.X;
    }

    protected override double GetLongitude(Point current) {
        // Validate parameter

        return current.Y;
    }
}
```

---

## Expected Output

```text
Encoded polyline string:
{sample output will be generated at runtime}

Decoded coordinates:
Latitude: 48.85837, Longitude: 2.294481
Latitude: 51.500729, Longitude: -0.124625
```

---

## Notes

- You can further extend this pattern to use any coordinate or geometry type supported by NetTopologySuite.
- The sample demonstrates real usage of a custom `PolylineEncoder`/`PolylineDecoder` in a typical .NET application.

---
