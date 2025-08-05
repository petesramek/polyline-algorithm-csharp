//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;

/// <summary>
/// Options for configuring polyline encoding.
/// </summary>
/// <remarks>
/// This class allows you to set options such as buffer size and logger factory for encoding operations.
/// </remarks>
[DebuggerDisplay("MaxBufferSize: {MaxBufferSize}, MaxBufferLength: {MaxBufferLength}, LoggerFactoryType: {LoggerFactory.GetType().Name}")]
public sealed class PolylineEncodingOptions {
    /// <summary>
    /// Gets the maximum buffer size for encoding operations.
    /// </summary>
    /// <remarks>
    /// The default buffer size is 64,000 bytes (64 KB). This can be adjusted based on the expected size of the polyline data.
    /// </remarks>
    public int MaxBufferSize { get; internal set; } = 64_000;

    /// <summary>
    /// Gets or sets the precision for encoding coordinates.
    /// </summary>
    /// <remarks>
    /// The default logger factory is <see cref="NullLoggerFactory"/>, which does not log any messages.
    /// </remarks>
    public ILoggerFactory LoggerFactory { get; internal set; } = NullLoggerFactory.Instance;


    /// <summary>
    /// Gets the maximum length of the encoded polyline string.
    /// </summary>
    /// <remarks>
    /// The maximum length is calculated based on the buffer size divided by the size of a character.
    /// </remarks>
    internal int MaxBufferLength => MaxBufferSize / sizeof(char);
}