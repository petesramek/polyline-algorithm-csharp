namespace PolylineAlgorithm.Internal;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[StructLayout(LayoutKind.Auto)]
internal struct CoordinateVariance {
    private (int Latitude, int Longitude) _current = (0, 0);

    private CoordinateVariance(int latitude, int longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    public int Latitude { get; private set; }

    public int Longitude { get; private set; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Next(int latitude, int longitude) {
        Latitude = Variance(_current.Latitude, latitude);
        Longitude = Variance(_current.Longitude, longitude);

        _current = (latitude, longitude);
    }

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

    public override readonly string ToString()
        => $"Variance: {{ Latitude: {Latitude}, Longitude: {Longitude} }}";
}
