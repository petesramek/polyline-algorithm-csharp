//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Gps.Internal;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Diagnostics;
using System.Runtime.CompilerServices;

namespace PolylineAlgorithm.Gps.Abstraction {
    /// <summary>
    /// Decodes encoded polyline strings into sequences of geographic coordinates.
    /// Implements the <see cref="IPolylineDecoder{TPolyline, TValue}"/> interface.
    /// </summary>
    /// <remarks>
    /// This abstract class provides a base implementation for decoding polylines, allowing subclasses
    /// to define how to handle specific polyline input types and how decoded coordinates are represented.
    ///
    /// The <see cref="Decode(TPolyline, CancellationToken)"/> method uses deferred execution and yields
    /// coordinates as they are decoded. Consumers should be aware that decoding occurs during enumeration;
    /// any exceptions (format errors, cancellations) are thrown when the returned <see cref="IEnumerable{T}"/>
    /// is iterated.
    ///
    /// The class is safe to use concurrently for decoding different inputs, provided the concrete
    /// implementations of <see cref="GetReadOnlyMemory(in TPolyline)"/> and <see cref="CreateCoordinate(double, double)"/>
    /// are themselves thread-safe.
    /// </remarks>
    public abstract class AbstractPolylineDecoder<TPolyline, TValue> : IPolylineDecoder<TPolyline, TValue> {
        private readonly ILogger<AbstractPolylineDecoder<TPolyline, TValue>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TValue}"/> class with default encoding options.
        /// </summary>
        protected AbstractPolylineDecoder()
            : this(new PolylineEncodingOptions()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPolylineDecoder{TPolyline, TValue}"/> class with the specified encoding options.
        /// </summary>
        /// <param name="options">
        /// The <see cref="PolylineEncodingOptions"/> to use for encoding operations.
        /// This value supplies settings such as numeric precision and a logger factory used to create diagnostic loggers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="options"/> is <see langword="null" />.
        /// </exception>
        protected AbstractPolylineDecoder(PolylineEncodingOptions options) {
            if (options is null) {
                ExceptionGuard.ThrowArgumentNull(nameof(options));
            }

            Options = options;
            _logger = Options
                .LoggerFactory
                .CreateLogger<AbstractPolylineDecoder<TPolyline, TValue>>();
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
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TValue"/> produced by decoding the provided <paramref name="polyline"/>.
        /// The enumeration performs the decoding work lazily; exceptions for invalid input or cancellation are raised while enumerating.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the polyline contains invalid characters or an otherwise invalid format.
        /// </exception>
        /// <exception cref="InvalidPolylineException">
        /// Thrown when the polyline does not meet minimum structural requirements (for example, too short),
        /// or when decoding finds an incomplete coordinate pair.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown when the provided <paramref name="cancellationToken"/> requests cancellation during decoding.
        /// </exception>
        public IEnumerable<TValue> Decode(TPolyline polyline, CancellationToken cancellationToken = default) {
            const string OperationName = nameof(Decode);

            try {
                _logger?.LogOperationStartedDebug(OperationName);

                ValidateNullPolyline(polyline, _logger);

                ReadOnlyMemory<char> sequence = GetReadOnlyMemory(in polyline);

                ValidateSequence(sequence, _logger);
                ValidateFormat(sequence, _logger);

                int position = 0;
                int encodedLatitude = 0;
                int encodedLongitude = 0;

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
            const int minLength = 2;

            if (polylineSequence.Length < minLength) {
                logger?.LogOperationFailedDebug(nameof(Decode));
                logger?.LogPolylineCannotBeShorterThanWarning(polylineSequence.Length, 2);

                ExceptionGuard.ThrowInvalidPolylineLength(polylineSequence.Length, minLength);
            }
        }

        /// <summary>
        /// Validates the polyline format for allowed characters and structural expectations.
        /// </summary>
        /// <param name="sequence">The character sequence representing the polyline to validate.</param>
        /// <param name="logger">Optional logger used to emit diagnostics when validation fails.</param>
        /// <remarks>
        /// The default implementation delegates to <see cref="PolylineEncoding.ValidateFormat(ReadOnlySpan{char})"/>.
        /// Subclasses MAY override this method to accept alternative encodings or to provide additional validation rules.
        /// When validation fails an <see cref="ArgumentException"/> is thrown and a diagnostic warning is logged if a logger is provided.
        /// </remarks>
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
        /// Converts the input <typeparamref name="TPolyline"/> into a <see cref="ReadOnlyMemory{Char}"/> suitable for decoding.
        /// </summary>
        /// <param name="polyline">The polyline input instance. Implementations receive the value by reference to avoid copies where possible.</param>
        /// <returns>
        /// A <see cref="ReadOnlyMemory{Char}"/> containing the characters to decode.
        /// Implementations should avoid allocating when possible and may return a memory slice of the original input.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract ReadOnlyMemory<char> GetReadOnlyMemory(in TPolyline polyline);

        /// <summary>
        /// Creates a coordinate instance of type <typeparamref name="TValue"/> from decoded latitude and longitude.
        /// </summary>
        /// <param name="latitude">Decoded latitude value in decimal degrees.</param>
        /// <param name="longitude">Decoded longitude value in decimal degrees.</param>
        /// <returns>A <typeparamref name="TValue"/> representing the decoded coordinate pair.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract TValue CreateCoordinate(double latitude, double longitude);
    }
}