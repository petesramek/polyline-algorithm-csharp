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
public class PolylineEncodingOptions<TCoordinate> {
    public static PolylineEncodingOptions<TCoordinate> Default => new();

    /// <summary>
    /// Gets the maximum buffer size for encoding operations.
    /// </summary>
    public int MaxBufferSize { get; } = 64_000;

    public CoordinateValidator<TCoordinate> Validator { get; } = new NullValidator<TCoordinate>();

    internal int GetMaxCharCount() => MaxBufferSize / sizeof(char);

    internal void Validate() {
        if (MaxBufferSize < Defaults.Polyline.MaxEncodedCoordinateLength * sizeof(char)) {
            throw new ArgumentException();
        }
    }
}