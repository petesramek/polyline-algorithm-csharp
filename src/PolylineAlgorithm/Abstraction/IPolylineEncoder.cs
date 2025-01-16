//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;

/// <summary>
/// Converts a set of latitude and longitude coordinates into an encoded polyline string.
/// </summary>
public interface IPolylineEncoder {
    /// <summary>
    /// Encodes a set of value tuples representing latitude and longitude coordinates into an encoded polyline string.
    /// </summary>
    /// <param name="source">A set of value tuples representing latitude and longitude coordinates.</param>
    /// <returns>An encoded polyline string representing a set of latitude and longitude coordinates.</returns>
    string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates);
}