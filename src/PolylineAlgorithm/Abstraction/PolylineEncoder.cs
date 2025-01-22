//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Performs polyline algorithm decoding and encoding
/// </summary>
public abstract class PolylineEncoder<TCoordinate>() : IPolylineEncoder<TCoordinate> {
    //public ICoordinateValidator Validator { get; } = validator ?? throw new ArgumentNullException(nameof(validator));

    /// <summary>
    /// Encodes coordinates to polyline representation
    /// </summary>
    /// <param name="coordinates">Coordinates to encode</param>
    /// <returns>Polyline encoded representation</returns>
    /// <exception cref="ArgumentNullException">If coordinates parameter is null</exception>
    /// <exception cref="ArgumentException">If coordinates parameter is empty</exception>
    /// <exception cref="AggregateException">If one or more coordinate is out of valid range</exception>
    public Polyline Encode(IEnumerable<TCoordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        Span<TCoordinate> temp = coordinates.ToArray();

        if (temp.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerable, nameof(coordinates));
        }

        // Initializing local variables
        int index = 0;
        Memory<char> buffer = new char[GetSafeBufferSize(temp.Length)];
        int initialLatitude = 0;
        int initialLongitude = 0;

        // Looping over coordinates and building encoded result
        foreach (var coordinate in temp) {
            int latitude = Round(GetLatitude(in coordinate));
            int longitude = Round(GetLongitude(in coordinate));

            WriteNext(ref buffer, ref index, ref latitude, ref initialLatitude);
            WriteNext(ref buffer, ref index, ref longitude, ref initialLongitude);
        }

        return new Polyline(buffer[..index]);
    }

    // Each coordinate consist of two values, each one is 4 or 5 chars long
    // We use 12 = [2 coordinate values * 6 characters per each] to create safe buffer size
    static int GetSafeBufferSize(int count) => count * 12;

    protected abstract double GetLatitude(ref readonly TCoordinate coordinate);

    protected abstract double GetLongitude(ref readonly TCoordinate coordinate);

    static int Round(double value) {
        return (int)Math.Round(value * Constants.Precision);
    }

    static void WriteNext(ref Memory<char> buffer, ref int index, ref int current, ref int previous) {
        int value = current - previous;
        int shifted = value << 1;

        if (value < 0) {
            shifted = ~shifted;
        }

        int rem = shifted;

        while (rem >= Constants.ASCII.Space) {
            buffer.Span[index] = (char)((Constants.ASCII.Space | rem & Constants.ASCII.UnitSeparator) + Constants.ASCII.QuestionMark);
            rem >>= Constants.ShiftLength;
            index++;
        }

        buffer.Span[index] = (char)(rem + Constants.ASCII.QuestionMark);

        index++;

        previous = current;
    }
}
