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
    private uint _batchSize = 10000;

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public async IAsyncEnumerable<Polyline> EncodeAsync(IAsyncEnumerable<Coordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        ulong index = 0;
        CoordinateDifference diff = new();
        Polyline polyline = new();

        await foreach (var coordinate in coordinates.ConfigureAwait(false)) {
            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            diff.DiffNext(coordinate);

            var result = EncodingAlgorithm.EncodeNext(diff.Latitude, diff.Longitude);
            
            polyline = polyline
                .Append(Polyline.FromMemory(result));

            index++;

            if (index == _batchSize) {
                var temp = polyline;

                polyline = new();

                yield return temp;
            }
        }
    }
}