//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;

/// <summary>
/// Converts an encoded polyline string into a set of latitude and longitude coordinates.
/// </summary>
public interface IPolylineDecoder {
    /// <summary>
    /// Decodes an encoded polyline string into a set of value tuples representing latitude and longitude coordinates.
    /// </summary>
    /// <param name="source">An encoded polyline string to decode.</param>
    /// <returns>A set of value tuples representing latitude and longitude coordinates.</returns>
    IEnumerable<(double Latitude, double Longitude)> Decode(string polyline);
}