//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a contract for zero-allocation encoding of geographic coordinates written directly to a
/// <see cref="PipeWriter"/>.
/// </summary>
/// <remarks>
/// Implementations write encoded polyline bytes directly into pipe buffers, avoiding intermediate string or
/// character-array allocations and making this interface well-suited for high-throughput or memory-constrained
/// scenarios where the encoded result must be streamed over a network or to another
/// <see cref="System.IO.Pipelines.IDuplexPipe"/> consumer.
/// </remarks>
public interface IPolylinePipeEncoder<TCoordinate> {
    /// <summary>
    /// Asynchronously encodes a sequence of geographic coordinates and writes the encoded polyline bytes directly
    /// into <paramref name="writer"/>, operating with zero intermediate allocations.
    /// </summary>
    /// <param name="coordinates">
    /// The asynchronous collection of <typeparamref name="TCoordinate"/> instances to encode.
    /// </param>
    /// <param name="writer">
    /// The <see cref="PipeWriter"/> to which the encoded polyline bytes are written.
    /// The writer is flushed but not completed by this method; the caller is responsible for its lifetime.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask"/> that represents the asynchronous encode-and-write operation.
    /// </returns>
    ValueTask EncodeAsync(IAsyncEnumerable<TCoordinate> coordinates, PipeWriter writer, CancellationToken cancellationToken);
}
