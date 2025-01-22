//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Autofac.Tests;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.DependencyInjection.Autofac;

[TestClass]
public class ContainerBuilderExtensionsTests {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private static IContainer Container { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [ClassInitialize]
    public static void ClassInitialize(TestContext context) {
        var builder = new ContainerBuilder();

        builder
            .RegisterDefaultPolylineAlgorithm();

        Container = builder
            .Build();
    }

    [TestMethod]
    public void Resolve_PolylineEncoder_Test() {
        // Arrange
        var container = Container;

        // Act
        var encoder = container
            .Resolve<IPolylineEncoder<Coordinate>>();

        // Assert
        Assert.IsInstanceOfType<IPolylineEncoder<Coordinate>>(encoder);
    }

    [TestMethod]
    public void Resolve_PolylineDecoder_Test() {
        // Arrange
        var container = Container;

        // Act
        var decoder = container
            .Resolve<IPolylineDecoder<Coordinate>>();

        // Assert
        Assert.IsInstanceOfType<IPolylineDecoder<Coordinate>>(decoder);
    }
}