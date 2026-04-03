//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
/// Provides a base implementation for encoding sequences of geographic coordinates into encoded polyline strings.
/// </summary>
/// <remarks>
/// Derive from this class to implement an encoder for a specific coordinate and polyline type. Override
/// <see cref="GetLatitude"/>, <see cref="GetLongitude"/>, and <see cref="CreatePolyline"/> to provide type-specific behavior.
/// </remarks>
/// <typeparam name="TCoordinate">The type that represents a geographic coordinate to encode.</typeparam>
/// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
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
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        Options = options;
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
    /// <exception cref="InvalidOperationException">
    /// Thrown when the internal encoding buffer cannot accommodate the encoded value.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "Method contains local methods. Actual method only 55 lines.")]
    public TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates, CancellationToken cancellationToken) {
        const string OperationName = nameof(Encode);

        _logger
            .LogOperationStartedDebug(OperationName);

        Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

        ValidateEmptyCoordinates(ref coordinates, _logger);

        CoordinateDelta delta = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxBufferLength(coordinates.Length);

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        try {
            for (var i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                delta
                    .Next(
                        PolylineEncoding.Normalize(GetLatitude(coordinates[i]), Options.Precision),
                        PolylineEncoding.Normalize(GetLongitude(coordinates[i]), Options.Precision)
                    );

                if (!PolylineEncoding.TryWriteValue(delta.Latitude, buffer, ref position)
                    || !PolylineEncoding.TryWriteValue(delta.Longitude, buffer, ref position)
                ) {
                    // This shouldn't happen, but if it does, log the error and throw an exception.
                    _logger
                        .LogOperationFailedDebug(OperationName);
                    _logger
                        .LogCannotWriteValueToBufferWarning(position, consumed);

                    ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();

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
        static int GetMaxBufferLength(int count) {
            Debug.Assert(count > 0, "Count must be greater than zero.");

            int requestedBufferLength = count * 2 * Defaults.Polyline.Block.Length.Max;

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

                ExceptionGuard.ThrwoArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
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

