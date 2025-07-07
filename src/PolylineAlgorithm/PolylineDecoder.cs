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
public sealed class PolylineDecoder : AbstractPolylineDecoder<Polyline, Coordinate> {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Coordinate CreateCoordinate(double latitude, double longitude) {
        return new(latitude, longitude);
    }

    /// <summary>
    /// Converts the provided polyline instance into a <see cref="ReadOnlyMemory{Char}"/> for decoding.
    /// </summary>
    /// <param name="polyline"></param>
    /// <returns></returns>
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(Polyline polyline) {
        return polyline.Value;
    }
}