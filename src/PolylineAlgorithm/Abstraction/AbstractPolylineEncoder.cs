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
using System.Buffers;
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
    private readonly ILogger<AbstractPolylineEncoder<TCoordinate, TPolyline>> _logger;
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
        _logger = Options
            .LoggerFactory
            .CreateLogger<AbstractPolylineEncoder<TCoordinate, TPolyline>>();
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
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is an empty enumeration.
    /// </exception>
    /// <exception cref="InternalBufferOverflowException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates) {
        const string OperationName = nameof(Encode);

        _logger
            .LogOperationStartedDebug(OperationName);

        Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

        ValidateEmptyCoordinates(ref coordinates, _logger);

        CoordinateVariance variance = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxBufferLength(coordinates.Length, _logger);

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        try {
            for (var i = 0; i < coordinates.Length; i++) {
                variance
                    .Next(
                        PolylineEncoding.Normalize(GetLatitude(coordinates[i]), CoordinateValueType.Latitude),
                        PolylineEncoding.Normalize(GetLongitude(coordinates[i]), CoordinateValueType.Longitude)
                    );

                ValidateBuffer(variance, position, buffer, _logger);

                if (!PolylineEncoding.TryWriteValue(variance.Latitude, buffer, ref position)
                    || !PolylineEncoding.TryWriteValue(variance.Longitude, buffer, ref position)
                ) {
                    // This shouldn't happen, but if it does, log the error and throw an exception.
                    _logger
                        .LogOperationFailedDebug(OperationName);
                    _logger
                        .LogCannotWriteValueToBufferWarning(position, consumed);

                    throw new InvalidOperationException(ExceptionMessageResource.CouldNotWriteEncodedValueToTheBuffer);
                }

                consumed++;
            }
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }



        _logger
            .LogOperationFinishedDebug(OperationName);

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
        int GetMaxBufferLength(int count, ILogger logger) {
            Debug.Assert(count > 0, "Count must be greater than zero.");

            int requestedBufferLength = count * Defaults.Polyline.Block.Length.Max;

            Debug.Assert(requestedBufferLength > 0, "Requested buffer length must be greater than zero.");

            return requestedBufferLength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateEmptyCoordinates(ref ReadOnlySpan<TCoordinate> coordinates, ILogger logger) {
            if (coordinates.Length < 1) {
                logger
                    .LogOperationFailedDebug(OperationName);
                logger
                    .LogEmptyArgumentWarning(nameof(coordinates));

                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeEmptyEnumerationMessage, nameof(coordinates));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateBuffer(CoordinateVariance variance, int position, Span<char> buffer, ILogger logger) {
            if (GetRemainingBufferSize(position, buffer.Length) < GetRequiredLength(variance)) {
                logger
                    .LogOperationFailedDebug(OperationName);
                logger
                    .LogInternalBufferOverflowWarning(position, buffer.Length, GetRequiredLength(variance));

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

