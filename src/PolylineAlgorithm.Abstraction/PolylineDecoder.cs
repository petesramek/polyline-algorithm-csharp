//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Properties;
using System;
using System.Buffers;

/// <summary>
/// Decodes encoded polyline strings into sequences of geographic coordinates.
/// Implements the <see cref="IPolylineDecoder"/> interface.
/// </summary>
public abstract class PolylineDecoder<TCoordinate, TPolyline> : IPolylineDecoder<TCoordinate, TPolyline> {
    /// <summary>
    /// Decodes an encoded <see cref="Polyline"/> into a sequence of <see cref="Coordinate"/> instances.
    /// </summary>
    /// <param name="polyline">
    /// The <see cref="Polyline"/> instance containing the encoded polyline string to decode.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="Coordinate"/> representing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="polyline"/> is empty.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline format is invalid or malformed at a specific position.
    /// </exception>
    public IEnumerable<TCoordinate> Decode(TPolyline polyline) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }

        ReadOnlySequence<char> source = GetReadOnlySequence(polyline);

        if (source.Length < Defaults.Polyline.MinEncodedCoordinateLength) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        ReadOnlySequence<char>.Enumerator enumerator = source.GetEnumerator();

        int consumed = 0;
        int latitude = 0;
        int longitude = 0;

        int position;
        ReadOnlyMemory<char> sequence;

        while (enumerator.MoveNext()) {
            position = 0;
            sequence = enumerator.Current;

            while (PolylineEncoding.Default.TryReadValue(ref latitude, ref sequence, ref position)
                && PolylineEncoding.Default.TryReadValue(ref longitude, ref sequence, ref position)
            ) {
                yield return CreateCoordinate(PolylineEncoding.Default.Denormalize(latitude), PolylineEncoding.Default.Denormalize(longitude));
            }

            consumed += position;

            if (sequence.Length != position) {
                InvalidPolylineException.Throw(consumed);
            }
        }
    }

    protected abstract ReadOnlySequence<char> GetReadOnlySequence(TPolyline? polyline);

    protected abstract TCoordinate CreateCoordinate(double latitude, double longitude);
}