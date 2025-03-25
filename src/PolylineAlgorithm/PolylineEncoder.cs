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
using System.Runtime.CompilerServices;
using System.Text;
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
    public Polyline Encode(IEnumerable<Coordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        int size = count * Defaults.Polyline.MaxEncodedCoordinateLength;

        CoordinateVariance variance;
        Coordinate previous = Coordinate.Default;

        PolylineBuilder builder = new();

        int position = 0;
        Span<char> buffer = size < 32_000 ? new char[size] : new char[32_000];

        foreach (Coordinate current in coordinates) {
            variance = CoordinateVariance.Calculate(previous, current);

            if((position
                + PolylineEncoding.Default.GetRequiredCharCount(variance.Latitude)
                + PolylineEncoding.Default.GetRequiredCharCount(variance.Longitude))
                > buffer.Length) {
                builder
                    .Append(buffer[..position].ToArray());
                position = 0;
            }

            if (PolylineEncoding.Default.TryWriteValue(variance.Latitude, ref buffer, ref position)
                && PolylineEncoding.Default.TryWriteValue(variance.Longitude, ref buffer, ref position)
            ) {
                throw new InvalidOperationException();
            }

            previous = current;
        }

        builder
            .Append(buffer[..position].ToArray());

        return builder.Build();
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
}