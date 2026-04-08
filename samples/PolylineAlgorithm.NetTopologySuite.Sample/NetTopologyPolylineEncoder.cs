//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm;
using PolylineAlgorithm.Abstraction;
using System.Threading;

/// <summary>
/// Polyline encoder using NetTopologySuite's Point type.
/// </summary>
internal sealed class NetTopologyPolylineEncoder : IPolylineEncoder<Point, string> {
    private readonly PolylineEncoder<Point, string> _inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetTopologyPolylineEncoder"/> class.
    /// </summary>
    internal NetTopologyPolylineEncoder() {
        PolylineFormatter<Point, string> formatter =
            FormatterBuilder<Point, string>.Create()
                // NetTopologySuite Point: Y = latitude, X = longitude
                .AddValue("lat", static p => { ArgumentNullException.ThrowIfNull(p); return p.Y; })
                .AddValue("lon", static p => { ArgumentNullException.ThrowIfNull(p); return p.X; })
                .ForPolyline(
                    static m => m.IsEmpty ? string.Empty : new string(m.Span),
                    static s => s.AsMemory())
                .Build();

        _inner = new PolylineEncoder<Point, string>(new PolylineOptions<Point, string>(formatter));
    }

    /// <inheritdoc/>
    public string Encode(ReadOnlySpan<Point> coordinates, CancellationToken cancellationToken = default)
        => _inner.Encode(coordinates, cancellationToken);
}