namespace PolylineAlgorithm.Internal
{
    using System.Runtime.CompilerServices;


    /// <summary>
    /// Performs coordinate validation
    /// </summary>
    internal class CoordinateValidator
    {
        #region Methods

        /// <summary>
        /// Performs coordinate validation
        /// </summary>
        /// <param name="coordinate">Coordinate to validate</param>
        /// <returns>Returns validation result. If valid then true, otherwise false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid((double Latitude, double Longitude) coordinate)
        {
            return IsValidLatitude(coordinate.Latitude) && IsValidLongitude(coordinate.Longitude);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidLatitude(double latitude)
        {
            return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidLongitude(double longitude)
        {
            return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
        }

        #endregion
    }
}
