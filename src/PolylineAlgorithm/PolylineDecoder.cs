//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;
using System.Runtime.CompilerServices;

/// <inheritdoc cref="AbstractPolylineDecoder{TPolyline, TCoordinate}" />
public sealed class PolylineDecoder : AbstractPolylineDecoder<Polyline, Coordinate> {
    /// <inheritdoc />
    public PolylineDecoder()
        : base() { }

    /// <inheritdoc />
    public PolylineDecoder(PolylineEncodingOptions options)
        : base(options) { }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Coordinate CreateCoordinate(double latitude, double longitude) {
        return new(latitude, longitude);
    }

    /// <inheritdoc />
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(Polyline polyline) {
        return polyline.Value;
    }
}