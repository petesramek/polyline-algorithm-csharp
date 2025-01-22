//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Internal;

/// <inheritdoc cref="IPolylineDecoder{TResult}" />
/// <param name="validator">A coordinate validator.</param>
public abstract class PolylineDecoder<TCoordinate> : IPolylineDecoder<TCoordinate> {

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public Span<TCoordinate> Decode(Polyline polyline) {
        // Checking null and at least one character
        if (polyline.IsEmpty) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullEmptyOrWhitespace, nameof(polyline));
        }

        // Initialize local variables
        int position = 0;
        int roundedLatitude = 0;
        int roundedLongitude = 0;
        int count = (int)Math.Ceiling(polyline.Length / 8d);
        Span<TCoordinate> coordinates = new TCoordinate[count];
        int index = 0;

        // Looping through encoded polyline char array
        while (position < polyline.Length) {
            // Attempting to calculate next latitude value. If failed exception is thrown.
            if (!TryDecodeNext(ref polyline, ref position, ref roundedLatitude)) {
                throw new InvalidOperationException(ExceptionMessageResource.PolylineStringIsMalformed);
            }

            // Attempting to calculate next longitude value. If failed exception is thrown.
            if (!TryDecodeNext(ref polyline, ref position, ref roundedLongitude)) {
                throw new InvalidOperationException(ExceptionMessageResource.PolylineStringIsMalformed);
            }

            double latitude = Precise(ref roundedLatitude);
            double longitude = Precise(ref roundedLongitude);

            coordinates[index] = CreateCoordinate(in latitude, in longitude);
            index++;
        }

        return coordinates[..index];
    }

    static bool TryDecodeNext(ref Polyline polyline, ref int position, ref int value) {
        // Initialize local variables
        int chunk;
        int sum = 0;
        int shifter = 0;

        do {
            chunk = polyline[position++] - Constants.ASCII.QuestionMark;
            sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
            shifter += Constants.ShiftLength;
        } while (chunk >= Constants.ASCII.Space && position < polyline.Length);

        if (position >= polyline.Length && chunk >= Constants.ASCII.Space)
            return false;

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return true;
    }

    private static double Precise(ref int value) {
        return Convert.ToDouble(value) / Constants.Precision;
    }

    /// <summary>
    /// Creates an instance of <seealso cref="TCoordinate">
    /// </summary>
    /// <param name="latitude">A latitude value</param>
    /// <param name="longitude">A longitude value</param>
    /// <returns>An instance of <seealso cref="TCoordinate"></returns>
    protected abstract TCoordinate CreateCoordinate(ref readonly double latitude, ref readonly double longitude);
}