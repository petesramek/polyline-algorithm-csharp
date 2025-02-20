//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System.Collections.Generic;

/// <summary>
/// Defines a method to decode an encoded polyline into a set of coordinates.
/// </summary>
public interface IPolylineDecoder {
    /// <summary>
    /// Converts an encoded polyline to a set of coordinates.
    /// </summary>
    /// <param name="polyline">An encoded polyline to decode.</param>
    /// <returns>A set of coordinates represented by the encoded polyline.</returns>
    IEnumerable<Coordinate> Decode(ref readonly Polyline polyline);
}

