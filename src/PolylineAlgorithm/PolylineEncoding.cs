namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;

internal class PolylineEncoding {
    public int GetChars(in int variance, ref Span<char> buffer) {
        int index = 0;
        int rem = variance << 1;

        if (variance < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            buffer[index++] = (char)((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        buffer[index++] = (char)(rem + Defaults.Algorithm.QuestionMark);

        return index;
    }

    public Coordinate GetCoordinate(ReadOnlyMemory<byte> buffer, in Coordinate previous = default) {
        var latitudeEnd = buffer.Span.IndexOfAny(Defaults.Polyline.Delimiters) + 1;
        var longitudeEnd = buffer.Span.LastIndexOfAny(Defaults.Polyline.Delimiters) + 1;

        var latitudeVariance = Decode(buffer[..latitudeEnd].Span);
        var longitudeVariance = Decode(buffer[latitudeEnd..longitudeEnd].Span);

        var latitude = Denormalize(Normalize(previous.Latitude) + latitudeVariance);
        var longitude = Denormalize(Normalize(previous.Longitude) + longitudeVariance);

        return new Coordinate(latitude, longitude);

        static int Decode(in ReadOnlySpan<byte> buffer) {
            int position = 0;
            int chunk = 0;
            int sum = 0;
            int shifter = 0;

            while (position < buffer.Length) {
                chunk = buffer[position++] - Defaults.Algorithm.QuestionMark;
                sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
                shifter += Defaults.Algorithm.ShiftLength;

                if (chunk < Defaults.Algorithm.Space) {
                    break;
                }
            }

            if (buffer.Length == position && chunk >= Defaults.Algorithm.Space) {
                InvalidPolylineException.Throw(position);
            }

            return (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;
        }
    }

    public int GetCharCount(in int variation) => variation switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };

    private double Denormalize(int value) => value / Defaults.Algorithm.Precision;

    private int Normalize(double value) => (int)(value * Defaults.Algorithm.Precision);
}
