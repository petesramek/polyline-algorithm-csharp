//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Represents the differences (deltas) between consecutive sets of N encoded values.
/// </summary>
/// <remarks>
/// This struct computes and stores the change in coordinate values as integer deltas between successive items.
/// The number of values per item is fixed at construction time.
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
[StructLayout(LayoutKind.Auto)]
internal struct CoordinateDelta {
    private readonly int[] _current;
    private readonly int[] _deltas;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateDelta"/> struct for items with the specified number of values.
    /// </summary>
    /// <param name="count">The number of values per item. Must be greater than zero.</param>
    public CoordinateDelta(int count) {
        Debug.Assert(count > 0, "Count must be greater than zero.");

        _current = new int[count];
        _deltas = new int[count];
    }

    /// <summary>
    /// Gets the current delta values between the most recent and previous item.
    /// </summary>
    public ReadOnlySpan<int> Deltas => _deltas;

    /// <summary>
    /// Updates the delta values based on the next set of encoded values, and sets those values as the new baseline.
    /// </summary>
    /// <param name="values">The next set of encoded integer values. Length must equal the count passed to the constructor.</param>
    public void Next(ReadOnlySpan<int> values) {
        Debug.Assert(values.Length == _current.Length, "Values length must match the count passed to the constructor.");

        for (int i = 0; i < values.Length; i++) {
            _deltas[i] = values[i] - _current[i];
            _current[i] = values[i];
        }
    }

    /// <summary>
    /// Returns a string representation of the current coordinate delta.
    /// </summary>
    /// <returns>
    /// A string in the format <c>{ Coordinate: [v0, v1, ...], Delta: [d0, d1, ...] }</c> representing the current values and their deltas to the previous item.
    /// </returns>
    public override readonly string ToString() {
        StringBuilder sb = new();

        sb.Append("{ Coordinate: [");
        for (int i = 0; i < _current.Length; i++) {
            if (i > 0) {
                sb.Append(", ");
            }

            sb.Append(_current[i]);
        }

        sb.Append("], Delta: [");
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