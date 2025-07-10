//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;

[TestClass]
public class PolylineEncodingOptionsTest {
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange && Act
        var options = new PolylineEncodingOptions();

        // Assert
        Assert.AreEqual(64_000, options.BufferSize);
        Assert.AreEqual(64_000 / sizeof(char), options.MaxLength);
        Assert.IsInstanceOfType<NullLoggerFactory>(options.LoggerFactory);
    }

    [TestMethod]
    public void Constructor_ValidOptions_Ok() {
        // Arrange
        var bufferSize = 32_000;
        var loggerFactory = new FakeLoggerFactory(new FakeLoggerProvider());

        // Act
        var options = new PolylineEncodingOptions() {
            BufferSize = bufferSize,
            LoggerFactory = loggerFactory
        };

        // Assert
        Assert.AreEqual(bufferSize, options.BufferSize);
        Assert.AreEqual(bufferSize / sizeof(char), options.MaxLength);
        Assert.IsInstanceOfType<FakeLoggerFactory>(options.LoggerFactory);
    }
}
