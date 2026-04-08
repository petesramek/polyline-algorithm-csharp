//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Polyline decoder using NetTopologySuite.
/// </summary>
internal sealed class NetTopologyPolylineDecoder : IPolylineDecoder<string, Point> {
    private readonly PolylineDecoder<string, Point> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetTopologyPolylineDecoder"/> class.
    /// </summary>
    internal NetTopologyPolylineDecoder() {
        PolylineFormatter<Point, string> formatter =
            FormatterBuilder<Point, string>.Create()
                // NetTopologySuite Point: Y = latitude, X = longitude
                .AddValue("lat", static p => p.Y)
                .AddValue("lon", static p => p.X)
                // v[0] = scaled latitude, v[1] = scaled longitude (factor = 1e5 for default precision 5)
                .WithCreate(static v => new Point(x: v[1] / 1e5, y: v[0] / 1e5))
                .ForPolyline(
                    static m => m.IsEmpty ? string.Empty : new string(m.Span),
                    static s => s.AsMemory())
                .Build();

        _inner = new PolylineDecoder<string, Point>(new PolylineOptions<Point, string>(formatter));
    }

    /// <inheritdoc/>
    public IEnumerable<Point> Decode(string polyline, CancellationToken cancellationToken = default)
        => _inner.Decode(polyline, cancellationToken);
}