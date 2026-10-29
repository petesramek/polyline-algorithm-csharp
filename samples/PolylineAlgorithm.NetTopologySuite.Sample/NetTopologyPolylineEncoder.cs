//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;

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
    /// Gets latitude from point.
    /// </summary>
    /// <param name="current">Point instance.</param>
    /// <returns>Latitude value.</returns>
    protected override double GetLatitude(Point current) {
        ArgumentNullException.ThrowIfNull(current);

        // NetTopologySuite Point: Y = latitude
        return current.Y;
    }

    /// <summary>
    /// Gets longitude from point.
    /// </summary>
    /// <param name="current">Point instance.</param>
    /// <returns>Longitude value.</returns>
    protected override double GetLongitude(Point current) {
        ArgumentNullException.ThrowIfNull(current);

        // NetTopologySuite Point: X = longitude
        return current.X;
    }
}