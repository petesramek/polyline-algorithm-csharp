//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System.Buffers;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods to encode a set of geographic coordinates into a polyline string.
/// This class implements the <see cref="IPolylineEncoder"/> interface.
/// </summary>
public sealed class CoordinateEncoder : PolylineEncoder<Coordinate, Polyline> {

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override double GetLatitude(Coordinate coordinate) {
        return coordinate.Latitude;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override double GetLongitude(Coordinate coordinate) {
        return coordinate.Longitude;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Polyline CreatePolyline(ReadOnlyMemory<char> polyline) {
        return Polyline.FromMemory(polyline);
    }
}

