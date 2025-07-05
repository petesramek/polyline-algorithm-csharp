//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Utility;

using System.Collections.Generic;

/// <summary>
/// Provides static, predefined coordinate and polyline values for use in tests and benchmarks.
/// </summary>
internal static class StaticValueProvider {
    internal static class Valid {
        /// <summary>
        /// A predefined polyline instance representing a fixed encoded polyline string.
        /// </summary>
        private static readonly string _polyline = "???_gsia@_cidP??~fsia@?~fsia@~bidP?~bidP??_gsia@";

        /// <summary>
        /// A predefined collection of <see cref="Coordinate"/> instances representing a closed path around the globe.
        /// </summary>
        private static readonly IEnumerable<(double Latitude, double Longitude)> _coordinates = [
            new (0, 0),
            new (0, 180),
            new (90, 180),
            new (90, 0),
            new (90, -180),
            new (0, -180),
            new (-90, -180),
            new (-90, 0)
        ];

        /// <summary>
        /// Gets the predefined collection of <see cref="Coordinate"/> instances.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Coordinate}"/> containing the static coordinates.</returns>
        public static IEnumerable<(double Latitude, double Longitude)> GetCoordinates() {
            return _coordinates;
        }

        /// <summary>
        /// Gets the predefined <see cref="Polyline"/> instance.
        /// </summary>
        /// <returns>The static <see cref="Polyline"/> value.</returns>
        public static string GetPolyline() {
            return _polyline;
        }
    }

    internal static class Invalid {
        //public static string GetMalformedPolyline() {
        //    return ((char)(127)).ToString();
        //}

        public static IEnumerable<(double, double)> GetNotANumberAndInfinityCoordinates() {
            yield return (double.NaN, 0);
            yield return (0, double.NaN);
            yield return (double.PositiveInfinity, 0);
            yield return (0, double.PositiveInfinity);
            yield return (double.NegativeInfinity, 0);
            yield return (0, double.NegativeInfinity);
        }

        public static IEnumerable<(double, double)> GetMinAndMaxCoordinates() {
            yield return (double.MinValue, 0);
            yield return (0, double.MinValue);
            yield return (double.MaxValue, 0);
            yield return (0, double.MaxValue);
            yield return (double.MinValue, double.MinValue);
            yield return (double.MaxValue, double.MaxValue);
        }
    }
}
