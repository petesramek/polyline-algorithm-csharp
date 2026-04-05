//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Represents the running delta state for an arbitrary number of encoded values between consecutive items.
/// </summary>
/// <remarks>
/// This struct computes and stores the change in each value dimension as integer deltas between successive items.
/// The number of dimensions is specified at construction time, enabling support for any number of encoded fields
/// (e.g. latitude/longitude, latitude/longitude/altitude, or arbitrary sensor fields).
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
internal struct CoordinateDelta {
    private readonly int[] _current;
    private readonly int[] _deltas;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateDelta"/> struct for the specified number of value dimensions.
    /// </summary>
    /// <param name="count">The number of value dimensions to track. Must be greater than zero.</param>
    public CoordinateDelta(int count) {
        Debug.Assert(count > 0, "Count must be greater than zero.");

        _current = new int[count];
        _deltas = new int[count];
    }

    /// <summary>
    /// Gets the current deltas computed by the most recent call to <see cref="Next"/>.
    /// </summary>
    public ReadOnlySpan<int> Deltas => _deltas;

    /// <summary>
    /// Updates the delta values based on the next set of values, and sets them as the baseline for the next call.
    /// </summary>
    /// <param name="values">
    /// The next normalized integer values. Must have the same length as the <c>count</c> passed to the constructor.
    /// </param>
    public void Next(ReadOnlySpan<int> values) {
        Debug.Assert(values.Length == _current.Length, "Values length must match the delta dimension count.");

        for (int i = 0; i < values.Length; i++) {
            _deltas[i] = values[i] - _current[i];
            _current[i] = values[i];
        }
    }

    /// <summary>
    /// Returns a string representation of the current values and deltas.
    /// </summary>
    /// <returns>
    /// A string in the format <c>{ Values: [v0, v1, ...], Deltas: [d0, d1, ...] }</c>.
    /// </returns>
    public override readonly string ToString() {
        var sb = new StringBuilder("{ Values: [");
        for (int i = 0; i < _current.Length; i++) {
            if (i > 0) {
                sb.Append(", ");
            }

            sb.Append(_current[i]);
        }

        sb.Append("], Deltas: [");
        for (int i = 0; i < _deltas.Length; i++) {
            if (i > 0) {
                sb.Append(", ");
            }

            sb.Append(_deltas[i]);
        }

        sb.Append("] }");
        return sb.ToString();
    }
}