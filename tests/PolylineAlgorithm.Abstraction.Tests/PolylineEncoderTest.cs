namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm.Utility;
using System;

[TestClass]
public class PolylineEncoderTest {
    private static readonly PolylineEncoder _encoder = new PolylineEncoder();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange && Act
        var encoder = new PolylineEncoder();

        // Assert
        Assert.IsNotNull(encoder);
        Assert.IsNotNull(encoder.Options);
    }

    [TestMethod]
    public void Constructor_ValidOptions_Ok() {
        // Arrange
        var options = new PolylineEncodingOptions();

        // Act
        var encoder = new PolylineEncoder(options);

        // Assert
        Assert.IsNotNull(encoder);
        Assert.AreSame(options, encoder.Options);
    }

    [TestMethod]
    public void Encode_NullCoordinates_Throws_ArgumentException() {
        // Arrange
        void Encode() => _encoder.Encode(null!);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_EmptyCoordinates_Throws_ArgumentException() {
        // Arrange
        void Encode() => _encoder.Encode(Array.Empty<(double Latitude, double Longitude)>());

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_BufferTooSmall_Throws_InternalBufferOverflowException() {
        // Arrange
        PolylineEncoder _encoder = new PolylineEncoder(new PolylineEncodingOptions { BufferSize = 12});
        IEnumerable<(double Latitude, double Longitude)> coordinates = RandomValueProvider.GetCoordinates(2);

        // Act
        var exception = Assert.ThrowsExactly<InternalBufferOverflowException>(() => _encoder.Encode(coordinates));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    [DynamicData(nameof(CoordinateCount))]
    public void Encode_RandomValue_ValidInput_Ok(int count) {
        // Arrange
        IEnumerable<(double Latitude, double Longitude)> coordinates = RandomValueProvider.GetCoordinates(count);
        string expected = RandomValueProvider.GetPolyline(count);

        // Act
        var result = _encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    [TestMethod]
    public void Encode_StaticValue_ValidInput_Ok() {
        // Arrange
        IEnumerable<(double Latitude, double Longitude)> coordinates = StaticValueProvider.Valid.GetCoordinates();
        string expected = StaticValueProvider.Valid.GetPolyline();

        // Act
        var result = _encoder.Encode(coordinates);

        // Assert
        Assert.AreEqual(expected.Length, result.Length);
        Assert.IsTrue(expected.Equals(result));
    }

    public class PolylineEncoder : PolylineEncoder<(double Latitude, double Longitude), string> {
        public PolylineEncoder()
            : base() { }

        public PolylineEncoder(PolylineEncodingOptions options)
            : base(options) { }

        protected override string CreatePolyline(ReadOnlyMemory<char> polyline) => polyline.ToString();
        protected override double GetLatitude((double Latitude, double Longitude) coordinate) => coordinate.Latitude;
        protected override double GetLongitude((double Latitude, double Longitude) coordinate) => coordinate.Longitude;
    }
}
