//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using Newtonsoft.Json.Linq;
using PolylineAlgorithm.Tests.Internal;
using System;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class CoordinateTest {
    public static IEnumerable<object[]> ValidParamaters => [
        [ 0d, 0d, true, true ],
        [ 90, 180, true, false ],
        [ -90, -180, true, false  ],
        [ 90, -180, true, false  ],
        [ -90, 180, true, false  ],
    ];

    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        double latitude = 0d;
        double longitude = 0d;

        // Act
        Coordinate result = new();

        // Assert
        Assert.IsTrue(result.IsValid);
        Assert.IsTrue(result.IsDefault);
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }

    [TestMethod]
    [DynamicData(nameof(ValidParamaters))]
    public void Constructor_Double_Double_Paramaters_Ok(double latitude, double longitude, bool valid, bool @default) {
        // Arrange
        // Act
        Coordinate result = new(latitude, longitude);

        // Assert
        Assert.AreEqual(valid, result.IsValid);
        Assert.AreEqual(@default, result.IsDefault);
        Assert.AreEqual(latitude, result.Latitude);
        Assert.AreEqual(longitude, result.Longitude);
    }
}
