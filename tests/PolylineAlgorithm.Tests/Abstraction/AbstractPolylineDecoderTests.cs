using PolylineAlgorithm.Abstraction;

namespace PolylineAlgorithm.Tests.Abstraction {
    [TestClass]
    public sealed class AbstractPolylineDecoderTests {
        private sealed class TestStringDecoder : AbstractPolylineDecoder<string, (double Latitude, double Longitude)> {
            protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();
            protected override (double Latitude, double Longitude) CreateCoordinate(double latitude, double longitude) => (latitude, longitude);
        }

        [TestMethod]
        public void Decode_Null_Polyline_Throws_ArgumentNullException() {
            // Arrange
            var decoder = new TestStringDecoder();

            // Act & Assert
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => decoder.Decode((string?)null!).ToList());
            Assert.AreEqual("polyline", ex.ParamName);
        }

        [TestMethod]
        public void Decode_Empty_Polyline_Throws_InvalidPolylineException() {
            // Arrange
            var decoder = new TestStringDecoder();

            // Act & Assert - empty string is too short (min block length = 1)
            Assert.ThrowsExactly<InvalidPolylineException>(() => decoder.Decode(string.Empty).ToList());
        }

        [TestMethod]
        public void Decode_Invalid_Character_Polyline_Throws_InvalidPolylineException() {
            // Arrange
            var decoder = new TestStringDecoder();

            // '!' (33) is below allowed range ('?' == 63) and should trigger invalid polyline character
            Assert.ThrowsExactly<InvalidPolylineException>(() => decoder.Decode("!").ToList());
        }
    }
}