//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using System;
using System.Collections.Generic;

/// <summary>
/// Defines method to decode a polyline.
/// </summary>
public interface IPolylineDecoder {
    /// <summary>
    /// Converts an encoded polyline to a set of coordinates.
    /// </summary>
    /// <param name="polyline">An encoded polyline to decode.</param>
    /// <returns>A set of coordinates.</returns>
    IEnumerable<Coordinate> Decode(ref readonly ReadOnlySpan<char> polyline);
}