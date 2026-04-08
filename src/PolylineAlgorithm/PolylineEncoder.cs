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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
/// Encodes sequences of geographic coordinates into encoded polyline representations.
/// </summary>
/// <typeparam name="TCoordinate">The type that represents a geographic coordinate to encode.</typeparam>
/// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
/// <remarks>
/// Pass a <see cref="PolylineOptions{TCoordinate, TPolyline}"/> that carries a
/// <see cref="IPolylineFormatter{TCoordinate, TPolyline}"/> to the constructor. The formatter handles
/// all type-specific concerns; no subclassing is required.
/// </remarks>
public class PolylineEncoder<TCoordinate, TPolyline> : IPolylineEncoder<TCoordinate, TPolyline> {
    private readonly IPolylineFormatter<TCoordinate, TPolyline> _formatter;
    private readonly PolylineOptions<TCoordinate, TPolyline> _options;
    private readonly ILogger<PolylineEncoder<TCoordinate, TPolyline>> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineEncoder{TCoordinate, TPolyline}"/>.
    /// </summary>
    /// <param name="options">
    /// A <see cref="PolylineOptions{TCoordinate, TPolyline}"/> that carries the formatter and settings.
    /// Must not be <see langword="null"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Null is verified before use via ExceptionGuard.ThrowArgumentNull, which is annotated [DoesNotReturn]. CA1062 does not recognise custom [DoesNotReturn] helpers as null guards.")]
    public PolylineEncoder(PolylineOptions<TCoordinate, TPolyline> options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        _options = options;
        _formatter = options.Formatter;
        _logger = options.LoggerFactory.CreateLogger<PolylineEncoder<TCoordinate, TPolyline>>();
    }

    /// <summary>
    /// Encodes a collection of <typeparamref name="TCoordinate"/> instances into an encoded
    /// <typeparamref name="TPolyline"/>.
    /// </summary>
    /// <param name="coordinates">The collection of coordinates to encode.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded coordinates.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the internal encoding buffer cannot accommodate the encoded value.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled.
    /// </exception>
    public TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Encode);

        _logger.LogOperationStartedDebug(OperationName);

        Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

        if (coordinates.Length < 1) {
            _logger.LogOperationFailedDebug(OperationName);
            _logger.LogEmptyArgumentWarning(nameof(coordinates));
            ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
        }

        int width = _formatter.Width;
        int length = GetMaxBufferLength(coordinates.Length, width);

        char[]? temp = length <= _options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        int position = 0;
        long[] previous = new long[width];
        long[] values = new long[width];

        for (int j = 0; j < width; j++) {
            previous[j] = _formatter.GetBaseline(j);
        }

        try {
            for (int i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                _formatter.GetValues(coordinates[i], values.AsSpan());

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetMaxBufferLength(int count, int valuesPerItem) {
        Debug.Assert(count > 0, "Count must be greater than zero.");
        Debug.Assert(valuesPerItem > 0, "Values per item must be greater than zero.");

        int requestedBufferLength = count * valuesPerItem * Defaults.Polyline.Block.Length.Max;

        Debug.Assert(requestedBufferLength > 0, "Requested buffer length must be greater than zero.");

        return requestedBufferLength;
    }
}
