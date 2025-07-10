//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Properties;
using System;

/// <summary>
/// Decodes encoded polyline strings into sequences of geographic coordinates.
/// Implements the <see cref="IPolylineDecoder{TPolyline, TCoordinate}"/> interface.
/// </summary>
/// <remarks>
/// This abstract class provides a base implementation for decoding polylines, allowing subclasses to define how to handle specific polyline formats.
/// </remarks>
public abstract class AbstractPolylineDecoder<TPolyline, TCoordinate> : IPolylineDecoder<TPolyline, TCoordinate> {
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/> class with default encoding options.
    /// </summary>
    public AbstractPolylineDecoder()
        : this(new PolylineEncodingOptions()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions"/> to use for encoding operations.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null" />
    /// </exception>
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
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>"
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

        if (sequence.Length < Defaults.Polyline.Block.Length.Min) {
            Options
                .GetLoggerFor<AbstractPolylineDecoder<TPolyline, TCoordinate>>()
                .LogPolylineCannotBeShorterThanError(nameof(sequence), sequence.Length, Defaults.Polyline.Block.Length.Min);

            throw new ArgumentException(string.Format(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage, sequence.Length), nameof(polyline));
        }

        int position = 0;
        int latitude = 0;
        int longitude = 0;

        
        while (true) {
            // Check if we have reached the end of the sequence
            if (sequence.Length == position) {
                break;
            }

            // Read the next value from the polyline encoding
            if (!PolylineEncoding.TryReadValue(ref latitude, ref sequence, ref position)
                || !PolylineEncoding.TryReadValue(ref longitude, ref sequence, ref position)
            ) {
                Options
                    .GetLoggerFor<AbstractPolylineDecoder<TPolyline, TCoordinate>>()
                    .LogInvalidPolylineError(position);

                InvalidPolylineException.Throw(position);
            }

            yield return CreateCoordinate(PolylineEncoding.Denormalize(latitude, PolylineEncoding.ValueType.Latitude), PolylineEncoding.Denormalize(longitude, PolylineEncoding.ValueType.Longitude));
        }
    }

    /// <summary>
    /// Converts the provided polyline instance into a <see cref="ReadOnlyMemory{T}"/> for decoding.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline data to decode.
    /// </param>
    /// <returns>
    /// A <see cref="ReadOnlyMemory{T}"/> representing the encoded polyline data.
    /// </returns>
    protected abstract ReadOnlyMemory<char> GetReadOnlyMemory(TPolyline? polyline);

    /// <summary>
    /// Creates a coordinate instance from the given latitude and longitude values.
    /// </summary>
    /// <param name="latitude">
    /// The latitude value.
    /// </param>
    /// <param name="longitude">
    /// The longitude value.
    /// </param>
    /// <returns>
    /// A coordinate instance of type <typeparamref name="TCoordinate"/>.
    /// </returns>
    protected abstract TCoordinate CreateCoordinate(double latitude, double longitude);
}