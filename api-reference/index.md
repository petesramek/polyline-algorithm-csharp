---
_layout: landing
---

# PolylineAlgorithm for .NET

[![NuGet](https://img.shields.io/nuget/v/PolylineAlgorithm.svg)](https://www.nuget.org/packages/PolylineAlgorithm/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/sramekpete/polyline-algorithm-csharp/actions/workflows/build.yml/badge.svg)](https://github.com/sramekpete/polyline-algorithm-csharp/actions/workflows/build.yml)

Lightweight .NET Standard 2.1 library implementing Google Encoded Polyline Algorithm.
Package should be primarily used as baseline for libraries that implement polyline encoding/decoding functionality.

More info about the algorithm can be found at [Google Developers](https://developers.google.com/maps/documentation/utilities/polylinealgorithm).

## Prerequisites

PolylineAlgorithm for .NET is available as a NuGet package targeting .NET Standard 2.1.

.NET CLI: `dotnet add package PolylineAlgorithm`

Package Manager Console: `Install-Package PolylineAlgorithm`

## How to use it

In the majority of cases you would like to inherit `AbstractPolylineDecoder<TPolyline, TCoordinate>` and `AbstractPolylineEncoder<TCoordinate, TPolyline>` classes and implement abstract methods that are mainly responsible for extracting data from your coordinate and polyline types and creating new instances of them.

In some cases you may want to implement your own decoder and encoder from scratch.
In that case you can use `PolylineEncoding` static class that offers static methods for encoding and decoding polyline segments.

## Documentation

Documentation is can be found at [https://sramekpete.github.io/polyline-algorithm-csharp/](https://sramekpete.github.io/polyline-algorithm-csharp/).