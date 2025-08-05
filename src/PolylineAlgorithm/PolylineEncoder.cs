//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System.Runtime.CompilerServices;

/// <inheritdoc cref="AbstractPolylineEncoder{TCoordinate, TPolyline}" />
public sealed class PolylineEncoder : AbstractPolylineEncoder<Coordinate, Polyline> {
    /// <inheritdoc />
    public PolylineEncoder()
        : base() { }

    /// <inheritdoc />
    public PolylineEncoder(PolylineEncodingOptions options)
        : base(options) { }

    /// <inheritdoc />
    protected override double GetLatitude(Coordinate coordinate) {
        return coordinate.Latitude;
    }

    /// <inheritdoc />
    protected override double GetLongitude(Coordinate coordinate) {
        return coordinate.Longitude;
    }

    /// <inheritdoc />
    protected override Polyline CreatePolyline(ReadOnlyMemory<char> polyline) {
        return Polyline.FromMemory(polyline);
    }
}

