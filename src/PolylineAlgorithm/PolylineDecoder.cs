//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Properties;
using System;
using System.Runtime.CompilerServices;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public abstract class PolylineDecoder<TCoordinate> : IPolylineDecoder<TCoordinate> {
    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<TCoordinate> Decode(Polyline polyline) {
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int consumed = 0;

        int latitude = 0;
        int longitude = 0;

        var enumerator = polyline.GetEnumerator();

        while (enumerator.MoveNext()) {
            int position = 0;
            ReadOnlyMemory<char> buffer = enumerator.Current;

            while (PolylineEncoding.Default.TryReadValue(ref latitude, ref buffer, ref position)
                && PolylineEncoding.Default.TryReadValue(ref longitude, ref buffer, ref position)
            ) {
                yield return Construct(PolylineEncoding.Default.Denormalize(latitude), PolylineEncoding.Default.Denormalize(longitude));
            }

            consumed += position;

            if (buffer.Length != position) {
                InvalidPolylineException.Throw(consumed);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public abstract TCoordinate Construct(double latitude, double longitude);
}
