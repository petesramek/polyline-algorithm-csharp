//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Internal.Logging;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Decodes encoded polyline strings into sequences of geographic coordinates.
/// Implements the <see cref="IPolylineDecoder{TPolyline, TCoordinate}"/> interface.
/// </summary>
/// <remarks>
/// This abstract class provides a base implementation for decoding polylines, allowing subclasses to define how to handle specific polyline formats.
/// </remarks>
public abstract class AbstractPolylineDecoder<TPolyline, TCoordinate> : IPolylineDecoder<TPolyline, TCoordinate>, IAsyncPolylineDecoder<TPolyline, TCoordinate>, IPolylinePipeDecoder<TCoordinate> {
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
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets the encoding options used by this polyline encoder.
    /// </summary>
    public PolylineEncodingOptions Options { get; }

    /// <summary>
    /// Decodes an encoded <typeparamref name="TPolyline"/> into a sequence of <typeparamref name="TCoordinate"/> instances.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline string to decode.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>"
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="polyline"/> is empty.
    /// </exception>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline format is invalid or malformed at a specific position.
    /// </exception>
    public IEnumerable<TCoordinate> Decode(TPolyline polyline) {
        var logger = Options
            .LoggerFactory
            .CreateLogger<AbstractPolylineDecoder<TPolyline, TCoordinate>>();

        logger.
            LogOperationStartedInfo(nameof(Decode));

        ValidateNullPolyline(polyline, logger);

        ReadOnlyMemory<char> sequence = GetReadOnlyMemory(polyline);

        ValidateEmptySequence(logger, sequence);

        int position = 0;
        int latitude = 0;
        int longitude = 0;

        while (true) {
            // Check if we have reached the end of the sequence
            if (sequence.Length == position) {
                break;
            }

            // Read the next value from the polyline encoding
            if (!PolylineEncoding.TryReadValue(ref latitude, ref sequence, ref position)
                || !PolylineEncoding.TryReadValue(ref longitude, ref sequence, ref position)
            ) {
                logger
                    .LogInvalidPolylineWarning(position);
                logger.
                    LogOperationFailedInfo(nameof(Decode));

                InvalidPolylineException.Throw(position);
            }

            yield return CreateCoordinate(PolylineEncoding.Denormalize(latitude, CoordinateValueType.Latitude), PolylineEncoding.Denormalize(longitude, CoordinateValueType.Longitude));
        }

        logger
            .LogOperationFinishedInfo(nameof(Decode));


        static void ValidateNullPolyline(TPolyline polyline, ILogger<AbstractPolylineDecoder<TPolyline, TCoordinate>> logger) {
            if (polyline is null) {
                logger
                    .LogNullArgumentWarning(nameof(polyline));

                throw new ArgumentNullException(nameof(polyline));
            }
        }

        static void ValidateEmptySequence(ILogger<AbstractPolylineDecoder<TPolyline, TCoordinate>> logger, ReadOnlyMemory<char> sequence) {
            if (sequence.Length < Defaults.Polyline.Block.Length.Min) {
                logger
                    .LogPolylineCannotBeShorterThanWarning(nameof(sequence), sequence.Length, Defaults.Polyline.Block.Length.Min);
                logger.
                    LogOperationFailedInfo(nameof(Decode));

                throw new ArgumentException(string.Format(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage, sequence.Length), nameof(polyline));
            }
        }
    }

    /// <summary>
    /// Converts the provided polyline instance into a <see cref="ReadOnlyMemory{T}"/> for decoding.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline data to decode.
    /// </param>
    /// <returns>
    /// A <see cref="ReadOnlyMemory{T}"/> representing the encoded polyline data.
    /// </returns>
    protected abstract ReadOnlyMemory<char> GetReadOnlyMemory(TPolyline polyline);

    /// <summary>
    /// Asynchronously decodes the specified encoded polyline into a sequence of geographic coordinates by
    /// iterating the synchronous <see cref="Decode"/> implementation and checking the cancellation token
    /// between each yielded coordinate.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline string to decode.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while iterating.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded
    /// latitude and longitude pairs.
    /// </returns>
    public async IAsyncEnumerable<TCoordinate> DecodeAsync(
        TPolyline polyline,
        [EnumeratorCancellation] CancellationToken cancellationToken) {

        foreach (TCoordinate coordinate in Decode(polyline)) {
            cancellationToken.ThrowIfCancellationRequested();
            yield return coordinate;
        }
    }

    /// <summary>
    /// Asynchronously decodes encoded polyline bytes read from <paramref name="reader"/> into a sequence of
    /// geographic coordinates with zero intermediate allocations.
    /// </summary>
    /// <remarks>
    /// The method processes the pipe in chunks using <see cref="SequenceReader{T}"/> to handle multi-segment
    /// <see cref="ReadOnlySequence{T}"/> buffers transparently. The pipe reader is not completed by this method.
    /// </remarks>
    /// <param name="reader">
    /// The <see cref="PipeReader"/> from which the encoded polyline bytes are consumed.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for data from the pipe.
    /// </param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TCoordinate"/> representing the decoded
    /// latitude and longitude pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="reader"/> is <see langword="null"/>.
    /// </exception>
    public async IAsyncEnumerable<TCoordinate> DecodeAsync(
        PipeReader reader,
        [EnumeratorCancellation] CancellationToken cancellationToken) {

        if (reader is null) {
            throw new ArgumentNullException(nameof(reader));
        }

        int latitude = 0;
        int longitude = 0;
        bool firstRead = true;

        while (true) {
            ReadResult result = await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            ReadOnlySequence<byte> buffer = result.Buffer;

            if (firstRead && buffer.IsEmpty && result.IsCompleted) {
                throw new ArgumentException(
                    string.Format(ExceptionMessageResource.PolylineCannotBeShorterThanExceptionMessage, 0),
                    nameof(reader));
            }

            firstRead = false;

            // Process the buffer synchronously so that the SequenceReader<byte> (a ref struct) never lives
            // across a yield boundary.
            var decoded = new List<TCoordinate>();
            (SequencePosition consumed, latitude, longitude) = ProcessPipeBuffer(buffer, latitude, longitude, decoded);

            foreach (TCoordinate coordinate in decoded) {
                yield return coordinate;
            }

            // Tell the pipe how far we have consumed and examined.
            reader.AdvanceTo(consumed, buffer.End);

            if (result.IsCompleted) {
                break;
            }
        }
    }

    /// <summary>
    /// Synchronously processes a <see cref="ReadOnlySequence{T}"/> pipe buffer, decoding as many complete
    /// coordinate pairs as possible and returning the updated variance state and the consumed position.
    /// <see cref="System.Buffers.SequenceReader{T}"/> is used here because this method is not an async iterator
    /// and therefore the ref-struct constraint does not apply.
    /// </summary>
    private (SequencePosition consumed, int latitude, int longitude) ProcessPipeBuffer(
        ReadOnlySequence<byte> buffer,
        int latitude,
        int longitude,
        List<TCoordinate> results) {

        var sequenceReader = new SequenceReader<byte>(buffer);
        SequencePosition consumed = buffer.Start;

        while (!sequenceReader.End) {
            // Save state before attempting to decode a coordinate pair so we can roll back if only
            // part of the pair is available in the current buffer.
            SequencePosition pairStart = sequenceReader.Position;
            int savedLatitude = latitude;
            int savedLongitude = longitude;

            if (!PolylineEncoding.TryReadValue(ref latitude, ref sequenceReader)
                || !PolylineEncoding.TryReadValue(ref longitude, ref sequenceReader)) {

                latitude = savedLatitude;
                longitude = savedLongitude;
                break;
            }

            consumed = sequenceReader.Position;
            results.Add(CreateCoordinate(
                PolylineEncoding.Denormalize(latitude, CoordinateValueType.Latitude),
                PolylineEncoding.Denormalize(longitude, CoordinateValueType.Longitude)));
        }

        return (consumed, latitude, longitude);
    }

    /// <summary>
    /// Creates a coordinate instance from the given latitude and longitude values.
    /// </summary>
    /// <param name="latitude">
    /// The latitude value.
    /// </param>
    /// <param name="longitude">
    /// The longitude value.
    /// </param>
    /// <returns>
    /// A coordinate instance of type <typeparamref name="TCoordinate"/>.
    /// </returns>
    protected abstract TCoordinate CreateCoordinate(double latitude, double longitude);
}