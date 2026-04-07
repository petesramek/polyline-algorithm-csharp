//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using System;

/// <summary>
/// Provides unified configuration for a formatter-driven encoding or decoding operation.
/// </summary>
/// <typeparam name="TValue">The coordinate or item type understood by the value formatter.</typeparam>
/// <typeparam name="TPolyline">The polyline surface type understood by the polyline formatter.</typeparam>
/// <remarks>
/// Combines an <see cref="IPolylineValueFormatter{TValue}"/> (which defines the column schema, scaling
/// rules, and item factory) with an <see cref="IPolylineFormatter{TPolyline}"/> (which converts between
/// the raw character buffer and the surface type) and a <see cref="PolylineEncodingOptions"/> (which
/// controls buffer sizes, precision for legacy paths, and logging).
/// </remarks>
public sealed class PolylineOptions<TValue, TPolyline> {
    /// <summary>
    /// Initializes a new instance of <see cref="PolylineOptions{TValue, TPolyline}"/>.
    /// </summary>
    /// <param name="valueFormatter">
    /// The formatter that defines the column schema, scaling rules, and item factory. Must not be
    /// <see langword="null"/>.
    /// </param>
    /// <param name="polylineFormatter">
    /// The formatter that converts between the raw character buffer and
    /// <typeparamref name="TPolyline"/>. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="encoding">
    /// The encoding options that control buffer sizes, precision, and logging.
    /// Pass <see langword="null"/> to use default options.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="valueFormatter"/> or <paramref name="polylineFormatter"/> is
    /// <see langword="null"/>.
    /// </exception>
    public PolylineOptions(
        IPolylineValueFormatter<TValue> valueFormatter,
        IPolylineFormatter<TPolyline> polylineFormatter,
        PolylineEncodingOptions? encoding = null) {
        if (valueFormatter is null) {
            throw new ArgumentNullException(nameof(valueFormatter));
        }

        if (polylineFormatter is null) {
            throw new ArgumentNullException(nameof(polylineFormatter));
        }

        ValueFormatter = valueFormatter;
        PolylineFormatter = polylineFormatter;
        Encoding = encoding ?? new PolylineEncodingOptions();
    }

    /// <summary>
    /// Gets the formatter that defines the column schema, scaling rules, and item factory.
    /// </summary>
    public IPolylineValueFormatter<TValue> ValueFormatter { get; }

    /// <summary>
    /// Gets the formatter that converts between the raw character buffer and
    /// <typeparamref name="TPolyline"/>.
    /// </summary>
    public IPolylineFormatter<TPolyline> PolylineFormatter { get; }

    /// <summary>
    /// Gets the encoding options that control buffer sizes, precision, and logging.
    /// </summary>
    public PolylineEncodingOptions Encoding { get; }
}
