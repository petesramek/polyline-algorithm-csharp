namespace PolylineAlgorithm.Utility;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public static class ValueProvider {
    private static readonly Random _random = new(DateTime.Now.Millisecond);
    private static readonly ConcurrentDictionary<int, CoordinatePair> _cache = new();
    private static readonly PolylineEncoder _encoder = new();

    public static IEnumerable<Coordinate> GetCoordinates(int count) {
        var entry = GetCaheEntry(count);

        using var enumerator = entry.Coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            yield return enumerator.Current;
        }
    }

    public static Polyline GetPolyline(int count) {
        var entry = GetCaheEntry(count);

        return entry.Polyline;
    }

    private static CoordinatePair GetCaheEntry(int count) {
        if (_cache.TryGetValue(count, out var entry)) {
            return entry;
        }

        var enumeration = Enumerable
                            .Range(0, count)
                            .Select(i => new Coordinate(RandomLatitude(), RandomLongitude()))
                            .ToList();

        entry = _cache.GetOrAdd(count, _ => new CoordinatePair(enumeration, _encoder.Encode(enumeration)));

        return entry;
    }

    private static double RandomLongitude() {
        return Math.Round(_random.Next(-180, 180) + _random.NextDouble(), 5);
    }

    private static double RandomLatitude() {
        return Math.Round(_random.Next(-90, 90) + _random.NextDouble(), 5);
    }

    private readonly struct CoordinatePair {
        public CoordinatePair(IEnumerable<Coordinate> coordinates, Polyline polyline) {
            Coordinates = coordinates;
            Polyline = polyline;
        }

        public IEnumerable<Coordinate> Coordinates { get; }
        public Polyline Polyline { get; }
    }
}