//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides default values and constants used throughout the Polyline Algorithm.
/// Organizes defaults for algorithm parameters, polyline encoding, and geographic coordinates into nested static classes.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Defaults {
    /// <summary>
    /// Contains default values and constants specific to the polyline encoding logging.
    /// </summary>
    internal static class Logging {
        /// <summary>
        /// Log level multiplier used to distinguish event identification for each log level
        /// </summary>
        internal const int LogLevelMultiplier = 100;
    }
    /// <summary>
    /// Contains default values and constants specific to the polyline encoding algorithm.
    /// </summary>
    internal static class Algorithm {
        /// <summary>
        /// The precision factor used to round coordinate values during polyline encoding.
        /// </summary>
        internal const int Precision = 100_000;

        /// <summary>
        /// The number of bits to shift during polyline encoding.
        /// </summary>
        internal const byte ShiftLength = 5;

        /// <summary>
        /// The ASCII value for the question mark character ('?').
        /// </summary>
        internal const byte QuestionMark = 63;

        /// <summary>
        /// The ASCII value for the space character (' ').
        /// </summary>
        internal const byte Space = 32;

        /// <summary>
        /// The ASCII value for the unit separator character.
        /// </summary>
        internal const byte UnitSeparator = 31;
    }

    internal static class Coordinate {
        /// <summary>
        /// Provides constants representing latitude values, including the default, minimum, and maximum valid values.
        /// </summary>
        internal static class Latitude {
            /// <summary>
            /// The default value for latitude, representing the equator.
            /// </summary>
            internal const double Default = 0.00000;
            /// <summary>
            /// The minimum valid latitude value.
            /// </summary>
            internal const double Min = -90.00000;
            /// <summary>
            /// The maximum valid latitude value.
            /// </summary>
            internal const double Max = 90.00000;

            /// <summary>
            /// Contains constants related to normalized latitude values.
            /// </summary>
            [SuppressMessage("Critical Code Smell", "S3218:Inner class members should not shadow outer class \"static\" or type members", Justification = "Internal use only.")]
            internal static class Normalized {
                /// <summary>
                /// The minimum normalized latitude value.
                /// </summary>
                internal const int Min = (int)(Latitude.Min * Algorithm.Precision);
                /// <summary>
                /// The maximum normalized latitude value.
                /// </summary>
                internal const int Max = (int)(Latitude.Max * Algorithm.Precision);
            }
        }

        /// <summary>
        /// Provides constants representing longitude values, including the default, minimum, and maximum valid values.
        /// </summary>
        internal static class Longitude {
            /// <summary>
            /// The default value for longitude, representing the equator.
            /// </summary>
            internal const double Default = 0.00000;
            /// <summary>
            /// The minimum valid longitude value.
            /// </summary>
            internal const double Min = -180.00000;
            /// <summary>
            /// The maximum valid longitude value.
            /// </summary>
            internal const double Max = 180.00000;

            /// <summary>
            /// Contains constants related to normalized longitude values.
            /// </summary>
            [SuppressMessage("Critical Code Smell", "S3218:Inner class members should not shadow outer class \"static\" or type members", Justification = "Internal use only.")]
            internal static class Normalized {
                /// <summary>
                /// The minimum normalized latitude value.
                /// </summary>
                internal const int Min = (int)(Longitude.Min * Algorithm.Precision);
                /// <summary>
                /// The maximum normalized latitude value.
                /// </summary>
                internal const int Max = (int)(Longitude.Max * Algorithm.Precision);
            }
        }
    }

    /// <summary>
    /// Contains default values and constants related to polyline.
    /// </summary>
    internal static class Polyline {
        /// <summary>
        /// Contains constants related to the polyline blocks.
        /// </summary>
        internal static class Block {
            /// <summary>
            /// Contains constants related to the length of encoded coordinates in polyline encoding.
            /// </summary>
            internal static class Length {
                /// <summary>
                /// The minimum number of characters required to represent an encoded coordinate.
                /// </summary>
                internal const int Min = 2;

                /// <summary>
                /// The maximum number of characters allowed to represent an encoded coordinate.
                /// </summary>
                internal const int Max = 12;
            }
        }
    }
}
