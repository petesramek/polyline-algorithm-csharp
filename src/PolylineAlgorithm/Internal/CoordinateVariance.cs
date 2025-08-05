//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

/// <summary>
/// Represents the difference (variance) in latitude and longitude between consecutive geographic coordinates.
/// This struct is used to compute and store the change in coordinate values as integer deltas.
/// </summary>
[DebuggerDisplay($"{{{nameof(ToString)}(),nq}}")]
[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
internal struct CoordinateVariance {
    private (int Latitude, int Longitude) _current;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateVariance"/> struct with the default latitude and longitude deltas.
    /// </summary>
    public CoordinateVariance() {
        _current =(default, default);
    }

    /// <summary>
    /// Gets the current variance in latitude between the most recent and previous coordinate.
    /// </summary>
    public int Latitude { get; private set; }

    /// <summary>
    /// Gets the current variance in longitude between the most recent and previous coordinate.
    /// </summary>
    public int Longitude { get; private set; }

    /// <summary>
    /// Updates the variance values based on the next latitude and longitude, and sets the current coordinate as next variance baseline.
    /// </summary>
    /// <param name="latitude">The next latitude value.</param>
    /// <param name="longitude">The next longitude value.</param>

    public void Next(int latitude, int longitude) {
        Latitude = Variance(_current.Latitude, latitude);
        Longitude = Variance(_current.Longitude, longitude);

        _current.Latitude = latitude;
        _current.Longitude = longitude;
    }

    /// <summary>
    /// Calculates the variance (delta) between two coordinate values.
    /// </summary>
    /// <remarks>
    /// This method computes the difference between two integer coordinate values, handling cases where the values may be positive or negative.
    /// </remarks>
    /// <param name="initial">The previous coordinate value.</param>
    /// <param name="next">The next coordinate value.</param>
    /// <returns>The computed variance between <paramref name="initial"/> and <paramref name="next"/>.</returns>

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
    /// Returns a string representation of the current coordinate variance.
    /// </summary>
    /// <returns>
    /// A string in the format <c>{ Coordinate:  { Latitude: [int], Longitude: [int] }, Variance: { Latitude: [int], Longitude: [int] } }</c> representing the current coordinate and deltas to previous coordinate.
    /// </returns>
    public override readonly string ToString()
        => $"{{ Coordinate:  {{ Latitude: {Latitude}, Longitude: {Longitude} }}, Variance: {{ Latitude: {Latitude}, Longitude: {Longitude} }} }}";
}
