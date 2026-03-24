//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Defines a contract for asynchronously decoding an encoded polyline into a sequence of geographic coordinates.
/// </summary>
public interface IAsyncPolylineDecoder<TPolyline, TCoordinate> {
    /// <summary>
    /// Asynchronously decodes the specified encoded polyline into a sequence of geographic coordinates.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline string to decode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded
    /// latitude and longitude pairs, streamed asynchronously.
    /// </returns>
    IAsyncEnumerable<TCoordinate> DecodeAsync(TPolyline polyline, CancellationToken cancellationToken);
}
