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
/// Provides a base implementation for decoding encoded polyline strings into sequences of items.
/// </summary>
/// <remarks>
/// Derive from this class to implement a decoder for a specific polyline type. Override <see cref="GetReadOnlyMemory"/>
/// and <see cref="Read"/> to provide type-specific behavior.
/// </remarks>
/// <typeparam name="TPolyline">The type that represents the encoded polyline input.</typeparam>
/// <typeparam name="TCoordinate">The type that represents a decoded item.</typeparam>
public abstract class AbstractPolylineDecoder<TPolyline, TCoordinate> : IPolylineDecoder<TPolyline, TCoordinate> {
    private readonly ILogger<AbstractPolylineDecoder<TPolyline, TCoordinate>> _logger;

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
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded items.
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
    public IEnumerable<TCoordinate> Decode(TPolyline polyline, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Decode);

        _logger?.LogOperationStartedDebug(OperationName);

        ValidateNullPolyline(polyline, _logger);

        ReadOnlyMemory<char> sequence = GetReadOnlyMemory(in polyline);

        ValidateSequence(sequence, _logger);
        ValidateFormat(sequence, _logger);

        PolylineReader reader = new(sequence, Options.Precision);

        try {
            while (!reader.IsEmpty) {
                cancellationToken.ThrowIfCancellationRequested();

                reader.BeginItem();
                TCoordinate item = Read(reader);

                _logger?.LogDecodedItemDebug(reader.SlotIndex, reader.Position);

                yield return item;
            }
        } finally {
            _logger?.LogOperationFinishedDebug(OperationName);
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract ReadOnlyMemory<char> GetReadOnlyMemory(in TPolyline polyline);

    /// <summary>
    /// Reads field values from the polyline decoding pipeline and constructs one <typeparamref name="TCoordinate"/> item.
    /// </summary>
    /// <param name="reader">
    /// The <see cref="IPolylineReader"/> cursor provided by the engine. Call <see cref="IPolylineReader.Read"/>
    /// once for each expected field value, in the same order used by the corresponding encoder's
    /// <see cref="Write"/> override.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TCoordinate"/> instance constructed from the decoded field values.
    /// </returns>
    /// <remarks>
    /// Implementations must always call <see cref="IPolylineReader.Read"/> the same number of times,
    /// in the same field order, for every item. The number of reads must match the number of writes
    /// performed by the corresponding encoder's <see cref="Write"/> override.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract TCoordinate Read(IPolylineReader reader);
}
