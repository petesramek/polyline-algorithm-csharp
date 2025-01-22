//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Microsoft.Tests;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.DependencyInjection.Microsoft;

[TestClass]
public class ServiceCollectionExtensionsTests {
    private static IServiceCollection Services { get; } = new ServiceCollection().AddDefaultPolylineAlgorithm();

    [TestMethod]
    public void Resolve_PolylineEncoder_Test() {
        // Arrange
        var provider = Services
            .BuildServiceProvider();

        // Act
        var encoder = provider
            .GetRequiredService<IPolylineEncoder<Coordinate>>();

        // Assert
        Assert.IsInstanceOfType<IPolylineEncoder<Coordinate>>(encoder);
    }

    [TestMethod]
    public void Resolve_PolylineDecoder_Test() {
        // Arrange
        var provider = Services
            .BuildServiceProvider();

        // Act
        var decoder = provider
            .GetRequiredService<IPolylineEncoder<Coordinate>>();

        // Assert
        Assert.IsInstanceOfType<IPolylineEncoder<Coordinate>>(decoder);
    }
}