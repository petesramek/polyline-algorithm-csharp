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
public class PolylineDecoder : IPolylineDecoder {
    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<Coordinate> Decode(Polyline polyline) {
        // Checking null and at least one character
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespaceMessage, nameof(polyline));
        }

        int latitude = 0;
        int longitude = 0;
        int length = 0;
        long capacity = polyline.Length / Defaults.Polyline.MinEncodedCoordinateLength;
        SequenceReader<char> reader = new(polyline.Value);
        Span<char> buffer =
            reader.Length > Defaults.Polyline.MaxEncodedCoordinateLength
            ? stackalloc char[Defaults.Polyline.MaxEncodedCoordinateLength]
            : stackalloc char[(int)reader.Length];

        var result = new List<Coordinate>(capacity > int.MaxValue ? int.MaxValue : (int)capacity);

        while (reader.TryCopyTo(buffer)) {
            length = PolylineEncoding.Default.GetNextValue(buffer, ref latitude);
            length += PolylineEncoding.Default.GetNextValue(buffer[length..], ref longitude);
            reader.Advance(length);

            Coordinate coordinate = Coordinate.FromImprecise(latitude, longitude);

            InvalidCoordinateException.ThrowIfNotValid(coordinate);

            result.Add(coordinate);

            if (reader.Remaining < buffer.Length) {
                buffer = buffer[..(int)reader.Remaining];
            }

            if (reader.Remaining == 0) {
                break;
            }
        }

        return result;
    }
}