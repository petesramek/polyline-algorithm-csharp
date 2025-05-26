namespace PolylineAlgorithm.Utility;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

internal static class RandomValueProvider {
    private static readonly Random _random = new(DateTime.Now.Millisecond);
    private static readonly ConcurrentDictionary<int, PolylineCoordinateCollectionPair> _cache = new();
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

    private static PolylineCoordinateCollectionPair GetCaheEntry(int count) {
        if (_cache.TryGetValue(count, out var entry)) {
            return entry;
        }

        var enumeration = Enumerable
                            .Range(0, count)
                            .Select(i => new Coordinate(RandomLatitude(), RandomLongitude()))
                            .ToList();

        entry = _cache.GetOrAdd(count, _ => new PolylineCoordinateCollectionPair(enumeration, _encoder.Encode(enumeration)));

        return entry;
    }

    private static double RandomLongitude() {
        return Math.Round(_random.Next(-180, 180) + _random.NextDouble(), 5);
    }

    private static double RandomLatitude() {
        return Math.Round(_random.Next(-90, 90) + _random.NextDouble(), 5);
    }

    private readonly struct PolylineCoordinateCollectionPair(IEnumerable<Coordinate> coordinates, Polyline polyline) {
        public IEnumerable<Coordinate> Coordinates { get; } = coordinates;
        public Polyline Polyline { get; } = polyline;
    }
}