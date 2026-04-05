//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using System;

/// <summary>
/// Polyline decoder using NetTopologySuite.
/// </summary>
internal sealed class NetTopologyPolylineDecoder : AbstractPolylineDecoder<string, Point> {
    /// <summary>
    /// Gets the number of encoded values per item: latitude (index 0) and longitude (index 1).
    /// </summary>
    protected override int ValuesPerItem => 2;

    /// <summary>
    /// Creates a NetTopologySuite point from the decoded values.
    /// </summary>
    /// <param name="values">
    /// A memory region containing two values: index 0 is latitude, index 1 is longitude.
    /// </param>
    /// <returns>Point instance.</returns>
    protected override Point CreateItem(ReadOnlyMemory<double> values) {
        ReadOnlySpan<double> span = values.Span;
        // NetTopologySuite Point: x = longitude, y = latitude
        return new Point(span[1], span[0]);
    }

    /// <summary>
    /// Converts polyline string to read-only memory.
    /// </summary>
    /// <param name="polyline">Encoded polyline string.</param>
    /// <returns>ReadOnlyMemory of characters.</returns>
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) {
        return polyline.AsMemory();
    }
}