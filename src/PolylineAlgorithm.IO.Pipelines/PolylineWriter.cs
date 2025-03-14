namespace PolylineAlgorithm.IO.Pipelines
{
    using PolylineAlgorithm.IO.Pipelines.Internal;
    using System.Buffers;

    public ref struct PolylineWriter
    {
        public void Write(ref Span<char> buffer, ref readonly int difference) {
            int rem = difference << 1;
            int index = 0;

            if (difference < 0) {
                rem = ~rem;
            }

            while (rem >= Defaults.Algorithm.Space) {
                buffer[index++] = Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
                rem >>= Defaults.Algorithm.ShiftLength;
            }

            buffer[index++] = Convert.ToChar(rem + Defaults.Algorithm.QuestionMark);
        }
    }
}
