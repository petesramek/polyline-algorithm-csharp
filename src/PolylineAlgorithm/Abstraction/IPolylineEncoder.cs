//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;
/// <summary>
/// Defines a contract for encoding a sequence of geographic coordinates into an encoded polyline string.
/// Implementations are responsible for interpreting the generic <typeparamref name="TCoordinate"/> type
/// and producing an encoded representation of those coordinates as <typeparamref name="TPolyline"/>.
/// </summary>
/// <typeparam name="TCoordinate">
/// The concrete coordinate representation used by the encoder (for example a struct or class containing
/// latitude/longitude values). Implementations must document the expected shape and units of this type.
/// </typeparam>
/// <typeparam name="TPolyline">
/// The encoded polyline representation returned by the encoder (for example <c>string</c>,
/// <c>ReadOnlyMemory<char></c>, or a custom wrapper type).
/// </typeparam>
public interface IPolylineEncoder<TCoordinate, TPolyline> {
    /// <summary>
    /// Encodes a sequence of geographic coordinates into an encoded polyline representation.
    /// The order of coordinates in <paramref name="coordinates"/> is preserved in the encoded result.
    /// </summary>
    /// <param name="coordinates">
    /// The collection of <typeparamref name="TCoordinate"/> instances to encode into a polyline.
    /// The span may be empty; implementations should return an appropriate empty encoded representation.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="System.Threading.CancellationToken"/> that can be used to cancel the encoding operation.
    /// Implementations should observe this token and throw <see cref="System.OperationCanceledException"/>
    /// when cancellation is requested.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TPolyline"/> containing the encoded polyline that represents the input coordinates.
    /// </returns>
    /// <remarks>
    /// - Specific encoding details (precision, coordinate projection, expected fields on <typeparamref name="TCoordinate"/>)
    ///   are implementation-specific and should be documented by concrete encoder types.
    /// - Implementations should aim to be efficient with memory (hence the use of <see cref="ReadOnlySpan{T}"/>)
    ///   and should document any thread-safety guarantees.
    /// </remarks>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown if the operation is canceled via <paramref name="cancellationToken"/>.
    /// </exception>
    TPolyline Encode(ReadOnlySpan<TCoordinate> coordinates, CancellationToken cancellationToken = default);
}
