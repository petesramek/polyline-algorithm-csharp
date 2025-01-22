//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Microsoft;

using global::Microsoft.Extensions.DependencyInjection;
using PolylineAlgorithm.Abstraction;

public static class ServiceCollectionExtensions {
    /// <summary>
    /// Adds a singleton service of the type specified in <see cref="IPolylineEncoder{TCoordinate}"/> and <see cref="IPolylineDecoder{TCoordinate}"/>
    /// with an implementation type <see cref="DefaultPolylineEncoder"/> and <see cref="DefaultPolylineDecoder"/>
    /// to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>A reference to <paramref name="services"/> instance after the operation has completed..</returns>
    /// <remarks>
    /// TCoordinate is <see cref="Coordinate" />.
    /// </remarks>
    public static IServiceCollection AddDefaultPolylineAlgorithm(this IServiceCollection services) {
        return services
            .AddSingleton<IPolylineEncoder<Coordinate>, DefaultPolylineEncoder>()
            .AddSingleton<IPolylineDecoder<Coordinate>, DefaultPolylineDecoder>();

    }
}
