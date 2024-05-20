//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Tests.Encoding
{
    using DropoutCoder.PolylineAlgorithm.Encoding;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="PolylineEncodingTest" />
    /// </summary>
    [TestClass]
    [TestCategory(nameof(PolylineEncoding))]
    public class PolylineEncodingTest : PolylineEncoding
    {
        /// <summary>
        /// The CreateResult_AreEqual
        /// </summary>
        [TestMethod]
        public void CreateResult_AreEqual()
        {
            // Arrange
            var validCoordinate = Defaults.Coordinate.Valid.First();

            // Act
            var result = this.CreateResult(validCoordinate.Latitude, validCoordinate.Longitude);

            // Assert
            Assert.AreEqual(validCoordinate, result);
        }

        /// <summary>
        /// The GetCoordinate_AreEqual
        /// </summary>
        [TestMethod]
        public void GetCoordinate_AreEqual()
        {
            // Arrange
            var validCoordinate = Defaults.Coordinate.Valid.First();

            // Act
            var result = this.GetCoordinate(validCoordinate);

            // Assert
            Assert.AreEqual(validCoordinate, result);
        }
    }
}
