//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Encoding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// /// <summary>
    /// Defines base class for all polyline encodings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PolylineEncodingBase<T> : IPolylineEncoding<T>
    {
        /// <summary>
        /// Method performs decode operation and coversion to desired type
        /// </summary>
        /// <param name="source">The <see cref="string"/></param>
        /// <returns>The <see cref="IEnumerable{T}"/></returns>
        public IEnumerable<T> Decode(string polyline)
        {
            if (string.IsNullOrWhiteSpace(polyline))
            {
                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullOrEmpty, nameof(polyline));
            }

            return PolylineAlgorithm.Decode(polyline)
                .Select(c => CreateResult(c.Latitude, c.Longitude));
        }

        /// <summary>
        /// Method performs conversion to coordinate tuple and encode operation.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string Encode(IEnumerable<T> source)
        {
            if (source == null || !source.Any())
            {
                throw new ArgumentException(ExceptionMessageResource.ArgumentCannotBeNullOrEmpty, nameof(source));
            }

            var coordinates = source.Select(s => GetCoordinate(s));

            return PolylineAlgorithm.Encode(coordinates);
        }

        /// <summary>
        /// Method creates <see cref="T"/> result from passed latitude and longitude arguments
        /// </summary>
        /// <param name="latitude">Latitude value</param>
        /// <param name="longitude">Longitude value</param>
        /// <returns>Returns created instance of <see cref="T"/></returns>
        protected abstract T CreateResult(double latitude, double longitude);

        /// <summary>
        /// The GetCoordinates
        /// </summary>
        /// <param name="source">The <see cref="T"/></param>
        /// <returns>The <see cref="System.ValueTuple[double, double]"/></returns>
        protected abstract (double Latitude, double Longitude) GetCoordinate(T source);
    }
}
