//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Tests {

    [TestClass]
    public class ServiceCollectionExtensionsTests {
        private static IServiceCollection Services { get; } = new ServiceCollection().AddPolylineAlgorithm();

        [TestMethod]
        public void Add_PolylineEncoder_Test() {
            // Arrange
            var provider = Services
                .BuildServiceProvider();

            // Act
            var encoder = provider
                .GetRequiredService<IPolylineEncoder>();

            // Assert
            Assert.IsInstanceOfType<IPolylineEncoder>(encoder);
        }

        [TestMethod]
        public void Add_PolylineDecoder_Test() {
            // Arrange
            var provider = Services
                .BuildServiceProvider();

            // Act
            var decoder = provider
                .GetRequiredService<IPolylineDecoder>();

            // Assert
            Assert.IsInstanceOfType<IPolylineDecoder>(decoder);
        }
    }
}