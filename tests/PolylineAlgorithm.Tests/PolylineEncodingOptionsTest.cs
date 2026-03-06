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
        Assert.AreEqual(512, options.StackAllocLimit);
        Assert.IsInstanceOfType<NullLoggerFactory>(options.LoggerFactory);
    }

    [TestMethod]
    public void Constructor_ValidOptions_Ok() {
        // Arrange
        var stackAllocLimit = 256;
        var loggerFactory = new FakeLoggerFactory(new FakeLoggerProvider());

        // Act
        var options = new PolylineEncodingOptions() {
            StackAllocLimit = stackAllocLimit,
            LoggerFactory = loggerFactory
        };

        // Assert
        Assert.AreEqual(stackAllocLimit, options.StackAllocLimit);
        Assert.IsInstanceOfType<FakeLoggerFactory>(options.LoggerFactory);
    }
}
