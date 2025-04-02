namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Runtime.InteropServices;
using System.Text;

[StructLayout(LayoutKind.Auto)]
public ref struct PolylineReader {
    private readonly ReadOnlyMemory<byte> _value;
    private double _latitude;
    private double _longitude;

    public PolylineReader(string value) {
        _value = Encoding.UTF8.GetBytes(value);
    }

    public PolylineReader(byte[] value) {
        _value = value.AsMemory();
    }

    public int Position { get; private set; } = 0;

    public readonly double Latitude => _latitude / Defaults.Algorithm.Precision;

    public readonly double Longitude => _longitude / Defaults.Algorithm.Precision;

    public bool Read() {
        if (Position == _value.Length) {
            return false;
        }

        var latitudeEnd = Position + _value.Span[Position..].IndexOfAny(Defaults.Polyline.Delimiters) + 1;
        var longitudeEnd = latitudeEnd + _value.Span[latitudeEnd..].IndexOfAny(Defaults.Polyline.Delimiters) + 1;

        var latitudeSpan = _value.Span[Position..latitudeEnd];
        var longitudeSpan = _value.Span[latitudeEnd..longitudeEnd];

        if (TryDecode(latitudeSpan, out int latitudeVariance) && TryDecode(longitudeSpan, out int longitudeVariance)) {
            _latitude += latitudeVariance;
            _longitude += longitudeVariance;

            Position = longitudeEnd;

            return true;
        }

        return false;

        static bool TryDecode(in ReadOnlySpan<byte> source, out int variance) {
            int position = 0;
            int chunk = 0;
            int sum = 0;
            int shifter = 0;


            while (position < source.Length) {
                chunk = source[position++] - Defaults.Algorithm.QuestionMark;
                sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
                shifter += Defaults.Algorithm.ShiftLength;

                if (chunk < Defaults.Algorithm.Space) {
                    break;
                }
            }

            if (position == source.Length && chunk >= Defaults.Algorithm.Space) {
                variance = 0;
                return false;
            }

            variance = (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;
            return true;
        }
    }
}
