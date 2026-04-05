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
/// Because sensor data is one-dimensional (a single temperature per reading), the base class designed
/// for two-dimensional coordinate pairs is not used. Instead, <see cref="PolylineEncoding"/> static
/// helpers are called directly to read delta-encoded characters and denormalise the recovered values.
/// </para>
/// <para>
/// Timestamps cannot be recovered from the encoded string.
/// The decoded <see cref="SensorReading"/> instances will have <see cref="SensorReading.Timestamp"/>
/// set to <see langword="default"/>.
/// </para>
/// </remarks>
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
    /// <see cref="SensorReading.Temperature"/> values are recovered from the encoded string.
    /// <see cref="SensorReading.Timestamp"/> will be <see langword="default"/> for every element.
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

        return DecodeIterator(polyline.AsMemory(), cancellationToken);
    }

    private IEnumerable<SensorReading> DecodeIterator(ReadOnlyMemory<char> memory, CancellationToken cancellationToken) {
        int position = 0;
        int accumulated = 0;

        while (position < memory.Length) {
            cancellationToken.ThrowIfCancellationRequested();

            if (!PolylineEncoding.TryReadValue(ref accumulated, memory, ref position)) {
                yield break;
            }

            double temperature = PolylineEncoding.Denormalize(accumulated, Options.Precision);

            yield return new SensorReading(default, temperature);
        }
    }
}
