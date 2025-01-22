//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;

/// <summary>
/// Performs polyline algorithm decoding and encoding
/// </summary>
public sealed class DefaultPolylineEncoder() : PolylineEncoder<Coordinate> {
    protected override double GetLatitude(ref readonly Coordinate coordinate) {
        return coordinate.Latitude;
    }

    protected override double GetLongitude(ref readonly Coordinate coordinate) {
        return coordinate.Longitude;
    }
}
