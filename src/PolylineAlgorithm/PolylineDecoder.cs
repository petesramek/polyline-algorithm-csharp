//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System.Buffers;
using System.Drawing;
using System.Runtime.InteropServices;
using static PolylineAlgorithm.Internal.Defaults.Coordinate;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder : IPolylineDecoder {

    private static readonly int _size = Marshal.SizeOf<Coordinate>();

    private IMemoryOwner<Coordinate>? _pool;

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<Coordinate> Decode(Polyline polyline) {
        // Checking null and at least one character
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int index = 0;
        int latitude = 0;
        int longitude = 0;
        long capacity = polyline.Length / Defaults.Polyline.MinEncodedCoordinateLength;
        SequenceReader<byte> reader = new(polyline.AsSequence());
        Span<Coordinate> coordinates = _size * capacity <= 32_000 ? stackalloc Coordinate[(int)capacity] : RentMemory((int)capacity);
        Span<byte> buffer = stackalloc byte[6];

        while (true) {
            if (reader.Remaining == 0) {
                break;
            }

            UpdateBufferSize(ref buffer, reader.Remaining);

            if (reader.TryCopyTo(buffer)) {
                int consumed = VarianceEncoding.Default.Decode(buffer, out int variance);
                reader.Advance(consumed);
                latitude += variance;
            } else {
                throw new InvalidOperationException();
            }

            UpdateBufferSize(ref buffer, reader.Remaining);

            if (reader.TryCopyTo(buffer)) {
                int consumed = VarianceEncoding.Default.Decode(buffer, out int variance);
                reader.Advance(consumed);
                longitude += variance;
            } else {
                throw new InvalidOperationException();
            }

            Coordinate coordinate = new PolylineCoordinate(latitude, longitude);

            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            coordinates[index++] = coordinate;
        }

        IEnumerable<Coordinate> result = coordinates[..index].ToArray();

        ReturnMemory();

        return result;

        static void UpdateBufferSize(ref Span<byte> buffer, long size) {
            if (buffer.Length > size) {
                buffer = buffer[..Convert.ToInt32(size)];
            }
        }
    }
    private Span<Coordinate> RentMemory(int capacity) {
        _pool = MemoryPool<Coordinate>.Shared.Rent(capacity);

        return _pool.Memory.Span;
    }

    private void ReturnMemory() {
        _pool?.Dispose();
    }
}