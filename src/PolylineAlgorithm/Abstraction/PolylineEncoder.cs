//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static PolylineAlgorithm.Polyline;

/// <summary>
/// Provides methods to encode a set of coordinates into a polyline string.
/// </summary>\
public abstract class PolylineEncoder<TCoordinate> : IPolylineEncoder<TCoordinate> {
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
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        int size = GetMaximumLength(count);

        CoordinateVariance variance = new();

        PolylineBuilder builder = new();

        int position = 0;
        Span<char> buffer = size * sizeof(char) < 64_000 ? stackalloc char[size] : stackalloc char[64_000 / sizeof(char)];

        foreach (TCoordinate coordinate in coordinates) {
            (int Latitude, int Longitude) value = Normalize(Deconstruct(coordinate));

            variance
                .Next(value);

            if (position
                + PolylineEncoding.Default.GetCharCount(variance.Latitude)
                + PolylineEncoding.Default.GetCharCount(variance.Longitude)
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

        static int GetMaximumLength(int count) => count > 1 ? count * Defaults.Polyline.MaxEncodedCoordinateLength : int.MaxValue;

        static (int Latitude, int Longitude) Normalize((double Latitude, double Longitude) coordinate) =>
                    (PolylineEncoding.Default.Normalize(coordinate.Latitude), PolylineEncoding.Default.Normalize(coordinate.Longitude));
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
        _ => -1
    };
}