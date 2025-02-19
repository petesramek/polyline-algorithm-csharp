//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Validation;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Defines default values
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Defaults {
    public static class Algorithm {
        /// <summary>
        /// Defines the coordinate precision
        /// </summary>
        public const double Precision = 1E5;

        /// <summary>
        /// Defines the shift length
        /// </summary>
        public const byte ShiftLength = 5;

        /// <summary>
        /// Defines the ASCII Question Mark
        /// </summary>
        public const byte QuestionMark = 63;

        /// <summary>
        /// Defines the ASCII Space
        /// </summary>
        public const byte Space = 32;

        /// <summary>
        /// Defines the ASCII Unit Separator
        /// </summary>
        public const byte UnitSeparator = 31;
    }

    /// <summary>
    /// Defines coordinates default values
    /// </summary>
    public static class Coordinate {
        /// <summary>
        /// Defines latitude default values
        /// </summary>
        public static class Latitude {
            /// <summary>
            /// Defines the maximum value for latitude
            /// </summary>
            public const sbyte Min = -Max;

            /// <summary>
            /// Defines the maximum value for latitude
            /// </summary>
            public const byte Max = 90;
        }

        /// <summary>
        /// Defines longitude default values
        /// </summary>
        public static class Longitude {
            /// <summary>
            /// Defines the maximum value for longitude
            /// </summary>
            public const short Min = -Max;

            /// <summary>
            /// Defines the maximum value for longitude
            /// </summary>
            public const byte Max = 180;
        }

        /// <summary>
        /// Defines default ranges for latitude and longitude
        /// </summary>
        public static class Range {
            /// <summary>
            /// Defines latitude validation range
            /// </summary>
            public static readonly CoordinateRange Latitude = new(Coordinate.Latitude.Min, Coordinate.Latitude.Max);

            /// <summary>
            /// Defines longitude validation range
            /// </summary>
            public static readonly CoordinateRange Longitude = new(Coordinate.Longitude.Min, Coordinate.Longitude.Max);
        }
    }
}
