//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Internal.Logging;

using Microsoft.Extensions.Logging;

internal static partial class LogWarningExtensions {
    private const LogLevel LOG_LEVEL = LogLevel.Warning;
    private const int EVENT_ID_BASE = (int)LOG_LEVEL * LoggingDefaults.LogLevelMultiplier;

    [LoggerMessage(EVENT_ID_BASE + 1, LOG_LEVEL, "Argument {argumentName} is null.")]
    internal static partial void LogNullArgumentWarning(this ILogger logger, string argumentName);

    [LoggerMessage(EVENT_ID_BASE + 2, LOG_LEVEL, "Argument {argumentName} is empty.")]
    internal static partial void LogEmptyArgumentWarning(this ILogger logger, string argumentName);

    [LoggerMessage(EVENT_ID_BASE + 3, LOG_LEVEL, "Internal buffer has size of {bufferLength}. At position {position} is required additional {requiredSpace} space.")]
    internal static partial void LogInternalBufferOverflowWarning(this ILogger logger, int position, int bufferLength, int requiredSpace);

    [LoggerMessage(EVENT_ID_BASE + 4, LOG_LEVEL, "Cannot write to internal buffer at position {position}. Current coordinate is at index {coordinateIndex}.")]
    internal static partial void LogCannotWriteValueToBufferWarning(this ILogger logger, int position, int coordinateIndex);

    [LoggerMessage(EVENT_ID_BASE + 5, LOG_LEVEL, "Argument {argumentName} is too short. Minimal length is {minimumLength}. Actual length is {actualLength}.")]
    internal static partial void LogPolylineCannotBeShorterThanWarning(this ILogger logger, string argumentName, int actualLength, int minimumLength);

    [LoggerMessage(EVENT_ID_BASE + 6, LOG_LEVEL, "Requested buffer size of {requestedBufferLength} exceeds maximum allowed buffer length of {maxBufferLength}.")]
    internal static partial void LogRequestedBufferSizeExceedsMaxBufferLengthWarning(this ILogger logger, int requestedBufferLength, int maxBufferLength);

    [LoggerMessage(EVENT_ID_BASE + 7, LOG_LEVEL, "Polyline is invalid or malformed at position {position}.")]
    internal static partial void LogInvalidPolylineWarning(this ILogger logger, int position);
}
