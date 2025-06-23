//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System.Buffers;
using System.Runtime.CompilerServices;

/// <summary>
/// Decodes encoded polyline strings into sequences of geographic coordinates.
/// Implements the <see cref="IPolylineDecoder{TCoordinate, TPolyline}"/> interface.
/// </summary>
public sealed class PolylineDecoder : PolylineDecoder<Coordinate, Polyline>
{
    public override PolylineEncodingOptions<Coordinate> Options { get; } = new PolylineEncodingOptions<Coordinate>();

    /// <summary>
    /// Creates a <see cref="Coordinate"/> instance from the given latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude of the coordinate.</param>
    /// <param name="longitude">The longitude of the coordinate.</param>
    /// <returns>A <see cref="Coordinate"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Coordinate CreateCoordinate(double latitude, double longitude)
    {
        return new Coordinate(latitude, longitude);
    }

    /// <summary>
    /// Retrieves the <see cref="ReadOnlySequence{T}"/> of characters from the given <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The polyline to extract the sequence from.</param>
    /// <returns>A <see cref="ReadOnlySequence{Task }"/> representing the polyline.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override ReadOnlySequence<char> GetReadOnlySequence(Polyline polyline)
    {
        return polyline.Value;
    }
}