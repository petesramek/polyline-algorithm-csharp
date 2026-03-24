//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
/// Provides extension methods for the <see cref="IAsyncPolylineDecoder{TPolyline, TCoordinate}"/> interface and
/// for adapting synchronous decoder types to support asynchronous decoding from common input representations.
/// </summary>
public static class AsyncPolylineDecoderExtensions {
    /// <summary>
    /// Asynchronously decodes an encoded polyline string into a sequence of geographic coordinates.
    /// </summary>
    /// <param name="decoder">
    /// The <see cref="IAsyncPolylineDecoder{TPolyline, TCoordinate}"/> instance used to perform the decoding.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline string to decode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while iterating.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <see cref="Coordinate"/> representing the decoded pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>
    public static IAsyncEnumerable<Coordinate> DecodeAsync(
        this IAsyncPolylineDecoder<Polyline, Coordinate> decoder,
        string polyline,
        CancellationToken cancellationToken) {

        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.DecodeAsync(Polyline.FromString(polyline), cancellationToken);
    }

    /// <summary>
    /// Asynchronously decodes an encoded polyline represented as a character array into a sequence of geographic
    /// coordinates.
    /// </summary>
    /// <param name="decoder">
    /// The <see cref="IAsyncPolylineDecoder{TPolyline, TCoordinate}"/> instance used to perform the decoding.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline as a character array to decode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while iterating.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <see cref="Coordinate"/> representing the decoded pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>
    public static IAsyncEnumerable<Coordinate> DecodeAsync(
        this IAsyncPolylineDecoder<Polyline, Coordinate> decoder,
        char[] polyline,
        CancellationToken cancellationToken) {

        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.DecodeAsync(Polyline.FromCharArray(polyline), cancellationToken);
    }

    /// <summary>
    /// Asynchronously decodes an encoded polyline represented as a read-only memory of characters into a sequence
    /// of geographic coordinates.
    /// </summary>
    /// <param name="decoder">
    /// The <see cref="IAsyncPolylineDecoder{TPolyline, TCoordinate}"/> instance used to perform the decoding.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline as a read-only memory of characters to decode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while iterating.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <see cref="Coordinate"/> representing the decoded pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>
    public static IAsyncEnumerable<Coordinate> DecodeAsync(
        this IAsyncPolylineDecoder<Polyline, Coordinate> decoder,
        ReadOnlyMemory<char> polyline,
        CancellationToken cancellationToken) {

        if (decoder is null) {
            throw new ArgumentNullException(nameof(decoder));
        }

        return decoder.DecodeAsync(Polyline.FromMemory(polyline), cancellationToken);
    }
}
