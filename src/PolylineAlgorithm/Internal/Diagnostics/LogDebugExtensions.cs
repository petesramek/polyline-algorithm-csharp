//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal.Diagnostics;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides strongly-typed logging extension methods for debug-level diagnostics used throughout the library.
/// These methods are partial and annotated with <see cref="LoggerMessageAttribute"/> so the .NET
/// source generator can produce efficient logging implementations at compile time.
/// </summary>
/// <remarks>
/// Event IDs are derived from the debug <see cref="LogLevel"/> value: <c>(int)LogLevel.Debug * 100</c>.
/// For the current <see cref="LogLevel.Debug"/> value this yields a base of <c>100</c>,
/// so individual event IDs range from <c>101</c> to <c>104</c>.
/// </remarks>
internal static partial class LogDebugExtensions {
    private const LogLevel LOG_LEVEL = LogLevel.Debug;
    private const int EVENT_ID_BASE = (int)LOG_LEVEL * Defaults.Logging.LogLevelMultiplier;

    /// <summary>
    /// Logs a debug message when an operation has started.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="operationName">The name of the operation that has started.</param>
    [LoggerMessage(EVENT_ID_BASE + 1, LOG_LEVEL, "Operation {operationName} has started.")]
    internal static partial void LogOperationStartedDebug(this ILogger logger, string operationName);

    /// <summary>
    /// Logs a debug message when an operation has failed.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="operationName">The name of the operation that has failed.</param>
    [LoggerMessage(EVENT_ID_BASE + 2, LOG_LEVEL, "Operation {operationName} has failed.")]
    internal static partial void LogOperationFailedDebug(this ILogger logger, string operationName);

    /// <summary>
    /// Logs a debug message when an operation has finished successfully.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="operationName">The name of the operation that has finished.</param>
    [LoggerMessage(EVENT_ID_BASE + 3, LOG_LEVEL, "Operation {operationName} has finished.")]
    internal static partial void LogOperationFinishedDebug(this ILogger logger, string operationName);

    /// <summary>
    /// Logs a debug message containing the decoded values and position.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="latitude">The decoded latitude value.</param>
    /// <param name="longitude">The decoded longitude value.</param>
    /// <param name="position">The position in the polyline buffer at which the value was decoded.</param>
    [LoggerMessage(EVENT_ID_BASE + 4, LOG_LEVEL, "Decoded coordinate: (Latitude: {latitude}, Longitude: {longitude}) at position {position}.")]
    internal static partial void LogDecodedCoordinateDebug(this ILogger logger, double latitude, double longitude, int position);
}
