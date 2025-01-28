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
[TestCategory(nameof(PolylineDecoder))]
public class PolylineDecoderTest {
    public PolylineDecoder Decoder = new PolylineDecoder();

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. Empty <see langword="char"/>[] is passed as parameter.
    /// Expected result is <see cref="ArgumentException"/>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Decode_InvalidInput_ThrowsException() {
        // Arrange
        ReadOnlySpan<char> invalidPolylineCharArray = Defaults.Polyline.Invalid;

        // Act
        var result = Decoder.Decode(in invalidPolylineCharArray);
    }

    /// <summary>
    /// Method is testing <see cref="PolylineEncoder.Decode(char[])" /> method. <see langword="char"/>[] with valid coordinates is passed as parameter.
    /// Expected result is <see cref="CollectionAssert.AreEquivalent(System.Collections.ICollection, System.Collections.ICollection)"/>.
    /// </summary>
    [TestMethod]
    public void Decode_ValidInput_AreEquivalent() {
        // Arrange
        ReadOnlySpan<char> validPolylineCharArray = Defaults.Polyline.Valid;

        // Act
        var result = Decoder.Decode(in validPolylineCharArray);

        // Assert
        CollectionAssert.AreEqual(Defaults.Coordinates.Valid.ToArray(), result.ToArray());
    }
}
