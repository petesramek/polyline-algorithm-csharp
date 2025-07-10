//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Internal;

using Microsoft.Extensions.Logging;

internal static partial class Logging {
    [LoggerMessage(1, LogLevel.Warning, "Argument {argumentName} is null.")]
    internal static partial void LogNullArgumentWarning(this ILogger logger, string argumentName);

    [LoggerMessage(2, LogLevel.Warning, "Argument {argumentName} is empty.")]
    internal static partial void LogEmptyArgumentWarning(this ILogger logger, string argumentName);

    [LoggerMessage(3, LogLevel.Warning, "Internal buffer has {bufferLength} length. At position {position} is required additional {requiredSpace} space.")]
    internal static partial void LogInternalBufferOverflowWarning(this ILogger logger, int position, int bufferLength, int requiredSpace);

    [LoggerMessage(4, LogLevel.Warning, "Cannot write to internal buffer at position {position}. Current coordinate is at position {coordinateIndex}.")]
    internal static partial void LogCannotWriteValueToBufferWarning(this ILogger logger, int position, int coordinateIndex);

    [LoggerMessage(5, LogLevel.Warning, "Argument {argumentName} is too short. Minimal length is {minimumLength}. Actual length is {actualLength}.")]
    internal static partial void LogPolylineCannotBeShorterThanWarning(this ILogger logger, string argumentName, int actualLength, int minimumLength);

    [LoggerMessage(6, LogLevel.Warning, "Requested buffer size of {requestedBufferLength} exceeds maximum allowed buffer length of {maxBufferLength}.")]
    internal static partial void LogRequestedBufferSizeExceedsMaxBufferLengthWarning(this ILogger logger, int requestedBufferLength, int maxBufferLength);

    [LoggerMessage(7, LogLevel.Warning, "Polyline is invalid at position is {position}.")]
    internal static partial void LogInvalidPolylineWarning(this ILogger logger, int position);
}
