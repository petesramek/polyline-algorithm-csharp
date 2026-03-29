using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Abstraction;

namespace PolylineAlgorithm.Tests.Abstraction {
    [TestClass]
    public sealed class AbstractPolylineDecoderMoreTests {
        private sealed class FakeStringDecoder : AbstractPolylineDecoder<string, Coordinate> {
            protected override ReadOnlyMemory<char> GetReadOnlyMemory(in string polyline) => polyline.AsMemory();

            protected override Coordinate CreateCoordinate(double latitude, double longitude) => new(latitude, longitude);
        }

        [TestMethod]
        public void Decode_NullString_ThrowsArgumentNullException() {
            // Arrange
            var decoder = new FakeStringDecoder();

            // Act & Assert
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => decoder.Decode((string?)null!).ToList());
            Assert.AreEqual("polyline", ex.ParamName);
        }

        [TestMethod]
        public void Decode_EmptyString_ThrowsInvalidPolylineException() {
            // Arrange
            var decoder = new FakeStringDecoder();

            // Act & Assert - empty string is too short (min block length = 1)
            Assert.ThrowsExactly<InvalidPolylineException>(() => decoder.Decode(string.Empty).ToList());
        }

        [TestMethod]
        public void Decode_InvalidCharacter_ThrowsInvalidPolylineException() {
            // Arrange
            var decoder = new FakeStringDecoder();

            // '!' (33) is below allowed range ('?' == 63) and should trigger invalid polyline character
            Assert.ThrowsExactly<InvalidPolylineException>(() => decoder.Decode("!").ToList());
        }

        [TestMethod]
        public void Polyline_FromString_Null_ThrowsArgumentNullException() {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => Polyline.FromString(null!));
            Assert.AreEqual("polyline", ex.ParamName);
        }

        [TestMethod]
        public void Polyline_FromCharArray_Null_ThrowsArgumentNullException() {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => Polyline.FromCharArray(null!));
            Assert.AreEqual("polyline", ex.ParamName);
        }

        [TestMethod]
        public void Polyline_CopyTo_NullDestination_ThrowsArgumentNullException() {
            // Arrange
            Polyline p = Polyline.FromString("_p~iF~ps|U_ulLnnqC_mqNvxq`@");

            // Act & Assert
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => p.CopyTo(null!));
            Assert.AreEqual("destination", ex.ParamName);
        }

        [TestMethod]
        public void Polyline_CopyTo_ShortDestination_ThrowsArgumentException() {
            // Arrange
            Polyline p = Polyline.FromString("_p~iF~ps|U_ulLnnqC_mqNvxq`@");
            char[] dest = new char[Math.Max(0, p.Length - 1)];

            // Act & Assert
            var ex = Assert.ThrowsExactly<ArgumentException>(() => p.CopyTo(dest));
            Assert.AreEqual("destination", ex.ParamName);
        }
    }
}