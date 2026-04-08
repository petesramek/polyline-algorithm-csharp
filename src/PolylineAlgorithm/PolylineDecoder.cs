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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

/// <summary>
/// Decodes encoded polyline representations into sequences of geographic coordinates.
/// </summary>
/// <typeparam name="TPolyline">The type that represents the encoded polyline input.</typeparam>
/// <typeparam name="TCoordinate">The type that represents a decoded geographic coordinate.</typeparam>
/// <remarks>
/// Pass a <see cref="PolylineOptions{TCoordinate, TPolyline}"/> that carries a
/// <see cref="IPolylineFormatter{TCoordinate, TPolyline}"/> to the constructor. The formatter handles
/// all type-specific concerns; no subclassing is required.
/// </remarks>
public class PolylineDecoder<TPolyline, TCoordinate> : IPolylineDecoder<TPolyline, TCoordinate> {
    private readonly IPolylineFormatter<TCoordinate, TPolyline> _formatter;
    private readonly ILogger<PolylineDecoder<TPolyline, TCoordinate>> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineDecoder{TPolyline, TCoordinate}"/>.
    /// </summary>
    /// <param name="options">
    /// A <see cref="PolylineOptions{TCoordinate, TPolyline}"/> that carries the formatter and settings.
    /// Must not be <see langword="null"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Null is verified before use via ExceptionGuard.ThrowArgumentNull, which is annotated [DoesNotReturn]. CA1062 does not recognise custom [DoesNotReturn] helpers as null guards.")]
    public PolylineDecoder(PolylineOptions<TCoordinate, TPolyline> options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        _formatter = options.Formatter;
        _logger = options.LoggerFactory.CreateLogger<PolylineDecoder<TPolyline, TCoordinate>>();
    }

    /// <summary>
    /// Decodes an encoded <typeparamref name="TPolyline"/> into a sequence of
    /// <typeparamref name="TCoordinate"/> instances.
    /// </summary>
    /// <param name="polyline">The encoded polyline to decode. Must not be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded
    /// coordinates in the original order.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline format is invalid or malformed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled during decoding.
    /// </exception>
    public IEnumerable<TCoordinate> Decode(TPolyline polyline, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Decode);

        _logger.LogOperationStartedDebug(OperationName);

        if (polyline is null) {
            _logger.LogNullArgumentWarning(nameof(polyline));
            ExceptionGuard.ThrowArgumentNull(nameof(polyline));
        }

        ReadOnlyMemory<char> sequence = _formatter.Read(polyline);

        if (sequence.Length < Defaults.Polyline.Block.Length.Min) {
            _logger.LogOperationFailedDebug(OperationName);
            _logger.LogPolylineCannotBeShorterThanWarning(sequence.Length, Defaults.Polyline.Block.Length.Min);
            ExceptionGuard.ThrowInvalidPolylineLength(sequence.Length, Defaults.Polyline.Block.Length.Min);
        }

        try {
            PolylineEncoding.ValidateFormat(sequence.Span);
        } catch (ArgumentException ex) {
            _logger.LogInvalidPolylineFormatWarning(ex);
            throw;
        }

        int width = _formatter.Width;
        int[] accumulated = new int[width];
        long[] longValues = new long[width];
        int position = 0;

        try {
            while (position < sequence.Length) {
                cancellationToken.ThrowIfCancellationRequested();

                for (int j = 0; j < width; j++) {
                    if (!PolylineEncoding.TryReadValue(ref accumulated[j], sequence, ref position)) {
                        _logger.LogOperationFailedDebug(OperationName);
                        _logger.LogInvalidPolylineWarning(position);
                        ExceptionGuard.ThrowInvalidPolylineFormat(position);
                    }
                }

                for (int j = 0; j < width; j++) {
                    longValues[j] = accumulated[j];
                }

                yield return _formatter.CreateItem(longValues.AsSpan());
            }
        } finally {
            _logger.LogOperationFinishedDebug(OperationName);
        }
    }
}
