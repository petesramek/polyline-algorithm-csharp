//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;

/// <summary>
/// Represents a polyline decoder that converts encoded polyline strings into a collection of geographic coordinates using NetTopologySuite.
/// </summary>
public sealed class NetTopologyPolylineDecoder : AbstractPolylineDecoder<string, Point> {
    protected override Point CreateCoordinate(double latitude, double longitude) {
        return new Point(latitude, longitude);
    }

    /// <summary>
    /// Converts the provided polyline string into a read-only memory of characters.
    /// </summary>
    /// <param name="polyline">
    /// The encoded polyline string to be converted into a read-only memory of characters.
    /// </param>
    /// <returns>
    /// A <see cref="ReadOnlyMemory{T}"/> containing the characters of the polyline string.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided polyline string is null, empty, or consists only of whitespace characters.
    /// </exception>
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(string polyline) {
        if (string.IsNullOrWhiteSpace(polyline)) {
            throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(polyline));
        }

        return polyline.AsMemory();
    }
}
