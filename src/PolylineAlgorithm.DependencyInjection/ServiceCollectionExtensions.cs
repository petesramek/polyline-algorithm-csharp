﻿//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection {
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Registers singleton instance of <seealso cref="DefaultPolylineEncoding" /> to dependency container.
        /// </summary>
        /// <param name="services">Instance of <seealso cref="IServiceCollection"/></param>
        /// <returns>nstance of <seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPolylineAlgorithm(this IServiceCollection services) {
            return services
                .AddSingleton<ICoordinateValidator, CoordinateValidator>()
                .AddSingleton<IPolylineEncoder, PolylineEncoder>()
                .AddSingleton<IPolylineDecoder, PolylineDecoder>();
        }
    }
}
