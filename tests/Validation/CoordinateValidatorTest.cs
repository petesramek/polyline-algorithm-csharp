//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Tests.Validation
{
    using DropoutCoder.PolylineAlgorithm.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="CoordinateValidatorTest" />
    /// </summary>
    [TestClass]
    [TestCategory(nameof(CoordinateValidator))]
    public class CoordinateValidatorTest
    {
        /// <summary>
        /// The IsValid_InvalidInput
        /// </summary>
        [TestMethod]
        public void IsValid_InvalidInput_IsFalse()
        {
            // Act
            var invalidCoordinateCollection = Defaults.Coordinate.Invalid;

            foreach (var item in invalidCoordinateCollection)
            {
                // Arrange
                var result = CoordinateValidator.IsValid(item);

                // Assert
                Assert.IsFalse(result);
            }
        }

        /// <summary>
        /// The IsValid_ValidInput
        /// </summary>
        [TestMethod]
        public void IsValid_ValidInput_IsTrue()
        {
            // Act
            var validCoordinateCollection = Defaults.Coordinate.Valid;

            foreach (var item in validCoordinateCollection)
            {
                // Arrange
                var result = CoordinateValidator.IsValid(item);

                // Assert
                Assert.IsTrue(result);
            }
        }

        /// <summary>
        /// The IsValidLatitude_InvalidInput
        /// </summary>
        [TestMethod]
        public void IsValidLatitude_InvalidInput_IsFalse()
        {
            // Act
            var invalidCoordinateCollection = Defaults.Coordinate.Invalid;

            foreach (var item in invalidCoordinateCollection)
            {
                // Act
                var result = CoordinateValidator.IsValidLatitude(item.Latitude);

                // Arrange
                Assert.IsFalse(result);
            }
        }

        /// <summary>
        /// The IsValidLatitude_ValidInput
        /// </summary>
        [TestMethod]
        public void IsValidLatitude_ValidInput_IsTrue()
        {
            // Arrange
            var validCoordinateCollection = Defaults.Coordinate.Valid;

            foreach ((double Latitude, double Longitude) in validCoordinateCollection)
            {
                // Act
                var result = CoordinateValidator.IsValidLatitude(Latitude);

                // Assert
                Assert.IsTrue(result);
            }
        }

        /// <summary>
        /// The IsValidLongitude_InvalidInput
        /// </summary>
        [TestMethod]
        public void IsValidLongitude_InvalidInput_IsFalse()
        {
            // Arrange
            var invalidCoordinateCollection = Defaults.Coordinate.Invalid;

            foreach (var item in invalidCoordinateCollection)
            {
                // Act
                var result = CoordinateValidator.IsValidLongitude(item.Longitude);

                // Assert
                Assert.IsFalse(result);
            }
        }

        /// <summary>
        /// The IsValidLongitude_ValidInput
        /// </summary>
        [TestMethod]
        public void IsValidLongitude_ValidInput_IsTrue()
        {
            // Arrange
            var validCoordinateCollection = Defaults.Coordinate.Valid;

            foreach (var item in validCoordinateCollection)
            {
                // Act
                var result = CoordinateValidator.IsValidLongitude(item.Longitude);

                // Assert
                Assert.IsTrue(result);
            }
        }
    }
}
