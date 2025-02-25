//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System.Buffers;
using System.Runtime.InteropServices;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder : IPolylineDecoder {

    private static readonly int _size = Marshal.SizeOf<Coordinate>();
    private IMemoryOwner<Coordinate>? _pool;

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<Coordinate> Decode(ref readonly Polyline polyline) {
        // Checking null and at least one character
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        // Initialize local variables
        int capacity = polyline.Length / Defaults.Polyline.MinEncodedCoordinateLength;
        Span<Coordinate> buffer = _size * capacity <= 512_000 ? stackalloc Coordinate[capacity] : RentMemory(capacity);

        PolylineReader reader = new(in polyline);
        int index = 0;

        // Looping through encoded polyline char array
        while (reader.CanRead) {
            var coordinate = reader.Read();
            
            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            buffer[index] = coordinate;
            index++;
        }

        var result = buffer[..index].ToArray();

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