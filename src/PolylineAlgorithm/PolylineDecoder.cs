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
using System.Runtime.CompilerServices;

/// <summary>
/// Decodes encoded polyline strings into sequences of geographic coordinates.
/// Implements the <see cref="IPolylineDecoder"/> interface.
/// </summary>
public sealed class PolylineDecoder : PolylineDecoder<Coordinate, Polyline> {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Coordinate CreateCoordinate(double latitude, double longitude) {
        return new Coordinate(latitude, longitude);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override ReadOnlySequence<char> GetReadOnlySequence(Polyline polyline) {
        return polyline.Value;
    }
}