namespace PolylineAlgorithm.Internal;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Represents the variance between consecutive geographic coordinates (latitude and longitude).
/// This struct is used to calculate and store the differences between coordinate values.
/// </summary>
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[StructLayout(LayoutKind.Auto)]
internal struct CoordinateVariance {
    private (int Latitude, int Longitude) _current = (0, 0);

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateVariance"/> struct with the specified latitude and longitude values.
    /// </summary>
    /// <param name="latitude">The initial latitude value.</param>
    /// <param name="longitude">The initial longitude value.</param>
    private CoordinateVariance(int latitude, int longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets the variance in latitude between the current and previous coordinates.
    /// </summary>
    public int Latitude { get; private set; }

    /// <summary>
    /// Gets the variance in longitude between the current and previous coordinates.
    /// </summary>
    public int Longitude { get; private set; }

    /// <summary>
    /// Updates the variance based on the next set of latitude and longitude values.
    /// </summary>
    /// <param name="latitude">The next latitude value.</param>
    /// <param name="longitude">The next longitude value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Next(int latitude, int longitude) {
        Latitude = Variance(_current.Latitude, latitude);
        Longitude = Variance(_current.Longitude, longitude);

        _current = (latitude, longitude);
    }

    /// <summary>
    /// Calculates the variance between two coordinate values.
    /// </summary>
    /// <param name="initial">The initial coordinate value.</param>
    /// <param name="next">The next coordinate value.</param>
    /// <returns>The calculated variance between the two values.</returns>
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

    /// <summary>
    /// Returns a string representation of the coordinate variance.
    /// </summary>
    /// <returns>A string in the format: Variance: { Latitude: [int], Longitude: [int] }.</returns>
    public override readonly string ToString()
        => $"Variance: {{ Latitude: {Latitude}, Longitude: {Longitude} }}";
}
