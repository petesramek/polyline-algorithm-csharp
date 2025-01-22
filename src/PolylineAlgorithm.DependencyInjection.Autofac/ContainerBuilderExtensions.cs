//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Autofac;

using global::Autofac;
using PolylineAlgorithm.Abstraction;

public static class ContainerBuilderExtensions {
    /// <summary>
    /// Registers singleton instances of
    /// <see cref="IPolylineDecoder{TCoordinate}" />
    /// and <see cref="IPolylineEncoder{TCoordinate}" />
    /// into <see cref="ContainerBuilder" />.
    /// </summary>
    /// <param name="builder">Instance of <seealso cref="ContainerBuilder"/> to register polyline services into.</param>
    /// <remarks>
    /// TCoordinate is <see cref="Coordinate" />.
    /// </remarks>
    public static void RegisterDefaultPolylineAlgorithm(this ContainerBuilder builder) {
        builder.RegisterType<DefaultPolylineEncoder>()
            .As<IPolylineEncoder<Coordinate>>()
            .SingleInstance();

        builder.RegisterType<DefaultPolylineDecoder>()
            .As<IPolylineDecoder<Coordinate>>()
            .SingleInstance();
    }
}
