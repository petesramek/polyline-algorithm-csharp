//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PolylineAlgorithm.Properties;

/// <summary>
/// Provides a builder for configuring options for polyline encoding operations.
/// </summary>
public class PolylineEncodingOptionsBuilder {
    private int _maxPolylineLength = 1_024;
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
            MaxPolylineLength = _maxPolylineLength,
            LoggerFactory = _loggerFactory
        };
    }

    /// <summary>
    /// Sets the maximum length of the polyline string that can be encoded.
    /// </summary>
    /// <param name="maxLength">
    /// The maximum length of the polyline string in characters.
    /// </param>
    /// <returns>
    /// The current builder instance, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="maxLength"/> is less than 1024 characters.
    /// </exception>
    public PolylineEncodingOptionsBuilder WithMaxPolylineLength(int maxLength) {
        _maxPolylineLength = maxLength >= 1024 ? maxLength : throw new ArgumentOutOfRangeException(nameof(maxLength), string.Format(ExceptionMessageResource.BufferSizeMustBeGreaterThanMessageFormat, 11));

        return this;
    }

    /// <summary>
    /// Sets the logger factory for logging during encoding operations.
    /// </summary>
    /// <param name="loggerFactory">
    /// The instance of a logger factory.
    /// </param>
    /// <returns>
    /// The current builder instance, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="loggerFactory"/> is <see langword="null"/>.
    /// </exception>
    public PolylineEncodingOptionsBuilder UseLoggerFactory(ILoggerFactory loggerFactory) {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        return this;
    }
}