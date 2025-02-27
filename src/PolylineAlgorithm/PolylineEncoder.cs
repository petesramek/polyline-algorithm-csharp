//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Extensions;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods to encode a set of coordinates into a polyline string.
/// </summary>\
public class PolylineEncoder : IPolylineEncoder {
    private static readonly int _size = sizeof(char);
    private IMemoryOwner<char>? _pool;

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

        int capacity = count * Defaults.Polyline.MaxEncodedCoordinateLength;
        Span<char> buffer = _size * capacity <= 512_000 ? stackalloc char[capacity] : RentMemory(capacity);
        CoordinateDifference diff = new();
        int index = 0;

        foreach (var coordinate in coordinates) {
            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            diff.DiffNext(coordinate);

            var next = EncodingAlgorithm.EncodeNext(diff.Latitude, diff.Longitude);

            index = buffer
                 .Write(in next, ref index);
        }

        return new Polyline(buffer[..index].ToArray());
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


    private Span<char> RentMemory(int capacity) {
        _pool = MemoryPool<char>.Shared.Rent(capacity);

        return _pool.Memory.Span;
    }

    private void ReturnMemory() {
        _pool?.Dispose();
    }
}