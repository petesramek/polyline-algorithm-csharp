//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal.Diagnostics;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Internal.Diagnostics;
using PolylineAlgorithm.Tests.Properties;
using System;
using System.Collections.Generic;

/// <summary>
/// Tests for <see cref="LogDebugExtensions"/>.
/// </summary>
[TestClass]
public sealed class LogDebugExtensionsTests {
    private sealed class TestLogger : ILogger {
        public List<(LogLevel Level, EventId EventId, string Message, Exception? Exception)> Logs { get; } = [];

        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
            Logs.Add((logLevel, eventId, formatter(state, exception), exception));
        }

        private sealed class NullScope : IDisposable {
            public static NullScope Instance { get; } = new();
            public void Dispose() { }
        }
    }

    [TestMethod]

    public void LogOperationStartedDebug_WithOperationName_LogsStartedMessage() {
        var logger = new TestLogger();
        const string operationName = "TestOperation";

        logger.LogOperationStartedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Operation {operationName} has started.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationFailedDebug_WithOperationName_LogsFailedMessage() {
        var logger = new TestLogger();
        const string operationName = "TestOperation";

        logger.LogOperationFailedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Operation {operationName} has failed.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationFinishedDebug_WithOperationName_LogsFinishedMessage() {
        var logger = new TestLogger();
        const string operationName = "TestOperation";

        logger.LogOperationFinishedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Operation {operationName} has finished.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogDecodedCoordinateDebug_WithCoordinatesAndPosition_LogsDecodedCoordinateMessage() {
        var logger = new TestLogger();
        const double latitude = 38.5;
        const double longitude = -120.2;
        const int position = 42;

        logger.LogDecodedCoordinateDebug(latitude, longitude, position);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Decoded coordinate: (Latitude: {latitude}, Longitude: {longitude}) at position {position}.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationStartedDebug_WithNullOperationName_LogsMessage() {
        var logger = new TestLogger();
        const string? operationName = null;

        logger.LogOperationStartedDebug(operationName!);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationFailedDebug_WithNullOperationName_LogsMessage() {
        var logger = new TestLogger();
        const string? operationName = null;

        logger.LogOperationFailedDebug(operationName!);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationFinishedDebug_WithNullOperationName_LogsMessage() {
        var logger = new TestLogger();
        const string? operationName = null;

        logger.LogOperationFinishedDebug(operationName!);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogDecodedCoordinateDebug_WithZeroCoordinates_LogsMessage() {
        var logger = new TestLogger();
        const double latitude = 0.0;
        const double longitude = 0.0;
        const int position = 0;

        logger.LogDecodedCoordinateDebug(latitude, longitude, position);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Decoded coordinate", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogDecodedCoordinateDebug_WithNegativeCoordinates_LogsMessage() {
        var logger = new TestLogger();
        const double latitude = -90.0;
        const double longitude = -180.0;
        const int position = 100;

        logger.LogDecodedCoordinateDebug(latitude, longitude, position);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Latitude: {latitude}, Longitude: {longitude}", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationStartedDebug_WithEmptyOperationName_LogsMessage() {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationStartedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationFailedDebug_WithEmptyOperationName_LogsMessage() {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationFailedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    [TestMethod]

    public void LogOperationFinishedDebug_WithEmptyOperationName_LogsMessage() {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationFinishedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }
}