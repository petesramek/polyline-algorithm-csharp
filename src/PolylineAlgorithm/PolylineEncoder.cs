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

        int size = count * Defaults.Polyline.MaxEncodedCoordinateLength * sizeof(char);
        //int length = size > 32_000 ? size : 32_000;

        int position = 0;
        CoordinateDifference diff = new();
        Memory<char> buffer = new char[size];
        Span<char> temp;
        //Polyline? result = null;

        foreach (var coordinate in coordinates) {
            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            diff.Next(coordinate);

            //if (index + length > buffer.Length) {
            //    result = result?.Append(buffer[..index].AsMemory()) ?? Polyline.FromCharArray(buffer[..index]);
            //    index = 0;
            //}

            temp = buffer.Span[position..6];

            position += PolylineEncoding.Default.GetChars(diff.Latitude, ref temp);

            temp = buffer.Span[position..6];

            position += PolylineEncoding.Default.GetChars(diff.Longitude, ref temp);
        }

        return /*result?.Append(buffer[..position]) ?? */ Polyline.FromMemory(buffer[..position]);
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