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
/// using the polyline delta-encoding algorithm applied to both the <see cref="SensorReading.Timestamp"/>
/// and <see cref="SensorReading.Temperature"/> fields.
/// </summary>
/// <remarks>
/// <para>
/// This class demonstrates implementing <see cref="IPolylineEncoder{TValue,TPolyline}"/> for a custom
/// scalar type, following the same structural pattern as <see cref="AbstractPolylineEncoder{TCoordinate,TPolyline}"/>.
/// </para>
/// <para>
/// Because sensor readings carry two numeric dimensions (timestamp and temperature), the base class designed
/// for geographic coordinate pairs is not used. Instead, <see cref="PolylineEncoding"/> static
/// helpers are called directly to perform normalisation, delta computation, and character-level encoding.
/// </para>
/// <para>
/// Each reading is encoded as a pair of delta-compressed values:
/// the Unix timestamp in seconds (precision 0) followed by the temperature (at <see cref="PolylineEncodingOptions.Precision"/>).
/// </para>
/// </remarks>
internal sealed class SensorDataEncoder : IPolylineEncoder<SensorReading, string> {
    // 2020-01-01 00:00:00 UTC in Unix seconds. Used as the delta-encoding base for timestamps
    // so that the first absolute delta stays within the int32 safe range of the polyline algorithm.
    internal const int TimestampBaseEpochSeconds = 1_577_836_800;

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
    /// The sensor readings to encode. Each reading contributes a delta-compressed Unix timestamp
    /// (seconds since Unix epoch, precision 0) and a delta-compressed temperature value.
    /// Must contain at least one element.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the encoding operation.
    /// </param>
    /// <returns>
    /// A polyline-encoded string representing the delta-compressed timestamp and temperature series.
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

        // Each reading encodes two values: Unix timestamp (precision 0) + temperature.
        // The polyline algorithm uses signed int32 internally, limiting safe absolute values to ~1.07B.
        // Current Unix time in seconds (~1.74B) exceeds this. We therefore delta-encode relative to
        // 2020-01-01 00:00:00 UTC (= 1 577 836 800 s), keeping the initial delta well within range.
        int previousTimestampNormalized = TimestampBaseEpochSeconds;
        int previousTemperatureNormalized = 0;
        int position = 0;
        int length = coordinates.Length * 2 * MaxEncodedCharsPerValue;

        char[]? temp = length <= Options.StackAllocLimit
            ? null
            : ArrayPool<char>.Shared.Rent(length);

        Span<char> buffer = temp is null ? stackalloc char[length] : temp.AsSpan(0, length);

        try {
            for (int i = 0; i < coordinates.Length; i++) {
                cancellationToken.ThrowIfCancellationRequested();

                // Encode Unix timestamp in whole seconds (precision 0).
                int normalizedTimestamp = PolylineEncoding.Normalize((double)coordinates[i].Timestamp.ToUnixTimeSeconds(), precision: 0);
                int timestampDelta = normalizedTimestamp - previousTimestampNormalized;

                // Encode temperature at the configured precision.
                int normalizedTemperature = PolylineEncoding.Normalize(coordinates[i].Temperature, Options.Precision);
                int temperatureDelta = normalizedTemperature - previousTemperatureNormalized;

                if (!PolylineEncoding.TryWriteValue(timestampDelta, buffer, ref position)
                    || !PolylineEncoding.TryWriteValue(temperatureDelta, buffer, ref position)) {
                    throw new InvalidOperationException("Encoding buffer is too small to hold the encoded value.");
                }

                previousTimestampNormalized = normalizedTimestamp;
                previousTemperatureNormalized = normalizedTemperature;
            }

            return buffer[..position].ToString();
        } finally {
            if (temp is not null) {
                ArrayPool<char>.Shared.Return(temp);
            }
        }
    }
}
