//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Provides per-call options-based chunked (stateless-continuation) decoding without inheriting the
/// covariant <see cref="IPolylineDecoder{TPolyline, TValue}"/>. Implement both interfaces on the
/// concrete decoder class to support both the standard and chunked overloads.
/// </summary>
/// <typeparam name="TPolyline">The encoded polyline type.</typeparam>
/// <typeparam name="TValue">The coordinate type.</typeparam>
/// <remarks>
/// Use this interface when you need to decode a polyline that was produced by chunked encoding.
/// Pass <see cref="PolylineDecodingOptions{TValue}.Previous"/> set to the last coordinate of the
/// preceding decoded chunk to seed the accumulated-delta state correctly.
/// </remarks>
public interface IChunkedPolylineDecoder<TPolyline, TValue> {
    /// <summary>
    /// Decodes an encoded <typeparamref name="TPolyline"/> into a sequence of coordinates, applying
    /// the per-call <paramref name="options"/> to control the accumulated-delta seed.
    /// </summary>
    /// <param name="polyline">The encoded polyline to decode. Must not be <see langword="null"/>.</param>
    /// <param name="options">
    /// Per-call options that control the starting accumulated-delta seed. Pass
    /// <see langword="null"/> or an instance with <see cref="PolylineDecodingOptions{TValue}.Previous"/>
    /// set to <see langword="null"/> to start from zero (the default behaviour).
    /// </param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TValue"/> representing the decoded
    /// coordinates.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline format is invalid or malformed.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    IEnumerable<TValue> Decode(
        TPolyline polyline,
        PolylineDecodingOptions<TValue>? options,
        CancellationToken cancellationToken);
}
