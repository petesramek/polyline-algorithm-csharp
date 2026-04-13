//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Buffers;
using System.Collections.Generic;
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
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Null is verified before use via ExceptionGuard.ThrowArgumentNull, which is annotated [DoesNotReturn]. CA1062 does not recognise custom [DoesNotReturn] helpers as null guards.")]
    public TPolyline Encode(IEnumerable<TValue> coordinates, CancellationToken cancellationToken = default) {
        _logger.LogOperationStartedDebug(nameof(Encode));

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

        return EncodeCore(coordinates, null, cancellationToken);
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
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Null is verified before use via ExceptionGuard.ThrowArgumentNull, which is annotated [DoesNotReturn]. CA1062 does not recognise custom [DoesNotReturn] helpers as null guards.")]
    public TPolyline Encode(IEnumerable<TValue> coordinates, PolylineEncodingOptions<TValue>? options, CancellationToken cancellationToken) {
        _logger.LogOperationStartedDebug(nameof(Encode));

        if (coordinates is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(coordinates));
        }

        return EncodeCore(coordinates, options, cancellationToken);
    }

    private TPolyline EncodeCore(IEnumerable<TValue> coordinates, PolylineEncodingOptions<TValue>? options, CancellationToken cancellationToken) {
        const string OperationName = nameof(Encode);
        const int MaxStackWidth = 8;

        int width = _formatter.Width;

        Span<long> previous = width <= MaxStackWidth ? stackalloc long[MaxStackWidth] : new long[width];
        Span<long> values   = width <= MaxStackWidth ? stackalloc long[MaxStackWidth] : new long[width];
        previous = previous[..width];
        values   = values[..width];

        SeedPrevious(previous, options);

        int stackLimit = _options.StackAllocLimit;
        Span<char> buffer = stackalloc char[stackLimit];
        char[]? rented = null;

        int position = 0;
        bool anyItemProcessed = false;

        try {
            foreach (TValue item in coordinates) {
                cancellationToken.ThrowIfCancellationRequested();
                anyItemProcessed = true;
                _formatter.GetValues(item, values);

                for (int j = 0; j < width; j++) {
                    long current = values[j];
                    long delta = current - previous[j];
                    previous[j] = current;

                    while (!PolylineEncoding.TryWriteValue(delta, buffer, ref position)) {
                        int newSize = rented is null ? stackLimit * 2 : rented.Length * 2;
                        char[] newRented = ArrayPool<char>.Shared.Rent(newSize);
                        buffer[..position].CopyTo(newRented);
                        if (rented is not null) {
                            ArrayPool<char>.Shared.Return(rented);
                        }
                        rented = newRented;
                        buffer = rented.AsSpan();
                    }
                }
            }

            if (!anyItemProcessed) {
                _logger.LogOperationFailedDebug(OperationName);
                _logger.LogEmptyArgumentWarning(nameof(coordinates));
                ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
            }

            string encodedResult = buffer[..position].ToString();

            _logger.LogOperationFinishedDebug(OperationName);

            return _formatter.Write(encodedResult.AsMemory());
        } finally {
            if (rented is not null) {
                ArrayPool<char>.Shared.Return(rented);
            }
        }
    }

    private void SeedPrevious(Span<long> previous, PolylineEncodingOptions<TValue>? options) {
        if (options is { HasPrevious: true }) {
            _formatter.GetValues(options.Previous, previous);
        } else {
            for (int j = 0; j < previous.Length; j++) {
                previous[j] = _formatter.GetBaseline(j);
            }
        }
    }
}
