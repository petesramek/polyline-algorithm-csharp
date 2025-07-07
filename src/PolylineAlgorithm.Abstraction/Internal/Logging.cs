namespace PolylineAlgorithm.Abstraction.Internal;

using Microsoft.Extensions.Logging;

internal static partial class Logging {
    [LoggerMessage(1, LogLevel.Error, "Argument {argumentName} cannot be null.")]
    internal static partial void LogNullArgumentError(this ILogger logger, string argumentName);

    [LoggerMessage(2, LogLevel.Error, "Argument {argumentName} cannot be empty.")]
    internal static partial void LogEmptyArgumentError(this ILogger logger, string argumentName);

    [LoggerMessage(3, LogLevel.Error, "Internal buffer has {bufferLength} length. At position {position} is required additional {requiredSpace} length.")]
    internal static partial void LogInternalBufferOverflowError(this ILogger logger, int position, int bufferLength, int requiredSpace);
    
    [LoggerMessage(4, LogLevel.Error, "Cannot write to internal buffer at position {position}.")]
    internal static partial void LogCannotWriteValueToBufferError(this ILogger logger, int position);

    [LoggerMessage(5, LogLevel.Error, "Argument {argumentName} is too short. Minimal length is {minimumLength}. Actual length is {actualLength}.")]
    internal static partial void LogPolylineCannotBeShorterThanError(this ILogger logger, string argumentName, int actualLength, int minimumLength);

    [LoggerMessage(6, LogLevel.Warning, "Requested buffer size of {requestedBufferLength} exceeds maximum allowed buffer length of {maxBufferLength}.")]
    internal static partial void LogRequestedBufferSizeExceedsMaxBufferLengthWarning(this ILogger logger, int requestedBufferLength, int maxBufferLength);
}
