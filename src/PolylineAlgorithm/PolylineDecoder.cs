//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;

/// <inheritdoc cref="IPolylineDecoder" />
/// <param name="validator">A coordinate validator.</param>
public sealed class PolylineDecoder(ICoordinateValidator validator) : IPolylineDecoder {
    public ICoordinateValidator Validator { get; } = validator ?? throw new ArgumentNullException(nameof(validator));

    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="polyline"/> argument is null -or- empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="polyline"/> is not in correct format.</exception>
    public IEnumerable<(double Latitude, double Longitude)> Decode(string polyline) {
        // Checking null and at least one character
        if (polyline == null || polyline.Length == 0) {
            throw new ArgumentException(string.Empty, nameof(polyline));
        }

        // Initialize local variables
        int index = 0;
        int latitude = 0;
        int longitude = 0;

        // Looping through encoded polyline char array
        while (index < polyline.Length) {
            // Attempting to calculate next latitude value. If failed exception is thrown.
            if (!TryDecodeNext(polyline, ref index, ref latitude)) {
                throw new InvalidOperationException(string.Empty);
            }

            // Attempting to calculate next longitude value. If failed exception is thrown.
            if (!TryDecodeNext(polyline, ref index, ref longitude)) {
                throw new InvalidOperationException(string.Empty);
            }

            var coordinate = (GetCoordinate(latitude), GetCoordinate(longitude));

            // Validating decoded coordinate. If not valid exception is thrown.
            if (!Validator.IsValid(coordinate)) {
                throw new InvalidOperationException(string.Empty);
            }

            yield return coordinate;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="polyline"></param>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static bool TryDecodeNext(string polyline, ref int index, ref int value) {
        // Initialize local variables
        int chunk;
        int sum = 0;
        int shifter = 0;

        do {
            chunk = polyline[index++] - Constants.ASCII.QuestionMark;
            sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
            shifter += Constants.ShiftLength;
        } while (chunk >= Constants.ASCII.Space && index < polyline.Length);

        if (index >= polyline.Length && chunk >= Constants.ASCII.Space)
            return false;

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return true;
    }

    private static double GetCoordinate(int value) {
        return Convert.ToDouble(value) / Constants.Precision;
    }
}