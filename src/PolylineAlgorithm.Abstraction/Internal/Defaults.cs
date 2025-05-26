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
