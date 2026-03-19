//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

/// <summary>
/// Tests for the <see cref="PolylineEncodingOptionsBuilder"/> type.
/// </summary>
[TestClass]
public class PolylineEncodingOptionsBuilderTests {
    #region Create Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.Create"/> returns a builder instance.
    /// </summary>
    [TestMethod]
    public void Create_ReturnsBuilderInstance() {
        // Act
        PolylineEncodingOptionsBuilder result = PolylineEncodingOptionsBuilder.Create();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<PolylineEncodingOptionsBuilder>(result);
    }

    #endregion

    #region Build Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.Build"/> returns options with default values.
    /// </summary>
    [TestMethod]
    public void Build_DefaultValues_ReturnsOptionsWithDefaults() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5u, result.Precision);
        Assert.AreEqual(512, result.StackAllocLimit);
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.Build"/> returns options with custom precision.
    /// </summary>
    [TestMethod]
    public void Build_WithCustomPrecision_ReturnsOptionsWithCustomPrecision() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(6);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(6u, result.Precision);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.Build"/> returns options with custom stack alloc limit.
    /// </summary>
    [TestMethod]
    public void Build_WithCustomStackAllocLimit_ReturnsOptionsWithCustomLimit() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithStackAllocLimit(1024);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(1024, result.StackAllocLimit);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.Build"/> returns options with custom logger factory.
    /// </summary>
    [TestMethod]
    public void Build_WithCustomLoggerFactory_ReturnsOptionsWithCustomFactory() {
        // Arrange
        Mock<ILoggerFactory> mockLoggerFactory = new Mock<ILoggerFactory>();
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(mockLoggerFactory.Object);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreSame(mockLoggerFactory.Object, result.LoggerFactory);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.Build"/> returns options with all custom values.
    /// </summary>
    [TestMethod]
    public void Build_WithAllCustomValues_ReturnsOptionsWithAllCustomValues() {
        // Arrange
        Mock<ILoggerFactory> mockLoggerFactory = new Mock<ILoggerFactory>();
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(7)
            .WithStackAllocLimit(2048)
            .WithLoggerFactory(mockLoggerFactory.Object);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(7u, result.Precision);
        Assert.AreEqual(2048, result.StackAllocLimit);
        Assert.AreSame(mockLoggerFactory.Object, result.LoggerFactory);
    }

    #endregion

    #region WithStackAllocLimit Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithStackAllocLimit"/> sets the stack alloc limit.
    /// </summary>
    [TestMethod]
    public void WithStackAllocLimit_ValidValue_SetsStackAllocLimit() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithStackAllocLimit(256);

        // Assert
        Assert.AreSame(builder, result);
        PolylineEncodingOptions options = result.Build();
        Assert.AreEqual(256, options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithStackAllocLimit"/> accepts minimum value of 1.
    /// </summary>
    [TestMethod]
    public void WithStackAllocLimit_MinimumValue_SetsStackAllocLimit() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithStackAllocLimit(1);

        // Assert
        PolylineEncodingOptions options = result.Build();
        Assert.AreEqual(1, options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithStackAllocLimit"/> accepts large values.
    /// </summary>
    [TestMethod]
    public void WithStackAllocLimit_LargeValue_SetsStackAllocLimit() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithStackAllocLimit(int.MaxValue);

        // Assert
        PolylineEncodingOptions options = result.Build();
        Assert.AreEqual(int.MaxValue, options.StackAllocLimit);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithStackAllocLimit"/> throws when value is zero.
    /// </summary>
    [TestMethod]
    public void WithStackAllocLimit_ZeroValue_ThrowsArgumentOutOfRangeException() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => builder.WithStackAllocLimit(0));
        Assert.AreEqual("stackAllocLimit", exception.ParamName);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithStackAllocLimit"/> throws when value is negative.
    /// </summary>
    [TestMethod]
    public void WithStackAllocLimit_NegativeValue_ThrowsArgumentOutOfRangeException() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act & Assert
        ArgumentOutOfRangeException exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => builder.WithStackAllocLimit(-1));
        Assert.AreEqual("stackAllocLimit", exception.ParamName);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithStackAllocLimit"/> returns the same builder for chaining.
    /// </summary>
    [TestMethod]
    public void WithStackAllocLimit_ValidValue_ReturnsSameBuilder() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithStackAllocLimit(100);

        // Assert
        Assert.AreSame(builder, result);
    }

    #endregion

    #region WithPrecision Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithPrecision"/> sets the precision.
    /// </summary>
    [TestMethod]
    public void WithPrecision_ValidValue_SetsPrecision() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithPrecision(6);

        // Assert
        Assert.AreSame(builder, result);
        PolylineEncodingOptions options = result.Build();
        Assert.AreEqual(6u, options.Precision);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithPrecision"/> accepts zero.
    /// </summary>
    [TestMethod]
    public void WithPrecision_ZeroValue_SetsPrecision() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithPrecision(0);

        // Assert
        PolylineEncodingOptions options = result.Build();
        Assert.AreEqual(0u, options.Precision);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithPrecision"/> accepts maximum uint value.
    /// </summary>
    [TestMethod]
    public void WithPrecision_MaxValue_SetsPrecision() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithPrecision(uint.MaxValue);

        // Assert
        PolylineEncodingOptions options = result.Build();
        Assert.AreEqual(uint.MaxValue, options.Precision);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithPrecision"/> returns the same builder for chaining.
    /// </summary>
    [TestMethod]
    public void WithPrecision_ValidValue_ReturnsSameBuilder() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithPrecision(10);

        // Assert
        Assert.AreSame(builder, result);
    }

    #endregion

    #region WithLoggerFactory Tests

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithLoggerFactory"/> sets the logger factory.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_ValidFactory_SetsLoggerFactory() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();
        Mock<ILoggerFactory> mockLoggerFactory = new Mock<ILoggerFactory>();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithLoggerFactory(mockLoggerFactory.Object);

        // Assert
        Assert.AreSame(builder, result);
        PolylineEncodingOptions options = result.Build();
        Assert.AreSame(mockLoggerFactory.Object, options.LoggerFactory);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithLoggerFactory"/> uses NullLoggerFactory when null is passed.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_NullFactory_UsesNullLoggerFactory() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithLoggerFactory(null!);

        // Assert
        PolylineEncodingOptions options = result.Build();
        Assert.IsInstanceOfType<NullLoggerFactory>(options.LoggerFactory);
        Assert.AreSame(NullLoggerFactory.Instance, options.LoggerFactory);
    }

    /// <summary>
    /// Tests that <see cref="PolylineEncodingOptionsBuilder.WithLoggerFactory"/> returns the same builder for chaining.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_ValidFactory_ReturnsSameBuilder() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();
        Mock<ILoggerFactory> mockLoggerFactory = new Mock<ILoggerFactory>();

        // Act
        PolylineEncodingOptionsBuilder result = builder.WithLoggerFactory(mockLoggerFactory.Object);

        // Assert
        Assert.AreSame(builder, result);
    }

    #endregion
}
