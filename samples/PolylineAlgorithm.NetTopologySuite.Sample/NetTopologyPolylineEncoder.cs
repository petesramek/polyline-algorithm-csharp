//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;

/// <summary>
/// Polyline encoder using NetTopologySuite's Point type.
/// </summary>
internal sealed class NetTopologyPolylineEncoder : AbstractPolylineEncoder<Point, string> {
    /// <summary>
    /// Gets the number of values per item: latitude (index 0) and longitude (index 1).
    /// </summary>
    protected override int ValuesPerItem => 2;

    /// <summary>
    /// Creates encoded polyline string from memory.
    /// </summary>
    /// <param name="polyline">Polyline memory.</param>
    /// <returns>Encoded polyline string.</returns>
    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return string.Empty;
        }

        return polyline.ToString();
    }

    /// <summary>
    /// Fills destination with latitude (index 0) and longitude (index 1) from the point.
    /// </summary>
    /// <param name="item">Point instance.</param>
    /// <param name="destination">Span of length 2 to fill.</param>
    protected override void GetValues(Point item, Span<double> destination) {
        ArgumentNullException.ThrowIfNull(item);

        // NetTopologySuite Point: Y = latitude, X = longitude
        destination[0] = item.Y;
        destination[1] = item.X;
    }
}