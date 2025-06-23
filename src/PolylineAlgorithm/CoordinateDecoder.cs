//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System.Runtime.CompilerServices;


/// <summary>
/// Performs decoding of encoded polyline strings into a sequence of geographic coordinates.
/// </summary>
public class CoordinateDecoder : PolylineDecoder<Coordinate> {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override Coordinate CreateCoordinate(in double latitude, in double longitude) {
        return new(latitude, longitude);
    }
}