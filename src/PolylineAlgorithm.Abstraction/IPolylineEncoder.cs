//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;

/// <summary>
/// Defines a contract for encoding a collection of geographic coordinates into an encoded polyline string.
/// </summary>
public interface IPolylineEncoder<TCoordinate, TPolyline> {
    /// <summary>
    /// Encodes a sequence of geographic coordinates into an encoded polyline representation.
    /// </summary>
    /// <param name="coordinates">
    /// The collection of <see cref="Coordinate"/> instances to encode into a polyline.
    /// </param>
    /// <returns>
    /// A <see cref="Polyline"/> containing the encoded polyline string that represents the input coordinates.
    /// </returns>
    TPolyline Encode(IEnumerable<TCoordinate> coordinates);
}
