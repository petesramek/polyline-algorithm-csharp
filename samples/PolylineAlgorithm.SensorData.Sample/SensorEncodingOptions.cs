//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.SensorData.Sample;

/// <summary>
/// Encoding options used by <see cref="SensorDataEncoder"/> and <see cref="SensorDataDecoder"/>.
/// </summary>
internal sealed class SensorEncodingOptions {
    /// <summary>
    /// Initializes a new instance of the <see cref="SensorEncodingOptions"/> class with default values.
    /// </summary>
    public SensorEncodingOptions()
        : this(precision: 5, stackAllocLimit: 512) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorEncodingOptions"/> class.
    /// </summary>
    /// <param name="precision">Number of decimal places to use when encoding floating-point values. Defaults to <c>5</c>.</param>
    /// <param name="stackAllocLimit">
    /// Maximum number of characters to allocate on the stack during encoding. Defaults to <c>512</c>.
    /// </param>
    public SensorEncodingOptions(uint precision, int stackAllocLimit) {
        Precision = precision;
        StackAllocLimit = stackAllocLimit;
    }

    /// <summary>
    /// Gets the number of decimal places used when encoding floating-point values.
    /// </summary>
    public uint Precision { get; }

    /// <summary>
    /// Gets the maximum number of characters to allocate on the stack during encoding.
    /// </summary>
    public int StackAllocLimit { get; }
}
