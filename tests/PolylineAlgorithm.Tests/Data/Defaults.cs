//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests.Data;

using System.Collections.Generic;

/// <summary>
/// Defines default values and objects used for testing purposes
/// </summary>
internal static class Defaults {
    private static readonly Random _random = new(DateTime.Now.Millisecond);

    /// <summary>
    /// Defines default decoded values and objects udśed for testing purposes
    /// </summary>
    public static class Coordinates {
        /// <summary>
        /// Defines empty range of coordinates. Equals to decoded <seealso cref="Polyline.Empty"/>
        /// </summary>
        public static readonly IEnumerable<Coordinate> Empty = [];

        /// <summary>
        /// Defines range of invalid coordinates. Equals to decoded <seealso cref="Polyline.Invalid"/>
        /// </summary>
        public static readonly IEnumerable<Coordinate> Invalid = [
            new(149.47383, 259.06250),
            new(-158.37407, 225.31250),
            new(152.99363, -220.93750),
            new(-144.49024, -274.37500)
        ];

        /// <summary>
        /// Defines range of valid coordinates. Equals to decoded <seealso cref="Polyline.Valid"/>
        /// </summary>
        public static readonly IEnumerable<Coordinate> Valid = [
            new(49.47383, 59.06250),
            new(-58.37407, 25.31250),
            new(52.99363, -120.93750),
            new(-44.49024, -174.37500)
        ];
    }

    /// <summary>
    /// Defines default encoded values and objects udśed for testing purposes
    /// </summary>
    public static class Polyline {
        /// <summary>
        /// Defines empty string of polyline encoded coordinates. Equals to encoded <seealso cref="Coordinates.Empty"/>
        /// </summary>
        public static readonly string Empty = string.Empty;

        /// <summary>
        /// Defines polyline encoded range of invalid coordinates. Equals to encoded <seealso cref="Coordinates.Invalid"/>
        /// </summary>
        public static readonly string Invalid = "mnc~Qsm_ja@";

        /// <summary>
        /// Defines polyline encoded range of valid coordinates. Equals to encoded <seealso cref="Coordinates.Valid"/>
        /// </summary>
        public static readonly string Valid = "mz}lHssngJj`gqSnx~lEcovfTnms{Zdy~qQj_deI";
    }

    public static class MalformedPolylineException {
        public static readonly int Position = _random.Next();
    }

    public static class InvalidCoordinateException {
        public static readonly Coordinate Coordinate = new(_random.NextDouble(), _random.NextDouble());
    }
}
