//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods to encode a set of geographic coordinates into a polyline string.
/// This class implements the <see cref="IPolylineEncoder"/> interface.
/// </summary>
public class PolylineEncoder : IPolylineEncoder {
    private const int MaxByteSize = 64_000;
    private const int MaxChars = MaxByteSize / sizeof(char);
    private const int MaxCount = MaxChars / Defaults.Polyline.MaxEncodedCoordinateLength;

    /// <summary>
    /// Encodes a set of geographic coordinates into a polyline string.
    /// </summary>
    /// <param name="coordinates">The collection of <see cref="Coordinate"/> objects to encode.</param>
    /// <returns>
    /// A <see cref="Polyline"/> representing the encoded coordinates. 
    /// Returns <see langword="default"/> if the input collection is empty or null.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="coordinates"/> argument is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="coordinates"/> argument is an empty enumeration.
    /// </exception>
    public Polyline Encode(IEnumerable<Coordinate> coordinates) {
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
        bool asMultiSegment = count == -1 || count > MaxCount;
        PolylineBuilder builder = new();
        Span<char> buffer = stackalloc char[length];

        using var enumerator = coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            variance
                .Next(PolylineEncoding.Default.Normalize(enumerator.Current.Latitude), PolylineEncoding.Default.Normalize(enumerator.Current.Longitude));

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

        if (consumed == 0) {
            return default;
        }

        builder
            .Append(buffer[..position].ToString().AsMemory());

        return builder.Build();

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
        static int GetCount(IEnumerable<Coordinate> coordinates) => coordinates switch {
            ICollection<Coordinate> collection => collection.Count,
            IEnumerable<Coordinate> enumerable => enumerable.Count(),
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
}

