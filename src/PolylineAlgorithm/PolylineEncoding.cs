namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;

public class PolylineEncoding
{
    public static PolylineEncoding Default { get; } = new PolylineEncoding();
    private PolylineEncoding() { }

    public int GetChars(int difference, ref Span<char> buffer) {
        int index = 0;
        int rem = difference << 1;

        if (difference < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            buffer[index++] = Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        buffer[index++] = Convert.ToChar(rem + Defaults.Algorithm.QuestionMark);

        return index;
    }

    public int GetNextValue(ReadOnlySpan<char> source, ref int value) {
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

        if (source.Length == position && chunk >= Defaults.Algorithm.Space) {
            InvalidPolylineException.Throw(position);
        }

        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return position;
    }

    public int GetCharCount(int difference) => difference switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };
}
