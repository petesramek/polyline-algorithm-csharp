//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

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
    public int BufferSize { get; internal set; } = 64_000;

    /// <summary>
    /// Gets the validator used to validate coordinates, latitude, and longitude values.
    /// </summary>
    public Validator<TCoordinate> Validator { get; internal set; } = new NullValidator<TCoordinate>();
}