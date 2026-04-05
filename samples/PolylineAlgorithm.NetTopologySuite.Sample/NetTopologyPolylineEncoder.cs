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
    /// Gets the number of values per point (latitude + longitude = 2).
    /// </summary>
    protected override int ValuesPerItem => 2;

    /// <summary>
    /// Extracts latitude and longitude from a NetTopologySuite Point into the provided span.
    /// </summary>
    /// <param name="item">The point to extract values from.</param>
    /// <param name="values">The span to receive the values: values[0] = latitude (Y), values[1] = longitude (X).</param>
    protected override void GetValues(Point item, Span<double> values) {
        ArgumentNullException.ThrowIfNull(item);

        // NetTopologySuite Point: Y = latitude, X = longitude
        values[0] = item.Y;
        values[1] = item.X;
    }
}