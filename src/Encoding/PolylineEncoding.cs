//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Encoding
{
    /// <summary>
    /// Defines default polyline encoding with generic <see cref="System.ValueTuple[double, double]"/>
    /// </summary>
    public class PolylineEncoding : PolylineEncodingBase<(double Latitude, double Longitude)>
    {
        /// <summary>
        /// Method creates <see cref="System.ValueTuple[double, double]"/> result from passed latitude and longitude arguments
        /// </summary>
        /// <param name="latitude">Latitude value</param>
        /// <param name="longitude">Longitude value</param>
        /// <returns>Returns created instance of <see cref="System.ValueTuple[double, double]"/></returns>
        protected override (double Latitude, double Longitude) CreateResult(double latitude, double longitude)
        {
            return (latitude, longitude);
        }

        /// <summary>
        /// Method creates <see cref="System.ValueTuple[double, double]"/>
        /// </summary>
        /// <param name="source">The <see cref="System.ValueTuple[double, double]"/></param>
        /// <returns>Returns created coordinate <see cref="System.ValueTuple[double, double]"/></returns>
        protected override (double Latitude, double Longitude) GetCoordinate((double Latitude, double Longitude) source)
        {
            return source;
        }
    }
}
