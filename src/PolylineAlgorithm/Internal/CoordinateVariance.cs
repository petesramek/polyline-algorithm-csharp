namespace PolylineAlgorithm.Internal; 

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

    public static CoordinateVariance Calculate(PolylineCoordinate initial, PolylineCoordinate next) {
        int latitude = Variance(initial.Latitude, next.Latitude);
        int longitude = Variance(initial.Longitude, next.Longitude);

        return new CoordinateVariance(latitude, longitude);
    }

    public override readonly string ToString()
        => $"Variance: {{ Latitude: {Latitude}, Longitude: {Longitude} }}";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Variance(int initial, int next) => (initial, next) switch {
        (0, 0) => 0,
        (0, _) => next,
        (_, 0) => -initial,
        ( < 0, < 0) => -(Math.Abs(next) - Math.Abs(initial)),
        ( < 0, > 0) => next + Math.Abs(initial),
        ( > 0, < 0) => -(Math.Abs(next) + initial),
        ( > 0, > 0) => next - initial,
    };

    public static PolylineCoordinate operator +(PolylineCoordinate coordinate, CoordinateVariance variance) {
        return Add(coordinate, variance);
    }

    public static PolylineCoordinate Add(PolylineCoordinate coordinate, CoordinateVariance variance) {
        var latitude = coordinate.Latitude + variance.Latitude;
        var longitude = coordinate.Longitude + variance.Longitude;

        return new PolylineCoordinate(latitude, longitude);
    }
}
