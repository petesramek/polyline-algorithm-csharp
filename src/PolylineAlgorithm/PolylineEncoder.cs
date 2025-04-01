//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static PolylineAlgorithm.Polyline;

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
    [MethodImpl(MethodImplOptions.NoInlining)]
    public Polyline Encode(IEnumerable<Coordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        int size = GetMaximumLength(count);

        CoordinateVariance variance = new();

        PolylineBuilder builder = new();

        int position = 0;
        Span<char> buffer = size * sizeof(char) < 64_000 ? stackalloc char[size] : stackalloc char[64_000 / sizeof(char)];

        foreach (Coordinate coordinate in coordinates) {
            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            (int Latitude, int Longitude) next = GetNormalizedTuple(coordinate);

            variance
                .Next(next);

            if ((position
                + PolylineEncoding.Default.GetRequiredCharCount(variance.Latitude)
                + PolylineEncoding.Default.GetRequiredCharCount(variance.Longitude))
                > buffer.Length) {

                builder
                    .Append(buffer[..position].ToString().AsMemory());

                position = 0;
            }

            if (!PolylineEncoding.Default.TryWriteValue(variance.Latitude, ref buffer, ref position)
                || !PolylineEncoding.Default.TryWriteValue(variance.Longitude, ref buffer, ref position)
            ) {
                throw new InvalidOperationException();
            }
        }

        builder
            .Append(buffer[..position].ToString().AsMemory());

        return builder.Build();

        static int GetMaximumLength(int count) => count * Defaults.Polyline.MaxEncodedCoordinateLength;

        static (int Latitude, int Longitude) GetNormalizedTuple(Coordinate coordinate) =>
                    (PolylineEncoding.Default.Normalize(coordinate.Latitude), PolylineEncoding.Default.Normalize(coordinate.Longitude));
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
        _ => coordinates.Count()
    };
}