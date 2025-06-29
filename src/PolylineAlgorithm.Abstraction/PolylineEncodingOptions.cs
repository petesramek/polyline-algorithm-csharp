//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

/// <summary>
/// Options for configuring polyline encoding.
/// </summary>
/// <typeparam name="TCoordinate">The type representing a coordinate.</typeparam>
public sealed class PolylineEncodingOptions {
    /// <summary>
    /// Gets the maximum buffer size for encoding operations.
    /// </summary>
    public int BufferSize { get; internal set; } = 64_000;

    public int MaxLength => BufferSize / sizeof(char);
}