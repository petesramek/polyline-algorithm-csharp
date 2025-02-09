//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// Performs polyline algorithm encoding
/// </summary>
public class PolylineEncoder : IPolylineEncoder {
    /// <summary>
    /// Encodes a set of coordinates to polyline.
    /// </summary>
    /// <param name="coordinates">Coordinates to encode.</param>
    /// <returns>Polyline encoded representation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="coordinates"/> argument is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="coordinates"/> argument is empty enumeration.</exception>
    public Polyline Encode(IEnumerable<Coordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(in coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        int capacity = count * 12;
        Memory<char> buffer = new char[capacity];
        PolylineWriter writer = new(in buffer);

        foreach (var coordinate in coordinates) {
            if (!coordinate.IsValid) {
                throw new InvalidCoordinateException(coordinate);
            }

            writer.Write(in coordinate);
        }

        return writer.ToPolyline();

        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetCount(ref readonly IEnumerable<Coordinate> coordinates) => coordinates switch {
            ICollection<Coordinate> collection => collection.Count,
            _ => coordinates.Count(),
        };
    }
}
