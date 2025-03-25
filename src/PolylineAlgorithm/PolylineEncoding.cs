namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Runtime.CompilerServices;

public class PolylineEncoding {
    public static readonly PolylineEncoding Default = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadValue(ref int variance, ref ReadOnlyMemory<char> buffer, ref int position) {
        if (position == buffer.Length) {
            return false;
        }

        int chunk = 0;
        int sum = 0;
        int shifter = 0;

        while (position < buffer.Length) {
            chunk = buffer.Span[position++] - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;

            if (chunk < Defaults.Algorithm.Space) {
                break;
            }
        }

        variance += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return chunk < Defaults.Algorithm.Space;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Denormalize(int value) => value / Defaults.Algorithm.Precision;

    public bool TryWriteValue(in int variance, ref Span<char> buffer, ref int position) {
        if(buffer.Length > position + GetRequiredCharCount(variance)) {
            return false;
        }

        int rem = variance << 1;

        if (variance < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            buffer[position++] = (char)((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        buffer[position++] = (char)(rem + Defaults.Algorithm.QuestionMark);

        return true;
    }

    private int Normalize(double value) => (int)(value * Defaults.Algorithm.Precision);

    public int GetRequiredCharCount(in int variation) => variation switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };
}
