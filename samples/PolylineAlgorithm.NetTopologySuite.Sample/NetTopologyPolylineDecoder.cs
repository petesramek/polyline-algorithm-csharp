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
    /// Creates a NetTopologySuite Point from decoded field values.
    /// </summary>
    /// <param name="reader">The reader provided by the engine. Field 0 = latitude, field 1 = longitude.</param>
    /// <returns>Point instance.</returns>
    protected override Point Read(IPolylineReader reader) {
        double latitude = reader.Read();
        double longitude = reader.Read();

        // NetTopologySuite Point: x = longitude, y = latitude
        return new Point(longitude, latitude);
    }
}