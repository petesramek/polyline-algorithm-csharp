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
public sealed class PolylineDecoder : PolylineDecoder<Coordinate, Polyline> {
    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineDecoder"/> class with default encoding options.
    /// </summary>
    public PolylineDecoder()
        : this(new()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineDecoder"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">The encoding options to use for decoding polylines.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
    public PolylineDecoder(PolylineEncodingOptions<Coordinate> options) {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the encoding options used by this polyline decoder.
    /// </summary>
    public override PolylineEncodingOptions<Coordinate> Options { get; }

    /// <summary>
    /// Creates a <see cref="Coordinate"/> instance from the given latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude of the coordinate.</param>
    /// <param name="longitude">The longitude of the coordinate.</param>
    /// <returns>A <see cref="Coordinate"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override Coordinate CreateCoordinate(double latitude, double longitude) {
        if (!Options.Validator.IsValidLatitude(latitude)) {
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be between -90 and 90 degrees.");
        }

        if (!Options.Validator.IsValidLongitude(longitude)) {
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be between -180 and 180 degrees.");
        }

        return new Coordinate(latitude, longitude);
    }

    /// <summary>
    /// Retrieves the <see cref="ReadOnlySequence{T}"/> of characters from the given <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The polyline to extract the sequence from.</param>
    /// <returns>A <see cref="ReadOnlySequence{Task }"/> representing the polyline.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override ReadOnlySequence<char> GetReadOnlySequence(Polyline polyline) {
        return polyline.Value;
    }
}