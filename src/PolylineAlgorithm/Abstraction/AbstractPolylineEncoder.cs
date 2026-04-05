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
        Span<double> doubleValues = stackalloc double[valuesPerItem];
        Span<int> intValues = stackalloc int[valuesPerItem];

        string encodedResult;

        try {
            for (var i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                GetValues(coordinates[i], doubleValues);

                for (int v = 0; v < valuesPerItem; v++) {
                    intValues[v] = PolylineEncoding.Normalize(doubleValues[v], Options.Precision);
                }

                delta.Next(intValues);

                bool writeSucceeded = true;
                for (int v = 0; v < valuesPerItem; v++) {
                    if (!PolylineEncoding.TryWriteValue(delta.Deltas[v], buffer, ref position)) {
                        writeSucceeded = false;
                        break;
                    }
                }

                if (!writeSucceeded) {
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
        static int GetMaxBufferLength(int count, int valuesPerItem) {
            Debug.Assert(count > 0, "Count must be greater than zero.");
            Debug.Assert(valuesPerItem > 0, "ValuesPerItem must be greater than zero.");

            int requestedBufferLength = count * valuesPerItem * Defaults.Polyline.Block.Length.Max;

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
    /// Gets the number of values extracted from each <typeparamref name="TCoordinate"/> item during encoding.
    /// </summary>
    /// <remarks>
    /// Must be greater than zero. Each value is written as a separate delta-encoded block in the output polyline.
    /// For a standard geographic coordinate pair (latitude + longitude), return <c>2</c>.
    /// </remarks>
    protected abstract int ValuesPerItem { get; }

    /// <summary>
    /// Extracts the encoded values from the specified item into the provided span.
    /// </summary>
    /// <param name="item">The item from which to extract values.</param>
    /// <param name="values">
    /// A span that receives the extracted values. Its length equals <see cref="ValuesPerItem"/>.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract void GetValues(TCoordinate item, Span<double> values);
}

