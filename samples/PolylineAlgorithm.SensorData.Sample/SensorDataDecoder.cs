//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.SensorData.Sample;

using PolylineAlgorithm.Abstraction;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Decodes a compact polyline string produced by <see cref="SensorDataEncoder"/> back into a sequence
/// of <see cref="SensorReading"/> values.
/// </summary>
/// <remarks>
/// <para>
/// This class demonstrates implementing <see cref="IPolylineDecoder{TPolyline,TValue}"/> for a custom
/// scalar type, following the same structural pattern as <see cref="AbstractPolylineDecoder{TPolyline,TCoordinate}"/>.
/// </para>
/// <para>
/// Each encoded pair consists of a delta-compressed Unix timestamp (seconds since Unix epoch, precision 0)
/// followed by a delta-compressed temperature value (at <see cref="PolylineEncodingOptions.Precision"/>).
/// Both are recovered and used to reconstruct the original <see cref="SensorReading"/>.
/// </para>
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar", "S4456:Parameter validation in yielding methods should be wrapped", Justification = "Inlined by design to demonstrate a simple iterator without a wrapper method.")]
internal sealed class SensorDataDecoder : IPolylineDecoder<string, SensorReading> {
    /// <summary>
    /// Initializes a new instance of the <see cref="SensorDataDecoder"/> class with default encoding options.
    /// </summary>
    public SensorDataDecoder()
        : this(new PolylineEncodingOptions()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorDataDecoder"/> class with the specified encoding options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="PolylineEncodingOptions"/> to use for decoding operations.
    /// The <see cref="PolylineEncodingOptions.Precision"/> value must match the precision used during encoding.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public SensorDataDecoder(PolylineEncodingOptions options) {
        ArgumentNullException.ThrowIfNull(options);

        Options = options;
    }

    /// <summary>
    /// Gets the encoding options used by this decoder.
    /// </summary>
    public PolylineEncodingOptions Options { get; }

    /// <summary>
    /// Decodes a polyline string back into a sequence of <see cref="SensorReading"/> values.
    /// </summary>
    /// <param name="polyline">
    /// The polyline-encoded string produced by <see cref="SensorDataEncoder.Encode"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the decoding operation.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="SensorReading"/> whose
    /// <see cref="SensorReading.Timestamp"/> and <see cref="SensorReading.Temperature"/> values
    /// are recovered from the encoded string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="polyline"/> is empty.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when <paramref name="cancellationToken"/> requests cancellation.
    /// </exception>
    public IEnumerable<SensorReading> Decode(string polyline, CancellationToken cancellationToken = default) {
        ArgumentNullException.ThrowIfNull(polyline);

        if (polyline.Length < 1) {
            throw new ArgumentException("Encoded polyline must not be empty.", nameof(polyline));
        }

        ReadOnlyMemory<char> memory = polyline.AsMemory();
        int position = 0;
        // Mirror the encoder's base epoch so the first delta decodes back to the correct Unix seconds.
        int accumulatedTimestamp = SensorDataEncoder.TimestampBaseEpochSeconds;
        int accumulatedTemperature = 0;

        while (position < memory.Length) {
            cancellationToken.ThrowIfCancellationRequested();

            // Read Unix timestamp delta (precision 0) then temperature delta.
            if (!PolylineEncoding.TryReadValue(ref accumulatedTimestamp, memory, ref position)
                || !PolylineEncoding.TryReadValue(ref accumulatedTemperature, memory, ref position)) {
                yield break;
            }

            long unixSeconds = (long)PolylineEncoding.Denormalize(accumulatedTimestamp, precision: 0);
            double temperature = PolylineEncoding.Denormalize(accumulatedTemperature, Options.Precision);

            yield return new SensorReading(DateTimeOffset.FromUnixTimeSeconds(unixSeconds), temperature);
        }
    }
}
