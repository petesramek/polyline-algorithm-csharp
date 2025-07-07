namespace PolylineAlgorithm.Abstraction.Tests.Internal;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using PolylineAlgorithm.Abstraction.Internal;

[TestClass]
public class LoggingTest {
    static FakeLoggerProvider _loggerProvider = new FakeLoggerProvider();
    static ILoggerFactory _loggerFactory = new FakeLoggerFactory(_loggerProvider);

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("argumentName")]
    public void ILogger_LogNullArgumentError_Ok(string value) {
        // Arrange
        string argumentName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogNullArgumentError(argumentName);

        // Assert
        Assert.AreEqual(1, _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Error, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual($"Argument {value ?? "(null)"} cannot be null.", _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("argumentName")]
    public void ILogger_LogEmptyArgumentError_Ok(string value) {
        // Arrange
        string argumentName = value;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogEmptyArgumentError(argumentName);

        // Assert
        Assert.AreEqual(2, _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Error, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual($"Argument {value ?? "(null)"} cannot be empty.", _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    public void ILogger_LogInternalBufferOverflowError_Ok() {
        // Arrange
        int position = 5;
        int bufferLength = 10;
        int requiredSpace = 15;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogInternalBufferOverflowError(position, bufferLength, requiredSpace);

        // Assert
        Assert.AreEqual(3, _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Error, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Internal buffer has {bufferLength} length. At position {position} is required additional {requiredSpace} length.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    public void ILogger_LogCannotWriteValueToBufferError_Ok() {
        // Arrange
        int position = 5;
        int index = 1;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogCannotWriteValueToBufferError(position, index);

        // Assert
        Assert.AreEqual(4, _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Error, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Cannot write to internal buffer at position {position}. Current coordinate index is {index}.",
            _loggerProvider.Collector.LatestRecord.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("argumentName")]
    public void ILogger_LogPolylineCannotBeShorterThanError_Ok(string value) {
        // Arrange
        string argumentName = value;
        int actualLength = 10;
        int minimumLength = 5;

        // Act
        _loggerFactory
            .CreateLogger<LoggingTest>()
            .LogPolylineCannotBeShorterThanError(argumentName, actualLength, minimumLength);

        // Assert
        Assert.AreEqual(5, _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Error, _loggerProvider.Collector.LatestRecord.Level);
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
        Assert.AreEqual(6, _loggerProvider.Collector.LatestRecord.Id);
        Assert.AreEqual(LogLevel.Warning, _loggerProvider.Collector.LatestRecord.Level);
        Assert.AreEqual(
            $"Requested buffer size of {requestedBufferLength} exceeds maximum allowed buffer length of {maxBufferLength}.",
            _loggerProvider.Collector.LatestRecord.Message);
    }
}
