//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using Newtonsoft.Json.Linq;
using PolylineAlgorithm.Tests.Internal;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class PolylineTest {
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        int expectedLength = 0;

        // Act
        Polyline polyline = new();

        // Assert
        Assert.IsTrue(polyline.IsEmpty);

        Assert.AreEqual(expectedLength, polyline.Length);
    }

    [TestMethod]
    public void Constructor_String_Empty_Ok() {
        // Arrange
        string value = Defaults.Polyline.Empty;
        int expectedLength = value.Length;

        // Act
        Polyline polyline = new(Defaults.Polyline.Empty);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(expectedLength, polyline.Length);
    }

    [TestMethod]
    public void Constructor_String_Valid_Ok() {
        // Arrange
        string value = Defaults.Polyline.Valid;
        int expectedLength = value.Length;

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(expectedLength, polyline.Length);
    }
}
