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
/// <see cref="Write"/>, and <see cref="CreatePolyline"/> to provide type-specific behavior.
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
    public TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates, CancellationToken cancellationToken = default) {
        const string OperationName = nameof(Encode);

        _logger
            .LogOperationStartedDebug(OperationName);

        Debug.Assert(coordinates.Length >= 0, "Count must be non-negative.");

        ValidateEmptyCoordinates(ref coordinates, _logger, OperationName);

        // Worst-case maximum: every value uses the maximum number of encoded characters.
        int maxCapacity = coordinates.Length * ValuesPerItem * Defaults.Polyline.Block.Length.Max;

        // Use stackalloc for small buffers (zero heap allocation); fall back to ArrayPool for large ones.
        const int StackAllocThreshold = 512;
        char[]? rentedBuffer = null;
        Span<char> buffer = maxCapacity > StackAllocThreshold
            ? (rentedBuffer = ArrayPool<char>.Shared.Rent(maxCapacity)).AsSpan(0, maxCapacity)
            : stackalloc char[maxCapacity];

        PolylineWriter writer = new(buffer, Options.Precision);
        PolylineValueState[] states = new PolylineValueState[ValuesPerItem];

        TPolyline result;

        try {
            for (var i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                Write(coordinates[i], ref writer, states);
            }

            result = CreatePolyline(writer.WrittenSpan);
        } finally {
            if (rentedBuffer is not null) {
                ArrayPool<char>.Shared.Return(rentedBuffer);
            }
        }

        _logger
            .LogOperationFinishedDebug(OperationName);

        return result;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ValidateEmptyCoordinates(ref ReadOnlySpan<TCoordinate> coordinates, ILogger logger, string operationName) {
            if (coordinates.Length < 1) {
                logger
                    .LogOperationFailedDebug(operationName);
                logger
                    .LogEmptyArgumentWarning(nameof(coordinates));

                ExceptionGuard.ThrowArgumentCannotBeEmptyEnumerationMessage(nameof(coordinates));
            }
        }
    }

    /// <summary>
    /// Gets the number of field values encoded or decoded per item.
    /// </summary>
    /// <remarks>
    /// The base class uses this value to allocate the per-call state array that is passed to
    /// <see cref="Write"/>. Implementations must return a constant value that matches the
    /// number of <see cref="PolylineWriter.Write"/> calls made inside <see cref="Write"/>.
    /// </remarks>
    protected abstract int ValuesPerItem { get; }

    /// <summary>
    /// Creates a polyline instance from the provided read-only span of characters.
    /// </summary>
    /// <param name="polyline">A <see cref="ReadOnlySpan{T}"/> containing the encoded polyline characters.</param>
    /// <returns>
    /// An instance of <typeparamref name="TPolyline"/> representing the encoded polyline.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract TPolyline CreatePolyline(ReadOnlySpan<char> polyline);

    /// <summary>
    /// Writes the field values of the specified item into the polyline encoding pipeline.
    /// </summary>
    /// <param name="item">The item whose field values are to be encoded.</param>
    /// <param name="writer">
    /// The <see cref="PolylineWriter"/> cursor provided by the engine. Call <see cref="PolylineWriter.Write"/>
    /// once for each field value, in a fixed, consistent order. The engine handles delta computation,
    /// zigzag encoding, and output buffering.
    /// </param>
    /// <param name="states">
    /// The per-field delta accumulation state array, allocated by the base class for the duration of
    /// the <see cref="Encode"/> call. Index into this array in the same fixed order used for each field
    /// (e.g. <c>states[0]</c> for latitude, <c>states[1]</c> for longitude).
    /// </param>
    /// <remarks>
    /// Implementations must always call <see cref="PolylineWriter.Write"/> the same number of times,
    /// in the same field order, for every item. The number of calls must match <see cref="ValuesPerItem"/>.
    /// The corresponding <see cref="Read"/> override must call <see cref="PolylineReader.Read"/> the
    /// same number of times in the same order.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract void Write(TCoordinate item, ref PolylineWriter writer, PolylineValueState[] states);
}
