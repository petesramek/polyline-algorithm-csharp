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
        Assert.AreEqual(1_024, options.MaxPolylineLength);
        Assert.IsInstanceOfType<NullLoggerFactory>(options.LoggerFactory);
    }

    [TestMethod]
    public void Property_Initializer_Ok() {
        // Arrange
        var maxPolylineLength = 32_000;
        var loggerFactory = new FakeLoggerFactory(new FakeLoggerProvider());

        // Act
        var options = new PolylineEncodingOptions() {
            MaxPolylineLength = maxPolylineLength,
            LoggerFactory = loggerFactory
        };

        // Assert
        Assert.AreEqual(maxPolylineLength, options.MaxPolylineLength);
        Assert.IsInstanceOfType<FakeLoggerFactory>(options.LoggerFactory);
    }
}
