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
    /// Converts polyline string to read-only memory.
    /// </summary>
    /// <param name="polyline">Encoded polyline string.</param>
    /// <returns>ReadOnlyMemory of characters.</returns>
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) {
        return polyline.AsMemory();
    }

    /// <summary>
    /// Gets the number of values per point (latitude + longitude = 2).
    /// </summary>
    protected override int ValuesPerItem => 2;

    /// <summary>
    /// Creates a NetTopologySuite Point from decoded values.
    /// </summary>
    /// <param name="values">Decoded values: values[0] = latitude, values[1] = longitude.</param>
    /// <returns>Point instance.</returns>
    protected override Point CreateItem(ReadOnlyMemory<double> values) {
        ReadOnlySpan<double> span = values.Span;

        // NetTopologySuite Point: x = longitude (values[1]), y = latitude (values[0])
        return new Point(span[1], span[0]);
    }
}