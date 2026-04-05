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
/// Provides a base implementation for encoding sequences of items into encoded polyline strings.
/// </summary>
/// <remarks>
/// Derive from this class to implement an encoder for a specific item and polyline type. Override
/// <see cref="ValuesPerItem"/>, <see cref="GetValues"/>, and <see cref="CreatePolyline"/> to provide type-specific behavior.
/// <para>
/// The polyline format encodes each item as a fixed-length run of <see cref="ValuesPerItem"/> delta-compressed
/// values. All items in a single polyline must have the same number of values. For example, a 2D GPS encoder
/// sets <see cref="ValuesPerItem"/> to 2 (latitude, longitude), while a 3D GPS encoder sets it to 3
/// (latitude, longitude, altitude).
/// </para>
/// </remarks>
/// <typeparam name="TCoordinate">The type that represents an item to encode.</typeparam>
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
    /// Gets the number of values that make up a single encoded item.
    /// </summary>
    /// <remarks>
    /// Override this property to specify the arity of each item. For example, return <c>2</c> for
    /// latitude/longitude pairs, <c>3</c> for latitude/longitude/altitude triples, or any other count
    /// that matches your encoding scheme.
    /// </remarks>
    protected abstract int ValuesPerItem { get; }

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

        int valuesPerItem = ValuesPerItem;
        CoordinateDelta delta = new(valuesPerItem);

        int position = 0;
        int consumed = 0;
        int length = GetMaxBufferLength(coordinates.Length, valuesPerItem);

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        int[]? normalizedRent = ArrayPool<int>.Shared.Rent(valuesPerItem);
        Span<int> normalized = normalizedRent.AsSpan(0, valuesPerItem);

        double[]? valuesRent = ArrayPool<double>.Shared.Rent(valuesPerItem);
        Span<double> values = valuesRent.AsSpan(0, valuesPerItem);

        string encodedResult;

        try {
            for (var i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                GetValues(coordinates[i], values);

                for (int j = 0; j < valuesPerItem; j++) {
                    normalized[j] = PolylineEncoding.Normalize(values[j], Options.Precision);
                }

                delta.Next(normalized);

                ReadOnlySpan<int> deltas = delta.Deltas;
                for (int j = 0; j < deltas.Length; j++) {
                    if (!PolylineEncoding.TryWriteValue(deltas[j], buffer, ref position)) {
                        // This shouldn't happen, but if it does, log the error and throw an exception.
                        _logger
                            .LogOperationFailedDebug(OperationName);
                        _logger
                            .LogCannotWriteValueToBufferWarning(position, consumed);

                        ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
                    }
                }

                consumed++;
            }

            encodedResult = buffer[..position].ToString();
        } finally {
            ArrayPool<double>.Shared.Return(valuesRent!);
            ArrayPool<int>.Shared.Return(normalizedRent!);
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }

        _logger
            .LogOperationFinishedDebug(OperationName);

        return CreatePolyline(encodedResult.AsMemory());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetMaxBufferLength(int count, int perItem) {
            Debug.Assert(count > 0, "Count must be greater than zero.");

            int requestedBufferLength = count * perItem * Defaults.Polyline.Block.Length.Max;

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract TPolyline CreatePolyline(ReadOnlyMemory<char> polyline);

    /// <summary>
    /// Fills <paramref name="destination"/> with the values to encode for the specified item.
    /// </summary>
    /// <param name="item">The item from which to extract values.</param>
    /// <param name="destination">
    /// A <see cref="Span{T}"/> of length <see cref="ValuesPerItem"/> to fill with the item's values,
    /// in the order they should be encoded.
    /// </param>
    /// <remarks>
    /// Implementations should write exactly <see cref="ValuesPerItem"/> values into <paramref name="destination"/>.
    /// For example, a 2D GPS encoder writes <c>destination[0] = latitude; destination[1] = longitude;</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract void GetValues(TCoordinate item, Span<double> destination);
}

