//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System.Collections.Generic;

/// <summary>
/// Defines method to encode a set of coordinates.
/// </summary>
public interface IPolylineEncoder
{
    /// <summary>
    /// Converts a set of coordinates to an encoded polyline.
    /// </summary>
    /// <param name="coordinates">A set of coordinates to encode.</param>
    /// <returns>An encoded polyline.</returns>
    Polyline Encode(IEnumerable<Coordinate> coordinates);
}