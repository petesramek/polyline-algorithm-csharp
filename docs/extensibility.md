# Extensibility

This guide explains how to use PolylineAlgorithm with your own coordinate types and polyline representations.

## Design Overview

The library is built around two generic abstract base classes:

| Class | Purpose |
|---|---|
| `AbstractPolylineEncoder<TCoordinate, TPolyline>` | Encodes a sequence of coordinates into an encoded polyline |
| `AbstractPolylineDecoder<TPolyline, TCoordinate>` | Decodes an encoded polyline into a sequence of coordinates |

Both implement corresponding interfaces (`IPolylineEncoder<TCoordinate, TPolyline>` and `IPolylineDecoder<TPolyline, TCoordinate>`).

Type parameters:

| Parameter | Meaning | Examples |
|---|---|---|
| `TCoordinate` | Your coordinate type | `(double Lat, double Lon)`, a custom `GeoPoint` class |
| `TPolyline` | Your polyline representation | `string`, `char[]`, `ReadOnlyMemory<char>`, a custom wrapper |

## Adding a Custom Encoder

Subclass `AbstractPolylineEncoder<TCoordinate, TPolyline>` and implement the three abstract methods:

| Method | Signature | What to return |
|---|---|---|
| `GetLatitude` | `double GetLatitude(TCoordinate current)` | The latitude in decimal degrees |
| `GetLongitude` | `double GetLongitude(TCoordinate current)` | The longitude in decimal degrees |
| `CreatePolyline` | `TPolyline CreatePolyline(ReadOnlyMemory<char> polyline)` | Your output type built from the encoded char buffer |

Example — encode `(double Latitude, double Longitude)` tuples to `string`:

```csharp
//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;

/// <summary>
/// Encodes geographic coordinate tuples into a Google-encoded polyline string.
/// </summary>
public sealed class TuplePolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
    /// <inheritdoc/>
    protected override string CreatePolyline(ReadOnlyMemory<char> polyline)
        => polyline.ToString();

    /// <inheritdoc/>
    protected override double GetLatitude((double Latitude, double Longitude) current)
        => current.Latitude;

    /// <inheritdoc/>
    protected override double GetLongitude((double Latitude, double Longitude) current)
        => current.Longitude;
}
```

To use custom encoding options (e.g. precision 6):

```csharp
var options = new PolylineEncodingOptionsBuilder()
    .WithPrecision(6)
    .Build();

var encoder = new TuplePolylineEncoder(options);
```

## Adding a Custom Decoder

Subclass `AbstractPolylineDecoder<TPolyline, TCoordinate>` and implement the two abstract methods:

| Method | Signature | What to return |
|---|---|---|
| `GetReadOnlyMemory` | `ReadOnlyMemory<char> GetReadOnlyMemory(in TPolyline polyline)` | A `ReadOnlyMemory<char>` view over the encoded polyline |
| `CreateCoordinate` | `TCoordinate CreateCoordinate(double latitude, double longitude)` | An instance of your coordinate type |

Example — decode a `string` polyline into `(double Latitude, double Longitude)` tuples:

```csharp
//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;

/// <summary>
/// Decodes a Google-encoded polyline string into geographic coordinate tuples.
/// </summary>
public sealed class TuplePolylineDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
    /// <inheritdoc/>
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline)
        => polyline.AsMemory();

    /// <inheritdoc/>
    protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude)
        => (latitude, longitude);
}
```

## Encoding Options

`PolylineEncodingOptions` controls shared behavior. Configure it via `PolylineEncodingOptionsBuilder`:

```csharp
var options = new PolylineEncodingOptionsBuilder()
    .WithPrecision(5)                   // decimal digits (default: 5)
    .WithLoggerFactory(myLoggerFactory) // enables internal logging
    .Build();
```

Pass the options to the constructor of any encoder or decoder:

```csharp
var encoder = new TuplePolylineEncoder(options);
var decoder = new TuplePolylineDecoder(options);
```

## Extension Methods

The library provides extension methods for `IPolylineEncoder` and `IPolylineDecoder` to support common collection types (`IEnumerable<T>`, arrays, `ReadOnlyMemory<T>`). These are in `PolylineAlgorithm.Extensions`. Your custom implementations automatically benefit from these extension methods as long as you implement the interfaces.
