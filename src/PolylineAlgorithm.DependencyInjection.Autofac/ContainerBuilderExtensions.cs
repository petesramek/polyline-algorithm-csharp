//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Autofac;

using global::Autofac;

public static class ContainerBuilderExtensions {
    /// <summary>
    /// Registers singleton instances of <seealso cref="ICoordinateValidator" />
    /// , <seealso cref="IPolylineEncoder" />
    /// and <seealso cref="IPolylineDecoder" />
    /// into <seealso cref="ContainerBuilder" />.
    /// </summary>
    /// <param name="builder">Instance of <seealso cref="ContainerBuilder"/> to register polyline services into.</param>
    public static void RegisterPolylineAlgorithm(this ContainerBuilder builder) {
        builder.RegisterType<CoordinateValidator>()
            .As<ICoordinateValidator>()
            .SingleInstance();

        builder.RegisterType<PolylineEncoder>()
            .As<IPolylineEncoder>()
            .SingleInstance();

        builder.RegisterType<PolylineDecoder>()
            .As<IPolylineDecoder>()
            .SingleInstance();
    }
}
