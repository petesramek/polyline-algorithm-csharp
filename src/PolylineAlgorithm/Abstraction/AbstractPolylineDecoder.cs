//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Diagnostics;
using System.Runtime.CompilerServices;

namespace PolylineAlgorithm.Abstraction;

/// <summary>
/// Provides a base implementation for decoding encoded polyline strings into sequences of geographic coordinates.
/// </summary>
/// <remarks>
/// <para>
/// <b>Formatter-based use (no subclassing required):</b>
/// Supply a <see cref="PolylineOptions{TCoordinate, TPolyline}"/> via the
/// <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}(PolylineOptions{TCoordinate, TPolyline})"/>
/// constructor. The formatters handle all type-specific concerns; override nothing.
/// </para>
/// <para>
/// <b>Legacy override-based use:</b>
/// Derive from this class and override <see cref="GetReadOnlyMemory"/> and <see cref="CreateCoordinate"/>
/// to provide type-specific behaviour. These overrides take priority over any registered formatter.
/// </para>
/// </remarks>
/// <typeparam name="TPolyline">The type that represents the encoded polyline input.</typeparam>
/// <typeparam name="TCoordinate">The type that represents a decoded geographic coordinate.</typeparam>
public class AbstractPolylineDecoder<TPolyline, TCoordinate> : IPolylineDecoder<TPolyline, TCoordinate> {
    private readonly ILogger<AbstractPolylineDecoder<TPolyline, TCoordinate>> _logger;
    private readonly IPolylineValueFormatter<TCoordinate>? _valueFormatter;
    private readonly IPolylineFormatter<TPolyline>? _polylineFormatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/> class with default encoding options.
    /// </summary>
    protected AbstractPolylineDecoder()
        : this(new PolylineEncodingOptions()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions"/> to use for encoding operations.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    protected AbstractPolylineDecoder(PolylineEncodingOptions options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        Options = options;
        _logger = Options
            .LoggerFactory
            .CreateLogger<AbstractPolylineDecoder<TPolyline, TCoordinate>>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TCoordinate}"/> class
    /// using the supplied <see cref="PolylineOptions{TCoordinate, TPolyline}"/>.
    /// </summary>
    /// <remarks>
    /// Use this constructor when you want formatter-driven decoding without subclassing.
    /// The <see cref="GetReadOnlyMemory"/> and <see cref="CreateCoordinate"/> hooks are not called;
    /// all type-specific logic is delegated to the formatters.
    /// </remarks>
    /// <param name="options">
    /// A <see cref="PolylineOptions{TCoordinate, TPolyline}"/> that carries both the value formatter and
    /// the polyline formatter together with the underlying <see cref="PolylineEncodingOptions"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public AbstractPolylineDecoder(PolylineOptions<TCoordinate, TPolyline> options) {
        if (options is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(options));
        }

        Options = options.Encoding;
        _polylineFormatter = options.PolylineFormatter;
        _valueFormatter = options.ValueFormatter;
        _logger = Options
            .LoggerFactory
            .CreateLogger<AbstractPolylineDecoder<TPolyline, TCoordinate>>();
    }

    /// <summary>
    /// Gets the encoding options used by this polyline decoder.
    /// </summary>
    public PolylineEncodingOptions Options { get; }

    /// <summary>
    /// Decodes an encoded <typeparamref name="TPolyline"/> into a sequence of <typeparamref name="TCoordinate"/> instances,
    /// with support for cancellation.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline string to decode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the decoding operation.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="polyline"/> is empty.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline format is invalid or malformed at a specific position.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> is canceled during decoding.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "Method contains two path implementations.")]
    public IEnumerable<TCoordinate> Decode(TPolyline polyline, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Decode);

        _logger?.LogOperationStartedDebug(OperationName);

        ValidateNullPolyline(polyline, _logger);

        ReadOnlyMemory<char> sequence = (_valueFormatter is not null && _polylineFormatter is not null)
            ? _polylineFormatter.Read(polyline)
            : GetReadOnlyMemory(in polyline);

        ValidateSequence(sequence, _logger);
        ValidateFormat(sequence, _logger);

        int position = 0;

        if (_valueFormatter is not null && _polylineFormatter is not null) {
            int width = _valueFormatter.Width;
            int[] accumulated = new int[width];
            long[] longValues = new long[width];

            try {
                while (position < sequence.Length) {
                    cancellationToken.ThrowIfCancellationRequested();

                    for (int j = 0; j < width; j++) {
                        if (!PolylineEncoding.TryReadValue(ref accumulated[j], sequence, ref position)) {
                            _logger?.LogOperationFailedDebug(OperationName);
                            _logger?.LogInvalidPolylineWarning(position);
                            ExceptionGuard.ThrowInvalidPolylineFormat(position);
                        }
                    }

                    for (int j = 0; j < width; j++) {
                        longValues[j] = accumulated[j];
                    }

                    yield return _valueFormatter.CreateItem(longValues.AsSpan());
                }
            } finally {
                _logger?.LogOperationFinishedDebug(OperationName);
            }
        } else {
            int encodedLatitude = 0;
            int encodedLongitude = 0;

            try {
                while (position < sequence.Length) {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (!PolylineEncoding.TryReadValue(ref encodedLatitude, sequence, ref position)
                        || !PolylineEncoding.TryReadValue(ref encodedLongitude, sequence, ref position)) {
                        _logger?.LogOperationFailedDebug(OperationName);
                        _logger?.LogInvalidPolylineWarning(position);

                        ExceptionGuard.ThrowInvalidPolylineFormat(position);
                    }

                    double decodedLatitude = PolylineEncoding.Denormalize(encodedLatitude, Options.Precision);
                    double decodedLongitude = PolylineEncoding.Denormalize(encodedLongitude, Options.Precision);

                    _logger?.LogDecodedCoordinateDebug(decodedLatitude, decodedLongitude, position);

                    yield return CreateCoordinate(decodedLatitude, decodedLongitude);
                }
            } finally {
                _logger?.LogOperationFinishedDebug(OperationName);
            }
        }
    }

    /// <summary>
    /// Validates that the provided polyline is not <see langword="null"/>.
    /// </summary>
    /// <param name="polyline">The polyline instance to validate.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> used to log a warning when validation fails.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateNullPolyline(TPolyline polyline, ILogger? logger) {
        if (polyline is null) {
            logger?.LogNullArgumentWarning(nameof(polyline));
            ExceptionGuard.ThrowArgumentNull(nameof(polyline));
        }
    }

    /// <summary>
    /// Validates that the polyline character sequence meets the minimum required length.
    /// </summary>
    /// <param name="polylineSequence">The polyline character sequence to validate.</param>
    /// <param name="logger">An optional <see cref="ILogger"/> used to log diagnostic messages when validation fails.</param>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when <paramref name="polylineSequence"/> is shorter than the minimum allowed length.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ValidateSequence(ReadOnlyMemory<char> polylineSequence, ILogger? logger) {
        if (polylineSequence.Length < Defaults.Polyline.Block.Length.Min) {
            logger?.LogOperationFailedDebug(nameof(Decode));
            logger?.LogPolylineCannotBeShorterThanWarning(polylineSequence.Length, Defaults.Polyline.Block.Length.Min);

            ExceptionGuard.ThrowInvalidPolylineLength(polylineSequence.Length, Defaults.Polyline.Block.Length.Min);
        }
    }

    /// <summary>
    /// Validates the format of the polyline character sequence, ensuring all characters are within the allowed range.
    /// </summary>
    /// <param name="sequence">
    /// The read-only memory region of characters representing the polyline to validate.
    /// </param>
    /// <param name="logger">
    /// An optional <see cref="ILogger"/> used to log a warning when format validation fails.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the polyline contains characters outside the valid encoding range or has an invalid block structure.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void ValidateFormat(ReadOnlyMemory<char> sequence, ILogger? logger) {
        try {
            PolylineEncoding.ValidateFormat(sequence.Span);
        } catch (ArgumentException ex) {
            logger?.LogInvalidPolylineFormatWarning(ex);

            throw;
        }
    }

    /// <summary>
    /// Extracts the underlying read-only memory region of characters from the specified polyline instance.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance from which to extract the character sequence.
    /// </param>
    /// <returns>
    /// A <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> representing the encoded polyline characters.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown by the default implementation when no polyline formatter is registered and the method
    /// has not been overridden in a derived class.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual ReadOnlyMemory<char> GetReadOnlyMemory(in TPolyline polyline) {
        throw new NotSupportedException(
            $"Override {nameof(GetReadOnlyMemory)} in a derived class, or provide a " +
            $"{nameof(PolylineOptions<TCoordinate, TPolyline>)} with a polyline formatter.");
    }

    /// <summary>
    /// Creates a <typeparamref name="TCoordinate"/> instance from the specified latitude and longitude values.
    /// </summary>
    /// <param name="latitude">
    /// The latitude component of the coordinate, in degrees.
    /// </param>
    /// <param name="longitude">
    /// The longitude component of the coordinate, in degrees.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TCoordinate"/> instance representing the specified geographic coordinate.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown by the default implementation when no value formatter is registered and the method
    /// has not been overridden in a derived class.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual TCoordinate CreateCoordinate(double latitude, double longitude) {
        throw new NotSupportedException(
            $"Override {nameof(CreateCoordinate)} in a derived class, or provide a " +
            $"{nameof(PolylineOptions<TCoordinate, TPolyline>)} with a value formatter.");
    }
}
