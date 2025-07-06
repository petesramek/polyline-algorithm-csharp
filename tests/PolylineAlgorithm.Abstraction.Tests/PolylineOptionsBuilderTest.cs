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
        var bufferSize = 64_000;
        var loggerFactory = NullLoggerFactory.Instance;

        // Act
        var options = builder
            .Build();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(bufferSize, options.BufferSize);
        Assert.AreEqual(bufferSize / sizeof(char), options.MaxLength);
        Assert.AreEqual(loggerFactory, options.LoggerFactory);
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
        Assert.AreEqual(expected, options.BufferSize);
        Assert.AreEqual(expected / sizeof(char), options.MaxLength);
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
}
