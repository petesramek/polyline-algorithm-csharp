//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm;

internal sealed class NetTopologyPolylineEncoder : AbstractPolylineEncoder<Point, string> {

    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return string.Empty;
        }

        return polyline.ToString();
    }

    protected override double GetLatitude(Point? current) {
        return current?.X ?? 0d;
    }

    protected override double GetLongitude(Point? current) {
        return current?.Y ?? 0d;
    }
}