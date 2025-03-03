using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PolylineAlgorithm.Internal {
    [DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
    internal struct CoordinateDifference {
        public CoordinateDifference() {
            Coordinate = default;
            Latitude = default;
            Longitude = default;
        }

        public Coordinate Coordinate { get; private set; }
        public int Latitude { get; private set; }
        public int Longitude { get; private set; }

        public override string ToString()
            => $"Latitude: {Latitude}, Longitude: {Longitude}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DiffNext(Coordinate next) {
            var current = Exchange(next);

            current.Imprecise(out int currentLatitude, out int currentLongitude);
            next.Imprecise(out int nextLatitude, out int nextLongitude);

            Latitude = Difference(currentLatitude, nextLatitude);
            Longitude = Difference(currentLongitude, nextLongitude);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Difference(int first, int second) => (first, second) switch {
            (0, 0) => 0,
            (0, _) => second,
            (_, 0) => -first,
            ( < 0, < 0) => -(Math.Abs(second) - Math.Abs(first)),
            ( < 0, > 0) => second + Math.Abs(first),
            ( > 0, < 0) => -(Math.Abs(second) + first),
            ( > 0, > 0) => second - first,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Coordinate Exchange(Coordinate value) {
            var current = Coordinate;

            Coordinate = value;

            return current;
        }
    }
}
