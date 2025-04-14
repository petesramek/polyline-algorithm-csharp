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
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder : IPolylineDecoder {
    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
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
