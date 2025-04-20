//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;


/// <summary>
/// Performs decoding of encoded polyline strings into a sequence of geographic coordinates.
/// </summary>
public class PolylineDecoder : IPolylineDecoder {
    /// <inheritdoc />
    /// <summary>
    /// Decodes an encoded polyline into a sequence of <see cref="Coordinate"/> objects.
    /// </summary>
    /// <param name="polyline">The encoded polyline to decode.</param>
    /// <returns>An enumerable sequence of <see cref="Coordinate"/> objects representing the decoded polyline.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in the correct format.</exception>
    public IEnumerable<Coordinate> Decode(Polyline polyline) {
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int consumed = 0;
        int latitude = 0;
        int longitude = 0;

        ReadOnlySequence<char>.Enumerator enumerator = polyline.GetEnumerator();

        int position;
        ReadOnlyMemory<char> sequence = enumerator.Current;

        while (enumerator.MoveNext()) {
            position = 0;
            sequence = enumerator.Current;

            while (PolylineEncoding.Default.TryReadValue(ref latitude, ref sequence, ref position)
                && PolylineEncoding.Default.TryReadValue(ref longitude, ref sequence, ref position)
            ) {
                yield return new(PolylineEncoding.Default.Denormalize(latitude), PolylineEncoding.Default.Denormalize(longitude));
            }

            consumed += position;

            if (sequence.Length != position) {
                InvalidPolylineException.Throw(consumed);
            }
        }
    }
}