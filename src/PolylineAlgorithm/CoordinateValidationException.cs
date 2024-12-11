namespace PolylineAlgorithm
{
    using System;

    public class CoordinateValidationException : Exception
    {
        public CoordinateValidationException(double latitude, double longitude)
         : base(string.Format(ExceptionMessageResource.CoordinateValidationExceptionCoordinateIsOutOfRangeErrorMessageFormat, latitude, longitude)) { }

        public double Latitude { get; }

        public double Longitude { get; }
    }
}
