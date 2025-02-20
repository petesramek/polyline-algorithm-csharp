//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System.Collections.Generic;

/// <summary>
/// Defines a method to encode a set of coordinates into an encoded polyline.
/// </summary>
public interface IPolylineEncoder {
    /// <summary>
    /// Converts a set of coordinates to an encoded polyline.
    /// </summary>
    /// <param name="coordinates">A set of coordinates to encode.</param>
    /// <returns>An encoded polyline representing the set of coordinates.</returns>
    Polyline Encode(IEnumerable<Coordinate> coordinates);
}