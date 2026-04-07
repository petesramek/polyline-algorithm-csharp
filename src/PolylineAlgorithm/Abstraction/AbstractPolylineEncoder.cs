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
/// <para>
/// <b>Formatter-based use (no subclassing required):</b>
/// Supply a <see cref="PolylineOptions{TCoordinate, TPolyline}"/> via the
/// <see cref="AbstractPolylineEncoder{TCoordinate, TPolyline}(PolylineOptions{TCoordinate, TPolyline})"/>
/// constructor. The formatter handles all type-specific concerns; override nothing.
/// </para>
/// <para>
/// <b>Legacy override-based use:</b>
/// Derive from this class and override <see cref="GetLatitude"/>, <see cref="GetLongitude"/>,
/// and <see cref="CreatePolyline"/> to provide type-specific behaviour. These overrides take
/// priority over any registered formatter.
/// </para>
/// </remarks>
/// <typeparam name="TCoordinate">The type that represents a geographic coordinate to encode.</typeparam>
/// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
public class AbstractPolylineEncoder<TCoordinate, TPolyline> : IPolylineEncoder<TCoordinate, TPolyline> {
    private readonly ILogger<AbstractPolylineEncoder<TCoordinate, TPolyline>> _logger;
    private readonly IPolylineValueFormatter<TCoordinate>? _valueFormatter;
    private readonly IPolylineFormatter<TPolyline>? _polylineFormatter;

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
    /// Initializes a new instance of the <see cref="AbstractPolylineEncoder{TCoordinate, TPolyline}"/> class
    /// using the supplied <see cref="PolylineOptions{TCoordinate, TPolyline}"/>.
    /// </summary>
    /// <remarks>
    /// Use this constructor when you want formatter-driven encoding without subclassing.
    /// The <see cref="GetLatitude"/>, <see cref="GetLongitude"/>, and <see cref="CreatePolyline"/> hooks
    /// are not called; all type-specific logic is delegated to the formatters.
    /// </remarks>
    /// <param name="options">
    /// A <see cref="PolylineOptions{TCoordinate, TPolyline}"/> that carries both the value formatter and
    /// the polyline formatter together with the underlying <see cref="PolylineEncodingOptions"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public AbstractPolylineEncoder(PolylineOptions<TCoordinate, TPolyline> options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        Options = options.Encoding;
        _valueFormatter = options.ValueFormatter;
        _polylineFormatter = options.PolylineFormatter;
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
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the encoding operation.
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
    public TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Encode);

        _logger
            .LogOperationStartedDebug(OperationName);

        Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

        ValidateEmptyCoordinates(ref coordinates, _logger);

        if (_valueFormatter is not null && _polylineFormatter is not null) {
            return EncodeWithFormatter(coordinates, cancellationToken);
        }

        CoordinateDelta delta = new();

        int position = 0;
        int consumed = 0;
        int length = GetMaxBufferLength(coordinates.Length, 2);

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        string encodedResult;

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

            encodedResult = buffer[..position].ToString();
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }

        _logger
            .LogOperationFinishedDebug(OperationName);

        return CreatePolyline(encodedResult.AsMemory());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateEmptyCoordinates(ref ReadOnlySpan<TCoordinate> coordinates, ILogger logger) {
            if (coordinates.Length < 1) {
                logger
                    .LogOperationFailedDebug(OperationName);
                logger
                    .LogEmptyArgumentWarning(nameof(coordinates));

                ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
            }
        }
    }

    /// <summary>
    /// Encodes coordinates using the registered value and polyline formatters.
    /// </summary>
    private TPolyline EncodeWithFormatter(ReadOnlySpan<TCoordinate> coordinates, CancellationToken cancellationToken) {
        const string OperationName = nameof(Encode);
        int width = _valueFormatter!.Width;
        int length = GetMaxBufferLength(coordinates.Length, width);

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        int position = 0;
        int[] previous = new int[width];
        long[] values = new long[width];
        string encodedResult;

        try {
            for (var i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                _valueFormatter.GetValues(coordinates[i], values.AsSpan());

                for (int j = 0; j < width; j++) {
                    int current = (int)values[j];
                    int delta = current - previous[j];
                    previous[j] = current;

                    if (!PolylineEncoding.TryWriteValue(delta, buffer, ref position)) {
                        _logger.LogOperationFailedDebug(OperationName);
                        _logger.LogCannotWriteValueToBufferWarning(position, i);
                        ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
                    }
                }
            }

            encodedResult = buffer[..position].ToString();
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }

        _logger.LogOperationFinishedDebug(OperationName);

        return _polylineFormatter!.Write(encodedResult.AsMemory());
    }

    /// <summary>
    /// Creates a polyline instance from the provided read-only sequence of characters.
    /// </summary>
    /// <param name="polyline">A <see cref="ReadOnlyMemory{T}"/> containing the encoded polyline characters.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded polyline.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown by the default implementation when no polyline formatter is registered and the method
    /// has not been overridden in a derived class.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual TPolyline CreatePolyline(ReadOnlyMemory<char> polyline) {
        throw new NotSupportedException(
            $"Override {nameof(CreatePolyline)} in a derived class, or provide a " +
            $"{nameof(PolylineOptions<TCoordinate, TPolyline>)} with a polyline formatter.");
    }

    /// <summary>
    /// Extracts the longitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the longitude.</param>
    /// <returns>
    /// The longitude value as a <see cref="double"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown by the default implementation when no value formatter is registered and the method
    /// has not been overridden in a derived class.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual double GetLongitude(TCoordinate current) {
        throw new NotSupportedException(
            $"Override {nameof(GetLatitude)} and {nameof(GetLongitude)} in a derived class, or " +
            $"provide a {nameof(PolylineOptions<TCoordinate, TPolyline>)} with a value formatter.");
    }

    /// <summary>
    /// Extracts the latitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the latitude.</param>
    /// <returns>
    /// The latitude value as a <see cref="double"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown by the default implementation when no value formatter is registered and the method
    /// has not been overridden in a derived class.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual double GetLatitude(TCoordinate current) {
        throw new NotSupportedException(
            $"Override {nameof(GetLatitude)} and {nameof(GetLongitude)} in a derived class, or " +
            $"provide a {nameof(PolylineOptions<TCoordinate, TPolyline>)} with a value formatter.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetMaxBufferLength(int count, int valuesPerItem) {
        Debug.Assert(count > 0, "Count must be greater than zero.");
        Debug.Assert(valuesPerItem > 0, "Values per item must be greater than zero.");

        int requestedBufferLength = count * valuesPerItem * Defaults.Polyline.Block.Length.Max;

        Debug.Assert(requestedBufferLength > 0, "Requested buffer length must be greater than zero.");

        return requestedBufferLength;
    }
}
