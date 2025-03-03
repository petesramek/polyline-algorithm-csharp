//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class AsyncPolylineEncoder : IAsyncPolylineEncoder {
    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public async IAsyncEnumerable<Polyline> EncodeAsync(IAsyncEnumerable<Coordinate> coordinates, CancellationToken? cancellation = null) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        CoordinateDifference diff = new();

        await foreach (var coordinate in coordinates.ConfigureAwait(false)) {
            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            diff.DiffNext(coordinate);

            var result = EncodingAlgorithm.EncodeNext(diff.Latitude, diff.Longitude);

            yield return new Polyline(result);
        }
    }
}