//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Gps;

using PolylineAlgorithm.Gps.Abstraction;

/// <inheritdoc cref="AbstractPolylineDecoder{TPolyline, TValue}" />
public sealed class PolylineDecoder : AbstractPolylineDecoder<Polyline, Coordinate> {
    /// <inheritdoc />
    public PolylineDecoder()
        : base() { }

    /// <inheritdoc />
    public PolylineDecoder(PolylineEncodingOptions options)
        : base(options) { }

    /// <inheritdoc />
    protected override Coordinate CreateCoordinate(double latitude, double longitude) {
        return new(latitude, longitude);
    }

    /// <inheritdoc />
    protected override ReadOnlyMemory<char> GetReadOnlyMemory(in Polyline polyline) {
        return polyline.Value;
    }
}