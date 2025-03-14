//namespace PolylineAlgorithm.Internal;

//using System.Buffers;
//using System.Runtime.CompilerServices;

//internal static class DecodingAlgorithm {

//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static bool TryDecodeNext(ref int value, ref long position, ref readonly ReadOnlySequence<char> sequence) {
//        SequenceReader<char> reader = new(sequence);
//        reader.Advance(position);

//        if (reader.Remaining == 0) {
//            return false;
//        }

//        int chunk = 0;
//        int sum = 0;
//        int shifter = 0;

//        while (reader.TryRead(out char next)) {
//            chunk = next - Defaults.Algorithm.QuestionMark;
//            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
//            shifter += Defaults.Algorithm.ShiftLength;

//            if (chunk < Defaults.Algorithm.Space) {
//                break;
//            }
//        }

//        if (reader.Remaining == 0 && chunk >= Defaults.Algorithm.Space) {
//            InvalidPolylineException.Throw(reader.Length - reader.Remaining);
//        }

//        value += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;
//        position = reader.Length - reader.Remaining;

//        return true;
//    }
//}
