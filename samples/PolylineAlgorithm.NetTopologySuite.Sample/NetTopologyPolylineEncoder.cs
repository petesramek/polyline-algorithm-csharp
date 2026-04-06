//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using System;

/// <summary>
/// Polyline encoder using NetTopologySuite's Point type.
/// </summary>
internal sealed class NetTopologyPolylineEncoder : AbstractPolylineEncoder<Point, string> {
    private PolylineValueState _latitudeState;
    private PolylineValueState _longitudeState;

    /// <summary>
    /// Creates encoded polyline string from span.
    /// </summary>
    /// <param name="polyline">Polyline span.</param>
    /// <returns>Encoded polyline string.</returns>
    protected override string CreatePolyline(ReadOnlySpan<char> polyline) {
        if (polyline.IsEmpty) {
            return string.Empty;
        }

        return polyline.ToString();
    }

    /// <summary>
    /// Writes latitude and longitude from a NetTopologySuite Point into the polyline encoding pipeline.
    /// </summary>
    /// <param name="item">The point to write. Field 0 = latitude (Y), field 1 = longitude (X).</param>
    /// <param name="writer">The writer provided by the engine.</param>
    protected override void Write(Point item, ref PolylineWriter writer) {
        ArgumentNullException.ThrowIfNull(item);

        // NetTopologySuite Point: Y = latitude, X = longitude
        writer.Write(item.Y, ref _latitudeState);
        writer.Write(item.X, ref _longitudeState);
    }
}