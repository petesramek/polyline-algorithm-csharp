//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;
using PolylineAlgorithm.Tests.Fakes;

[TestClass]
public class PolylineEncodingOptionsTest {
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange && Act
        var options = new PolylineEncodingOptions();

        // Assert
        Assert.AreEqual(64_000, options.MaxBufferSize);
        Assert.AreEqual(64_000 / sizeof(char), options.MaxBufferLength);
        Assert.IsInstanceOfType<NullLoggerFactory>(options.LoggerFactory);
    }

    [TestMethod]
    public void Constructor_ValidOptions_Ok() {
        // Arrange
        var bufferSize = 32_000;
        var loggerFactory = new FakeLoggerFactory(new FakeLoggerProvider());

        // Act
        var options = new PolylineEncodingOptions() {
            MaxBufferSize = bufferSize,
            LoggerFactory = loggerFactory
        };

        // Assert
        Assert.AreEqual(bufferSize, options.MaxBufferSize);
        Assert.AreEqual(bufferSize / sizeof(char), options.MaxBufferLength);
        Assert.IsInstanceOfType<FakeLoggerFactory>(options.LoggerFactory);
    }
}
