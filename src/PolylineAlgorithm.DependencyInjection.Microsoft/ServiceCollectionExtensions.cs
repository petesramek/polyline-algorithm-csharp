//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection.Microsoft;

using global::Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {
    /// <summary>
    /// Registers singleton instances of <seealso cref="ICoordinateValidator" />
    /// , <seealso cref="IPolylineEncoder" />
    /// and <seealso cref="IPolylineDecoder" />
    /// to <seealso cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">Instance of <seealso cref="IServiceCollection"/></param>
    /// <returns>Instance of <seealso cref="IServiceCollection"/></returns>
    public static IServiceCollection AddPolylineAlgorithm(this IServiceCollection services) {
        return services
            .AddSingleton<ICoordinateValidator, CoordinateValidator>()
            .AddSingleton<IPolylineEncoder, PolylineEncoder>()
            .AddSingleton<IPolylineDecoder, PolylineDecoder>();
    }
}
