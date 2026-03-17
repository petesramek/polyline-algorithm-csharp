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
[DebuggerDisplay("StackAllocLimit: {StackAllocLimit}, LoggerFactoryType: {GetLoggerFactoryType()}")]
public sealed class PolylineEncodingOptions {
    /// <summary>
    /// Gets logger factory.
    /// </summary>
    /// <remarks>
    /// The default logger factory is <see cref="NullLoggerFactory"/>, which does not log any messages.
    /// </remarks>
    public ILoggerFactory LoggerFactory { get; internal set; } = NullLoggerFactory.Instance;

    /// <summary>
    /// Gets stackalloc limit.
    /// </summary>
    /// <remarks>
    /// The default stack alloc limit is 512.
    /// </remarks>
    public int StackAllocLimit { get; internal set; } = 512;

    /// <summary>
    /// Returns type name of a logger factory for debugging purposes.
    /// </summary>
    /// <returns>
    /// String representation of <see cref="LoggerFactory"/> property instance.
    /// </returns>
    private string GetLoggerFactoryType() => LoggerFactory.GetType().Name;
}