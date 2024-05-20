//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace DropoutCoder.PolylineAlgorithm.Validation
{
    using System;

    /// <summary>
    /// The exception that is thrown when one of the coordinates is not valid.
    /// </summary>
    public class CoordinateValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateValidationException"/> class with an error message and invalid coordinate values.
        /// </summary>
        /// <param name="latitude">The latitude value of invalid coodinate</param>
        /// <param name="longitude">The longitude value of invalid coodinate</param>
        public CoordinateValidationException(double latitude, double longitude)
         : base(string.Format(ExceptionMessageResource.CoordinateValidationExceptionCoordinateIsOutOfRangeErrorMessageFormat, latitude, longitude)) { }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}