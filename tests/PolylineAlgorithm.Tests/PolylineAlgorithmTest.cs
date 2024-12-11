//  
// Copyright (c) Petr �r�mek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests {

    /// <summary>
    /// Defines the <see cref="PolylineAlgorithmTest" />
    /// </summary>
    [TestClass]
    [TestCategory(nameof(PolylineEncoder))]
    public class PolylineAlgorithmTest {
        /// <summary>
        /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. Empty <see langword="char"/>[] is passed as parameter.
        /// Expected result is <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Decode_EmptyInput_ThrowsException() {
            // Arrange
            var emptyPolylineCharArray = Defaults.Polyline.Empty;

            // Act
            void DecodeEmptyPolylineCharArray() {
                PolylineEncoder.Decode(emptyPolylineCharArray).ToArray();
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => DecodeEmptyPolylineCharArray());
        }

        /// <summary>
        /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. <see langword="char"/>[] with invalid coordinates is passed as parameter.
        /// Expected result is <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Decode_InvalidInput_ThrowsException() {
            // Arrange
            var invalidPolylineCharrArray = Defaults.Polyline.Invalid;

            // Act
            void DecodeInvalidPolylineCharArray() {
                PolylineEncoder.Decode(invalidPolylineCharrArray).ToArray();
            }

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => DecodeInvalidPolylineCharArray());
        }

        /// <summary>
        /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. <see langword="null" /> is passed as parameter.
        /// Expected result is <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        public void Decode_NullInput_ThrowsException() {
            // Arrange
            var nullPolylineCharArray = (string)null!;

            // Act
            void DecodeNullPolylineCharArray() {
                PolylineEncoder.Decode(nullPolylineCharArray).ToArray();
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => DecodeNullPolylineCharArray());
        }

        /// <summary>
        /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. <see langword="char"/>[] with valid coordinates is passed as parameter.
        /// Expected result is <see cref="CollectionAssert.AreEquivalent(System.Collections.ICollection, System.Collections.ICollection)"/>.
        /// </summary>
        [TestMethod]
        public void Decode_ValidInput_AreEquivalent() {
            // Arrange
            var validPolylineCharArray = Defaults.Polyline.Valid;

            // Act
            var result = PolylineEncoder.Decode(validPolylineCharArray);

            // Assert
            CollectionAssert.AreEquivalent(Defaults.Coordinate.Valid.ToList(), result.ToList());
        }

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
                PolylineEncoder.Encode(emptyCoordinates);
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
                PolylineEncoder.Encode(invalidCoordinates);
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
                PolylineEncoder.Encode(nullCoordinates);
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => EncodeNullCoordinates());
        }

        /// <summary>
        /// The Encode_ValidInput
        /// </summary>
        [TestMethod]
        public void Encode_ValidInput_AreEqual() {
            // Arrange
            var validCoordinates = Defaults.Coordinate.Valid;

            // Act
            var result = PolylineEncoder.Encode(validCoordinates);

            // Assert
            Assert.AreEqual(Defaults.Polyline.Valid, result);
        }
    }
}
