//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.SensorData.Sample;

using PolylineAlgorithm.Abstraction;
using System.Buffers;
using System.Threading;

/// <summary>
/// Encodes a sequence of <see cref="SensorReading"/> values into a compact polyline string
/// using the polyline delta-encoding algorithm applied to the <see cref="SensorReading.Temperature"/> field.
/// </summary>
/// <remarks>
/// <para>
/// This class demonstrates implementing <see cref="IPolylineEncoder{TValue,TPolyline}"/> for a custom
/// scalar type, following the same structural pattern as <see cref="AbstractPolylineEncoder{TCoordinate,TPolyline}"/>.
/// </para>
/// <para>
/// Because sensor readings carry only a single numeric dimension (temperature), the base class designed
/// for two-dimensional coordinate pairs is not used. Instead, <see cref="PolylineEncoding"/> static
/// helpers are called directly to perform normalisation, delta computation, and character-level encoding.
/// </para>
/// <para>
/// Only <see cref="SensorReading.Temperature"/> values are encoded. Timestamps are not encoded and
/// will not be recovered on decoding.
/// </para>
/// </remarks>
internal sealed class SensorDataEncoder : IPolylineEncoder<SensorReading, string> {
    /// <summary>
    /// Initializes a new instance of the <see cref="SensorDataEncoder"/> class with default encoding options.
    /// </summary>
    public SensorDataEncoder()
        : this(new PolylineEncodingOptions()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorDataEncoder"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions"/> to use for encoding operations.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public SensorDataEncoder(PolylineEncodingOptions options) {
        ArgumentNullException.ThrowIfNull(options);

        Options = options;
    }

    /// <summary>
    /// Gets the encoding options used by this encoder.
    /// </summary>
    public PolylineEncodingOptions Options { get; }

    /// <summary>
    /// Encodes a sequence of <see cref="SensorReading"/> values into a polyline string.
    /// </summary>
    /// <param name="coordinates">
    /// The sensor readings whose <see cref="SensorReading.Temperature"/> values are to be encoded.
    /// Must contain at least one element.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the encoding operation.
    /// </param>
    /// <returns>
    /// A polyline-encoded string representing the delta-compressed temperature series.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="coordinates"/> is empty.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> requests cancellation.
    /// </exception>
    public string Encode(ReadOnlySpan<SensorReading> coordinates, CancellationToken cancellationToken = default) {
        if (coordinates.Length < 1) {
            throw new ArgumentException("Sequence must contain at least one element.", nameof(coordinates));
        }

        // Maximum number of ASCII characters required to encode a single 32-bit delta value
        // using the polyline algorithm (ceil(32 bits / 5 bits per chunk) + sign bit = 7).
        const int MaxEncodedCharsPerValue = 7;

        int previousNormalized = 0;
        int position = 0;
        int length = coordinates.Length * MaxEncodedCharsPerValue;

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        try {
            for (int i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                int normalized = PolylineEncoding.Normalize(coordinates[i].Temperature, Options.Precision);
                int delta = normalized - previousNormalized;

                if (!PolylineEncoding.TryWriteValue(delta, buffer, ref position)) {
                    throw new InvalidOperationException("Encoding buffer is too small to hold the encoded value.");
                }

                previousNormalized = normalized;
            }

            return buffer[..position].ToString();
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }
    }
}
