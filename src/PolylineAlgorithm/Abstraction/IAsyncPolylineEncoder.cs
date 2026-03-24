//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a contract for asynchronously encoding a sequence of geographic coordinates into an encoded polyline.
/// </summary>
public interface IAsyncPolylineEncoder<TCoordinate, TPolyline> {
    /// <summary>
    /// Asynchronously encodes a sequence of geographic coordinates into an encoded polyline representation.
    /// </summary>
    /// <param name="coordinates">
    /// The asynchronous collection of <typeparamref name="TCoordinate"/> instances to encode into a polyline.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> that represents the asynchronous operation, containing a
    /// <typeparamref name="TPolyline"/> with the encoded polyline string that represents the input coordinates.
    /// </returns>
    ValueTask<TPolyline> EncodeAsync(IAsyncEnumerable<TCoordinate> coordinates, CancellationToken cancellationToken);
}
