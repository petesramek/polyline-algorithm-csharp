//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Properties;
using System;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder<TCoordinate> : IPolylineDecoder<string, IEnumerable<Coordinate>> {
    public PolylineDecoder(ICoordinateFactory<TCoordinate> factory) {
        Factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public ICoordinateFactory<TCoordinate> Factory { get; }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<Coordinate> Decode(string polyline) {
        if (string.IsNullOrWhiteSpace(polyline)) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int position = 0;
        int latitude = 0;
        int longitude = 0;

        ReadOnlyMemory<char> buffer = polyline.AsMemory();

        while (
            PolylineEncoding.Default.TryReadValue(ref latitude, ref buffer, ref position)
            && PolylineEncoding.Default.TryReadValue(ref longitude, ref buffer, ref position)
        ) {
            yield return new(PolylineEncoding.Default.Denormalize(latitude), PolylineEncoding.Default.Denormalize(longitude));
        }
    }
}
