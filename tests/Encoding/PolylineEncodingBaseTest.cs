//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Tests.Encoding
{
    using DropoutCoder.PolylineAlgorithm.Encoding;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="PolylineEncodingBaseTest" />
    /// </summary>
    [TestClass]
    [TestCategory(nameof(PolylineEncodingBase<(double latitude, double longitude)>))]
    public class PolylineEncodingBaseTest : PolylineEncodingBase<(double latitude, double longitude)>
    {
        /// <summary>
        /// The Decode_NullInput
        /// </summary>
        [TestMethod]
        public void Decode_NullInput_ThrowsException()
        {
            // Arrange
            var nullPolylineString = (string)null;

            // Act
            void DecodeNullPolylineString()
            {
                this.Decode(nullPolylineString).ToArray();
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => DecodeNullPolylineString());
        }

        /// <summary>
        /// The Decode_EmptyInput
        /// </summary>
        [TestMethod]
        public void Decode_EmptyInput_ThrowsException()
        {
            // Arrange
            var emptyPolylineString = Defaults.Polyline.Empty;

            // Act
            void DecodeEmptyPolylineString()
            {
                this.Decode(emptyPolylineString).ToArray();
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => DecodeEmptyPolylineString());
        }

        /// <summary>
        /// The Decode_InvalidInput
        /// </summary>
        [TestMethod]
        public void Decode_InvalidInput_ThrowsException()
        {
            // Arrange
            var invalidPolylineString = Defaults.Polyline.Invalid;

            // Act
            void DecodeInvalidPolylineString()
            {
                this.Decode(Defaults.Polyline.Invalid).ToArray();
            }

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => DecodeInvalidPolylineString());
        }

        /// <summary>
        /// The Decode_ValidInput
        /// </summary>
        [TestMethod]
        public void Decode_ValidInput_AreEquivalent()
        {
            // Arrange
            var validPolylineString = Defaults.Polyline.Valid;

            // Act
            var result = this.Decode(validPolylineString).ToArray();

            // Assert
            CollectionAssert.AreEquivalent(Defaults.Coordinate.Valid.ToList(), result.ToList());
        }

        /// <summary>
        /// The Encode_NullInput
        /// </summary>
        [TestMethod]
        public void Encode_NullInput_ThrowsException()
        {
            // Arrange
            var nullCoordinates = (IEnumerable<(double, double)>)null;

            // Act
            void EncodeNullCoordinateCollection()
            {
                this.Encode(nullCoordinates);
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => EncodeNullCoordinateCollection());
        }

        /// <summary>
        /// The Encode_EmptyInput
        /// </summary>
        [TestMethod]
        public void Encode_EmptyInput_ThrowsException()
        {
            // Arrange
            var emptyCoordinates = Defaults.Coordinate.Empty;

            // Act
            void EncodeEmptyCoordinateCollection()
            {
                this.Encode(emptyCoordinates);
            }

            // Assert
            Assert.ThrowsException<ArgumentException>(() => EncodeEmptyCoordinateCollection());
        }

        /// <summary>
        /// The Encode_InvalidInput
        /// </summary>
        [TestMethod]
        public void Encode_InvalidInput_ThrowsException()
        {
            // Arrange
            var invalidCoordinates = Defaults.Coordinate.Invalid;

            // Act
            void EncodeInvalidCoordinateCollection()
            {
                this.Encode(invalidCoordinates);
            }

            // Assert
            Assert.ThrowsException<AggregateException>(() => EncodeInvalidCoordinateCollection());
        }

        /// <summary>
        /// The Encode_ValidInput
        /// </summary>
        [TestMethod]
        public void Encode_ValidInput_AreEqual()
        {
            // Arrange
            var validCoordinateCollection = Defaults.Coordinate.Valid;

            // Act
            var result = this.Encode(validCoordinateCollection);

            // Assert
            Assert.AreEqual(Defaults.Polyline.Valid, result);
        }

        #region Overriden methods

        /// <summary>
        /// The CreateResult
        /// </summary>
        /// <param name="latitude">The <see cref="double"/></param>
        /// <param name="longitude">The <see cref="double"/></param>
        /// <returns>The <see cref="(double latitude, double longitude)"/></returns>
        protected override (double latitude, double longitude) CreateResult(double latitude, double longitude)
        {
            return (latitude, longitude);
        }

        /// <summary>
        /// The GetCoordinate
        /// </summary>
        /// <param name="source">The <see cref="(double latitude, double longitude)"/></param>
        /// <returns>The <see cref="(double Latitude, double Longitude)"/></returns>
        protected override (double Latitude, double Longitude) GetCoordinate((double latitude, double longitude) source)
        {
            return source;
        }

        #endregion
    }
}
