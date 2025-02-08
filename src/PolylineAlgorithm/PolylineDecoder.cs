//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;


/// <summary>
/// Performs polyline algorithm decoding
/// </summary>
public class PolylineDecoder : IPolylineDecoder {

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<Coordinate> Decode(ref readonly Polyline polyline) {
        // Checking null and at least one character
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespace, nameof(polyline));
        }

        // Initialize local variables
        int capacity = polyline.Length / 9;
        var result = new List<Coordinate>(capacity);

        PolylineReader reader = new(in polyline);

        // Looping through encoded polyline char array
        while (reader.CanRead) {
            var coordinate = reader.Read();
            result.Add(coordinate);
        }

        return result;
    }
}