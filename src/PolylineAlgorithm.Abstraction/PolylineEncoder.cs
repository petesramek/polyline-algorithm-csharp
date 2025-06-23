//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Properties;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides functionality to encode a collection of geographic coordinates into an encoded polyline string.
/// Implements the <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> interface.
/// </summary>
public abstract class PolylineEncoder<TCoordinate, TPolyline> : IPolylineEncoder<TCoordinate, TPolyline> {
    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineEncoder{TCoordinate, TPolyline}"/> class
    /// using the default <see cref="PolylineEncodingOptions{TCoordinate}"/>.
    /// </summary>
    public PolylineEncoder()
        : this(new PolylineEncodingOptions<TCoordinate>()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineEncoder{TCoordinate, TPolyline}"/> class
    /// with the specified <see cref="PolylineEncodingOptions{TCoordinate}"/>.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions{TCoordinate}"/> to use for encoding operations.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public PolylineEncoder(PolylineEncodingOptions<TCoordinate> options) {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the encoding options used by this polyline encoder.
    /// </summary>
    public PolylineEncodingOptions<TCoordinate> Options { get; }

    /// <summary>
    /// Encodes a collection of <typeparamref name="TCoordinate"/> instances into an encoded <typeparamref name="TPolyline"/> string.
    /// </summary>
    /// <param name="coordinates">
    /// The collection of <typeparamref name="TCoordinate"/> objects to encode.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded coordinates.
    /// Returns <see langword="default"/> if the input collection is empty or null.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is an empty enumeration.
    /// </exception>
    public TPolyline Encode(IEnumerable<TCoordinate> coordinates) {
        if (coordinates is null) {
            throw new ArgumentNullException(nameof(coordinates));
        }

        int count = GetCount(coordinates);

        if (count == 0) {
            throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
        }

        CoordinateVariance variance = new();

        int position = 0;
        int consumed = 0;
        int length = count * Defaults.Polyline.MaxEncodedCoordinateLength;

        if (length > Options.MaxCharCount) {
            length = Options.MaxCharCount;
        }

        Span<char> buffer = stackalloc char[length];

        using var enumerator = coordinates.GetEnumerator();

        while (enumerator.MoveNext()) {
            variance
                .Next(PolylineEncoding.Normalize(GetLatitude(enumerator.Current)), PolylineEncoding.Normalize(GetLongitude(enumerator.Current)));

            if (GetRemainingBufferSize(position, buffer.Length) < GetRequiredLength(variance)) {
                throw new InternalBufferOverflowException();
            }

            if (!PolylineEncoding.TryWriteValue(variance.Latitude, ref buffer, ref position)
                || !PolylineEncoding.TryWriteValue(variance.Longitude, ref buffer, ref position)
            ) {
                throw new InvalidOperationException();
            }

            consumed++;
        }

        return CreatePolyline(new(buffer[..position].ToString().AsMemory()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetCount(IEnumerable coordinates) => coordinates switch {
            ICollection collection => collection.Count,
            _ => -1,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRequiredLength(CoordinateVariance variance) =>
            PolylineEncoding.GetCharCount(variance.Latitude) + PolylineEncoding.GetCharCount(variance.Longitude);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRemainingBufferSize(int position, int length) => length - position;
    }

    /// <summary>
    /// Creates a polyline instance from the provided read-only sequence of characters.
    /// </summary>
    /// <param name="polyline">A <see cref="ReadOnlySequence{T}"/> containing the encoded polyline characters.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded polyline.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract TPolyline CreatePolyline(ReadOnlySequence<char> polyline);

    /// <summary>
    /// Extracts the longitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the longitude.</param>
    /// <returns>
    /// The longitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLongitude(TCoordinate? current);

    /// <summary>
    /// Extracts the latitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the latitude.</param>
    /// <returns>
    /// The latitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLatitude(TCoordinate? current);
}

