namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;

public sealed class PolylineDecoder(ICoordinateValidator validator) : IPolylineDecoder {
    public ICoordinateValidator Validator { get; } = validator ?? throw new ArgumentNullException(nameof(validator));

    /// <summary>
    /// Method decodes polyline encoded representation to coordinates.
    /// </summary>
    /// <param name="source">Encoded polyline string to decode</param>
    /// <returns>Returns coordinates.</returns>
    /// <exception cref="ArgumentException">If polyline argument is null -or- empty char array.</exception>
    /// <exception cref="InvalidOperationException">If polyline representation is not in correct format.</exception>
    public IEnumerable<(double Latitude, double Longitude)> Decode(string polyline) {
        // Checking null and at least one character
        if (polyline == null || polyline.Length == 0) {
            throw new ArgumentException(string.Empty, nameof(polyline));
        }

        // Initialize local variables
        int index = 0;
        int latitude = 0;
        int longitude = 0;

        // Looping through encoded polyline char array
        while (index < polyline.Length) {
            // Attempting to calculate next latitude value. If failed exception is thrown
            if (!TryCalculateNext(polyline, ref index, ref latitude)) {
                throw new InvalidOperationException(string.Empty);
            }

            // Attempting to calculate next longitude value. If failed exception is thrown
            if (!TryCalculateNext(polyline, ref index, ref longitude)) {
                throw new InvalidOperationException(string.Empty);
            }

            var coordinate = (GetCoordinate(latitude), GetCoordinate(longitude));

            // Validating decoded coordinate. If not valid exception is thrown
            if (!Validator.IsValid(coordinate)) {
                throw new InvalidOperationException(string.Empty);
            }

            yield return coordinate;
        }
    }

    private static bool TryCalculateNext(string polyline, ref int index, ref int value) {
        // Initialize local variables
        int chunk;
        int sum = 0;
        int shifter = 0;

        do {
            chunk = polyline[index++] - Constants.ASCII.QuestionMark;
            sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
            shifter += Constants.ShiftLength;
        } while (chunk >= Constants.ASCII.Space && index < polyline.Length);

        if (index >= polyline.Length && chunk >= Constants.ASCII.Space)
            return false;

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return true;
    }

    private static double GetCoordinate(int value) {
        return Convert.ToDouble(value) / Constants.Precision;
    }
}