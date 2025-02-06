namespace PolylineAlgorithm.Internal;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Auto)]
internal ref struct PolylineReader {
    private ReaderState _state = new();
    private ReadOnlySpan<char> _polyline;

    public PolylineReader(ref readonly Polyline polyline) {
        _polyline = polyline.Span;
    }

    public readonly bool CanRead => _state.Position < _polyline.Length;

    public Coordinate Read() {
        int latitude = _state.Latitude;
        int longitude = _state.Longitude;

        DecodeNext(ref latitude, ref _polyline, ref _state);
        DecodeNext(ref longitude, ref _polyline, ref _state);

        _state.SetLatitude(in latitude);
        _state.SetLongitude(in longitude);

        Coordinate coordinate = new(Precise(ref latitude), Precise(ref longitude));

        if (!coordinate.IsValid) {
            throw new InvalidCoordinateException(coordinate);
        }

        return coordinate;

        static double Precise(ref int value) {
            return Convert.ToDouble(value) / Constants.Precision;
        }
    }

    static void DecodeNext(ref int value, ref ReadOnlySpan<char> polyline, ref ReaderState state) {
        // Initialize local variables
        int chunk;
        int sum = 0;
        int shifter = 0;

        do {
            chunk = polyline[state.Position] - Constants.ASCII.QuestionMark;
            sum |= (chunk & Constants.ASCII.UnitSeparator) << shifter;
            shifter += Constants.ShiftLength;
            state.Advance();
        } while (chunk >= Constants.ASCII.Space && state.Position < polyline.Length);

        if (state.Position >= polyline.Length && chunk >= Constants.ASCII.Space) {
            throw new InvalidOperationException(ExceptionMessageResource.PolylineStringIsMalformed);
        }

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    private ref struct ReaderState {
        public ReaderState() {
            Position = Latitude = Longitude = 0;
        }

        internal int Position { get; private set; }

        internal int Latitude { get; private set; }

        internal int Longitude { get; private set; }

        internal void Advance() {
            Position += 1;
        }

        internal void SetLatitude(ref readonly int latitude) {
            Latitude = latitude;
        }

        internal void SetLongitude(ref readonly int longitude) {
            Longitude = longitude;
        }
    }
}