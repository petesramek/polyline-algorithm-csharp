//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using Microsoft.Extensions.Logging;

/// <summary>
/// Interface for building polyline encoding options.
/// </summary>
/// <remarks>
/// This interface allows for configuring options such as buffer size and logger factory.
/// </remarks>

public interface IPolylineEncodingOptionsBuilder {
    /// <summary>
    /// Sets the maximum buffer size in bytes for encoding operations.
    /// </summary>
    /// <remarks>
    /// The buffer size determines how much data can be processed at once during encoding.
    /// </remarks>
    /// <param name="maxBufferSize">
    /// The maximum buffer size in bytes. Default is 64,000 bytes (64 KB).
    /// </param>
    /// <returns>
    /// Current instance of <see cref="IPolylineEncodingOptionsBuilder"/>.
    /// </returns>
    IPolylineEncodingOptionsBuilder WithBufferSize(int maxBufferSize);

    /// <summary>
    /// Sets the logger factory for logging purposes.
    /// </summary>
    /// <remarks>
    /// The logger factory is used to create loggers for encoding operations.
    /// </remarks>
    /// <param name="loggerFactory">
    /// The logger factory to use for logging. Default is <see cref="NullLoggerFactory"/>.
    /// </param>
    /// <returns>
    /// Current instance of <see cref="IPolylineEncodingOptionsBuilder"/>.
    /// </returns>
    IPolylineEncodingOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory);

    /// <summary>
    /// Builds the <see cref="PolylineEncodingOptions"/> with the specified configurations.
    /// </summary>
    /// <remarks>
    /// This method finalizes the configuration and returns a new instance of <see cref="PolylineEncodingOptions"/>.
    /// </remarks>
    /// <returns>
    /// A new instance of <see cref="PolylineEncodingOptions"/> with the configured options.
    /// </returns>
    PolylineEncodingOptions Build();
}
