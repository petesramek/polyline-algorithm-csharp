//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;

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
        var bufferSize = 2_048;
        var loggerFactory = NullLoggerFactory.Instance;

        // Act
        var options = builder
            .Build();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(bufferSize, options.BufferSizeInBytes);
        Assert.AreEqual(bufferSize / sizeof(char), options.MaxBufferLength);
        Assert.AreEqual(loggerFactory, options.LoggerFactory);
    }

    [TestMethod]
    public void WithBufferSize_Small_BufferSize_Parameter_Returns_Throws_ArgumentOutOfRangeException() {
        // Arrange
        void WithSmallBufferSize() => PolylineEncodingOptionsBuilder.Create()
            .WithBufferSize(11);

        // Act
        var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(WithSmallBufferSize);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("maxBufferSize", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Buffer size must be greater than 11."));
    }

    [TestMethod]
    public void Build_Returns_Instance_With_Expected_Buffer_Size() {
        // Arrange
        var builder = PolylineEncodingOptionsBuilder.Create();
        var expected = 32_000;

        // Act
        var options = builder
            .WithBufferSize(expected)
            .Build();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(expected, options.BufferSizeInBytes);
        Assert.AreEqual(expected / sizeof(char), options.MaxBufferLength);
    }

    [TestMethod]
    public void Build_Returns_Instance_With_Expected_LoggerFactory() {
        // Arrange
        var builder = PolylineEncodingOptionsBuilder.Create();
        var expected = new FakeLoggerFactory(new FakeLoggerProvider());

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
        void WithNullLoggerFactory() => PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(null!);

        // Act
        var exception = Assert.ThrowsException<ArgumentNullException>(WithNullLoggerFactory);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("loggerFactory", exception.ParamName);
        Assert.IsTrue(exception.Message.Contains("Logger factory cannot be null."));
    }
}
