//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;

/// <inheritdoc cref="PolylineDecoder{TResult, TCoordinate}" />
public sealed class DefaultPolylineDecoder : PolylineDecoder<Coordinate> {
    protected override Coordinate CreateCoordinate(ref readonly double latitude, ref readonly double longitude) {
        return Coordinate.Create(latitude, longitude);
    }
}