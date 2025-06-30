//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Internal;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides default values and constants used throughout the Polyline Algorithm.
/// Organizes defaults for algorithm parameters, polyline encoding, and geographic coordinates into nested static classes.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Defaults {
    /// <summary>
    /// Contains default values and constants specific to the polyline encoding algorithm.
    /// </summary>
    public static class Algorithm {
        /// <summary>
        /// The precision factor used to round coordinate values during polyline encoding.
        /// </summary>
        public const int Precision = 100_000;

        /// <summary>
        /// The number of bits to shift during polyline encoding.
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

    public static class Coordinate {
        /// <summary>
        /// Provides constants representing latitude values, including the default, minimum, and maximum valid values.
        /// </summary>
        /// <remarks>Latitude values are measured in degrees and represent the angular distance north or
        /// south of the equator. The <see cref="Default"/> constant represents the equator, while <see cref="Min"/> and
        /// <see cref="Max"/>  define the valid range of latitude values, corresponding to the poles.</remarks>
        public static class Latitude {
            /// <summary>
            /// The default value for latitude, representing the equator.
            /// </summary>
            public const double Default = 0.00000;
            /// <summary>
            /// The minimum valid latitude value.
            /// </summary>
            public const double Min = -90.00000;
            /// <summary>
            /// The maximum valid latitude value.
            /// </summary>
            public const double Max = 90.00000;

            public static class Normalized {
                /// <summary>
                /// The minimum normalized latitude value.
                /// </summary>
                public const int Min = (int)(Latitude.Min * Algorithm.Precision);
                /// <summary>
                /// The maximum normalized latitude value.
                /// </summary>
                public const int Max = (int)(Latitude.Max * Algorithm.Precision);
            }
        }

        public static class Longitude {
            /// <summary>
            /// The default value for longitude, representing the equator.
            /// </summary>
            public const double Default = 0.00000;
            /// <summary>
            /// The minimum valid longitude value.
            /// </summary>
            public const double Min = -180.00000;
            /// <summary>
            /// The maximum valid longitude value.
            /// </summary>
            public const double Max = 180.00000;

            public static class Normalized {
                /// <summary>
                /// The minimum normalized latitude value.
                /// </summary>
                public const int Min = (int)(Longitude.Min * Algorithm.Precision);
                /// <summary>
                /// The maximum normalized latitude value.
                /// </summary>
                public const int Max = (int)(Longitude.Max * Algorithm.Precision);
            }
        }
    }

    /// <summary>
    /// Contains default values and constants related to polyline encoding.
    /// </summary>
    public static class Polyline {
        /// <summary>
        /// An array of delimiter byte values used in polyline encoding, derived by adding the ASCII value of the question mark ('?') to a range of integers.
        /// </summary>
        public static readonly byte[] Delimiters = [.. Enumerable.Range(0, 32).Select(n => (byte)(n + Algorithm.QuestionMark))];

        /// <summary>
        /// The minimum number of characters required to represent an encoded coordinate.
        /// </summary>
        public const int MinEncodedCoordinateLength = 2;

        /// <summary>
        /// The maximum number of characters allowed to represent an encoded coordinate.
        /// </summary>
        public const int MaxEncodedCoordinateLength = 12;
    }
}
