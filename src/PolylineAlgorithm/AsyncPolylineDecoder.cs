//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System.Buffers;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class AsyncPolylineDecoder : IAsyncPolylineDecoder {
    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public async IAsyncEnumerable<Coordinate> DecodeAsync(Polyline polyline) {
        // Checking null and at least one character
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int latitude = 0;
        int longitude = 0;
        long position = 0;
        ReadOnlySequence<char> sequence = polyline.AsSequence();

        while (
            DecodingAlgorithm.DecodeNext(ref latitude, ref position, ref sequence)
            && DecodingAlgorithm.DecodeNext(ref longitude, ref position, ref sequence)
        ) {
            Coordinate coordinate = Coordinate.FromImprecise(latitude, longitude);

            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            yield return await new ValueTask<Coordinate>(coordinate)
                .ConfigureAwait(false);
        }
    }
}