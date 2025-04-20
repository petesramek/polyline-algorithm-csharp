//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;

/// <summary>
/// Defines a contract for encoding a collection of geographic coordinates into an encoded polyline string.
/// </summary>
public interface IPolylineEncoder {
    /// <summary>
    /// Encodes a collection of geographic coordinates into an encoded polyline string.
    /// </summary>
    /// <param name="coordinates">A collection of <see cref="Coordinate"/> objects to encode.</param>
    /// <returns>
    /// A <see cref="Polyline"/> instance representing the encoded polyline string.
    /// </returns>
    Polyline Encode(IEnumerable<Coordinate> coordinates);
}
