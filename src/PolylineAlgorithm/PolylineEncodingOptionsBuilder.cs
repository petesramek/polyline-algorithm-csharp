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
public class PolylineEncodingOptionsBuilder {
    private int _bufferSize = 64_000;
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
            BufferSizeInBytes = _bufferSize,
            LoggerFactory = _loggerFactory
        };
    }

    /// <summary>
    /// Sets the buffer size for encoding operations.
    /// </summary>
    /// <param name="maxBufferSize">
    /// The maximum buffer size. Must be greater than 11.
    /// </param>
    /// <returns>
    /// The current builder instance.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxBufferSize"/> is less than or equal to 11.</exception>
    public PolylineEncodingOptionsBuilder WithBufferSize(int maxBufferSize) {
        _bufferSize = maxBufferSize > 11 ? maxBufferSize : throw new ArgumentOutOfRangeException(nameof(maxBufferSize), "Buffer size must be greater than 11.");

        return this;
    }

    /// <summary>
    /// Sets the logger factory for logging during encoding operations.
    /// </summary>
    /// <param name="loggerFactory">
    /// The instance of a logger factory.
    /// </param>
    /// <returns>
    /// The current builder instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="loggerFactory"/> is <see langword="null"/>.
    /// </exception>
    public PolylineEncodingOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory) {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory), "Logger factory cannot be null.");

        return this;
    }
}