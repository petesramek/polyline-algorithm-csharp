//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;

/// <summary>
/// Defines a contract for zero-allocation decoding of an encoded polyline streamed from a <see cref="PipeReader"/>
/// into a sequence of geographic coordinates.
/// </summary>
/// <remarks>
/// Implementations operate directly on pipe buffers to avoid intermediate string or character-array allocations,
/// making this interface well-suited for high-throughput or memory-constrained scenarios where the encoded
/// polyline arrives over a network stream or another <see cref="System.IO.Pipelines.IDuplexPipe"/> source.
/// </remarks>
public interface IPolylinePipeDecoder<TCoordinate> {
    /// <summary>
    /// Asynchronously decodes encoded polyline bytes read from <paramref name="reader"/> into a sequence of
    /// geographic coordinates, operating with zero intermediate allocations.
    /// </summary>
    /// <param name="reader">
    /// The <see cref="PipeReader"/> from which the encoded polyline bytes are consumed.
    /// The reader is not completed by this method; the caller is responsible for its lifetime.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded
    /// latitude and longitude pairs, streamed asynchronously as they become available from the pipe.
    /// </returns>
    IAsyncEnumerable<TCoordinate> DecodeAsync(PipeReader reader, CancellationToken cancellationToken);
}
