//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests {
    /// <summary>
    /// Defines the <see cref="PolylineEncoderTest" />
    /// </summary>
    [TestClass]
    [TestCategory(nameof(PolylineEncoder))]
    public class PolylineEncoderTest {
        private static PolylineEncoder Encoder { get; } = new PolylineEncoder(new CoordinateValidator());

        /// <summary>
        /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. Empty is passed as parameter.
        /// Expected result is <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Encode_EmptyInput_ThrowsException() {
            // Arrange
            var emptyCoordinates = Defaults.Coordinate.Empty;

            // Act
            void EncodeEmptyCoordinates() {
                Encoder.Encode(emptyCoordinates);
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => EncodeEmptyCoordinates());
        }

        /// <summary>
        /// The Encode_InvalidInput
        /// </summary>
        [TestMethod]
        public void Encode_InvalidInput_ThrowsException() {
            // Arrange
            var invalidCoordinates = Defaults.Coordinate.Invalid;

            // Act
            void EncodeInvalidCoordinates() {
                Encoder.Encode(invalidCoordinates);
            }

            // Assert
            Assert.ThrowsException<AggregateException>(() => EncodeInvalidCoordinates());
        }

        /// <summary>
        /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{(double Latitude, double Longitude)})" /> method. <see langword="null" /> is passed as parameter.
        /// Expected result is <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Encode_NullInput_ThrowsException() {
            // Arrange
            var nullCoordinates = (IEnumerable<(double, double)>)null!;

            // Act
            void EncodeNullCoordinates() {
                Encoder.Encode(nullCoordinates);
            }

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => EncodeNullCoordinates());
        }

        /// <summary>
        /// The Encode_ValidInput
        /// </summary>
        [TestMethod]
        public void Encode_ValidInput_AreEqual() {
            // Arrange
            var validCoordinates = Defaults.Coordinate.Valid;

            // Act
            var result = Encoder.Encode(validCoordinates);

            // Assert
            Assert.AreEqual(Defaults.Polyline.Valid, result);
        }
    }
}
