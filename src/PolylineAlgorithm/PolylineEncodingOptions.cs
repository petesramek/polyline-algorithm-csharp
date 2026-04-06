//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides configuration options for polyline encoding operations.
/// </summary>
/// <remarks>
/// <para>
/// This class allows you to configure various aspects of polyline encoding, including:
/// </para>
/// <list type="bullet">
/// <item><description>The <see cref="Precision"/> level for coordinate encoding</description></item>
/// <item><description>The <see cref="LoggerFactory"/> for diagnostic logging</description></item>
/// </list>
/// <para>
/// All properties have internal setters and should be configured through a builder or factory pattern.
/// </para>
/// </remarks>
[DebuggerDisplay("Precision: {Precision}, LoggerFactoryType: {GetLoggerFactoryType()}")]
public sealed class PolylineEncodingOptions {
    /// <summary>
    /// Gets the logger factory used for diagnostic logging during encoding operations.
    /// </summary>
    /// <value>
    /// An <see cref="ILoggerFactory"/> instance. Defaults to <see cref="NullLoggerFactory.Instance"/>.
    /// </value>
    /// <remarks>
    /// The default logger factory is <see cref="NullLoggerFactory"/>, which does not log any messages.
    /// To enable logging, provide a custom <see cref="ILoggerFactory"/> implementation.
    /// </remarks>
    public ILoggerFactory LoggerFactory { get; internal set; } = NullLoggerFactory.Instance;

    /// <summary>
    /// Gets the precision level used for encoding coordinate values.
    /// </summary>
    /// <value>
    /// The number of decimal places to use when encoding coordinate values. Defaults to 5.
    /// </value>
    /// <remarks>
    /// <para>
    /// The precision determines the number of decimal places to which each coordinate value (latitude or longitude)
    /// is multiplied and truncated (not rounded) before encoding. For example, a precision of 5 means each coordinate is multiplied by 10^5
    /// and truncated to an integer before encoding.
    /// </para>
    /// <para>
    /// This setting does not directly correspond to a physical distance or accuracy in meters, but rather controls
    /// the granularity of the encoded values.
    /// </para>
    /// </remarks>
    public uint Precision { get; internal set; } = 5;

    /// <summary>
    /// Returns the type name of the logger factory for debugging purposes.
    /// </summary>
    /// <returns>
    /// A string containing the type name of the current <see cref="LoggerFactory"/> instance.
    /// </returns>
    [ExcludeFromCodeCoverage]
    private string GetLoggerFactoryType() => LoggerFactory.GetType().Name;
}