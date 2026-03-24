//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides extension methods for the <see cref="IAsyncPolylineEncoder{TCoordinate, TPolyline}"/> interface.
/// </summary>
public static class AsyncPolylineEncoderExtensions {
    /// <summary>
    /// Asynchronously encodes a collection of <see cref="Coordinate"/> instances into an encoded polyline by
    /// wrapping the synchronous collection as an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <param name="encoder">
    /// The <see cref="IAsyncPolylineEncoder{TCoordinate, TPolyline}"/> instance used to perform the encoding.
    /// </param>
    /// <param name="coordinates">
    /// The collection of <see cref="Coordinate"/> objects to encode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while encoding.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> containing the encoded <see cref="Polyline"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoder"/> or <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    public static ValueTask<Polyline> EncodeAsync(
        this IAsyncPolylineEncoder<Coordinate, Polyline> encoder,
        IEnumerable<Coordinate> coordinates,
        CancellationToken cancellationToken) {

        if (encoder is null) {
            throw new ArgumentNullException(nameof(encoder));
        }

        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        return encoder.EncodeAsync(ToAsyncEnumerable(coordinates, cancellationToken), cancellationToken);
    }

    private static async IAsyncEnumerable<Coordinate> ToAsyncEnumerable(
        IEnumerable<Coordinate> source,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken) {

        foreach (Coordinate item in source) {
            cancellationToken.ThrowIfCancellationRequested();
            yield return item;
        }

        await System.Threading.Tasks.Task.CompletedTask.ConfigureAwait(false);
    }
}
