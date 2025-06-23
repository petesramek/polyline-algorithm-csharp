//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods to encode a set of geographic coordinates into a polyline string.
/// This class implements the <see cref="IPolylineEncoder"/> interface.
/// </summary>
public class CoordinateEncoder : PolylineEncoder<Coordinate> {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override double GetLatitude(in Coordinate coordinate) {
        return coordinate.Latitude;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override double GetLongitude(in Coordinate coordinate) {
        return coordinate.Longitude;
    }
}

