//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Validation;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides default values and constants used throughout the Polyline Algorithm.
/// This class contains nested static classes for organizing defaults related to the algorithm,
/// polyline encoding, and geographic coordinates.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Defaults {
    /// <summary>
    /// Contains default values and constants related to the polyline encoding algorithm.
    /// </summary>
    public static class Algorithm {
        /// <summary>
        /// The coordinate rounding precision used in the polyline encoding algorithm.
        /// </summary>
        public const int Precision = 100_000;

        /// <summary>
        /// The bit shift length used in the polyline encoding algorithm.
        /// </summary>
        public const byte ShiftLength = 5;

        /// <summary>
        /// The ASCII value for the question mark character ('?').
        /// </summary>
        public const byte QuestionMark = 63;

        /// <summary>
        /// The ASCII value for the space character (' ').
        /// </summary>
        public const byte Space = 32;

        /// <summary>
        /// The ASCII value for the unit separator character.
        /// </summary>
        public const byte UnitSeparator = 31;
    }

    /// <summary>
    /// Contains default values and constants related to polyline encoding.
    /// </summary>
    public static class Polyline {
        /// <summary>
        /// An array of delimiters used in the polyline encoding process.
        /// Each delimiter is derived by adding the ASCII value of the question mark ('?') to a range of integers.
        /// </summary>
        public static readonly byte[] Delimiters = [.. Enumerable.Range(0, 32).Select(n => (byte)(n + Defaults.Algorithm.QuestionMark))];

        /// <summary>
        /// The minimum length of an encoded coordinate in the polyline format.
        /// </summary>
        public const int MinEncodedCoordinateLength = 2;

        /// <summary>
        /// The maximum length of an encoded coordinate in the polyline format.
        /// </summary>
        public const int MaxEncodedCoordinateLength = 12;
    }

    /// <summary>
    /// Contains default values and constants related to geographic coordinates.
    /// </summary>
    public static class Coordinate {
        /// <summary>
        /// Contains default values related to latitude.
        /// </summary>
        public static class Latitude {
            /// <summary>
            /// The minimum valid value for latitude, in degrees.
            /// </summary>
            public const sbyte Min = -Max;

            /// <summary>
            /// The maximum valid value for latitude, in degrees.
            /// </summary>
            public const byte Max = 90;
        }

        /// <summary>
        /// Contains default values related to longitude.
        /// </summary>
        public static class Longitude {
            /// <summary>
            /// The minimum valid value for longitude, in degrees.
            /// </summary>
            public const short Min = -Max;

            /// <summary>
            /// The maximum valid value for longitude, in degrees.
            /// </summary>
            public const byte Max = 180;
        }

        /// <summary>
        /// Contains default ranges for validating latitude and longitude values.
        /// </summary>
        public static class Range {
            /// <summary>
            /// The default validation range for latitude values.
            /// </summary>
            public static readonly CoordinateRange Latitude = new(Coordinate.Latitude.Min, Coordinate.Latitude.Max);

            /// <summary>
            /// The default validation range for longitude values.
            /// </summary>
            public static readonly CoordinateRange Longitude = new(Coordinate.Longitude.Min, Coordinate.Longitude.Max);
        }
    }
}
