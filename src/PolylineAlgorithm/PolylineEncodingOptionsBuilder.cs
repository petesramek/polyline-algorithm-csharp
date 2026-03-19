//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PolylineAlgorithm.Internal.Diagnostics;

/// <summary>
/// Provides a builder for configuring options for polyline encoding operations.
/// </summary>
public sealed class PolylineEncodingOptionsBuilder {
    private uint _precision = 5;
    private int _stackAllocLimit = 512;
    private ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;

    private PolylineEncodingOptionsBuilder() { }

    /// <summary>
    /// Creates a new <see cref="PolylineEncodingOptionsBuilder"/> instance for the specified coordinate type.
    /// </summary>
    /// <returns>
    /// An <see cref="PolylineEncodingOptionsBuilder"/> instance for configuring polyline encoding options.
    /// </returns>
    public static PolylineEncodingOptionsBuilder Create() {
        return new PolylineEncodingOptionsBuilder();
    }

    /// <summary>
    /// Builds a new <see cref="PolylineEncodingOptions"/> instance using the configured options.
    /// </summary>
    /// <returns>
    /// A configured <see cref="PolylineEncodingOptions"/> instance.
    /// </returns>
    public PolylineEncodingOptions Build() {
        return new PolylineEncodingOptions {
            Precision = _precision,
            StackAllocLimit = _stackAllocLimit,
            LoggerFactory = _loggerFactory,
        };
    }

    /// <summary>
    /// Configures the buffer size used for stack allocation during polyline encoding operations.
    /// </summary>
    /// <param name="stackAllocLimit">
    /// The maximum buffer size to use for stack allocation. Must be greater than or equal to 1.
    /// </param>
    /// <returns>
    /// Returns the current <see cref="PolylineEncodingOptionsBuilder"/> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="stackAllocLimit"/> is less than 1.
    /// </exception>
    /// <remarks>
    /// This method allows customization of the internal buffer size for encoding, which can impact performance and memory usage.
    /// </remarks>
    public PolylineEncodingOptionsBuilder WithStackAllocLimit(int stackAllocLimit) {
        const int minStackAllocLimit = 1;
        _stackAllocLimit = stackAllocLimit >= minStackAllocLimit
            ? stackAllocLimit
            : throw new ArgumentOutOfRangeException(nameof(stackAllocLimit), ExceptionMessages.FormatStackAllocLimit(minStackAllocLimit));
        return this;
    }

    /// <summary>
    /// Sets the precision for encoding values.
    /// </summary>
    /// <param name="precision">
    /// The number of decimal places to use for encoding values. Default is 5.
    /// </param>
    /// <returns>
    /// The current builder instance.
    /// </returns>
    public PolylineEncodingOptionsBuilder WithPrecision(uint precision) {
        _precision = precision;

        return this;
    }

    /// <summary>
    /// Configures the <see cref="ILoggerFactory"/> to be used for logging during polyline encoding operations.
    /// </summary>
    /// <param name="loggerFactory">
    /// The <see cref="ILoggerFactory"/> instance to use for logging. If <see langword="null"/>, a <see cref="NullLoggerFactory"/> will be used instead.
    /// </param>
    /// <returns>
    /// Returns the current <see cref="PolylineEncodingOptionsBuilder"/> instance for method chaining.
    /// </returns>
    public PolylineEncodingOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory) {
        _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;

        return this;
    }
}