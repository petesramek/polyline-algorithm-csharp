//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;

/// <summary>
/// Performs polyline algorithm decoding and encoding
/// </summary>
public class PolylineEncoder {
    /// <summary>
    /// Encodes coordinates to polyline representation
    /// </summary>
    /// <param name="coordinates">Coordinates to encode</param>
    /// <returns>Polyline encoded representation</returns>
    /// <exception cref="ArgumentNullException">If coordinates parameter is null</exception>
    /// <exception cref="ArgumentException">If coordinates parameter is empty</exception>
    /// <exception cref="AggregateException">If one or more coordinate is out of valid range</exception>
    public ReadOnlySpan<char> Encode(IEnumerable<Coordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(ref coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerable, nameof(coordinates));
        }

        // Initializing local variables
        int capacity = count * 11;
        Span<char> buffer = new char[capacity];
        PolylineWriter writer = new(in buffer);

        // Looping over coordinates and building encoded result
        foreach (var coordinate in coordinates) {
            writer.Write(in coordinate);
        }

        return writer.ToString();
    }

    static int GetCount(ref IEnumerable<Coordinate> coordinates) => coordinates switch {
        ICollection<Coordinate> collection => collection.Count,
        _ => coordinates.Count(),
    };
}
