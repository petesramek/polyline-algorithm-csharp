//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PolylineAlgorithm;
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
    public void Create_Returns_New_Builder() {
        // Act
        PolylineEncodingOptionsBuilder result = PolylineEncodingOptionsBuilder.Create();

        // Assert
        Assert.IsNotNull(result);
    }

    /// <summary>
    /// Tests that Create returns different instances on multiple calls.
    /// </summary>
    [TestMethod]
    public void Create_With_Multiple_Invocations_Returns_Different_Instances() {
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
    public void Build_With_Defaults_Returns_Options_With_Default_Values() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5u, result.Precision);
        Assert.IsNotNull(result.LoggerFactory);
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);
    }

    /// <summary>
    /// Tests that Build returns options with configured precision.
    /// </summary>
    [TestMethod]
    public void Build_With_Custom_Precision_Returns_Options_With_Custom_Precision() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(7);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(7u, result.Precision);
    }

    /// <summary>
    /// Tests that Build returns options with configured logger factory.
    /// </summary>
    [TestMethod]
    public void Build_With_Custom_Logger_Factory_Returns_Options_With_Custom_Logger_Factory() {
        // Arrange
        ILoggerFactory loggerFactory = LoggerFactory.Create(_ => { });
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
    public void Build_With_All_Custom_Values_Returns_Options_With_All_Custom_Values() {
        // Arrange
        ILoggerFactory loggerFactory = LoggerFactory.Create(_ => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(10)
            .WithLoggerFactory(loggerFactory);

        // Act
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.AreEqual(10u, result.Precision);
        Assert.AreSame(loggerFactory, result.LoggerFactory);

        // Cleanup
        loggerFactory.Dispose();
    }

    /// <summary>
    /// Tests that Build can be called multiple times on the same builder.
    /// </summary>
    [TestMethod]
    public void Build_With_Multiple_Invocations_Returns_Different_Instances_With_Same_Values() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(6);

        // Act
        PolylineEncodingOptions first = builder.Build();
        PolylineEncodingOptions second = builder.Build();

        // Assert
        Assert.AreNotSame(first, second);
        Assert.AreEqual(first.Precision, second.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision sets the value and returns the builder.
    /// </summary>
    [TestMethod]
    public void WithPrecision_With_Valid_Value_Sets_Value_And_Returns_Self() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptionsBuilder result = builder
            .WithPrecision(8);
        PolylineEncodingOptions options = builder.Build();

        // Assert
        Assert.AreSame(builder, result);
        Assert.AreEqual(8u, options.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision accepts zero value.
    /// </summary>
    [TestMethod]
    public void WithPrecision_With_Zero_Sets_Value() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder
            .WithPrecision(0)
            .Build();

        // Assert
        Assert.AreEqual(0u, result.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision accepts maximum uint value.
    /// </summary>
    [TestMethod]
    public void WithPrecision_With_Max_Value_Sets_Value() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder
            .WithPrecision(uint.MaxValue)
            .Build();

        // Assert
        Assert.AreEqual(uint.MaxValue, result.Precision);
    }

    /// <summary>
    /// Tests that WithPrecision can be called multiple times.
    /// </summary>
    [TestMethod]
    public void WithPrecision_With_Multiple_Calls_Last_Value_Wins() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder
            .WithPrecision(5)
            .WithPrecision(7)
            .WithPrecision(9)
            .Build();

        // Assert
        Assert.AreEqual(9u, result.Precision);
    }

    /// <summary>
    /// Tests that WithLoggerFactory sets the factory and returns the builder.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_With_Valid_Factory_Sets_Value_And_Returns_Self() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();
        ILoggerFactory loggerFactory = LoggerFactory.Create(_ => { });

        // Act
        PolylineEncodingOptionsBuilder result = builder
            .WithLoggerFactory(loggerFactory);
        PolylineEncodingOptions options = builder.Build();

        // Assert
        Assert.AreSame(builder, result);
        Assert.AreSame(loggerFactory, options.LoggerFactory);

        // Cleanup
        loggerFactory.Dispose();
    }

    /// <summary>
    /// Tests that WithLoggerFactory with null uses NullLoggerFactory.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_With_Null_Uses_Null_LoggerFactory() {
        // Arrange
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create();

        // Act
        PolylineEncodingOptions result = builder
            .WithLoggerFactory(null!)
            .Build();

        // Assert
        Assert.IsNotNull(result.LoggerFactory);
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);
    }

    /// <summary>
    /// Tests that WithLoggerFactory can replace a previously set factory.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_With_Replace_Previous_Factory_Updates_Value() {
        // Arrange
        using ILoggerFactory firstFactory = LoggerFactory.Create(_ => { });
        using ILoggerFactory secondFactory = LoggerFactory.Create(_ => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(firstFactory);

        // Act
        PolylineEncodingOptions result = builder
            .WithLoggerFactory(secondFactory)
            .Build();

        // Assert
        Assert.AreSame(secondFactory, result.LoggerFactory);
    }

    /// <summary>
    /// Tests that WithLoggerFactory can be set to null after setting a factory.
    /// </summary>
    [TestMethod]
    public void WithLoggerFactory_With_Null_After_Factory_Uses_Null_LoggerFactory() {
        // Arrange
        using ILoggerFactory factory = LoggerFactory.Create(_ => { });
        PolylineEncodingOptionsBuilder builder = PolylineEncodingOptionsBuilder.Create()
            .WithLoggerFactory(factory);

        // Act
        builder.WithLoggerFactory(null!);
        PolylineEncodingOptions result = builder.Build();

        // Assert
        Assert.IsInstanceOfType<NullLoggerFactory>(result.LoggerFactory);
    }

    /// <summary>
    /// Tests that builder supports method chaining for all methods.
    /// </summary>
    [TestMethod]
    public void MethodChaining_With_All_Methods_Returns_Builder_For_Chaining() {
        // Arrange
        using ILoggerFactory loggerFactory = LoggerFactory.Create(_ => { });

        // Act
        PolylineEncodingOptions result = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(6)
            .WithLoggerFactory(loggerFactory)
            .Build();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(6u, result.Precision);
        Assert.AreSame(loggerFactory, result.LoggerFactory);
    }
}