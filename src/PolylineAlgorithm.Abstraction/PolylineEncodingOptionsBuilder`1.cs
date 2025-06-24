//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Validation;
using PolylineAlgorithm.Abstraction.Validation.Abstraction;
using System;

/// <summary>
/// Provides a builder for configuring <see cref="PolylineEncodingOptions{TCoordinate}"/> instances.
/// </summary>
/// <typeparam name="TCoordinate">The type representing a coordinate.</typeparam>
internal class PolylineEncodingOptionsBuilder<TCoordinate> : IPolylineEncodingOptionsBuilder<TCoordinate> {
    private int _bufferSize = 64_000;
    private Validator<TCoordinate> _validator = new NullValidator<TCoordinate>();

    /// <summary>
    /// Builds a new <see cref="PolylineEncodingOptions{TCoordinate}"/> instance using the configured options.
    /// </summary>
    /// <returns>A configured <see cref="PolylineEncodingOptions{TCoordinate}"/> instance.</returns>
    PolylineEncodingOptions<TCoordinate> IPolylineEncodingOptionsBuilder<TCoordinate>.Build() {
        return new PolylineEncodingOptions<TCoordinate> {
            BufferSize = _bufferSize,
            Validator = _validator
        };
    }

    /// <summary>
    /// Sets the buffer size for encoding operations.
    /// </summary>
    /// <param name="maxBufferSize">The maximum buffer size. Must be greater than 11.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxBufferSize"/> is less than or equal to 11.</exception>
    IPolylineEncodingOptionsBuilder<TCoordinate> IPolylineEncodingOptionsBuilder<TCoordinate>.WithBufferSize(int maxBufferSize) {
        _bufferSize = maxBufferSize > 11 ? maxBufferSize : throw new ArgumentOutOfRangeException(nameof(maxBufferSize), "Buffer size must be greater than 11.");

        return this;
    }

    /// <summary>
    /// Sets the validator to use for coordinate validation.
    /// </summary>
    /// <param name="validator">The validator instance to use.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="validator"/> is <c>null</c>.</exception>
    IPolylineEncodingOptionsBuilder<TCoordinate> IPolylineEncodingOptionsBuilder<TCoordinate>.WithValidator(Validator<TCoordinate> validator) {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        return this;
    }
}
