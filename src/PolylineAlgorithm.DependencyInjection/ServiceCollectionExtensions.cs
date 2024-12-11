﻿//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.DependencyInjection {
    using Microsoft.Extensions.DependencyInjection;
    using PolylineAlgorithm.Encoding;

    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Registers singleton instance of <seealso cref="DefaultPolylineEncoding" /> to dependency container.
        /// </summary>
        /// <param name="services">Instance of <seealso cref="IServiceCollection"/></param>
        /// <returns>nstance of <seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPolylineEncoder(this IServiceCollection services) {
            return services
                .AddSingleton<DefaultPolylineEncoding, DefaultPolylineEncoding>();
        }

        /// <summary>
        /// Registers singleton instance of <seealso cref="TImplementation" /> to dependency container.
        /// </summary>
        /// <param name="services">Instance of <seealso cref="IServiceCollection"/></param>
        /// <returns>nstance of <seealso cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPolylineEncoder<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService {
            return services
                .AddSingleton<TService, TImplementation>();
        }
    }
}
