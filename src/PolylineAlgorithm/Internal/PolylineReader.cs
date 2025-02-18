namespace PolylineAlgorithm.Internal;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Auto)]
internal ref struct PolylineReader {
    private ReaderState _state = new();
    private readonly ReadOnlySpan<char> _polyline;

    public PolylineReader() {
        _polyline = [];
    }

    public PolylineReader(ref readonly Polyline polyline) {
        _polyline = polyline.Span;
    }

    public readonly bool CanRead => _state.Position < _polyline.Length;

    public readonly int Position => _state.Position;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Coordinate Read() {
        InvalidReaderStateException.ThrowIfCannotRead(CanRead, Position, _polyline.Length);

        GetCurrent(out int currentLatitude, out int currentLongitude);

        int latitude = ReadNext(in currentLatitude);
        int longitude = ReadNext(in currentLongitude);

        SetCurrent(in latitude, in longitude);

        return new(Precise(ref latitude), Precise(ref longitude));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double Precise(ref int value) {
            return value / Defaults.Algorithm.Precision;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Advance() {
        _state.Position += 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void SetCurrent(ref readonly int latitude, ref readonly int longitude) {
        _state.Latitude = latitude;
        _state.Longitude = longitude;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly void GetCurrent(out int latitude, out int longitude) {
        latitude = _state.Latitude;
        longitude = _state.Longitude;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int ReadNext(ref readonly int value) {
        int chunk;
        int sum = 0;
        int shifter = 0;

        do {
            InvalidReaderStateException.ThrowIfCannotRead(CanRead, Position, _polyline.Length);
            chunk = _polyline[Position] - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;
            Advance();
        } while (chunk >= Defaults.Algorithm.Space && CanRead);

        if (!CanRead && chunk >= Defaults.Algorithm.Space) {
            InvalidPolylineException.Throw(Position);
        }

        return value + ((sum & 1) == 1 ? ~(sum >> 1) : sum >> 1);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    private ref struct ReaderState {
        public ReaderState() {
            Position = Latitude = Longitude = 0;
        }


        public int Position { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }
    }
}