namespace PolylineAlgorithm.IO.Pipelines
{
    using PolylineAlgorithm.IO.Pipelines.Internal;
    using System.Buffers;

    public ref struct PolylineReader
    {
        public (int Latitude, int Longitude) Current { get; private set; }

        public (int Latitude, int Longitude) Read(ref readonly ReadOnlySequence<char> sequence, ref long position) {
            SequenceReader<char> reader = new(sequence);
            reader.Advance(position);

            int latitude = Current.Latitude;
            int longitude = Current.Longitude;

            if (!TryDecodeNext(in reader, ref latitude) || !TryDecodeNext(in reader, ref longitude)) {
                // throw exception
            }

            return Current = (latitude, longitude);
        }

        internal static bool TryDecodeNext(ref readonly SequenceReader<char> reader, ref int value) {
            if (reader.Remaining == 0) {
                return false;
            }

            int chunk = 0;
            int sum = 0;
            int shifter = 0;

            while (reader.TryRead(out char next)) {
                chunk = next - Defaults.Algorithm.QuestionMark;
                sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
                shifter += Defaults.Algorithm.ShiftLength;

                if (chunk < Defaults.Algorithm.Space) {
                    break;
                }
            }

            if (reader.Remaining == 0 && chunk >= Defaults.Algorithm.Space) {
                return false;
                //InvalidPolylineException.Throw(reader.Length - reader.Remaining);
            }

            value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

            return true;
        }
    }
}
