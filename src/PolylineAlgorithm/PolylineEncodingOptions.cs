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
    /// Gets or sets the maximum size of the buffer used for encoding.
    /// </summary>
    /// <remarks>
    /// The default value is 1,024 characters.
    /// </remarks>
    public int MaxPolylineLength { get; internal set; } = 1_024;

    /// <summary>
    /// Gets or sets the precision for encoding coordinates.
    /// </summary>
    /// <remarks>
    /// The default logger factory is <see cref="NullLoggerFactory"/>, which does not log any messages.
    /// </remarks>
    public ILoggerFactory LoggerFactory { get; internal set; } = NullLoggerFactory.Instance;
}