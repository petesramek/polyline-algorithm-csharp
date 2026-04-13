//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PolylineAlgorithm.Abstraction;
using System;

/// <summary>
/// Provides unified configuration for a formatter-driven encoding or decoding operation.
/// </summary>
/// <typeparam name="TValue">The value or item type understood by the formatter.</typeparam>
/// <typeparam name="TPolyline">The polyline surface type understood by the formatter.</typeparam>
/// <remarks>
/// Supply an <see cref="IPolylineFormatter{TValue, TPolyline}"/> and optional settings,
/// then pass this instance to <see cref="PolylineEncoder{TValue, TPolyline}"/> and/or
/// <see cref="PolylineDecoder{TPolyline, TValue}"/>.
/// </remarks>
public sealed class PolylineOptions<TValue, TPolyline> {
    /// <summary>
    /// Initializes a new instance of <see cref="PolylineOptions{TValue, TPolyline}"/>.
    /// </summary>
    /// <param name="formatter">
    /// The unified formatter that handles all type-specific concerns: value extraction, item
    /// reconstruction, and polyline surface conversion. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="stackAllocLimit">
    /// The maximum buffer size (in characters) for stack allocation. Defaults to 512.
    /// </param>
    /// <param name="loggerFactory">
    /// The logger factory for diagnostic logging. Pass <see langword="null"/> to use
    /// <see cref="NullLoggerFactory.Instance"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="formatter"/> is <see langword="null"/>.
    /// </exception>
    public PolylineOptions(
        IPolylineFormatter<TValue, TPolyline> formatter,
        int stackAllocLimit = 512,
        ILoggerFactory? loggerFactory = null) {
        if (formatter is null) {
            throw new ArgumentNullException(nameof(formatter));
        }

        Formatter = formatter;
        StackAllocLimit = stackAllocLimit;
        LoggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
    }

    /// <summary>
    /// Gets the unified formatter that handles value extraction, item reconstruction, and polyline
    /// surface conversion.
    /// </summary>
    public IPolylineFormatter<TValue, TPolyline> Formatter { get; }

    /// <summary>
    /// Gets the maximum buffer size (in characters) that may be allocated on the stack for encoding.
    /// When the required buffer size exceeds this limit, memory is rented from
    /// <see cref="System.Buffers.ArrayPool{T}"/> instead. Defaults to 512.
    /// </summary>
    public int StackAllocLimit { get; }

    /// <summary>
    /// Gets the logger factory used for diagnostic logging during encoding and decoding operations.
    /// Defaults to <see cref="NullLoggerFactory.Instance"/>.
    /// </summary>
    public ILoggerFactory LoggerFactory { get; }
}
