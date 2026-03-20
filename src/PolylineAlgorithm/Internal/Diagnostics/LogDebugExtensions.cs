//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal.Diagnostics;

using Microsoft.Extensions.Logging;

internal static partial class LogDebugExtensions {
    private const LogLevel LOG_LEVEL = LogLevel.Debug;
    private const int EVENT_ID_BASE = (int)LOG_LEVEL * Defaults.Logging.LogLevelMultiplier;

    [LoggerMessage(EVENT_ID_BASE + 1, LOG_LEVEL, "Operation {operationName} has started.")]
    internal static partial void LogOperationStartedDebug(this ILogger logger, string operationName);

    [LoggerMessage(EVENT_ID_BASE + 2, LOG_LEVEL, "Operation {operationName} has failed.")]
    internal static partial void LogOperationFailedDebug(this ILogger logger, string operationName);

    [LoggerMessage(EVENT_ID_BASE + 3, LOG_LEVEL, "Operation {operationName} has finished.")]
    internal static partial void LogOperationFinishedDebug(this ILogger logger, string operationName);

    [LoggerMessage(EVENT_ID_BASE + 4, LOG_LEVEL, "Decoded coordinate: (Latitude: {latitude}, Longitude: {longitude}) at position {position}.")]
    internal static partial void LogDecodedCoordinateDebug(this ILogger logger, double latitude, double longitude, int position);
}
