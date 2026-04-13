//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using NetTopologySuite.Geometries;
using PolylineAlgorithm;

public class Program {
    public static void Main(string[] args) {
        // Build a formatter for NetTopologySuite's Point type.
        // NTS convention: Y = latitude, X = longitude.
        PolylineFormatter<Point, string> formatter =
            FormatterBuilder<Point, string>.Create()
                .AddValue("lat", static p => p.Y)
                .AddValue("lon", static p => p.X)
                // The formatter automatically denormalizes scaled values, so v[0] = latitude, v[1] = longitude.
                .WithValueFactory(static v => new Point(x: v[1], y: v[0]))
                .WithReaderWriter(
                    static m => m.IsEmpty ? string.Empty : new string(m.Span),
                    static s => s.AsMemory())
                .Build();

        PolylineOptions<Point, string> options = new(formatter);
        PolylineEncoder<Point, string> encoder = new(options);
        PolylineDecoder<string, Point> decoder = new(options);

        // Sample route: Seattle → Bellevue → Redmond
        var points = new Point[]
        {
            new(x: -122.3503, y: 47.6219),  // Seattle (x = longitude, y = latitude)
            new(x: -122.2015, y: 47.6101),  // Bellevue
            new(x: -122.1215, y: 47.6740),  // Redmond
        };

        // Encode
        string encoded = encoder.Encode(points);

        Console.WriteLine("=== NetTopologySuite Polyline Sample ===");
        Console.WriteLine();
        Console.WriteLine("Input points (longitude, latitude):");

        foreach (Point p in points) {
            Console.WriteLine($"  ({p.X}, {p.Y})");
        }

        Console.WriteLine();
        Console.WriteLine($"Encoded polyline: {encoded}");
        Console.WriteLine();

        // Decode
        IEnumerable<Point> decoded = decoder.Decode(encoded);

        Console.WriteLine("Decoded points (longitude, latitude):");

        foreach (Point p in decoded) {
            Console.WriteLine($"  ({p.X}, {p.Y})");
        }
    }
}