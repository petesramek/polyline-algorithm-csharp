//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Validation;
using PolylineAlgorithm.Abstraction.Validation.Abstraction;

/// <summary>
/// Options for configuring polyline encoding.
/// </summary>
/// <typeparam name="TCoordinate">The type representing a coordinate.</typeparam>
public class PolylineEncodingOptions<TCoordinate> {
    /// <summary>
    /// Gets the maximum buffer size for encoding operations.
    /// </summary>
    public int MaxBufferSize { get; } = 64_000;

    /// <summary>
    /// Gets the validator used to validate coordinates, latitude, and longitude values.
    /// </summary>
    public Validator<TCoordinate> Validator { get; } = new NullValidator<TCoordinate>();

    /// <summary>
    /// Gets the maximum number of characters that can be used in the encoding buffer.
    /// </summary>
    /// <returns>The maximum character count based on the buffer size.</returns>
    internal int GetMaxCharCount() => MaxBufferSize / sizeof(char);

    /// <summary>
    /// Validates the encoding options to ensure they are within acceptable limits.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if <see cref="MaxBufferSize"/> is less than the minimum required buffer size.
    /// </exception>
    internal void Validate() {
        if (MaxBufferSize < Defaults.Polyline.MaxEncodedCoordinateLength * sizeof(char)) {
            throw new ArgumentException();
        }
    }
}