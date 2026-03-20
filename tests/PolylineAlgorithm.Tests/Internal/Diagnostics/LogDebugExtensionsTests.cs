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
using System.Linq;

/// <summary>
/// Tests for <see cref="LogDebugExtensions"/>.
/// </summary>
[TestClass]
public sealed class LogDebugExtensionsTests
{
    private sealed class TestLogger : ILogger
    {
        public List<(LogLevel Level, EventId EventId, string Message, Exception? Exception)> Logs { get; } = new();

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Logs.Add((logLevel, eventId, formatter(state, exception), exception));
        }

        private sealed class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new();
            public void Dispose() { }
        }
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationStartedDebug_WithOperationName_LogsStartedMessage()
    {
        var logger = new TestLogger();
        string operationName = "TestOperation";

        logger.LogOperationStartedDebug(operationName);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, $"Operation {operationName} has started.");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationFailedDebug_WithOperationName_LogsFailedMessage()
    {
        var logger = new TestLogger();
        string operationName = "TestOperation";

        logger.LogOperationFailedDebug(operationName);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, $"Operation {operationName} has failed.");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationFinishedDebug_WithOperationName_LogsFinishedMessage()
    {
        var logger = new TestLogger();
        string operationName = "TestOperation";

        logger.LogOperationFinishedDebug(operationName);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, $"Operation {operationName} has finished.");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogDecodedCoordinateDebug_WithCoordinatesAndPosition_LogsDecodedCoordinateMessage()
    {
        var logger = new TestLogger();
        double latitude = 38.5;
        double longitude = -120.2;
        int position = 42;

        logger.LogDecodedCoordinateDebug(latitude, longitude, position);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, $"Decoded coordinate: (Latitude: {latitude}, Longitude: {longitude}) at position {position}.");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationStartedDebug_WithNullOperationName_LogsMessage()
    {
        var logger = new TestLogger();
        string? operationName = null;

        logger.LogOperationStartedDebug(operationName!);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Operation");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationFailedDebug_WithNullOperationName_LogsMessage()
    {
        var logger = new TestLogger();
        string? operationName = null;

        logger.LogOperationFailedDebug(operationName!);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Operation");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationFinishedDebug_WithNullOperationName_LogsMessage()
    {
        var logger = new TestLogger();
        string? operationName = null;

        logger.LogOperationFinishedDebug(operationName!);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Operation");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogDecodedCoordinateDebug_WithZeroCoordinates_LogsMessage()
    {
        var logger = new TestLogger();
        double latitude = 0.0;
        double longitude = 0.0;
        int position = 0;

        logger.LogDecodedCoordinateDebug(latitude, longitude, position);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Decoded coordinate");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogDecodedCoordinateDebug_WithNegativeCoordinates_LogsMessage()
    {
        var logger = new TestLogger();
        double latitude = -90.0;
        double longitude = -180.0;
        int position = 100;

        logger.LogDecodedCoordinateDebug(latitude, longitude, position);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, $"Latitude: {latitude}, Longitude: {longitude}");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationStartedDebug_WithEmptyOperationName_LogsMessage()
    {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationStartedDebug(operationName);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Operation");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationFailedDebug_WithEmptyOperationName_LogsMessage()
    {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationFailedDebug(operationName);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Operation");
    }

    [TestMethod]
    [TestCategory(Category.Unit)]
    public void LogOperationFinishedDebug_WithEmptyOperationName_LogsMessage()
    {
        var logger = new TestLogger();
        string operationName = string.Empty;

        logger.LogOperationFinishedDebug(operationName);

        var log = logger.Logs.Single();
        Assert.AreEqual(LogLevel.Debug, log.Level);
        StringAssert.Contains(log.Message, "Operation");
    }
}
