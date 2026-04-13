//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Extensions;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal.Diagnostics;
using System;

/// <summary>
/// Provides extension methods for the <see cref="IPolylineEncoder{TValue, TPolyline}"/> interface to facilitate encoding values into polylines.
/// </summary>
public static class PolylineEncoderExtensions {
    /// <summary>
    /// Encodes an array of <typeparamref name="TValue"/> instances into an encoded polyline.
    /// </summary>
    /// <typeparam name="TValue">The type that represents a value to encode.</typeparam>
    /// <typeparam name="TPolyline">The type that represents the encoded polyline output.</typeparam>
    /// <param name="encoder">
    /// The <see cref="IPolylineEncoder{TValue, TPolyline}"/> instance used to perform the encoding operation.
    /// </param>
    /// <param name="values">
    /// The array of <typeparamref name="TValue"/> objects to encode.
    /// </param>
    /// <returns>
    /// A <typeparamref name="TPolyline"/> instance representing the encoded polyline for the provided values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoder"/> or <paramref name="values"/> is <see langword="null"/>.
    /// </exception>
    public static TPolyline Encode<TValue, TPolyline>(
        this IPolylineEncoder<TValue, TPolyline> encoder,
        TValue[] values,
        PolylineEncodingOptions<TValue>? options = null,
        CancellationToken cancellationToken = default) {
        if (encoder is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(encoder));
        }

        if (values is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(values));
        }

        return encoder.Encode(values.AsSpan(), options, cancellationToken);
    }
}
