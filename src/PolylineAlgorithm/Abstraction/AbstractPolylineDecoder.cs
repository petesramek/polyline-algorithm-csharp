//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Diagnostics;
using System.Runtime.CompilerServices;

namespace PolylineAlgorithm.Abstraction {
    /// <summary>
    /// Decodes encoded polyline strings into sequences of geographic coordinates.
    /// Implements the <see cref="IPolylineDecoder{TPolyline, TCoordinate}"/> interface.
    /// </summary>
    /// <remarks>
    /// This abstract class provides a base implementation for decoding polylines, allowing subclasses to define how to handle specific polyline formats.
    /// </remarks>
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
        /// Thrown when <paramref name="options"/> is <see langword="null" />
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
        /// Decodes an encoded polyline with cancellation support.
        /// </summary>
        /// <param name="polyline">The encoded polyline.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Decoded coordinates.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidPolylineException"/>
        public IEnumerable<TCoordinate> Decode(TPolyline polyline, CancellationToken cancellationToken = default) {
            const string OperationName = nameof(Decode);

            _logger?.LogOperationStartedDebug(OperationName);

            ValidateNullPolyline(polyline, _logger);

            ReadOnlyMemory<char> sequence = GetReadOnlyMemory(in polyline);

            ValidateSequence(sequence, _logger);
            ValidateFormat(sequence, _logger);

            int position = 0;
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

        /// <summary>
        /// Validates that the provided polyline is not <see langword="null"/>.
        /// Throws an <see cref="ArgumentNullException"/> if the polyline is <see langword="null"/>.
        /// Optionally logs a warning if a logger is provided.
        /// </summary>
        /// <param name="polyline">The polyline instance to validate.</param>
        /// <param name="logger">Optional logger for diagnostic messages.</param>
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
        /// Validates that the polyline sequence meets the minimum required length.
        /// Throws an <see cref="InvalidPolylineException"/> if the sequence is too short.
        /// Optionally logs diagnostic messages if a logger is provided.
        /// </summary>
        /// <param name="polylineSequence">The polyline character sequence to validate.</param>
        /// <param name="logger">Optional logger for diagnostic messages.</param>
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
        /// Validates the polyline format for allowed characters.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void ValidateFormat(ReadOnlyMemory<char> sequence, ILogger? logger) {
            try {
                PolylineEncoding.ValidateFormat(sequence.Span);
            } catch (ArgumentException ex) {
                logger?.LogInvalidPolylineFormatWarning(ex);

                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract ReadOnlyMemory<char> GetReadOnlyMemory(in TPolyline polyline);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract TCoordinate CreateCoordinate(double latitude, double longitude);
    }
}