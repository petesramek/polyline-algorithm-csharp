//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Internal.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// Engine-owned stateful cursor that delivers decoded field values to a formatter during polyline decoding.
/// </summary>
/// <remarks>
/// The reader wraps the raw encoded character sequence and applies the full decoding pipeline
/// (ASCII shift reversal, un-zigzag, per-slot delta accumulation) on each <see cref="Read"/> call.
/// The engine calls <see cref="BeginItem"/> before invoking the formatter for each item so that the
/// slot index resets correctly while delta state is preserved across item boundaries.
/// </remarks>
public sealed class PolylineReader {
    private readonly ReadOnlyMemory<char> _sequence;
    private int _position;
    private int[] _accumulated;
    private int _slotIndex;
    private readonly uint _precision;

    internal PolylineReader(ReadOnlyMemory<char> sequence, uint precision) {
        _sequence = sequence;
        _precision = precision;
        _accumulated = [];
        _slotIndex = 0;
        _position = 0;
    }

    /// <summary>
    /// Reads and returns the next decoded field value from the polyline sequence.
    /// </summary>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the data runs out before the formatter has finished reading an item's fields.
    /// </exception>
    public double Read() {
        // Grow the per-slot accumulator array on the first item (field discovery).
        if (_slotIndex >= _accumulated.Length) {
            Array.Resize(ref _accumulated, _slotIndex + 1);
        }

        if (!PolylineEncoding.TryReadValue(ref _accumulated[_slotIndex], _sequence, ref _position)) {
            ExceptionGuard.ThrowInvalidPolylineFormat(_position);
        }

        double result = PolylineEncoding.Denormalize(_accumulated[_slotIndex], _precision);
        _slotIndex++;
        return result;
    }

    /// <summary>
    /// Resets the intra-item slot index so that delta accumulation is applied to the correct field slot
    /// on the next item. Must be called by the engine before each call to the formatter's Read method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void BeginItem() => _slotIndex = 0;

    /// <summary>
    /// Gets the number of fields read in the current (or most recently completed) item.
    /// </summary>
    internal int SlotIndex => _slotIndex;

    /// <summary>
    /// Gets the current character position in the encoded sequence.
    /// </summary>
    internal int Position => _position;

    /// <summary>
    /// Returns <see langword="true"/> when all encoded characters have been consumed.
    /// </summary>
    internal bool IsEmpty => _position >= _sequence.Length;
}
