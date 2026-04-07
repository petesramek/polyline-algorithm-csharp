//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;

/// <summary>
/// Provides configuration for a <see cref="PolylineFormatter{T}"/>-driven encoding operation.
/// </summary>
/// <typeparam name="T">The source object type from which column values are extracted.</typeparam>
/// <remarks>
/// Combines a <see cref="PolylineFormatter{T}"/> (which defines the column schema and scaling rules)
/// with a <see cref="PolylineEncodingOptions"/> (which controls buffer sizes, precision, and logging).
/// </remarks>
public sealed class PolylineOptions<T> {
    /// <summary>
    /// Initializes a new instance of <see cref="PolylineOptions{T}"/>.
    /// </summary>
    /// <param name="formatter">
    /// The sealed formatter that defines the column schema. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="encoding">
    /// The encoding options that control buffer sizes, precision, and logging.
    /// Pass <see langword="null"/> to use default options.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="formatter"/> is <see langword="null"/>.
    /// </exception>
    public PolylineOptions(PolylineFormatter<T> formatter, PolylineEncodingOptions? encoding = null) {
        if (formatter is null) {
            throw new ArgumentNullException(nameof(formatter));
        }

        Formatter = formatter;
        Encoding = encoding ?? new PolylineEncodingOptions();
    }

    /// <summary>
    /// Gets the sealed formatter that defines the column schema and scaling rules.
    /// </summary>
    public PolylineFormatter<T> Formatter { get; }

    /// <summary>
    /// Gets the encoding options that control buffer sizes, precision, and logging.
    /// </summary>
    public PolylineEncodingOptions Encoding { get; }
}
