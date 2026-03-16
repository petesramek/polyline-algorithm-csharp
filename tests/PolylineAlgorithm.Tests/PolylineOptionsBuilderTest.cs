//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;
using PolylineAlgorithm;
using PolylineAlgorithm.Tests.Fakes;

[TestClass]
public class PolylineOptionsBuilderTest {
    [TestMethod]
    public void Create_Returns_Instance() {
        // Arrange && Act
        var builder = PolylineEncodingOptionsBuilder.Create();

        // Assert
        Assert.IsNotNull(builder);
    }

    [TestMethod]
    public void Build_Returns_Instance_With_Default_Values() {
        // Arrange
        var builder = PolylineEncodingOptionsBuilder.Create();
        const int stackAllocLimit = 512;
        var loggerFactory = NullLoggerFactory.Instance;

        // Act
        var options = builder
            .Build();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(stackAllocLimit, options.StackAllocLimit);
        Assert.AreEqual(loggerFactory, options.LoggerFactory);
    }

    [TestMethod]
    public void WithStackAllocLimit_Small_StackAllocLimit_Parameter_Returns_Throws_ArgumentOutOfRangeException() {
        // Arrange
        static void WithSmallBufferSize() => PolylineEncodingOptionsBuilder.Create()
            .WithStackAllocLimit(0);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(WithSmallBufferSize);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("stackAllocLimit", exception.ParamName);
        Assert.Contains("Stack alloc limit must be equal or greater than 1.", exception.Message, StringComparison.Ordinal);
    }

    [TestMethod]
    public void Build_Returns_Instance_With_Expected_StackAllocLimit() {
        // Arrange
        var builder = PolylineEncodingOptionsBuilder.Create();
        const int expected = 256;

        // Act
        var options = builder
            .WithStackAllocLimit(expected)
            .Build();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(expected, options.StackAllocLimit);
    }

    [TestMethod]
    public void Build_Returns_Instance_With_Expected_LoggerFactory() {
        // Arrange
        var builder = PolylineEncodingOptionsBuilder.Create();
        using var expected = new FakeLoggerFactory(new FakeLoggerProvider());

        // Act
        var options = builder
            .WithLoggerFactory(expected)
            .Build();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(expected, options.LoggerFactory);
    }

    [TestMethod]
    public void WithLoggerFactory_Null_Parameter_Returns_Throws_ArgumentNullException() {
        // Arrange
        static void WithNullLoggerFactory() => PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(null!);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(WithNullLoggerFactory);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("loggerFactory", exception.ParamName);
    }
}
