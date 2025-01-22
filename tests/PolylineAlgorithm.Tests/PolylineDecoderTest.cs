//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;

/// <summary>
/// Defines the <see cref="PolylineEncoderTest" />
/// </summary>
[TestClass]
[TestCategory(nameof(DefaultPolylineDecoder))]
public class PolylineDecoderTest {
    private static DefaultPolylineDecoder Decoder { get; } = new DefaultPolylineDecoder();

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. Empty <see langword="char"/>[] is passed as parameter.
    /// Expected result is <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    public void Decode_EmptyInput_ThrowsException() {
        // Arrange
        var emptyPolylineCharArray = new Polyline(Defaults.Polyline.Empty);

        // Act
        void DecodeEmptyPolylineCharArray() {
            Decoder.Decode(emptyPolylineCharArray).ToArray();
        }

        // Assert
        Assert.ThrowsException<ArgumentException>(DecodeEmptyPolylineCharArray);
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. <see langword="char"/>[] with valid coordinates is passed as parameter.
    /// Expected result is <see cref="CollectionAssert.AreEquivalent(System.Collections.ICollection, System.Collections.ICollection)"/>.
    /// </summary>
    [TestMethod]
    public void Decode_ValidInput_AreEquivalent() {
        // Arrange
        var validPolylineCharArray = new Polyline(Defaults.Polyline.Valid);

        // Act
        var result = Decoder.Decode(validPolylineCharArray);

        // Assert
        CollectionAssert.AreEquivalent(Defaults.Coordinates.Valid.ToArray(), result.ToArray());
    }
}
