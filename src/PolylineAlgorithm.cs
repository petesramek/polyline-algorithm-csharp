//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm
{
    using DropoutCoder.PolylineAlgorithm.Validation;
    using Microsoft.Extensions.ObjectPool;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Performs polyline algorithm decoding and encoding
    /// </summary>
    public static class PolylineAlgorithm
    {
        private static readonly ObjectPool<StringBuilder> _pool = new DefaultObjectPoolProvider().CreateStringBuilderPool(5, 250);

        /// <summary>
        /// Method decodes polyline encoded representation to coordinates.
        /// </summary>
        /// <param name="polyline">Encoded polyline char array to decode</param>
        /// <returns>Returns coordinates.</returns>
        /// <exception cref="ArgumentException">If polyline argument is null -or- empty char array.</exception>
        /// <exception cref="InvalidOperationException">If polyline representation is not in correct format.</exception>
        public static IEnumerable<(double Latitude, double Longitude)> Decode(string polyline)
        {
            // Checking null and at least one character
            if (polyline == null || polyline.Length == 0)
            {
                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullOrEmpty, nameof(polyline));
            }

            // Initialize local variables
            int index = 0;
            int latitude = 0;
            int longitude = 0;

            // Looping through encoded polyline char array
            while (index < polyline.Length)
            {
                // Attempting to calculate next latitude value. If failed exception is thrown
                if (!TryCalculateNext(polyline, ref index, ref latitude))
                {
                    throw new InvalidOperationException(ExceptionMessageResource.PolylineCharArrayIsMalformed);
                }

                // Attempting to calculate next longitude value. If failed exception is thrown
                if (!TryCalculateNext(polyline, ref index, ref longitude))
                {
                    throw new InvalidOperationException(ExceptionMessageResource.PolylineCharArrayIsMalformed);
                }

                var coordinate = (GetCoordinate(latitude), GetCoordinate(longitude));

                // Validating decoded coordinate. If not valid exception is thrown
                if (!CoordinateValidator.IsValid(coordinate))
                {
                    throw new InvalidOperationException(ExceptionMessageResource.PolylineCharArrayIsMalformed);
                }

                yield return coordinate;

                #region Local functions

                bool TryCalculateNext(string polyline, ref int index, ref int value)
                {
                    // Local variable initialization
                    int chunk;
                    int sum = 0;
                    int shifter = 0;

                    do
                    {
                        chunk = polyline[index++] - Constants.ASCII.QuestionMark;
                        sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
                        shifter += Constants.ShiftLength;
                    } while (chunk >= Constants.ASCII.Space && index < polyline.Length);

                    if (index >= polyline.Length && chunk >= Constants.ASCII.Space)
                        return false;

                    value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

                    return true;
                }

                double GetCoordinate(int value)
                {
                    return Convert.ToDouble(value) / Constants.Precision;
                }

                #endregion
            }
        }

        /// <summary>
        /// Method encodes coordinates to polyline encoded representation
        /// </summary>
        /// <param name="coordinates">Coordinates to encode</param>
        /// <returns>Polyline encoded representation</returns>
        /// <exception cref="ArgumentException">If coordinates parameter is null or empty enumerable</exception>
        /// <exception cref="AggregateException">If one or more coordinate is out of range</exception>
        public static string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates)
        {
            if (coordinates == null || !coordinates.GetEnumerator().MoveNext())
            {
                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullOrEmpty, nameof(coordinates));
            }

            // Validate collection of coordinates
            if (!TryValidate(coordinates, out var exceptions))
            {
                throw new AggregateException(exceptions);
            }

            // Initializing local variables
            int previousLatitude = 0;
            int previousLongitude = 0;
            var sb = new StringBuilder(coordinates.Count() * 5);

            // Looping over coordinates and building encoded result
            foreach (var coordinate in coordinates)
            {
                int latitude = Round(coordinate.Latitude);
                int longitude = Round(coordinate.Longitude);

                sb.Append(GetSequence(latitude - previousLatitude).ToArray());
                sb.Append(GetSequence(longitude - previousLongitude).ToArray());

                previousLatitude = latitude;
                previousLongitude = longitude;
            }

            return sb.ToString();

            #region Local functions

            bool TryValidate(IEnumerable<(double Latitude, double Longitude)> collection, out ICollection<CoordinateValidationException> exceptions)
            {
                exceptions = new List<CoordinateValidationException>(collection.Count());

                foreach (var item in collection)
                {
                    if (!CoordinateValidator.IsValid(item))
                    {
                        exceptions.Add(new CoordinateValidationException(item.Latitude, item.Longitude));
                    }
                }

                return !exceptions.GetEnumerator().MoveNext();
            }

            int Round(double value)
            {
                return (int)Math.Round(value * Constants.Precision);
            }

            IEnumerable<char> GetSequence(int value)
            {
                int shifted = value << 1;
                if (value < 0)
                    shifted = ~shifted;

                int rem = shifted;

                while (rem >= Constants.ASCII.Space)
                {
                    yield return (char)((Constants.ASCII.Space | rem & Constants.ASCII.UnitSeparator) + Constants.ASCII.QuestionMark);

                    rem >>= Constants.ShiftLength;
                }

                yield return (char)(rem + Constants.ASCII.QuestionMark);
            }

            #endregion
        }
    }
}
