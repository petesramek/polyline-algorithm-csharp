using PolylineAlgorithm.Internal;

namespace PolylineAlgorithm {
    /// <summary>
    /// Performs coordinate validation
    /// </summary>
    public sealed class CoordinateValidator : ICoordinateValidator {
        /// <summary>
        /// Performs coordinate validation
        /// </summary>
        /// <param name="coordinate">Coordinate to validate</param>
        /// <returns>Returns validation result. If valid then true, otherwise false.</returns>
        public bool IsValid((double Latitude, double Longitude) coordinate) {
            return IsValidLatitude(ref coordinate.Latitude) && IsValidLongitude(ref coordinate.Longitude);
        }

        private static bool IsValidLatitude(ref readonly double latitude) {
            return latitude >= Constants.Coordinate.MinLatitude && latitude <= Constants.Coordinate.MaxLatitude;
        }

        private static bool IsValidLongitude(ref readonly double longitude) {
            return longitude >= Constants.Coordinate.MinLongitude && longitude <= Constants.Coordinate.MaxLongitude;
        }
    }
}
