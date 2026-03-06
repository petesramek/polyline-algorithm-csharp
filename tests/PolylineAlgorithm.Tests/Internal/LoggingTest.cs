//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using PolylineAlgorithm.Internal.Logging;
using PolylineAlgorithm.Tests.Fakes;

[TestClass]
public class LoggingTest {
    private static readonly FakeLoggerProvider _loggerProvider = new();
    private static readonly ILoggerFactory _loggerFactory = new FakeLoggerFactory(_loggerProvider);

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("operationName")]
    public void ILogger_LogOperationStartedDebug_Ok(string value) {
        // Arrange
        string operationName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogOperationStartedDebug(operationName);

        // Assert
        Assert.AreEqual(new EventId(101, nameof(LogDebugExtensions.LogOperationStartedDebug)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Information, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Operation {value ?? "(null)"} has started.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("operationName")]
    public void ILogger_LogOperationFailedDebug_Ok(string value) {
        // Arrange
        string operationName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogOperationFailedDebug(operationName);

        // Assert
        Assert.AreEqual(new EventId(102, nameof(LogDebugExtensions.LogOperationFailedDebug)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Information, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Operation {value ?? "(null)"} has failed.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("operationName")]
    public void ILogger_LogOperationFinishedDebug_Ok(string value) {
        // Arrange
        string operationName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogOperationFinishedDebug(operationName);

        // Assert
        Assert.AreEqual(new EventId(103, nameof(LogDebugExtensions.LogOperationFinishedDebug)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Information, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Operation {value ?? "(null)"} has finished.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("argumentName")]
    public void ILogger_LogNullArgumentWarning_Ok(string value) {
        // Arrange
        string argumentName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogNullArgumentWarning(argumentName);

        // Assert
        Assert.AreEqual(new EventId(301, nameof(LogWarningExtensions.LogNullArgumentWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual($"Argument {value ?? "(null)"} is null.", _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("argumentName")]
    public void ILogger_LogEmptyArgumentWarning_Ok(string value) {
        // Arrange
        string argumentName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogEmptyArgumentWarning(argumentName);

        // Assert
        Assert.AreEqual(new EventId(302, nameof(LogWarningExtensions.LogEmptyArgumentWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual($"Argument {value ?? "(null)"} is empty.", _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    public void ILogger_LogInternalBufferOverflowWarning_Ok() {
        // Arrange
        int position = 5;
        int bufferLength = 10;
        int requiredSpace = 15;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogInternalBufferOverflowWarning(position, bufferLength, requiredSpace);

        // Assert
        Assert.AreEqual(new EventId(303, nameof(LogWarningExtensions.LogInternalBufferOverflowWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Internal buffer has size of {bufferLength}. At position {position} is required additional {requiredSpace} space.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    public void ILogger_LogCannotWriteValueToBufferWarning_Ok() {
        // Arrange
        int position = 5;
        int index = 1;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogCannotWriteValueToBufferWarning(position, index);

        // Assert
        Assert.AreEqual(new EventId(304, nameof(LogWarningExtensions.LogCannotWriteValueToBufferWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Cannot write to internal buffer at position {position}. Current coordinate is at index {index}.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("argumentName")]
    public void ILogger_LogPolylineCannotBeShorterThanWarning_Ok(string value) {
        // Arrange
        string argumentName = value;
        int actualLength = 10;
        int minimumLength = 5;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogPolylineCannotBeShorterThanWarning(argumentName, actualLength, minimumLength);

        // Assert
        Assert.AreEqual(new EventId(305, nameof(LogWarningExtensions.LogPolylineCannotBeShorterThanWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Argument {value} is too short. Minimal length is {minimumLength}. Actual length is {actualLength}.",
            _loggerProvider.Collector.LatestRecord.Message);
    }


    [TestMethod]
    public void ILogger_LogRequestedBufferSizeExceedsMaxBufferLengthWarning_Ok() {
        // Arrange
        int requestedBufferLength = 5;
        int maxBufferLength = 10;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogRequestedBufferSizeExceedsMaxBufferLengthWarning(requestedBufferLength, maxBufferLength);

        // Assert
        Assert.AreEqual(new EventId(306, nameof(LogWarningExtensions.LogRequestedBufferSizeExceedsMaxBufferLengthWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Requested buffer size of {requestedBufferLength} exceeds maximum allowed buffer length of {maxBufferLength}.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    public void ILogger_LogInvalidPolylineWarning_Ok() {
        // Arrange
        int position = 5;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogInvalidPolylineWarning(position);

        // Assert
        Assert.AreEqual(new EventId(307, nameof(LogWarningExtensions.LogInvalidPolylineWarning)), _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Polyline is invalid or malformed at position {position}.",
            _loggerProvider.Collector.LatestRecord.Message);
    }
}
