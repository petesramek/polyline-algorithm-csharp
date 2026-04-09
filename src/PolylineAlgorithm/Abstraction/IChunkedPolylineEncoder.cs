//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Extends <see cref="IPolylineEncoder{TValue, TPolyline}"/> with a per-call options overload that
/// supports chunked (stateless-continuation) encoding.
/// </summary>
/// <typeparam name="TValue">The coordinate type.</typeparam>
/// <typeparam name="TPolyline">The encoded polyline type.</typeparam>
/// <remarks>
/// Use this interface when you need to encode a large coordinate sequence in independent chunks that
/// can be concatenated into a single valid polyline. Pass
/// <see cref="PolylineEncodingOptions{TValue}.Previous"/> set to the last coordinate of the
/// preceding chunk to seed the delta baseline correctly.
/// </remarks>
public interface IChunkedPolylineEncoder<TValue, TPolyline> : IPolylineEncoder<TValue, TPolyline> {
    /// <summary>
    /// Encodes a sequence of geographic coordinates into an encoded polyline, applying the per-call
    /// <paramref name="options"/> to control the delta baseline.
    /// </summary>
    /// <param name="coordinates">The collection of coordinates to encode.</param>
    /// <param name="options">
    /// Per-call options that control the starting delta baseline. Pass
    /// <see langword="null"/> or an instance with <see cref="PolylineEncodingOptions{TValue}.Previous"/>
    /// set to <see langword="null"/> to use the formatter's default baseline.
    /// </param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded coordinates.
    /// </returns>
    /// <exception cref="System.ArgumentException">Thrown when <paramref name="coordinates"/> is empty.</exception>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    TPolyline Encode(
        ReadOnlySpan<TValue> coordinates,
        PolylineEncodingOptions<TValue>? options,
        CancellationToken cancellationToken);
}
