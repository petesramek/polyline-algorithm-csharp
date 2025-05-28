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
    /// Contains default values and constants related to polyline encoding.
    /// </summary>
    public static class Polyline {
        /// <summary>
        /// The minimum number of characters required to represent an encoded coordinate.
        /// </summary>
        public const int MinEncodedCoordinateLength = 2;

        /// <summary>
        /// The maximum number of characters allowed to represent an encoded coordinate.
        /// </summary>
        public const int MaxEncodedCoordinateLength = 12;
    }

    /// <summary>
    /// Contains default values and constants related to geographic coordinates.
    /// </summary>
    public static class Coordinate {
        /// <summary>
        /// Provides default values for latitude.
        /// </summary>
        public static class Latitude {
            /// <summary>
            /// The minimum valid latitude value, in degrees.
            /// </summary>
            public const sbyte Min = -Max;

            /// <summary>
            /// The maximum valid latitude value, in degrees.
            /// </summary>
            public const byte Max = 90;
        }

        /// <summary>
        /// Provides default values for longitude.
        /// </summary>
        public static class Longitude {
            /// <summary>
            /// The minimum valid longitude value, in degrees.
            /// </summary>
            public const short Min = -Max;

            /// <summary>
            /// The maximum valid longitude value, in degrees.
            /// </summary>
            public const byte Max = 180;
        }
    }
}
