//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Gps.Extensions;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides extension methods for the <see cref="IPolylineDecoder{TPolyline, TValue}"/> interface to facilitate decoding encoded polylines.
/// </summary>
public static class PolylineDecoderExtensions {
    /// <summary>
    /// Decodes an encoded polyline string into a sequence of geographic coordinates.
    /// </summary>
    /// <param name="decoder">
    /// The <see cref="IPolylineDecoder{TPolyline, TValue}"/> instance used to perform the decoding operation.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline string to decode.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{Coordinate}"/> containing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Method ExceptionGuard.ThrowArgumentNull(string) throws ArgumentNullException.")]
    public static IEnumerable<Coordinate> Decode(this IPolylineDecoder<Polyline, Coordinate> decoder, string polyline) {
        if (decoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromString(polyline));
    }

    /// <summary>
    /// Decodes an encoded polyline represented as a character array into a sequence of geographic coordinates.
    /// </summary>
    /// <param name="decoder">
    /// The <see cref="IPolylineDecoder{TPolyline, TValue}"/> instance used to perform the decoding operation.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline as a character array to decode.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{Coordinate}"/> containing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Method ExceptionGuard.ThrowArgumentNull(string) throws ArgumentNullException.")]
    public static IEnumerable<Coordinate> Decode(this IPolylineDecoder<Polyline, Coordinate> decoder, char[] polyline) {
        if (decoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromCharArray(polyline));
    }

    /// <summary>
    /// Decodes an encoded polyline represented as a read-only memory of characters into a sequence of geographic coordinates.
    /// </summary>
    /// <param name="decoder">
    /// The <see cref="IPolylineDecoder{TPolyline, TValue}"/> instance used to perform the decoding operation.
    /// </param>
    /// <param name="polyline">
    /// The encoded polyline as a read-only memory of characters to decode.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{Coordinate}"/> containing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="decoder"/> is <see langword="null"/>.
    /// </exception>

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Method ExceptionGuard.ThrowArgumentNull(string) throws ArgumentNullException.")]
    public static IEnumerable<Coordinate> Decode(this IPolylineDecoder<Polyline, Coordinate> decoder, ReadOnlyMemory<char> polyline) {
        if (decoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(decoder));
        }

        return decoder.Decode(Polyline.FromMemory(polyline));
    }
}