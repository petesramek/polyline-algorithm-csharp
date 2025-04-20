//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;

/// <summary>
/// Defines a contract for decoding an encoded polyline into a collection of geographic coordinates.
/// </summary>
public interface IPolylineDecoder {
    /// <summary>
    /// Decodes an encoded polyline string into a collection of geographic coordinates.
    /// </summary>
    /// <param name="polyline">The <see cref="Polyline"/> instance representing the encoded polyline string.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Coordinate"/> objects representing the decoded geographic coordinates.
    /// </returns>
    IEnumerable<Coordinate> Decode(Polyline polyline);
}
