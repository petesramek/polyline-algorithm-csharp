//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Properties;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// Provides functionality to encode a collection of geographic coordinates into an encoded polyline string.
/// Implements the <see cref="IPolylineEncoder"/> interface.
/// </summary>
public abstract class PolylineEncoder<TCoordinate, TPolyline> : IPolylineEncoder<TCoordinate, TPolyline> {
    private const int MaxByteSize = 64_000;
    private const int MaxChars = MaxByteSize / sizeof(char);
    private const int MaxCount = MaxChars / Defaults.Polyline.MaxEncodedCoordinateLength;

    /// <summary>
    /// Encodes a collection of <see cref="Coordinate"/> instances into an encoded <see cref="Polyline"/> string.
    /// </summary>
    /// <param name="coordinates">
    /// The collection of <see cref="Coordinate"/> objects to encode.
    /// </param>
    /// <returns>
    /// A <see cref="Polyline"/> representing the encoded coordinates.
    /// Returns <see langword="default"/> if the input collection is empty or null.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is an empty enumeration.
    /// </exception>
    public TPolyline Encode(IEnumerable<TCoordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        CoordinateVariance variance = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxLength(count);
        bool asMultiSegment = count == -1 || count > MaxCount;
        PolylineBuilder builder = new();
        Span<char> buffer = stackalloc char[length];

        using var enumerator = coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            variance
                .Next(PolylineEncoding.Default.Normalize(GetLatitude(enumerator.Current)), PolylineEncoding.Default.Normalize(GetLongitude(enumerator.Current)));

            if (asMultiSegment
                && GetRemainingBufferSize(position, buffer.Length) < GetRequiredLength(variance)) {
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
        }

        builder
            .Append(buffer[..position].ToArray().AsMemory());

        return CreatePolyline(builder.Build());

        /// <summary>
        /// Calculates the maximum length of the encoded polyline string based on the number of coordinates.
        /// </summary>
        /// <param name="count">The number of coordinates to encode.</param>
        /// <returns>
        /// The maximum length of the encoded polyline string in characters.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetMaxLength(int count) => count switch {
            1 => 12,
            > 1 and < MaxCount => count * Defaults.Polyline.MaxEncodedCoordinateLength,
            _ => MaxChars
        };

        /// <summary>
        /// Gets the count of coordinates in the enumerable.
        /// </summary>
        /// <param name="coordinates">The enumerable of <see cref="Coordinate"/> objects.</param>
        /// <returns>
        /// The count of coordinates in the collection.
        /// Returns -1 if the collection does not implement <see cref="ICollection{T}"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetCount(IEnumerable coordinates) => coordinates switch {
            ICollection collection => collection.Count,
            _ => -1,
        };

        /// <summary>
        /// Calculates the required buffer length to encode the current coordinate variance.
        /// </summary>
        /// <param name="variance">The <see cref="CoordinateVariance"/> representing the difference between coordinates.</param>
        /// <returns>The required buffer length in characters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRequiredLength(CoordinateVariance variance) =>
            PolylineEncoding.Default.GetCharCount(variance.Latitude) + PolylineEncoding.Default.GetCharCount(variance.Longitude);

        /// <summary>
        /// Calculates the remaining buffer size available for encoding.
        /// </summary>
        /// <param name="position">The current position in the buffer.</param>
        /// <param name="length">The total length of the buffer.</param>
        /// <returns>The remaining buffer size in characters.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRemainingBufferSize(int position, int length) => length - position;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract TPolyline CreatePolyline(ReadOnlySequence<char> readOnlySequence);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLongitude(TCoordinate? current);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLatitude(TCoordinate? current);
}

