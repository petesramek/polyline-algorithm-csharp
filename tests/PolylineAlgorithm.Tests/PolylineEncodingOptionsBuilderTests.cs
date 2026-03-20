//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PolylineAlgorithm.Tests.Properties;
using System;

/// <summary>
/// Tests for <see cref="PolylineEncodingOptionsBuilder"/>.
/// </summary>
[TestClass]
public sealed class PolylineEncodingOptionsBuilderTests {
    /// <summary>
    /// Tests that Create returns a new builder instance.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Create_ReturnsNewBuilder() {
        // Act
        PolylineEncodingOptionsBuilder result = PolylineEncodingOptionsBuilder.Create();

        // Assert
        Assert.IsNotNull(result);
    }

    /// <summary>
    /// Tests that Create returns different instances on multiple calls.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Create_MultipleInvocations_ReturnsDifferentInstances() {
        // Act
        PolylineEncodingOptionsBuilder first = PolylineEncodingOptionsBuilder.Create();
        PolylineEncodingOptionsBuilder second = PolylineEncodingOptionsBuilder.Create();

        // Assert
        Assert.AreNotSame(first, second);
    }

    /// <summary>
    /// Tests that Build returns options with default values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Build_WithDefaults_ReturnsOptionsWithDefaultValues() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5u, result.Precision);
        Assert.AreEqual(512, result.StackAllocLimit);
        Assert.IsNotNull(result.LoggerFactory);
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);
    }

    /// <summary>
    /// Tests that Build returns options with configured precision.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Build_WithCustomPrecision_ReturnsOptionsWithCustomPrecision() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(7);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(7u, result.Precision);
    }

    /// <summary>
    /// Tests that Build returns options with configured stack alloc limit.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Build_WithCustomStackAllocLimit_ReturnsOptionsWithCustomStackAllocLimit() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithStackAllocLimit(1024);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(1024, result.StackAllocLimit);
    }

    /// <summary>
    /// Tests that Build returns options with configured logger factory.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Build_WithCustomLoggerFactory_ReturnsOptionsWithCustomLoggerFactory() {
        // Arrange
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(loggerFactory);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreSame(loggerFactory, result.LoggerFactory);

        // Cleanup
        loggerFactory.Dispose();
    }

    /// <summary>
    /// Tests that Build returns options with all custom values.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Build_WithAllCustomValues_ReturnsOptionsWithAllCustomValues() {
        // Arrange
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(10)
            .WithStackAllocLimit(2048)
            .WithLoggerFactory(loggerFactory);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(10u, result.Precision);
        Assert.AreEqual(2048, result.StackAllocLimit);
        Assert.AreSame(loggerFactory, result.LoggerFactory);

        // Cleanup
        loggerFactory.Dispose();
    }

    /// <summary>
    /// Tests that Build can be called multiple times on the same builder.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void Build_MultipleInvocations_ReturnsDifferentInstancesWithSameValues() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(6);

        // Act
        PolylineEncodingOptions first = builder.Build();
        PolylineEncodingOptions second = builder.Build();

        // Assert
        Assert.AreNotSame(first, second);
        Assert.AreEqual(first.Precision, second.Precision);
        Assert.AreEqual(first.StackAllocLimit, second.StackAllocLimit);
    }

    /// <summary>
    /// Tests that WithStackAllocLimit sets the value and returns the builder.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithStackAllocLimit_ValidValue_SetsValueAndReturnsSelf() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithStackAllocLimit(256);

        // Assert
        Assert.AreSame(builder, result);
        PolylineEncodingOptions options = builder.Build();
        Assert.AreEqual(256, options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that WithStackAllocLimit accepts minimum value of 1.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithStackAllocLimit_MinimumValue_SetsValue() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithStackAllocLimit(1);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(1, result.StackAllocLimit);
    }

    /// <summary>
    /// Tests that WithStackAllocLimit throws ArgumentOutOfRangeException for zero.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithStackAllocLimit_Zero_ThrowsArgumentOutOfRangeException() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act & Assert
        try {
            builder.WithStackAllocLimit(0);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        } catch (ArgumentOutOfRangeException ex) {
            Assert.AreEqual("stackAllocLimit", ex.ParamName);
        }
    }

    /// <summary>
    /// Tests that WithStackAllocLimit throws ArgumentOutOfRangeException for negative value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithStackAllocLimit_NegativeValue_ThrowsArgumentOutOfRangeException() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act & Assert
        try {
            builder.WithStackAllocLimit(-10);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        } catch (ArgumentOutOfRangeException ex) {
            Assert.AreEqual("stackAllocLimit", ex.ParamName);
        }
    }

    /// <summary>
    /// Tests that WithStackAllocLimit accepts large value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithStackAllocLimit_LargeValue_SetsValue() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithStackAllocLimit(100000);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(100000, result.StackAllocLimit);
    }

    /// <summary>
    /// Tests that WithStackAllocLimit can be called multiple times.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithStackAllocLimit_MultipleCalls_LastValueWins() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithStackAllocLimit(100)
               .WithStackAllocLimit(200)
               .WithStackAllocLimit(300);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(300, result.StackAllocLimit);
    }

    /// <summary>
    /// Tests that WithPrecision sets the value and returns the builder.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithPrecision_ValidValue_SetsValueAndReturnsSelf() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithPrecision(8);

        // Assert
        Assert.AreSame(builder, result);
        PolylineEncodingOptions options = builder.Build();
        Assert.AreEqual(8u, options.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision accepts zero value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithPrecision_Zero_SetsValue() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithPrecision(0);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(0u, result.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision accepts maximum uint value.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithPrecision_MaxValue_SetsValue() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithPrecision(uint.MaxValue);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(uint.MaxValue, result.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision can be called multiple times.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithPrecision_MultipleCalls_LastValueWins() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithPrecision(5)
               .WithPrecision(7)
               .WithPrecision(9);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(9u, result.Precision);
    }

    /// <summary>
    /// Tests that WithLoggerFactory sets the factory and returns the builder.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithLoggerFactory_ValidFactory_SetsValueAndReturnsSelf() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithLoggerFactory(loggerFactory);

        // Assert
        Assert.AreSame(builder, result);
        PolylineEncodingOptions options = builder.Build();
        Assert.AreSame(loggerFactory, options.LoggerFactory);

        // Cleanup
        loggerFactory.Dispose();
    }

    /// <summary>
    /// Tests that WithLoggerFactory with null uses NullLoggerFactory.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithLoggerFactory_Null_UsesNullLoggerFactory() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        builder.WithLoggerFactory(null!);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.IsNotNull(result.LoggerFactory);
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);
    }

    /// <summary>
    /// Tests that WithLoggerFactory can replace a previously set factory.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithLoggerFactory_ReplacePreviousFactory_UpdatesValue() {
        // Arrange
        ILoggerFactory firstFactory = LoggerFactory.Create(builder => { });
        ILoggerFactory secondFactory = LoggerFactory.Create(builder => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(firstFactory);

        // Act
        builder.WithLoggerFactory(secondFactory);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreSame(secondFactory, result.LoggerFactory);

        // Cleanup
        firstFactory.Dispose();
        secondFactory.Dispose();
    }

    /// <summary>
    /// Tests that WithLoggerFactory can be set to null after setting a factory.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void WithLoggerFactory_NullAfterFactory_UsesNullLoggerFactory() {
        // Arrange
        ILoggerFactory factory = LoggerFactory.Create(builder => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(factory);

        // Act
        builder.WithLoggerFactory(null!);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);

        // Cleanup
        factory.Dispose();
    }

    /// <summary>
    /// Tests that builder supports method chaining for all methods.
    /// </summary>
    [TestMethod]
    [TestCategory(Category.Unit)]
    public void MethodChaining_AllMethods_ReturnsBuilderForChaining() {
        // Arrange
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { });

        // Act
        PolylineEncodingOptions result = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(6)
            .WithStackAllocLimit(1024)
            .WithLoggerFactory(loggerFactory)
            .Build();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(6u, result.Precision);
        Assert.AreEqual(1024, result.StackAllocLimit);
        Assert.AreSame(loggerFactory, result.LoggerFactory);

        // Cleanup
        loggerFactory.Dispose();
    }
}
