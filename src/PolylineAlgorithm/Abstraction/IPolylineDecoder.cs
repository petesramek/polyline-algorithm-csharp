//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Defines a contract for decoding an encoded polyline into a sequence of geographic coordinates.
/// </summary>
/// <typeparam name="TPolyline">
/// The type that represents the encoded polyline input. Common implementations use <see cref="string"/>,
/// but custom wrapper types are allowed to carry additional metadata.
/// </typeparam>
/// <typeparam name="TValue">
/// The coordinate type returned by the decoder. Typical implementations return a struct or class that
/// contains latitude and longitude (for example a <c>LatLng</c> type or a <c>ValueTuple&lt;double,double&gt;</c>).
/// </typeparam>
public interface IPolylineDecoder<TPolyline, TValue> {
    /// <summary>
    /// Decodes the specified encoded polyline into an ordered sequence of geographic coordinates.
    /// The sequence preserves the original vertex order encoded by the <paramref name="polyline"/>.
    /// </summary>
    /// <param name="polyline">
    /// The <typeparamref name="TPolyline"/> instance containing the encoded polyline to decode.
    /// Implementations SHOULD validate the input and may throw <see cref="System.ArgumentException"/>
    /// or <see cref="System.FormatException"/> for invalid formats.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while decoding. If cancellation is requested,
    /// implementations SHOULD stop work and throw an <see cref="OperationCanceledException"/>.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <typeparamref name="TValue"/> representing the decoded
    /// latitude/longitude pairs (or equivalent coordinates) in the same order they were encoded.
    /// </returns>
    /// <remarks>
    /// Implementations commonly follow the Google Encoded Polyline Algorithm Format, but this interface
    /// does not mandate a specific encoding. Consumers should rely on a concrete decoder's documentation
    /// to understand the exact encoding supported.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the provided <paramref name="cancellationToken"/> requests cancellation.
    /// </exception>
    IEnumerable<TValue> Decode(TPolyline polyline, PolylineDecodingOptions<TValue>? options = null, CancellationToken cancellationToken = default);
}