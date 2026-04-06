//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

/// <summary>
/// Provides a builder for configuring options for polyline encoding operations.
/// </summary>
public sealed class PolylineEncodingOptionsBuilder {
    private uint _precision = 5;
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
            LoggerFactory = _loggerFactory,
        };
    }

    /// <summary>
    /// Sets the coordinate encoding precision.
    /// </summary>
    /// <param name="precision">
    /// The number of decimal places to use for encoding coordinate values. Default is 5.
    /// </param>
    /// <returns>
    /// The current <see cref="PolylineEncodingOptionsBuilder"/> instance for method chaining.
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
    /// The current <see cref="PolylineEncodingOptionsBuilder"/> instance for method chaining.
    /// </returns>
    public PolylineEncodingOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory) {
        _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;

        return this;
    }
}