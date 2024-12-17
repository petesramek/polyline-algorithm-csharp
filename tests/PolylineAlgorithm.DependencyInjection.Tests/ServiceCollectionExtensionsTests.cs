//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Tests {

    [TestClass]
    public class ServiceCollectionExtensionsTests {
        private static IServiceCollection Services { get; } = new ServiceCollection().AddPolylineEncoder();

        //[TestMethod]
        //public void Add_DefaultPolylineEncoder_Test() {
        //    // Arrange
        //    var provider = Services
        //        .BuildServiceProvider();

        //    // Act
        //    var encoder = provider
        //        .GetRequiredService<DefaultPolylineEncoding>();

        //    // Assert
        //    Assert.IsInstanceOfType<DefaultPolylineEncoding>(encoder);
        //}
    }
}