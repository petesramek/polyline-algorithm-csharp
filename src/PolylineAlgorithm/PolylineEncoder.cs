//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
/// Encodes sequences of values into encoded polyline representations.
/// </summary>
/// <typeparam name="TValue">The type that represents a value to encode.</typeparam>
/// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
/// <remarks>
/// Pass a <see cref="PolylineOptions{TValue, TPolyline}"/> that carries a
/// <see cref="IPolylineFormatter{TValue, TPolyline}"/> to the constructor. The formatter handles
/// all type-specific concerns; no subclassing is required.
/// </remarks>
public class PolylineEncoder<TValue, TPolyline> : IPolylineEncoder<TValue, TPolyline> {
    private readonly IPolylineFormatter<TValue, TPolyline> _formatter;
    private readonly PolylineOptions<TValue, TPolyline> _options;
    private readonly ILogger<PolylineEncoder<TValue, TPolyline>> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineEncoder{TValue, TPolyline}"/>.
    /// </summary>
    /// <param name="options">
    /// A <see cref="PolylineOptions{TValue, TPolyline}"/> that carries the formatter and settings.
    /// Must not be <see langword="null"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Null is verified before use via ExceptionGuard.ThrowArgumentNull, which is annotated [DoesNotReturn]. CA1062 does not recognise custom [DoesNotReturn] helpers as null guards.")]
    public PolylineEncoder(PolylineOptions<TValue, TPolyline> options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        _options = options;
        _formatter = options.Formatter;
        _logger = options.LoggerFactory.CreateLogger<PolylineEncoder<TValue, TPolyline>>();
    }

    /// <summary>
    /// Encodes a collection of <typeparamref name="TValue"/> instances into an encoded
    /// <typeparamref name="TPolyline"/>.
    /// </summary>
    /// <param name="coordinates">The collection of values to encode.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the internal encoding buffer cannot accommodate the encoded value.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    public TPolyline Encode(IEnumerable<TValue> coordinates, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Encode);

        _logger.LogOperationStartedDebug(OperationName);

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

        IReadOnlyList<TValue> items = coordinates as IReadOnlyList<TValue> ?? [.. coordinates];

        if (items.Count < 1) {
            _logger.LogOperationFailedDebug(OperationName);
            _logger.LogEmptyArgumentWarning(nameof(coordinates));
            ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
        }

        int width = _formatter.Width;
        int length = GetMaxBufferLength(items.Count, width);

        char[]? temp = length <= _options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        int position = 0;
        long[] previous = new long[width];
        long[] values = new long[width];

        SeedPrevious(previous, null);

        try {
            for (int i = 0; i < items.Count; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                _formatter.GetValues(items[i], values.AsSpan());

                for (int j = 0; j < width; j++) {
                    long current = values[j];
                    long delta = current - previous[j];
                    previous[j] = current;

                    if (!PolylineEncoding.TryWriteValue(delta, buffer, ref position)) {
                        _logger.LogOperationFailedDebug(OperationName);
                        _logger.LogCannotWriteValueToBufferWarning(position, i);
                        ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
                    }
                }
            }

            // Convert to string inside the try block so the buffer is still valid.
            string encodedResult = buffer[..position].ToString();

            _logger.LogOperationFinishedDebug(OperationName);

            return _formatter.Write(encodedResult.AsMemory());
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }
    }

    /// <summary>
    /// Encodes a collection of <typeparamref name="TValue"/> instances into an encoded
    /// <typeparamref name="TPolyline"/>, applying per-call <paramref name="options"/> to control the
    /// delta baseline. Use this overload to encode large sequences in independent chunks that can be
    /// concatenated into a single valid polyline.
    /// </summary>
    /// <param name="coordinates">The collection of values to encode.</param>
    /// <param name="options">
    /// Per-call options that control the starting delta baseline. Pass <see langword="null"/> or an
    /// instance with <see cref="PolylineEncodingOptions{TValue}.Previous"/> set to
    /// <see langword="null"/> to use the formatter's default baseline (same as calling
    /// <see cref="Encode(IEnumerable{TValue}, CancellationToken)"/>).
    /// </param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="coordinates"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the internal encoding buffer cannot accommodate the encoded value.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    public TPolyline Encode(IEnumerable<TValue> coordinates, PolylineEncodingOptions<TValue>? options, CancellationToken cancellationToken) {
        const string OperationName = nameof(Encode);

        _logger.LogOperationStartedDebug(OperationName);

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

        IReadOnlyList<TValue> items = coordinates as IReadOnlyList<TValue> ?? [.. coordinates];

        if (items.Count < 1) {
            _logger.LogOperationFailedDebug(OperationName);
            _logger.LogEmptyArgumentWarning(nameof(coordinates));
            ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
        }

        int width = _formatter.Width;
        int length = GetMaxBufferLength(items.Count, width);

        char[]? temp = length <= _options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        int position = 0;
        long[] previous = new long[width];
        long[] values = new long[width];

        SeedPrevious(previous, options);

        try {
            for (int i = 0; i < items.Count; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                _formatter.GetValues(items[i], values.AsSpan());

                for (int j = 0; j < width; j++) {
                    long current = values[j];
                    long delta = current - previous[j];
                    previous[j] = current;

                    if (!PolylineEncoding.TryWriteValue(delta, buffer, ref position)) {
                        _logger.LogOperationFailedDebug(OperationName);
                        _logger.LogCannotWriteValueToBufferWarning(position, i);
                        ExceptionGuard.ThrowCouldNotWriteEncodedValueToBuffer();
                    }
                }
            }

            string encodedResult = buffer[..position].ToString();

            _logger.LogOperationFinishedDebug(OperationName);

            return _formatter.Write(encodedResult.AsMemory());
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }
    }

    private void SeedPrevious(long[] previous, PolylineEncodingOptions<TValue>? options) {
        int width = _formatter.Width;

        if (options is { HasPrevious: true }) {
            _formatter.GetValues(options.Previous, previous.AsSpan());
        } else {
            for (int j = 0; j < width; j++) {
                previous[j] = _formatter.GetBaseline(j);
            }
        }
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
