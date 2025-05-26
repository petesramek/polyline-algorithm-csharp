//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides functionality to encode a collection of geographic coordinates into an encoded polyline string.
/// Implements the <see cref="IPolylineEncoder"/> interface.
/// </summary>
public sealed class PolylineEncoder : PolylineEncoder<Coordinate, Polyline> {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Polyline CreatePolyline(ReadOnlySequence<char> polyline) {
        return Polyline.FromSequence(polyline);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override double GetLatitude(Coordinate current) {
        return current.Latitude;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override double GetLongitude(Coordinate current) {
        return current.Longitude;
    }
}

