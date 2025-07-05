namespace PolylineAlgorithm.Abstraction.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm.Utility;
using System;

[TestClass]
public class PolylineDecoderTest {
    private static readonly PolylineDecoder _decoder = new PolylineDecoder();

    public static IEnumerable<object[]> CoordinateCount => [[1], [10], [100], [1_000]];

    public static IEnumerable<(double, double)> NotANumberAndInfinityCoordinates => StaticValueProvider.Invalid.GetNotANumberAndInfinityCoordinates() ;

    public static IEnumerable<(double, double)> MinAndMaxCoordinates => StaticValueProvider.Invalid.GetMinAndMaxCoordinates();


    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange && Act
        var decoder = new PolylineDecoder();

        // Assert
        Assert.IsNotNull(decoder);
        Assert.IsNotNull(decoder.Options);
    }

    [TestMethod]
    public void Constructor_ValidOptions_Ok() {
        // Arrange
        var options = new PolylineEncodingOptions();

        // Act
        var decoder = new PolylineDecoder(options);

        // Assert
        Assert.IsNotNull(decoder);
        Assert.AreSame(options, decoder.Options);
    }

    [TestMethod]
    public void Encode_NullPolyline_Throws_ArgumentException() {
        // Arrange
        void Encode() => _decoder.Decode(null!);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_EmptyPolyline_Throws_ArgumentException() {
        // Arrange
        void Encode() => _decoder.Decode(string.Empty);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_WhitespacePolyline_Throws_ArgumentException() {
        // Arrange
        void Encode() => _decoder.Decode(string.Empty);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentException>(Encode);

        // Assert
        Assert.AreEqual("coordinates", exception.ParamName);
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    [TestMethod]
    public void Encode_BufferTooSmall_Throws_InternalBufferOverflowException() {
        // Arrange
        PolylineDecoder decoder = new PolylineDecoder(new PolylineEncodingOptions { BufferSize = 12});
         string polyline = RandomValueProvider.GetPolyline(2);

        // Act
        var exception = Assert.ThrowsExactly<InternalBufferOverflowException>(() => decoder.Decode(polyline));

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    //[TestMethod]
    //[DynamicData(nameof(NotANumberAndInfinityCoordinates))]
    //public void Encode_NotANumberAndInfinityCoordinate_Throws_ArgumentOutOfRangeException((double, double) coordinate) {
    //    // Arrange
        
    //    // Act
    //    var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _decoder.Encode([coordinate]));

    //    // Assert
    //    Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    //}

    //[TestMethod]
    //[DynamicData(nameof(MinAndMaxCoordinates))]
    //public void Encode_MinAndMaxCoordinate_Throws_ArgumentOutOfRangeException((double, double) coordinate) {
    //    // Arrange

    //    // Act
    //    var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _decoder.Encode([coordinate]));

    //    // Assert
    //    Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    //}

    //[TestMethod]
    //[DynamicData(nameof(CoordinateCount))]
    //public void Encode_RandomValue_ValidInput_Ok(int count) {
    //    // Arrange
    //    IEnumerable<(double Latitude, double Longitude)> coordinates = RandomValueProvider.GetCoordinates(count);
    //    string expected = RandomValueProvider.GetPolyline(count);

    //    // Act
    //    var result = _decoder.Encode(coordinates);

    //    // Assert
    //    Assert.AreEqual(expected.Length, result.Length);
    //    Assert.IsTrue(expected.Equals(result));
    //}

    //[TestMethod]
    //public void Encode_StaticValue_ValidInput_Ok() {
    //    // Arrange
    //    IEnumerable<(double Latitude, double Longitude)> coordinates = StaticValueProvider.Valid.GetCoordinates();
    //    string expected = StaticValueProvider.Valid.GetPolyline();

    //    // Act
    //    var result = _decoder.Encode(coordinates);

    //    // Assert
    //    Assert.AreEqual(expected.Length, result.Length);
    //    Assert.IsTrue(expected.Equals(result));
    //}

    public class PolylineDecoder : PolylineDecoder<string, (double Latitude, double Longitude)> {
        public PolylineDecoder()
            : base() { }

        public PolylineDecoder(PolylineEncodingOptions options)
            : base(options) { }

        protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) {
            return (latitude, longitude);
        }

        protected override ReadOnlyMemory<char> GetReadOnlyMemory(string? polyline) {
            if(string.IsNullOrWhiteSpace(polyline)) {
                throw new ArgumentException();
            }

            return polyline.AsMemory();
        }
    }
}
