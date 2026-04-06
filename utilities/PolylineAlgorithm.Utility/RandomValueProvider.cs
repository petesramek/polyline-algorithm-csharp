//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Utility;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

/// <summary>
/// Provides random generation and caching of coordinate collections and their encoded polyline representations.
/// Useful for testing and benchmarking polyline algorithms with reproducible random data.
/// </summary>
[SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "Internal use only.")]
internal static class RandomValueProvider {
    private static readonly Random _random = new(DateTime.Now.Millisecond);
    private static readonly ConcurrentDictionary<int, PolylineCoordinateCollectionPair> _cache = new();
    private static readonly PolylineEncoder _encoder = new();

    /// <summary>
    /// Gets a collection of random latitude/longitude tuples of the specified count.
    /// The same collection is cached and reused for the same count value.
    /// </summary>
    /// <param name="count">The number of coordinates to generate.</param>
    /// <returns>An enumerable collection of random latitude/longitude tuples.</returns>
    public static IEnumerable<(double Latitude, double Longitude)> GetCoordinates(int count) {
        var entry = GetCaheEntry(count);

        using var enumerator = entry.Coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            yield return enumerator.Current;
        }
    }

    /// <summary>
    /// Gets the encoded polyline string for a random collection of coordinates of the specified count.
    /// The same polyline is cached and reused for the same count value.
    /// </summary>
    /// <param name="count">The number of coordinates to generate and encode.</param>
    /// <returns>A <see cref="string"/> representing the encoded polyline.</returns>
    public static string GetPolyline(int count) {
        var entry = GetCaheEntry(count);

        return entry.Polyline;
    }

    /// <summary>
    /// Gets or creates a cached entry containing a collection of random coordinates and its encoded polyline for the specified count.
    /// </summary>
    /// <param name="count">The number of coordinates to generate and cache.</param>
    /// <returns>
    /// A <see cref="PolylineCoordinateCollectionPair"/> containing the coordinates and their encoded polyline.
    /// </returns>
    private static PolylineCoordinateCollectionPair GetCaheEntry(int count) {
        if (_cache.TryGetValue(count, out var entry)) {
            return entry;
        }

        var enumeration = Enumerable
                            .Range(0, count)
                            .Select(_ => (RandomLatitude(), RandomLongitude()))
                            .ToArray();

        return _cache.GetOrAdd(count, new PolylineCoordinateCollectionPair(enumeration, _encoder.Encode(enumeration)));
    }

    /// <summary>
    /// Generates a random longitude value in the range [-180, 180), rounded to 5 decimal places.
    /// </summary>
    /// <returns>A random longitude value.</returns>
    private static double RandomLongitude() {
        return Math.Round(_random.Next(-180, 180) + _random.NextDouble(), 5);
    }

    /// <summary>
    /// Generates a random latitude value in the range [-90, 90), rounded to 5 decimal places.
    /// </summary>
    /// <returns>A random latitude value.</returns>
    private static double RandomLatitude() {
        return Math.Round(_random.Next(-90, 90) + _random.NextDouble(), 5);
    }

    /// <summary>
    /// Represents a pair of a collection of coordinates and its encoded polyline.
    /// </summary>
    /// <param name="coordinates">The collection of coordinates.</param>
    /// <param name="polyline">The encoded polyline.</param>
    private readonly struct PolylineCoordinateCollectionPair(IEnumerable<(double Latitude, double Longitude)> coordinates, string polyline) {
        /// <summary>
        /// Gets the collection of coordinates.
        /// </summary>
        public IEnumerable<(double Latitude, double Longitude)> Coordinates { get; } = coordinates;

        /// <summary>
        /// Gets the encoded polyline.
        /// </summary>
        public string Polyline { get; } = polyline;
    }

    private sealed class PolylineEncoder : AbstractPolylineEncoder<(double Latitude, double Longitude), string> {
        private PolylineValueState _latitudeState;
        private PolylineValueState _longitudeState;

        protected override string CreatePolyline(ReadOnlySpan<char> polyline) {
            return polyline.ToString();
        }

        protected override void Write((double Latitude, double Longitude) item, ref PolylineWriter writer) {
            writer.Write(item.Latitude, ref _latitudeState);
            writer.Write(item.Longitude, ref _longitudeState);
        }
    }
}