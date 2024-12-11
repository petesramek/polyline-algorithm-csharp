//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests {
    using PolylineAlgorithm.Internal;

    /// <summary>
    /// Defines the <see cref="CoordinateValidatorTest" />
    /// </summary>
    [TestClass]
    [TestCategory(nameof(CoordinateValidator))]
    public class CoordinateValidatorTestCoordinate {
        private static CoordinateValidator Validator { get; } = new CoordinateValidator();

        #region Methods

        /// <summary>
        /// The IsValid_InvalidInput
        /// </summary>
        [TestMethod]
        public void IsValid_InvalidInput_IsFalse() {
            // Act
            var invalidCoordinateCollection = Defaults.Coordinate.Invalid;

            foreach (var item in invalidCoordinateCollection) {
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
        public void IsValid_ValidInput_IsTrue() {
            // Act
            var validCoordinateCollection = Defaults.Coordinate.Valid;

            foreach (var item in validCoordinateCollection) {
                // Arrange
                var result = CoordinateValidator.IsValid(item);

                // Assert
                Assert.IsTrue(result);
            }
        }

        #endregion
    }
}
