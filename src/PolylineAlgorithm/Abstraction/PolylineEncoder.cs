//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Internal;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods to encode a set of coordinates into a polyline string.
/// </summary>\
public abstract class PolylineEncoder<TCoordinate> : IPolylineEncoder<TCoordinate> {
    private const int MaxByteSize = 64_000;
    private const int MaxChars = MaxByteSize / sizeof(char);
    private const int MaxCount = MaxChars / Defaults.Polyline.MaxEncodedCoordinateLength;

    /// <summary>
    /// Encodes a set of coordinates into a polyline string.
    /// </summary>
    /// <param name="coordinates">The coordinates to encode.</param>
    /// <returns>A <see cref="Polyline"/> representing the encoded coordinates.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="coordinates"/> argument is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="coordinates"/> argument is an empty enumeration.</exception>
    public Polyline Encode(IEnumerable<TCoordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            return default;
        }

        CoordinateVariance variance = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxLength(count);
        bool isMultiSegment = count == -1 || count > MaxCount;
        Polyline.PolylineBuilder builder = new();
        Span<char> buffer = stackalloc char[length];

        using var enumerator = coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            variance
                .Next(Normalize(Deconstruct(enumerator.Current)));

            if (isMultiSegment
                && buffer.Length - position < 12) {
                builder
                    .Append(buffer[..position].ToString().AsMemory());

                position = 0;
            }

            if (!PolylineEncoding.Default.TryWriteValue(variance.Latitude, ref buffer, ref position)
                || !PolylineEncoding.Default.TryWriteValue(variance.Longitude, ref buffer, ref position)
            ) {
                throw new InvalidOperationException();
            }

            consumed++;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static (int Latitude, int Longitude) Normalize((double Latitude, double Longitude) coordinate) {
                return (PolylineEncoding.Default.Normalize(coordinate.Latitude), PolylineEncoding.Default.Normalize(coordinate.Longitude));
            }
        }

#pragma warning disable CA1508 // Avoid dead conditional code
        if (consumed == 0) {
            return default;
        }
#pragma warning restore CA1508 // Avoid dead conditional code

        builder
            .Append(buffer[..position].ToString().AsMemory());

        return builder.Build();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetMaxLength(int count) => count switch {
            1 => 12,
            > 1 and < MaxCount => count * Defaults.Polyline.MaxEncodedCoordinateLength,
            _ => MaxChars
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract (double Latitude, double Longitude) Deconstruct(TCoordinate source);

    /// <summary>
    /// Gets the count of coordinates in the enumerable.
    /// </summary>
    /// <param name="coordinates">The enumerable of coordinates.</param>
    /// <returns>The count of coordinates.</returns>
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static int GetCount(IEnumerable<TCoordinate> coordinates) => coordinates switch {
        ICollection collection => collection.Count,
        //IEnumerable<TCoordinate> enumerable => enumerable.Count(),
        _ => -1,
    };
}