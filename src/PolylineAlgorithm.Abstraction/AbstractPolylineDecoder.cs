//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Properties;
using System;
using System.Buffers;

/// <summary>
/// Decodes encoded polyline strings into sequences of geographic coordinates.
/// Implements the <see cref="IPolylineDecoder{TCoordinate, TPolyline}"/> interface.
/// </summary>
public abstract class AbstractPolylineDecoder<TPolyline, TCoordinate> : IPolylineDecoder<TPolyline, TCoordinate> {
    public AbstractPolylineDecoder()
        : this(new PolylineEncodingOptions()) { }

    public AbstractPolylineDecoder(PolylineEncodingOptions options) {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the encoding options used by this polyline encoder.
    /// </summary>
    public PolylineEncodingOptions Options { get; }

    /// <summary>
    /// Decodes an encoded <typeparamref name="TPolyline"/> into a sequence of <typeparamref name="TCoordinate"/> instances.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline string to decode.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="polyline"/> is empty.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline format is invalid or malformed at a specific position.
    /// </exception>
    public IEnumerable<TCoordinate> Decode(TPolyline polyline) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }

        ReadOnlyMemory<char> sequence = GetReadOnlyMemory(polyline);

        if (sequence.Length < Defaults.Polyline.MinEncodedCoordinateLength) {
            Options
                .UseLoggerFor<AbstractPolylineDecoder<TPolyline, TCoordinate>>()
                .LogPolylineCannotBeShorterThanError(nameof(sequence), sequence.Length, Defaults.Polyline.MinEncodedCoordinateLength);

            throw new ArgumentException(string.Format(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage, sequence.Length), nameof(polyline));
        }

        int position = 0;
        int latitude = 0;
        int longitude = 0;

        while (true) {
            if(sequence.Length == position) {
                break; // End of the polyline string
            }

            if(!PolylineEncoding.TryReadValue(ref latitude, ref sequence, ref position)
                || !PolylineEncoding.TryReadValue(ref longitude, ref sequence, ref position)
            ) {
                InvalidPolylineException.Throw(position);
            }

            yield return CreateCoordinate(PolylineEncoding.Denormalize(latitude, PolylineEncoding.ValueType.Latitude), PolylineEncoding.Denormalize(longitude, PolylineEncoding.ValueType.Longitude));
        }
    }

    /// <summary>
    /// Converts the provided polyline instance into a <see cref="ReadOnlySequence{Char}"/> for decoding.
    /// </summary>
    /// <param name="polyline">
    /// The polyline instance to convert. May be <c>null</c>.
    /// </param>
    /// <returns>
    /// A <see cref="ReadOnlySequence{T}"/> representing the encoded polyline data.
    /// </returns>
    protected abstract ReadOnlyMemory<char> GetReadOnlyMemory(TPolyline? polyline);

    /// <summary>
    /// Creates a coordinate instance from the given latitude and longitude values.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    /// <returns>A coordinate instance of type <typeparamref name="TCoordinate"/>.</returns>
    protected abstract TCoordinate CreateCoordinate(double latitude, double longitude);
}