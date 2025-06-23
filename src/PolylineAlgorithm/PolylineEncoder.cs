//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System.Buffers;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides functionality to encode a collection of geographic coordinates into an encoded polyline string.
/// Implements the <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> interface.
/// </summary>
public sealed class PolylineEncoder : PolylineEncoder<Coordinate, Polyline> {
    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineEncoder"/> class with default options.
    /// </summary>
    public PolylineEncoder()
        : this(new()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineEncoder"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to configure the polyline encoding.</param>
    public PolylineEncoder(PolylineEncodingOptions<Coordinate> options) {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the encoding options used by this polyline encoder.
    /// </summary>
    public override PolylineEncodingOptions<Coordinate> Options { get; }

    /// <summary>
    /// Creates a <see cref="Polyline"/> instance from the provided read-only sequence of characters.
    /// </summary>
    /// <param name="polyline">A <see cref="ReadOnlySequence{T}"/> containing the encoded polyline characters.</param>
    /// <returns>
    /// An instance of <see cref="Polyline"/> representing the encoded polyline.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Polyline CreatePolyline(ReadOnlySequence<char> polyline) {
        return Polyline.FromSequence(polyline);
    }

    /// <summary>
    /// Extracts the latitude value from the specified <see cref="Coordinate"/> instance.
    /// </summary>
    /// <param name="current">The <see cref="Coordinate"/> from which to extract the latitude.</param>
    /// <returns>
    /// The latitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override double GetLatitude(Coordinate current)
    {
        return current.Latitude;
    }

    /// <summary>
    /// Extracts the longitude value from the specified <see cref="Coordinate"/> instance.
    /// </summary>
    /// <param name="current">The <see cref="Coordinate"/> from which to extract the longitude.</param>
    /// <returns>
    /// The longitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override double GetLongitude(Coordinate current)
    {
        return current.Longitude;
    }
}

