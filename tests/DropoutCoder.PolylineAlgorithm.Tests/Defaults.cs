//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines default values and objects used for testing purposes
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// Defines default decoded values and objects udśed for testing purposes
        /// </summary>
        public static class Coordinate
        {
            /// <summary>
            /// Defines empty range of coordinates. Equals to decoded <seealso cref="Polyline.Empty"/>
            /// </summary>
            public static readonly IEnumerable<(double Latitude, double Longitude)> Empty = Enumerable.Empty<(double Latitude, double Longitude)>();

            /// <summary>
            /// Defines range of invalid coordinates. Equals to decoded <seealso cref="Polyline.Invalid"/>
            /// </summary>
            public static readonly IEnumerable<(double Latitude, double Longitude)> Invalid = new[] {
                (149.47383, 259.06250),
                (-158.37407, 225.31250),
                (152.99363, -220.93750),
                (-144.49024, -274.37500)
            };

            /// <summary>
            /// Defines range of valid coordinates. Equals to decoded <seealso cref="Polyline.Valid"/>
            /// </summary>
            public static readonly IEnumerable<(double Latitude, double Longitude)> Valid = new[] {
                (49.47383, 59.06250),
                (-58.37407, 25.31250),
                (52.99363, -120.93750),
                (-44.49024, -174.37500)
            };
        }

        /// <summary>
        /// Defines default encoded values and objects udśed for testing purposes
        /// </summary>
        public static class Polyline
        {
            /// <summary>
            /// Defines empty string of polyline encoded coordinates. Equals to encoded <seealso cref="Coordinate.Empty"/>
            /// </summary>
            public static readonly string Empty = String.Empty;

            /// <summary>
            /// Defines polyline encoded range of invalid coordinates. Equals to encoded <seealso cref="Coordinate.Invalid"/>
            /// </summary>
            public static readonly string Invalid = "mnc~Qsm_ja@";

            /// <summary>
            /// Defines polyline encoded range of valid coordinates. Equals to encoded <seealso cref="Coordinate.Valid"/>
            /// </summary>
            public static readonly string Valid = "mz}lHssngJj`gqSnx~lEcovfTnms{Zdy~qQj_deI";
        }
    }
}
