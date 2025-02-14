namespace PolylineAlgorithm.Internal;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Auto)]
internal ref struct PolylineWriter {
    private WriterState _state = new();
    private Memory<char> _buffer;

    public PolylineWriter(ref readonly Memory<char> buffer) {
        _buffer = buffer;
    }

    public readonly bool CanWrite => _state.Position < _buffer.Length;

    public readonly int Position => _state.Position;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ref readonly Coordinate coordinate) {
        InvalidCoordinateException.ThrowIfNotValid(coordinate);
        InvalidWriterStateException.ThrowIfCannotWrite(CanWrite, Position, _buffer.Length);

        Imprecise(coordinate.Latitude, out int latitude);
        Imprecise(coordinate.Longitude, out int longitude);

        UpdateCurrent(in latitude, in longitude, out int latDiff, out int longDiff);

        WriteNext(in latDiff);
        WriteNext(in longDiff);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Imprecise(double value, out int result) {
            result = Convert.ToInt32(value * Defaults.Algorithm.Precision);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void WriteNext(ref readonly int value) {
        int rem = value << 1;

        if (value < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            WriteChar(Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark));
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        WriteChar(Convert.ToChar(rem + Defaults.Algorithm.QuestionMark));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void WriteChar(char value) {
        InvalidWriterStateException.ThrowIfCannotWrite(CanWrite, Position, _buffer.Length);

        _buffer.Span[Position] = value;
        _state.Position += 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void UpdateCurrent(ref readonly int latitude, ref readonly int longitude, out int latDiff, out int longDiff) {
        latDiff = latitude - _state.Latitude;
        _state.Latitude = latitude;

        longDiff = longitude - _state.Longitude;
        _state.Longitude = longitude;
    }

    public readonly Polyline ToPolyline() {
        ReadOnlyMemory<char> buffer = _buffer[.._state.Position];
        var polyline = Polyline.FromMemory(in buffer);
        return polyline;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    private ref struct WriterState {
        public WriterState() {
            Position = Latitude = Longitude = 0;
        }

        public int Position { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }
    }
}
