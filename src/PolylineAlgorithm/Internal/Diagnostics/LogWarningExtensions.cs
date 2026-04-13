//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal.Diagnostics;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides strongly-typed logging extension methods for warning-level diagnostics used throughout the library.
/// These methods are partial and annotated with <see cref="LoggerMessageAttribute"/> so the .NET
/// source generator can produce efficient logging implementations at compile time.
/// </summary>
/// <remarks>
/// Event IDs are derived from the warning <see cref="LogLevel"/> value: <c>(int)LogLevel.Warning * 100</c>.
/// For the current <see cref="LogLevel.Warning"/> value this yields a base of <c>300</c>,
/// so individual event IDs range from <c>301</c> to <c>308</c>.
/// </remarks>
internal static partial class LogWarningExtensions {
    /// <summary>
    /// The log level used by all methods in this class.
    /// </summary>
    private const LogLevel LOG_LEVEL = LogLevel.Warning;

    /// <summary>
    /// Base value for event ids used by the <see cref="LoggerMessageAttribute"/> annotations.
    /// Computed as <c>(int)LOG_LEVEL * 100</c>.
    /// </summary>
    private const int EVENT_ID_BASE = (int)LOG_LEVEL * Defaults.Logging.LogLevelMultiplier;

    /// <summary>
    /// Logs a warning when a method argument is <c>null</c>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="argumentName">The name of the argument that was <c>null</c>.</param>
    [LoggerMessage(EVENT_ID_BASE + 1, LOG_LEVEL, "Argument {argumentName} is null.")]
    internal static partial void LogNullArgumentWarning(this ILogger logger, string argumentName);

    /// <summary>
    /// Logs a warning when a method argument is empty (for example, an empty string or collection).
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="argumentName">The name of the argument that was empty.</param>
    [LoggerMessage(EVENT_ID_BASE + 2, LOG_LEVEL, "Argument {argumentName} is empty.")]
    internal static partial void LogEmptyArgumentWarning(this ILogger logger, string argumentName);

    /// <summary>
    /// Logs a warning when an internal buffer does not have enough space for a required write.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="position">The attempted write position in the buffer.</param>
    /// <param name="bufferLength">The current buffer length.</param>
    /// <param name="requiredSpace">The additional space required at <paramref name="position"/>.</param>
    [LoggerMessage(EVENT_ID_BASE + 3, LOG_LEVEL, "Internal buffer has size of {bufferLength}. At position {position} is required additional {requiredSpace} space.")]
    internal static partial void LogInternalBufferOverflowWarning(this ILogger logger, int position, int bufferLength, int requiredSpace);

    /// <summary>
    /// Logs a warning when the library cannot write a value to an internal buffer at the requested position.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="position">The buffer position where the write was attempted.</param>
    /// <param name="valueIndex">The index of the current value that prevented the write.</param>
    [LoggerMessage(EVENT_ID_BASE + 4, LOG_LEVEL, "Cannot write to internal buffer at position {position}. Current value is at index {valueIndex}.")]
    internal static partial void LogCannotWriteValueToBufferWarning(this ILogger logger, int position, int valueIndex);

    /// <summary>
    /// Logs a warning when a polyline is shorter than the minimal required length.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="actualLength">The actual length of the polyline.</param>
    /// <param name="minimumLength">The minimal required length for the operation.</param>
    [LoggerMessage(EVENT_ID_BASE + 5, LOG_LEVEL, "Polyline is too short. Minimal length is {minimumLength}. Actual length is {actualLength}.")]
    internal static partial void LogPolylineCannotBeShorterThanWarning(this ILogger logger, int actualLength, int minimumLength);

    /// <summary>
    /// Logs a warning when a requested buffer size exceeds the maximum allowed buffer length.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="requestedBufferLength">The requested buffer length.</param>
    /// <param name="maxBufferLength">The maximum allowed buffer length.</param>
    [LoggerMessage(EVENT_ID_BASE + 6, LOG_LEVEL, "Requested buffer size of {requestedBufferLength} exceeds maximum allowed buffer length of {maxBufferLength}.")]
    internal static partial void LogRequestedBufferSizeExceedsMaxBufferLengthWarning(this ILogger logger, int requestedBufferLength, int maxBufferLength);

    /// <summary>
    /// Logs a warning when a polyline is detected as invalid or malformed at a specific position.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="position">The position within the polyline where the problem was detected.</param>
    [LoggerMessage(EVENT_ID_BASE + 7, LOG_LEVEL, "Polyline is invalid or malformed at position {position}.")]
    internal static partial void LogInvalidPolylineWarning(this ILogger logger, int position);

    /// <summary>
    /// Logs a general warning that a polyline is invalid or malformed. Includes an optional exception
    /// to help diagnose formatting or parsing issues.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used to write the log entry.</param>
    /// <param name="ex">The exception associated with the malformed polyline, if any.</param>
    [LoggerMessage(EVENT_ID_BASE + 8, LOG_LEVEL, "Polyline is invalid or malformed.")]
    internal static partial void LogInvalidPolylineFormatWarning(this ILogger logger, Exception ex);
}