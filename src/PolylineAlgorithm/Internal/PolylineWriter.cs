//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Engine-owned stateful cursor that feeds values written by a formatter into the polyline encoding pipeline.
/// </summary>
/// <remarks>
/// Each instance wraps a caller-provided <see cref="char"/> buffer sized to the worst-case maximum
/// capacity so that the buffer never needs to grow. The buffer may be stack-allocated or rented from
/// <see cref="System.Buffers.ArrayPool{T}"/>; the engine calls <see cref="BeginItem"/> before
/// invoking the formatter for each item so that the slot index resets correctly while delta state is
/// preserved across item boundaries.
/// </remarks>
public ref struct PolylineWriter {
    private Span<char> _buffer;
    private int _position;
    private readonly uint _precision;

    internal PolylineWriter(Span<char> buffer, uint precision) {
        _buffer = buffer;
        _precision = precision;
        _position = 0;
    }

    /// <summary>
    /// Emits one field value into the encoding pipeline.
    /// </summary>
    public void Write(double value, ref PolylineValueState state) {
        int normalized = PolylineEncoding.Normalize(value, _precision);
        
        int delta = normalized - state._value;

        state._value = normalized;

        if (!PolylineEncoding.TryWriteValue(delta, _buffer, ref _position)) {
            ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
        }
    }

    /// <summary>
    /// Returns the encoded polyline characters written so far.
    /// </summary>
    internal ReadOnlySpan<char> WrittenSpan => _buffer[.._position];
}
