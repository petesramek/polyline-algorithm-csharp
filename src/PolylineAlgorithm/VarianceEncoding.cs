namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System.Runtime.CompilerServices;

public class VarianceEncoding {
    public static VarianceEncoding Default { get; } = new VarianceEncoding();

    private VarianceEncoding() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Encode(int variance, ref Span<byte> buffer) {
        int index = 0;
        int rem = variance << 1;

        if (variance < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            buffer[index++] = Convert.ToByte((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        buffer[index++] = Convert.ToByte(rem + Defaults.Algorithm.QuestionMark);

        return index;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Decode(ReadOnlyMemory<char> buffer, out int variance) {
        int position = 0;
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

        if (chunk >= Defaults.Algorithm.Space) {
            InvalidPolylineException.Throw(position);
        }

        variance = (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return position;
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
}
