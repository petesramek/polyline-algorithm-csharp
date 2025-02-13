//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Internal {
    using PolylineAlgorithm.Validation;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
/// Defines default values
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class Defaults {
        internal static class Algorithm {
            /// <summary>
            /// Defines the coordinate precision
            /// </summary>
            internal const double Precision = 1E5;

            /// <summary>
            /// Defines the shift length
            /// </summary>
            internal const byte ShiftLength = 5;

            /// <summary>
            /// Defines the ASCII Question Mark
            /// </summary>
            internal const byte QuestionMark = 63;

            /// <summary>
            /// Defines the ASCII Space
            /// </summary>
            internal const byte Space = 32;

            /// <summary>
            /// Defines the ASCII Unit Separator
            /// </summary>
            internal const byte UnitSeparator = 31;
        }

        /// <summary>
        /// Defines coordinates default values
        /// </summary>
        internal static class Coordinate {
            /// <summary>
            /// Defines latitude default values
            /// </summary>
            internal static class Latitude {
                /// <summary>
                /// Defines the maximum value for latitude
                /// </summary>
                internal const sbyte Min = -Max;

                /// <summary>
                /// Defines the maximum value for latitude
                /// </summary>
                internal const byte Max = 90;
            }

            /// <summary>
            /// Defines longitude default values
            /// </summary>
            internal static class Longitude {
                /// <summary>
                /// Defines the maximum value for longitude
                /// </summary>
                internal const short Min = -Max;

                /// <summary>
                /// Defines the maximum value for longitude
                /// </summary>
                internal const byte Max = 180;
            }

            /// <summary>
            /// Defines default ranges for latitude and longitude
            /// </summary>
            internal static class Range {
                /// <summary>
                /// Defines latitude validation range
                /// </summary>
                internal static readonly CoordinateRange Latitude = new(Coordinate.Latitude.Min, Coordinate.Latitude.Max);

                /// <summary>
                /// Defines longitude validation range
                /// </summary>
                internal static readonly CoordinateRange Longitude = new(Coordinate.Longitude.Min, Coordinate.Longitude.Max);
            }
        }
    }
}
