//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;
/// <summary>
/// Defines the <see cref="PolylineEncoderTest" />
/// </summary>
[TestClass]
[TestCategory(nameof(PolylineEncoder))]
public class PolylineEncoderTest {
    public PolylineEncoder Encoder = new PolylineEncoder();
    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. Empty is passed as parameter.
    /// Expected result is <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_EmptyInput_ThrowsException() {
        // Arrange
        var emptyCoordinates = Defaults.Coordinates.Empty;

        // Act
        void EncodeEmptyCoordinates() {
            Encoder.Encode(emptyCoordinates);
        }

        // Assert
        Assert.ThrowsException<ArgumentException>(() => EncodeEmptyCoordinates());
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Encode(IEnumerable{(double Latitude, double Longitude)})" /> method. <see langword="null" /> is passed as parameter.
    /// Expected result is <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Encode_NullInput_ThrowsException() {
        // Arrange
        var nullCoordinates = (IEnumerable<Coordinate>)null!;

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
        var validCoordinates = Defaults.Coordinates.Valid;

        // Act
        var result = Encoder.Encode(validCoordinates);

        // Assert
        Assert.AreEqual(Defaults.Polyline.Valid, result.ToString());
    }
}
