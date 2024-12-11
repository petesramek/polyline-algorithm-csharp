//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Internal {
    /// <summary>
    /// Defines global constant values
    /// </summary>
    internal static class Constants {
        /// <summary>
        /// Defines the coordinate precision
        /// </summary>
        public const double Precision = 1E5;

        /// <summary>
        /// Defines the shift length
        /// </summary>
        public const int ShiftLength = 5;

        /// <summary>
        /// Defines ASCII characters constant values
        /// </summary>
        internal static class ASCII {
            /// <summary>
            /// Defines the ASCII Question Mark
            /// </summary>
            public const int QuestionMark = 63;

            /// <summary>
            /// Defines the ASCII Space
            /// </summary>
            public const int Space = 32;

            /// <summary>
            /// Defines the ASCII Unit Separator
            /// </summary>
            public const int UnitSeparator = 31;
        }

        /// <summary>
        /// Defines coordinates constant values
        /// </summary>
        internal static class Coordinate {
            /// <summary>
            /// Defines the maximum value for latitude
            /// </summary>
            public const int MaxLatitude = 90;

            /// <summary>
            /// Defines the maximum value for longitude
            /// </summary>
            public const int MaxLongitude = 180;

            /// <summary>
            /// Defines the maximum value for latitude
            /// </summary>
            public const int MinLatitude = -MaxLatitude;

            /// <summary>
            /// Defines the maximum value for longitude
            /// </summary>
            public const int MinLongitude = -MaxLongitude;
        }
    }
}
