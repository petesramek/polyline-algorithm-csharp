//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Internal;

using System;
using System.Buffers;
using System.Runtime.InteropServices;

/// <summary>
/// Provides an efficient mechanism for constructing a <see cref="Polyline"/> from multiple character segments.
/// This struct enables incremental building of a polyline by appending segments and producing a single concatenated result.
/// </summary>
[StructLayout(LayoutKind.Auto)]
internal struct PolylineBuilder {
    private PolylineSegment? _initial;
    private PolylineSegment? _last;

    /// <summary>
    /// Appends a segment of characters to the polyline under construction.
    /// </summary>
    /// <param name="value">
    /// The read-only memory region containing the characters to append as a new segment.
    /// </param>
    /// <remarks>
    /// A new <see cref="PolylineSegment"/> is created for the provided memory and linked to the existing chain of segments.
    /// </remarks>
    public void Append(ReadOnlyMemory<char> value) {
        var current = new PolylineSegment(value);

        _initial ??= current;

        _last?.Append(current);
        _last = current;
    }

    /// <summary>
    /// Constructs the final <see cref="Polyline"/> instance by concatenating all appended segments.
    /// </summary>
    /// <returns>
    /// A <see cref="Polyline"/> representing the combined character segments.
    /// If no segments have been appended, returns an empty <see cref="Polyline"/>.
    /// </returns>
    public readonly ReadOnlySequence<char> Build() {
        if (_initial is null) {
            return new();
        }

        return new(_initial, 0, _last, _last!.Memory.Length);
    }
}
