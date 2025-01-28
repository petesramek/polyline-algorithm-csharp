namespace PolylineAlgorithm.Internal;

using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Auto)]
internal ref struct PolylineWriter(ref readonly Span<char> buffer) {
    private WriterState _state = new();
    private Span<char> _buffer = buffer;

    public void Write(ref readonly Coordinate coordinate) {
        var latitude = Round(coordinate.Latitude);
        var longitude = Round(coordinate.Longitude);

        var latDiff = latitude - _state.ExchangeLatitude(ref latitude);
        var longDiff = longitude - _state.ExchangeLongitude(ref longitude);
        
        EncodeNext(ref latDiff, ref _buffer, ref _state);
        EncodeNext(ref longDiff, ref _buffer, ref _state);

        static int Round(double value) {
            return Convert.ToInt32(Math.Round(value * Constants.Precision));
        }
    }

    static void EncodeNext(ref int value, ref Span<char> buffer, ref WriterState state) {
        int shifted = value << 1;

        if (value < 0) {
            shifted = ~shifted;
        }

        int rem = shifted;

        while (rem >= Constants.ASCII.Space) {
            buffer[state.Position] = Convert.ToChar((Constants.ASCII.Space | rem & Constants.ASCII.UnitSeparator) + Constants.ASCII.QuestionMark);
            state.Advance();
            rem >>= Constants.ShiftLength;
        }

        buffer[state.Position] = Convert.ToChar(rem + Constants.ASCII.QuestionMark);
        state.Advance();
    }

    public override readonly string ToString() {
        return _buffer[.._state.Position].ToString();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    private ref struct WriterState {
        public WriterState() {
            Position = Latitude = Longitude = 0;
        }

        internal int Position { get; private set; }

        internal int Latitude { get; private set; }

        internal int Longitude { get; private set; }

        internal void Advance() {
            Position += 1;
        }

        internal int ExchangeLatitude(ref int latitude) {
            var current = Latitude;
            Latitude = latitude;
            return current;
        }

        internal int ExchangeLongitude(ref int longitude) {
            var current = Longitude;
            Longitude = longitude;
            return current;
        }
    }
}
