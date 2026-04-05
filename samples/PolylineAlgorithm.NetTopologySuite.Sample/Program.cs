//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using NetTopologySuite.Geometries;
using PolylineAlgorithm.NetTopologySuite.Sample;

// Sample route: Seattle → Bellevue → Redmond
var points = new Point[]
{
    new(x: -122.3503, y: 47.6219),  // Seattle (x = longitude, y = latitude)
    new(x: -122.2015, y: 47.6101),  // Bellevue
    new(x: -122.1215, y: 47.6740),  // Redmond
};

var encoder = new NetTopologyPolylineEncoder();
var decoder = new NetTopologyPolylineDecoder();

// Encode
string encoded = encoder.Encode(points);

Console.WriteLine("=== NetTopologySuite Polyline Sample ===");
Console.WriteLine();
Console.WriteLine("Input points (longitude, latitude):");
foreach (Point p in points)
{
    Console.WriteLine($"  ({p.X}, {p.Y})");
}
Console.WriteLine();
Console.WriteLine($"Encoded polyline: {encoded}");
Console.WriteLine();

// Decode
IEnumerable<Point> decoded = decoder.Decode(encoded);

Console.WriteLine("Decoded points (longitude, latitude):");
foreach (Point p in decoded)
{
    Console.WriteLine($"  ({p.X}, {p.Y})");
}
