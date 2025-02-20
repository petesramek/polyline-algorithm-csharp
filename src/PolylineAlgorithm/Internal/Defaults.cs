//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Validation;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides default values used in the Polyline Algorithm.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Defaults {
    /// <summary>
    /// Contains default values related to the algorithm.
    /// </summary>
    public static class Algorithm {
        /// <summary>
        /// The precision used for coordinates.
        /// </summary>
        public const double Precision = 1E5;

        /// <summary>
        /// The length of the shift used in the algorithm.
        /// </summary>
        public const byte ShiftLength = 5;

        /// <summary>
        /// The ASCII value for the question mark character.
        /// </summary>
        public const byte QuestionMark = 63;

        /// <summary>
        /// The ASCII value for the space character.
        /// </summary>
        public const byte Space = 32;

        /// <summary>
        /// The ASCII value for the unit separator character.
        /// </summary>
        public const byte UnitSeparator = 31;
    }

    /// <summary>
    /// Contains default values related to coordinates.
    /// </summary>
    public static class Coordinate {
        /// <summary>
        /// Contains default values related to latitude.
        /// </summary>
        public static class Latitude {
            /// <summary>
            /// The minimum value for latitude.
            /// </summary>
            public const sbyte Min = -Max;

            /// <summary>
            /// The maximum value for latitude.
            /// </summary>
            public const byte Max = 90;
        }

        /// <summary>
        /// Contains default values related to longitude.
        /// </summary>
        public static class Longitude {
            /// <summary>
            /// The minimum value for longitude.
            /// </summary>
            public const short Min = -Max;

            /// <summary>
            /// The maximum value for longitude.
            /// </summary>
            public const byte Max = 180;
        }

        /// <summary>
        /// Contains default ranges for latitude and longitude.
        /// </summary>
        public static class Range {
            /// <summary>
            /// The validation range for latitude.
            /// </summary>
            public static readonly CoordinateRange Latitude = new(Coordinate.Latitude.Min, Coordinate.Latitude.Max);

            /// <summary>
            /// The validation range for longitude.
            /// </summary>
            public static readonly CoordinateRange Longitude = new(Coordinate.Longitude.Min, Coordinate.Longitude.Max);
        }
    }
}
