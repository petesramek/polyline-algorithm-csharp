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
public sealed class NetTopologyPolylineDecoder : AbstractPolylineDecoder<string, Point>
{
    /// <summary>
    /// Creates a NetTopologySuite point from latitude and longitude.
    /// </summary>
    /// <param name="latitude">Latitude value.</param>
    /// <param name="longitude">Longitude value.</param>
    /// <returns>Point instance.</returns>
    protected override Point CreateCoordinate(double latitude, double longitude)
    {
        // NetTopologySuite Point: x = longitude, y = latitude
        return new Point(longitude, latitude);
    }

    /// <summary>
    /// Converts polyline string to read-only memory.
    /// </summary>
    /// <param name="polyline">Encoded polyline string.</param>
    /// <returns>ReadOnlyMemory of characters.</returns>
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline)
    {
        return polyline.AsMemory();
    }
}
