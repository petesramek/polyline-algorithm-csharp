namespace PolylineAlgorithm.Comparison.Benchmarks.Internal {
    using System.Collections.Concurrent;

    public static class ValueProvider {
        private static readonly ConcurrentDictionary<int, CoordinatePair> _cache = new();
        private static readonly DefaultPolylineEncoder _encoder = new();

        public static IEnumerable<Coordinate> GetCoordinates(int count) {
            var entry = GetCaheEntry(count);

            return entry.Coordinates;
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
            return Math.Round(Random.Shared.Next(-180, 180) + Random.Shared.NextDouble(), 5);
        }

        private static double RandomLatitude() {
            return Math.Round(Random.Shared.Next(-90, 90) + Random.Shared.NextDouble(), 5);
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
}
