//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Internal.Diagnostics;

using Microsoft.Extensions.Logging;
using PolylineAlgorithm.Internal.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;

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

    /// <summary>
    /// Tests that LogOperationStartedDebug WithOperationName LogsStartedMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationStartedDebug_With_Operation_Name_Logs_Started_Message() {
        var logger = new TestLogger();
        const string operationName = "TestOperation";

        logger.LogOperationStartedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Operation {operationName} has started.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationFailedDebug WithOperationName LogsFailedMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationFailedDebug_With_Operation_Name_Logs_Failed_Message() {
        var logger = new TestLogger();
        const string operationName = "TestOperation";

        logger.LogOperationFailedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Operation {operationName} has failed.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationFinishedDebug WithOperationName LogsFinishedMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationFinishedDebug_With_Operation_Name_Logs_Finished_Message() {
        var logger = new TestLogger();
        const string operationName = "TestOperation";

        logger.LogOperationFinishedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains($"Operation {operationName} has finished.", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogDecodedValuesDebug WithCountAndPosition LogsDecodedValuesMessage.
    /// </summary>
    [TestMethod]
    public void LogDecodedValuesDebug_With_Count_And_Position_Logs_Decoded_Values_Message() {
        var logger = new TestLogger();
        const int count = 2;
        const int position = 42;

        logger.LogDecodedValuesDebug(count, position);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Decoded", logger.Logs[0].Message, StringComparison.Ordinal);
        Assert.Contains(count.ToString(CultureInfo.InvariantCulture), logger.Logs[0].Message, StringComparison.Ordinal);
        Assert.Contains(position.ToString(CultureInfo.InvariantCulture), logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationStartedDebug WithNullOperationName LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationStartedDebug_With_Null_Operation_Name_Logs_Message() {
        var logger = new TestLogger();
        const string? operationName = null;

        logger.LogOperationStartedDebug(operationName!);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationFailedDebug WithNullOperationName LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationFailedDebug_With_Null_Operation_Name_Logs_Message() {
        var logger = new TestLogger();
        const string? operationName = null;

        logger.LogOperationFailedDebug(operationName!);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationFinishedDebug WithNullOperationName LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationFinishedDebug_With_Null_Operation_Name_Logs_Message() {
        var logger = new TestLogger();
        const string? operationName = null;

        logger.LogOperationFinishedDebug(operationName!);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogDecodedValuesDebug WithZeroCountAndPosition LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogDecodedValuesDebug_With_Zero_Count_Logs_Message() {
        var logger = new TestLogger();
        const int count = 0;
        const int position = 0;

        logger.LogDecodedValuesDebug(count, position);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Decoded", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogDecodedValuesDebug WithLargeCount LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogDecodedValuesDebug_With_Large_Count_Logs_Message() {
        var logger = new TestLogger();
        const int count = 5;
        const int position = 100;

        logger.LogDecodedValuesDebug(count, position);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains(count.ToString(CultureInfo.InvariantCulture), logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationStartedDebug WithEmptyOperationName LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationStartedDebug_With_Empty_Operation_Name_Logs_Message() {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationStartedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationFailedDebug WithEmptyOperationName LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationFailedDebug_With_Empty_Operation_Name_Logs_Message() {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationFailedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that LogOperationFinishedDebug WithEmptyOperationName LogsMessage.
    /// </summary>
    [TestMethod]
    public void LogOperationFinishedDebug_With_Empty_Operation_Name_Logs_Message() {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationFinishedDebug(operationName);

        Assert.HasCount(1, logger.Logs);
        Assert.AreEqual(LogLevel.Debug, logger.Logs[0].Level);
        Assert.Contains("Operation", logger.Logs[0].Message, StringComparison.Ordinal);
    }
}