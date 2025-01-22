//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.ObjectModel;

/// <summary>
/// Converts an encoded polyline string into a set of latitude and longitude coordinates.
/// </summary>
public interface IPolylineDecoder<TCoordinate> {
    /// <summary>
    /// Decodes an encoded polyline string into a set of <seealso cref="TCoordinate"/>.
    /// </summary>
    /// <param name="polyline">An encoded polyline string to decode.</param>
    /// <returns>A decoded polyline.</returns>
    Span<TCoordinate> Decode(Polyline polyline);
}