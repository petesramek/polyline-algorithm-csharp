//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Gps.Abstraction;
/// <summary>
/// Contract for encoding a sequence of geographic coordinates into an encoded polyline representation.
/// Implementations interpret the generic <typeparamref name="TValue"/> type and produce an encoded
/// representation of those coordinates as <typeparamref name="TPolyline"/>.
/// </summary>
/// <typeparam name="TValue">
/// The concrete coordinate representation used by the encoder (for example a struct or class containing
/// <c>Latitude</c> and <c>Longitude</c> values). Implementations must document the expected shape,
/// units (typically decimal degrees), and any required fields for <typeparamref name="TValue"/>.
/// Common shapes:
/// - A struct or class with two <c>double</c> properties named <c>Latitude</c> and <c>Longitude</c>.
/// - A tuple-like type (for example <c>ValueTuple&lt;double,double&gt;</c>) where the encoder documents
///   which element represents latitude and longitude.
/// </typeparam>
/// <typeparam name="TPolyline">
/// The encoded polyline representation returned by the encoder (for example <c>string</c>,
/// <c>ReadOnlyMemory&lt;char&gt;</c>, or a custom wrapper type). Concrete implementations should document
/// the chosen representation and any memory / ownership expectations.
/// </typeparam>
/// <remarks>
/// - This interface is intentionally minimal to allow different encoding strategies (Google encoded polyline,
///   precision/scale variants, or custom compressed formats) to be expressed behind a common contract.
/// - Implementations should document:
///   - Coordinate precision and rounding rules (for example 1e-5 for 5-decimal precision).
///   - Coordinate ordering and whether altitude or additional dimensions are supported.
///   - Thread-safety guarantees: whether instances are safe to reuse concurrently or must be instantiated per-call.
/// - Implementations are encouraged to be memory-efficient; the API accepts a <see cref="ReadOnlySpan{T}"/>
///   to avoid forced allocations when callers already have contiguous memory.
/// </remarks>
public interface IPolylineEncoder<TValue, TPolyline> {
    /// <summary>
    /// Encodes a sequence of geographic coordinates into an encoded polyline representation.
    /// The order of coordinates in <paramref name="coordinates"/> is preserved in the encoded result.
    /// </summary>
    /// <param name="coordinates">
    /// The collection of <typeparamref name="TValue"/> instances to encode into a polyline.
    /// The span may be empty; implementations should return an appropriate empty encoded representation
    /// (for example an empty string or an empty memory slice) rather than <c>null</c>.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the encoding operation.
    /// Implementations should observe this token and throw <see cref="System.OperationCanceledException"/>
    /// when cancellation is requested. For fast, in-memory encoders cancellation may be best-effort.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TPolyline"/> containing the encoded polyline that represents the input coordinates.
    /// The exact format and any delimiting/terminating characters are implementation-specific and must be
    /// documented by concrete encoder types.
    /// </returns>
    /// <example>
    /// <code>
    /// // Example pseudocode for typical usage with a string-based encoder:
    /// var coords = new[] {
    ///     new Coordinate { Latitude = 47.6219, Longitude = -122.3503 },
    ///     new Coordinate { Latitude = 47.6220, Longitude = -122.3504 }
    /// };
    /// IPolylineEncoder&lt;Coordinate,string&gt; encoder = new GoogleEncodedPolylineEncoder();
    /// string encoded = encoder.Encode(coords, CancellationToken.None);
    /// </code>
    /// </example>
    /// <remarks>
    /// - Implementations should validate input as appropriate and document any preconditions (for example
    ///   if coordinates must be within [-90,90] latitude and [-180,180] longitude).
    /// - For large input sequences, implementations may provide streaming or incremental encoders; those
    ///   variants can still implement this interface by materializing the final encoded result.
    /// </remarks>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown if the operation is canceled via <paramref name="cancellationToken"/>.
    /// </exception>
    TPolyline Encode(ReadOnlySpan<TValue> coordinates, CancellationToken cancellationToken = default);
}
