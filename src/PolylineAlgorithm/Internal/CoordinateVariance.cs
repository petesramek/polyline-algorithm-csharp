namespace PolylineAlgorithm.Internal;

using System;
using System.Diagnostics;

[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
public readonly ref struct CoordinateVariance {
    private CoordinateVariance(int latitude, int longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    public int Latitude { get; }
    public int Longitude { get; }

    public static CoordinateVariance Create(int latitude, int longitude) {
        return new CoordinateVariance(latitude, longitude);
    }

    public static CoordinateVariance Calculate(Coordinate initial, Coordinate next) {
        int latitude = Variance(Round(initial.Latitude), Round(next.Latitude));
        int longitude = Variance(Round(initial.Longitude), Round(next.Longitude));

        return new CoordinateVariance(latitude, longitude);
    }

    public override readonly string ToString()
        => $"Variance: {{ Latitude: {Latitude}, Longitude: {Longitude} }}";

    private static int Variance(int initial, int next) => (initial, next) switch {
        (0, 0) => 0,
        (0, _) => next,
        (_, 0) => -initial,
        ( < 0, < 0) => -(Math.Abs(next) - Math.Abs(initial)),
        ( < 0, > 0) => next + Math.Abs(initial),
        ( > 0, < 0) => -(Math.Abs(next) + initial),
        ( > 0, > 0) => next - initial,
    };

    private static int Round(double value) => (int)Math.Round(value * Defaults.Algorithm.Precision);
}
