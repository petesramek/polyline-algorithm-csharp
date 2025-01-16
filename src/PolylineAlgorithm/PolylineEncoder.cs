//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Performs polyline algorithm decoding and encoding
/// </summary>
public sealed class PolylineEncoder(ICoordinateValidator validator) : IPolylineEncoder {
    public ICoordinateValidator Validator { get; } = validator ?? throw new ArgumentNullException(nameof(validator));

    /// <summary>
    /// Encodes coordinates to polyline representation
    /// </summary>
    /// <param name="coordinates">Coordinates to encode</param>
    /// <returns>Polyline encoded representation</returns>
    /// <exception cref="ArgumentNullException">If coordinates parameter is null</exception>
    /// <exception cref="ArgumentException">If coordinates parameter is empty</exception>
    /// <exception cref="AggregateException">If one or more coordinate is out of valid range</exception>
    public string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        if (!coordinates.GetEnumerator().MoveNext()) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerable, nameof(coordinates));
        }

        int count = coordinates.Count();
        ICollection<CoordinateValidationException> exceptions = new List<CoordinateValidationException>(count);

        // Validate collection of coordinates
        if (!TryValidate(coordinates, ref exceptions)) {
            throw new AggregateException(ExceptionMessageResource.AggregateExceptionCoordinatesAreInvalidErrorMessage, exceptions);
        }

        // Initializing local variables
        int index = 0;
        Memory<char> buffer = new char[GetSafeBufferSize(count)];
        int previousLatitude = 0;
        int previousLongitude = 0;

        // Looping over coordinates and building encoded result
        foreach (var (Latitude, Longitude) in coordinates) {
            int latitude = Round(Latitude);
            int longitude = Round(Longitude);

            WriteNext(ref buffer, ref index, ref latitude, ref previousLatitude);
            WriteNext(ref buffer, ref index, ref longitude, ref previousLongitude);
        }

        return buffer[..index].ToString();

        // Each coordinate consist of two values, each one is 4 or 5 chars long
        // We use 12 = [2 * 6] to create safe buffer size
        static int GetSafeBufferSize(int count) => count * 12;
    }

    private bool TryValidate(IEnumerable<(double Latitude, double Longitude)> collection, ref ICollection<CoordinateValidationException> exceptions) {
        foreach (var item in collection) {
            if (!Validator.IsValid(item)) {
                exceptions.Add(new CoordinateValidationException(item));
            }
        }

        return exceptions.Count == 0;
    }

    private static int Round(double value) {
        return (int)Math.Round(value * Constants.Precision);
    }

    private static void WriteNext(ref Memory<char> buffer, ref int index, ref int current, ref int previous) {
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
