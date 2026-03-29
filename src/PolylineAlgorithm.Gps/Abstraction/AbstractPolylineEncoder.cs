//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Gps.Abstraction;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm;
using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Internal;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides functionality to encode a collection of geographic coordinates into an encoded polyline string.
/// Implements the <see cref="IPolylineEncoder{TValue, TPolyline}"/> interface.
/// </summary>
/// <remarks>
/// This abstract class serves as a base for specific polyline encoders, allowing customization of the encoding process.
/// Implementations supply how to extract latitude/longitude from a coordinate type (<typeparamref name="TValue"/>)
/// and how to produce a polyline representation (<typeparamref name="TPolyline"/>).
///
/// The encoder is designed to be efficient for typical usage patterns:
/// - It normalizes coordinates using <see cref="PolylineEncoding.Normalize(double, uint)"/> and encodes delta values.
/// - It writes encoded characters into a temporary char buffer which is either stack-allocated or rented from <see cref="ArrayPool{T}"/>.
/// - Logging is performed through the configured <see cref="PolylineEncodingOptions.LoggerFactory"/>.
///
/// Thread-safety: instances may be used concurrently only if the concrete implementation of <see cref="CreatePolyline(ReadOnlyMemory{char})"/>
/// and the provided <typeparamref name="TValue"/> handling are themselves thread-safe. The encoder does not store per-encode state
/// across calls (state is local to the call).
/// </remarks>
public abstract class AbstractPolylineEncoder<TValue, TPolyline> : IPolylineEncoder<TValue, TPolyline> {
    private readonly ILogger<AbstractPolylineEncoder<TValue, TPolyline>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineEncoder{TValue, TPolyline}"/> class with default encoding options.
    /// </summary>
    protected AbstractPolylineEncoder()
        : this(new PolylineEncodingOptions()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineEncoder{TValue, TPolyline}"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions"/> to use for encoding operations (precision, stack allocation limit, logger factory, etc.).
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null" />.</exception>
    protected AbstractPolylineEncoder(PolylineEncodingOptions options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        Options = options;
        _logger = Options
            .LoggerFactory
            .CreateLogger<AbstractPolylineEncoder<TValue, TPolyline>>();
    }

    /// <summary>
    /// Gets the encoding options used by this polyline encoder.
    /// </summary>
    /// <remarks>
    /// Options control behavior such as numeric precision, whether buffers are stack-allocated or pooled,
    /// and provide a <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/> for logging.
    /// </remarks>
    public PolylineEncodingOptions Options { get; }

    /// <summary>
    /// Encodes a collection of <typeparamref name="TValue"/> instances into an encoded <typeparamref name="TPolyline"/> string.
    /// </summary>
    /// <param name="coordinates">
    /// The collection of <typeparamref name="TValue"/> objects to encode. Must contain at least one element.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the operation. The token is observed at the start of the method.
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
    /// <exception cref="InternalBufferOverflowException">
    /// Thrown when the internal buffer cannot accept the encoded characters (should be prevented by sizing heuristics; included for completeness).
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an internal encoding invariant is violated. The method logs failure details before throwing.
    /// </exception>
    /// <remarks>
    /// Implementation notes:
    /// - Coordinates are first normalized to integer values based on <see cref="PolylineEncoding.Normalize(double,uint)"/> and the configured precision.
    /// - Deltas between consecutive coordinates are encoded; latitude and longitude deltas are written sequentially.
    /// - To avoid excessive heap allocations, a char buffer is created either on the stack using <c>stackalloc</c> when the requested
    ///   buffer length is less than or equal to <see cref="PolylineEncodingOptions.StackAllocLimit"/>, or rented from the shared
    ///   <see cref="ArrayPool{T}"/> otherwise. Rented buffers are returned in a finally block to ensure they are always returned.
    /// - Detailed operation start/finish and error conditions are logged at debug level using the configured logger.
    /// </remarks>
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Method contains local methods. Actual method only 55 lines.")]
    public TPolyline Encode(ReadOnlySpan<TValue> coordinates, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Encode);

        char[]? temp = null;

        try {
            cancellationToken.ThrowIfCancellationRequested();

            _logger
                .LogOperationStartedDebug(OperationName);

            Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

            ValidateEmptyCoordinates(ref coordinates, _logger);

            CoordinateDelta delta = new();

            int position = 0;
            int consumed = 0;
            int length = GetMaxBufferLength(coordinates.Length);

            temp = length <= Options.StackAllocLimit
                ? null
                : ArrayPool<char>.Shared.Rent(length);

            Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);
            for (var i = 0; i < coordinates.Length; i++) {

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

            _logger
                .LogOperationFinishedDebug(OperationName);

            return CreatePolyline(buffer[..position].ToString().AsMemory());
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }

        // Returns the maximum number of characters needed to encode `count` coordinates.
        // Calculation is conservative: each coordinate may produce up to two encoded values (lat & lon),
        // and each encoded value may require up to Defaults.Polyline.Block.Length.Max characters.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetMaxBufferLength(int count) {
            Debug.Assert(count > 0, "Count must be greater than zero.");

            const int maxBlockLength = 7;

            int requestedBufferLength = count * 2 * maxBlockLength;

            Debug.Assert(requestedBufferLength > 0, "Requested buffer length must be greater than zero.");

            return requestedBufferLength;
        }

        // Validates that the provided coordinates span is not empty.
        // Logs a failure and throws a descriptive exception if the span contains zero elements.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateEmptyCoordinates(ref ReadOnlySpan<TValue> coordinates, ILogger logger) {
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
    /// Creates a polyline instance from the provided read-only sequence of characters.
    /// </summary>
    /// <param name="polyline">A <see cref="ReadOnlyMemory{T}"/> containing the encoded polyline characters.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded polyline.
    /// </returns>
    /// <remarks>
    /// Concrete implementations control how the encoded memory is represented (for example, string, custom struct, or immutable wrapper).
    /// The memory provided is the exact encoded sequence length (no extra unused characters).
    /// </remarks>
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
    protected abstract double GetLongitude(TValue current);

    /// <summary>
    /// Extracts the latitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">The coordinate from which to extract the latitude.</param>
    /// <returns>
    /// The latitude value as a <see cref="double"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract double GetLatitude(TValue current);
}