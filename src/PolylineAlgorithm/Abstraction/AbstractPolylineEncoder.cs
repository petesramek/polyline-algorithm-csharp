//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Logging;
using PolylineAlgorithm.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides functionality to encode a collection of geographic coordinates into an encoded polyline string.
/// Implements the <see cref="IPolylineEncoder{TCoordinate, TPolyline}"/> interface.
/// </summary>
/// <remarks>
/// This abstract class serves as a base for specific polyline encoders, allowing customization of the encoding process.
/// </remarks>
public abstract class AbstractPolylineEncoder<TCoordinate, TPolyline> : IPolylineEncoder<TCoordinate, TPolyline> {
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineEncoder{TCoordinate, TPolyline}"/> class with default encoding options.
    /// </summary>
    protected AbstractPolylineEncoder()
        : this(new PolylineEncodingOptions()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineEncoder{TCoordinate, TPolyline}"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions"/> to use for encoding operations.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null" /></exception>
    protected AbstractPolylineEncoder(PolylineEncodingOptions options) {
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the encoding options used by this polyline encoder.
    /// </summary>
    public PolylineEncodingOptions Options { get; }

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
    public TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates) {
        var logger = Options
            .LoggerFactory
            .CreateLogger<AbstractPolylineEncoder<TCoordinate, TPolyline>>();

        logger
            .LogOperationStartedInfo(nameof(Encode));

        Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

        ValidateEmptyCoordinates(coordinates, logger);

        CoordinateVariance variance = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxBufferLength(coordinates.Length);

        Span<char> buffer = stackalloc char[length];

        for (var i = 0; i < coordinates.Length; i++) {
            variance
                .Next(
                    PolylineEncoding.Normalize(GetLatitude(coordinates[i]), CoordinateValueType.Latitude),
                    PolylineEncoding.Normalize(GetLongitude(coordinates[i]), CoordinateValueType.Longitude)
                );

            ValidateBuffer(logger, variance, position, buffer);

            if (!PolylineEncoding.TryWriteValue(variance.Latitude, buffer, ref position)
                || !PolylineEncoding.TryWriteValue(variance.Longitude, buffer, ref position)
            ) {
                // This shouldn't happen, but if it does, log the error and throw an exception.
                logger
                    .LogCannotWriteValueToBufferWarning(position, consumed);
                logger
                    .LogOperationFailedInfo(nameof(Encode));

                throw new InvalidOperationException(ExceptionMessageResource.CouldNotWriteEncodedValueToTheBuffer);
            }

            consumed++;
        }

        logger
            .LogOperationFinishedInfo(nameof(Encode));

        return CreatePolyline(buffer[..position].ToString().AsMemory());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRequiredLength(CoordinateVariance variance) =>
            PolylineEncoding.GetCharCount(variance.Latitude) + PolylineEncoding.GetCharCount(variance.Longitude);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetRemainingBufferSize(int position, int length) {
            Debug.Assert(length > 0, "Buffer length must be greater than zero.");
            Debug.Assert(position >= 0, "Position must be non-negative.");
            Debug.Assert(position < length, "Position must be less than buffer length.");
            Debug.Assert(length - position >= 0, "Remaining length must be non-negative.");

            return length - position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetMaxBufferLength(int count) {
            Debug.Assert(count > 0, "Count must be greater than zero.");

            int requestedBufferLength = count * Defaults.Polyline.Block.Length.Max;

            Debug.Assert(Options.MaxBufferLength > 0, "Max buffer length must be greater than zero.");
            Debug.Assert(requestedBufferLength > 0, "Requested buffer length must be greater than zero.");

            if (requestedBufferLength > Options.MaxBufferLength) {
                logger
                    .LogRequestedBufferSizeExceedsMaxBufferLengthWarning(requestedBufferLength, Options.MaxBufferLength);

                return Options.MaxBufferLength;
            }

            return requestedBufferLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateEmptyCoordinates(ReadOnlySpan<TCoordinate> coordinates, ILogger logger) {
            if (coordinates.Length < 1) {
                logger
                    .LogEmptyArgumentWarning(nameof(coordinates));
                logger
                    .LogOperationFailedInfo(nameof(Encode));

                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateBuffer(ILogger logger, CoordinateVariance variance, int position, Span<char> buffer) {
            if (GetRemainingBufferSize(position, buffer.Length) < GetRequiredLength(variance)) {
                logger
                    .LogInternalBufferOverflowWarning(position, buffer.Length, GetRequiredLength(variance));
                logger
                    .LogOperationFailedInfo(nameof(Encode));

                throw new InternalBufferOverflowException();
            }
        }
    }

    /// <summary>
    /// Creates a polyline instance from the provided read-only sequence of characters.
    /// </summary>
    /// <param name="polyline">A <see cref="ReadOnlyMemory{T}"/> containing the encoded polyline characters.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded polyline.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract TPolyline CreatePolyline(ReadOnlyMemory<char> polyline);

    /// <summary>
    /// Extracts the longitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the longitude.</param>
    /// <returns>
    /// The longitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLongitude(TCoordinate current);

    /// <summary>
    /// Extracts the latitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the latitude.</param>
    /// <returns>
    /// The latitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLatitude(TCoordinate current);
}

