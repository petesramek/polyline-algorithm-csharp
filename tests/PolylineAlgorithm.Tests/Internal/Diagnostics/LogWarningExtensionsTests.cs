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
/// Tests for <see cref="LogWarningExtensions"/>.
/// </summary>
[TestClass]
public sealed class LogWarningExtensionsTests {
    private sealed class TestLogger : ILogger {
        public List<(LogLevel Level, EventId EventId, string Message, Exception? Exception)> Logs { get; } = new();

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
    public void LogNullArgumentWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogNullArgumentWarning("foo");
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Argument foo is null.")));
    }

    [TestMethod]
    public void LogEmptyArgumentWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogEmptyArgumentWarning("bar");
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Argument bar is empty.")));
    }

    [TestMethod]
    public void LogInternalBufferOverflowWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogInternalBufferOverflowWarning(1, 2, 3);
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Internal buffer has size of 2. At position 1 is required additional 3 space.")));
    }

    [TestMethod]
    public void LogCannotWriteValueToBufferWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogCannotWriteValueToBufferWarning(4, 5);
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Cannot write to internal buffer at position 4. Current coordinate is at index 5.")));
    }

    [TestMethod]
    public void LogPolylineCannotBeShorterThanWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogPolylineCannotBeShorterThanWarning(6, 7);
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Polyline is too short. Minimal length is 7. Actual length is 6.")));
    }

    [TestMethod]
    public void LogRequestedBufferSizeExceedsMaxBufferLengthWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogRequestedBufferSizeExceedsMaxBufferLengthWarning(8, 9);
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Requested buffer size of 8 exceeds maximum allowed buffer length of 9.")));
    }

    [TestMethod]
    public void LogInvalidPolylineWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        logger.LogInvalidPolylineWarning(10);
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Polyline is invalid or malformed at position 10.")));
    }

    [TestMethod]
    public void LogInvalidPolylineFormatWarning_LogsExpectedMessage() {
        var logger = new TestLogger();
        var ex = new Exception("fail");
        logger.LogInvalidPolylineFormatWarning(ex);
        Assert.IsTrue(logger.Logs.Exists(l => l.Message.Contains("Polyline is invalid or malformed.", StringComparison.Ordinal) && l.Exception == ex));
    }
}
