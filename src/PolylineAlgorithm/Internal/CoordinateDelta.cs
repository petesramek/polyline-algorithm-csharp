//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System.Diagnostics;
using System.Runtime.InteropServices;

/// <summary>
/// Represents the difference (delta) in latitude and longitude between consecutive geographic coordinates.
/// </summary>
/// <remarks>
/// This struct computes and stores the change in coordinate values as integer deltas between successive coordinates.
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
internal struct CoordinateDelta {
    private (int Latitude, int Longitude) _current;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateDelta"/> struct with the default latitude and longitude deltas.
    /// </summary>
    public CoordinateDelta() {
        _current = (default, default);
    }

    /// <summary>
    /// Gets the current delta in latitude between the most recent and previous coordinate.
    /// </summary>
    public int Latitude { get; private set; }

    /// <summary>
    /// Gets the current delta in longitude between the most recent and previous coordinate.
    /// </summary>
    public int Longitude { get; private set; }

    /// <summary>
    /// Updates the delta values based on the next latitude and longitude, and sets the current coordinate as next delta baseline.
    /// </summary>
    /// <param name="latitude">The next latitude value.</param>
    /// <param name="longitude">The next longitude value.</param>
    public void Next(int latitude, int longitude) {
        Latitude = Delta(_current.Latitude, latitude);
        Longitude = Delta(_current.Longitude, longitude);

        _current.Latitude = latitude;
        _current.Longitude = longitude;
    }

    /// <summary>
    /// Calculates the delta between two coordinate values.
    /// </summary>
    /// <remarks>
    /// This method computes the difference between two integer coordinate values, handling cases where the values may be positive or negative.
    /// </remarks>
    /// <param name="initial">The previous coordinate value.</param>
    /// <param name="next">The next coordinate value.</param>
    /// <returns>The computed delta between <paramref name="initial"/> and <paramref name="next"/>.</returns>
    private static int Delta(int initial, int next) => next - initial;

    /// <summary>
    /// Returns a string representation of the current coordinate delta.
    /// </summary>
    /// <returns>
    /// A string in the format <c>{ Coordinate:  { Latitude: [int], Longitude: [int] }, Delta: { Latitude: [int], Longitude: [int] } }</c> representing the current coordinate and deltas to previous coordinate.
    /// </returns>
    public override readonly string ToString() =>
        $"{{ Coordinate: {{ Latitude: {_current.Latitude}, Longitude: {_current.Longitude} }}, " +
        $"Delta: {{ Latitude: {Latitude}, Longitude: {Longitude} }} }}";
}