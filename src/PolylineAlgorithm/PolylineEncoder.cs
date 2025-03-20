//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods to encode a set of coordinates into a polyline string.
/// </summary>\
public class PolylineEncoder : IPolylineEncoder {
    /// <summary>
    /// Encodes a set of coordinates into a polyline string.
    /// </summary>
    /// <param name="coordinates">The coordinates to encode.</param>
    /// <returns>A <see cref="Polyline"/> representing the encoded coordinates.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="coordinates"/> argument is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="coordinates"/> argument is an empty enumeration.</exception>
    public Polyline Encode(IEnumerable<Coordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        int size = count * Defaults.Polyline.MaxEncodedCoordinateLength * sizeof(byte);

        CoordinateVariance variance;
        PolylineCoordinate previous = Coordinate.Default;

        PolylineBuilder builder = new();

        using var buffer = new Buffer(size);

        foreach (PolylineCoordinate current in coordinates) {
            InvalidCoordinateException.ThrowIfNotValid(current);

            variance = GetVariance(previous, current);

            int requiredSize = GetRequiredSize(in variance);

            if (!buffer.HasRemaining(requiredSize)) {
                builder
                    .Append(buffer.GetData());
                buffer
                    .Reset();
            }

            EncodeVariance(variance.Latitude, buffer);
            EncodeVariance(variance.Longitude, buffer);

            previous = current;
        }

        builder
            .Append(buffer.GetData());

        return builder.Build();

        static CoordinateVariance GetVariance(PolylineCoordinate initial, PolylineCoordinate next) => initial - next;

        static int GetRequiredSize(ref readonly CoordinateVariance variance) => VarianceEncoding.Default.GetByteCount(variance.Latitude) + VarianceEncoding.Default.GetByteCount(variance.Longitude);

        static void EncodeVariance(int variance, Buffer buffer) {
            Span<byte> temp = buffer.Span;

            int consumed = VarianceEncoding.Default.Encode(variance, ref temp);

            buffer.Advance(consumed);
        }
    }



    /// <summary>
    /// Gets the count of coordinates in the enumerable.
    /// </summary>
    /// <param name="coordinates">The enumerable of coordinates.</param>
    /// <returns>The count of coordinates.</returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int GetCount(IEnumerable<Coordinate> coordinates) => coordinates switch {
        ICollection<Coordinate> collection => collection.Count,
        _ => coordinates.Count(),
    };

    private class PolylineBuilder {
        private PolylineSegment? _initial;
        private PolylineSegment? _last;

        public void Append(Memory<byte> value) {
            var next = new PolylineSegment(value);

            if (_initial is null) {
                _initial = next;
            }

            _last?.Append(next);
            _last = next;
        }

        public Polyline Build() {
            if (_initial is null) {
                return Polyline.FromSequence(ReadOnlySequence<byte>.Empty);
            }


            return Polyline.FromSequence(new(_initial, 0, _last, _last!.Memory.Length));
        }
    }

    private class Buffer : IDisposable {
        private const int MaxSize = 10;
        private static readonly ArrayPool<byte> _pool = ArrayPool<byte>.Create(MaxSize, 100);
        private byte[]? _buffer;
        private bool _disposed;
        private int _position;

        public Buffer(int size) {
            _buffer = _pool.Rent(size < MaxSize ? size : MaxSize);
        }

        public Span<byte> Span => _buffer!.AsSpan()[_position..];

        public void Advance(int length) {
            _position += length;
        }

        public bool HasRemaining(int requiredSize) {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(Buffer));
            }

            return _position + requiredSize <= _buffer!.Length;
        }

        public byte[] GetData() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(Buffer));
            }

            return [.._buffer![.._position]];
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _pool.Return(_buffer);
                }

                _buffer = null;
                _disposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal void Reset() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(Buffer));
            }

            _position = 0;
        }
    }
}