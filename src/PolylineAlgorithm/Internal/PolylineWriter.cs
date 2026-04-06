//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

/// <summary>
/// Engine-owned stateful cursor that feeds values written by a formatter into the polyline encoding pipeline.
/// </summary>
/// <remarks>
/// Each instance wraps a growable <see cref="ArrayPool{T}"/>-backed output buffer and a per-slot delta accumulator.
/// The engine calls <see cref="BeginItem"/> before invoking the formatter for each item so that the slot index
/// resets correctly while delta state is preserved across item boundaries.
/// </remarks>
internal sealed class PolylineWriter : IPolylineWriter {
    private char[] _buffer;
    private int _position;
    private readonly uint _precision;
    private int[] _previous;
    private int _slotIndex;

    internal PolylineWriter(int initialCapacity, uint precision) {
        _buffer = ArrayPool<char>.Shared.Rent(initialCapacity);
        _precision = precision;
        _previous = [];
        _slotIndex = 0;
        _position = 0;
    }

    /// <summary>
    /// Emits one field value into the encoding pipeline.
    /// </summary>
    public void Write(double value) {
        // Grow the per-slot delta array on the first item (field discovery).
        if (_slotIndex >= _previous.Length) {
            Array.Resize(ref _previous, _slotIndex + 1);
        }

        int normalized = PolylineEncoding.Normalize(value, _precision);
        int delta = normalized - _previous[_slotIndex];
        _previous[_slotIndex] = normalized;
        _slotIndex++;

        // Ensure the output buffer has room for the worst-case encoded chunk.
        EnsureCapacity(_position + Defaults.Polyline.Block.Length.Max);

        if (!PolylineEncoding.TryWriteValue(delta, _buffer.AsSpan(), ref _position)) {
            ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
        }
    }

    /// <summary>
    /// Resets the intra-item slot index so delta state is applied to the correct field on the next item.
    /// Must be called by the engine before each call to the formatter's Write method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void BeginItem() => _slotIndex = 0;

    /// <summary>
    /// Gets the number of fields written in the current (or most recently completed) item.
    /// </summary>
    internal int SlotIndex => _slotIndex;

    /// <summary>
    /// Returns the encoded polyline characters written so far.
    /// The caller must not use this memory after <see cref="ReturnBuffer"/> is called.
    /// </summary>
    internal ReadOnlyMemory<char> WrittenMemory => _buffer.AsMemory(0, _position);

    /// <summary>
    /// Returns the rented buffer to the pool. Must be called exactly once when encoding is complete.
    /// </summary>
    internal void ReturnBuffer() => ArrayPool<char>.Shared.Return(_buffer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacity(int required) {
        if (required <= _buffer.Length) {
            return;
        }

        int newSize = Math.Max(_buffer.Length * 2, required);
        char[] newBuffer = ArrayPool<char>.Shared.Rent(newSize);
        _buffer.AsSpan(0, _position).CopyTo(newBuffer);
        ArrayPool<char>.Shared.Return(_buffer);
        _buffer = newBuffer;
    }
}
