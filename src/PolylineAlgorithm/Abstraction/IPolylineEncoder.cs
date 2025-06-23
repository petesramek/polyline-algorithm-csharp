//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Internal;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Defines a contract for encoding a collection of geographic coordinates into an encoded polyline string.
/// </summary>
public interface IPolylineEncoder<TCoordinate> {
    /// <summary>
    /// Encodes a set of geographic coordinates into a polyline string.
    /// </summary>
    /// <param name="coordinates">The collection of <see cref="Coordinate"/> objects to encode.</param>
    /// <returns>
    /// A <see cref="Polyline"/> representing the encoded coordinates. 
    /// Returns <see langword="default"/> if the input collection is empty or null.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="coordinates"/> argument is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="coordinates"/> argument is an empty enumeration.
    /// </exception>
    public Polyline Encode(IEnumerable<TCoordinate> coordinates);

}
