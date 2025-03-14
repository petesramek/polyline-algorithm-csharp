//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System.Buffers;
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
        int position = 0;
        long capacity = polyline.Length / Defaults.Polyline.MinEncodedCoordinateLength;
        ReadOnlySpan<char> source = polyline.AsMemory().Span;
        Span<Coordinate> buffer = _size * capacity <= 512_000 ? stackalloc Coordinate[(int)capacity] : RentMemory((int)capacity);

        while (true) {
            if (position >= source.Length) {
                break;
            }

            try {
                position += PolylineEncoding.Default.GetNextValue(source[position..], ref latitude);
                position += PolylineEncoding.Default.GetNextValue(source[position..], ref longitude);

                Coordinate coordinate = Coordinate.FromImprecise(latitude, longitude);

                InvalidCoordinateException.ThrowIfNotValid(coordinate);

                buffer[index++] = coordinate;
            } catch (IndexOutOfRangeException ex) {
                throw;
            }
        }

        IEnumerable<Coordinate> result = buffer[..index].ToArray();

        ReturnMemory();

        return result;
    }

    private Span<Coordinate> RentMemory(int capacity) {
        _pool = MemoryPool<Coordinate>.Shared.Rent(capacity);

        return _pool.Memory.Span;
    }

    private void ReturnMemory() {
        _pool?.Dispose();
    }
}