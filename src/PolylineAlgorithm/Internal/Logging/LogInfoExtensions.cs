//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal.Logging;

using Microsoft.Extensions.Logging;

internal static partial class LogInfoExtensions {
    private const LogLevel LOG_LEVEL = LogLevel.Information;
    private const int EVENT_ID_BASE = (int)LOG_LEVEL * Defaults.Logging.LogLevelMultiplier;

    [LoggerMessage(EVENT_ID_BASE + 1, LOG_LEVEL, "Operation {operationName} has started.")]
    internal static partial void LogOperationStartedInfo(this ILogger logger, string operationName);

    [LoggerMessage(EVENT_ID_BASE + 2, LOG_LEVEL, "Operation {operationName} has failed.")]
    internal static partial void LogOperationFailedInfo(this ILogger logger, string operationName);

    [LoggerMessage(EVENT_ID_BASE + 3, LOG_LEVEL, "Operation {operationName} has finished.")]
    internal static partial void LogOperationFinishedInfo(this ILogger logger, string operationName);
}
