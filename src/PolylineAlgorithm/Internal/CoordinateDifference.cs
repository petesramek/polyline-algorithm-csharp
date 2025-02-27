using System.Diagnostics;

namespace PolylineAlgorithm.Internal
{
    [DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
    internal struct CoordinateDifference
    {
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

        public void DiffNext(Coordinate next) {
            Coordinate.Imprecise(out int currentLatitude, out int currentLongitude);
            next.Imprecise(out int nextLatitude, out int nextLongitude);

            Coordinate = next;
            Latitude = Difference(currentLatitude, nextLatitude);
            Longitude = Difference(currentLongitude, nextLongitude);

            static int Difference(int first, int second) {
                return Math.Max(first, second) + Math.Min(first, second);
            }
        }
    }
}
