//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;

internal sealed class Program {
    private static readonly NetTopologyPolylineDecoder _decoder = new();
    private static readonly NetTopologyPolylineEncoder _encoder = new();

    static void Main(string[] _) {
    Start:
        Console.WriteLine($"Please, select an operation:{Environment.NewLine}");
        Console.WriteLine($"[0] Decode{Environment.NewLine}");
        Console.WriteLine($"[1] Encode{Environment.NewLine}");

        var key = Console.ReadKey(false);

        switch (key.Key) {
            case ConsoleKey.D0:
                goto Decode;
            case ConsoleKey.D1:
                goto Encode;
            default:
                Console.WriteLine($"Wrong number!{Environment.NewLine}");
                goto Start;
        }

    Decode:

        Console.WriteLine($"{Environment.NewLine}Enter polyline string. You can make one on https://developers.google.com/maps/documentation/utilities/polylineutility.{Environment.NewLine}");

        var polyline = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(polyline)) {
            Console.WriteLine($"{Environment.NewLine}Polyline doesn't seems to be valid.{Environment.NewLine}");
            goto Decode;
        }

        try {
            var result = _decoder.Decode(polyline).ToList();

            Console.WriteLine($"{Environment.NewLine}Type: {result}{Environment.NewLine}");

            foreach (var item in result) {
                Console.WriteLine($"AsText: {item.AsText()}{Environment.NewLine}");
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
            goto Start;
        }

    Encode:
        Console.WriteLine($"{Environment.NewLine}Enter collection of NetTopology points in format latitude1,longitude1;latitude2,longitude2{Environment.NewLine}");

        var coordinates = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(coordinates) || !TryParse(coordinates!, out var points)) {
            Console.WriteLine($"{Environment.NewLine}Polyline doesn't seems to be valid.{Environment.NewLine}");
            goto Encode;
        }

        try {
            var result = _encoder.Encode(points);

            Console.WriteLine($"{Environment.NewLine}Polyline: {result}{Environment.NewLine}");
        } catch (Exception ex) {
            Console.WriteLine($"Error: {ex.Message}{Environment.NewLine}");
            goto Start;
        }
    }

    private static bool TryParse(string coordinates, out IEnumerable<Point> result) {
        var temp = new List<Point>();

        var pairs = coordinates
            .Split(';');

        foreach (var pair in pairs) {
            var coords = pair.Split(',');

            if (coords.Length != 2 || !double.TryParse(coords[0], out double x) || !double.TryParse(coords[1], out double y)) {
                result = temp;
                return false;
            }

            temp.Add(new Point(x, y));
        }

        result = temp;
        return true;
    }
}
