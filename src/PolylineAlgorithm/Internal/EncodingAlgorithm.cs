//namespace PolylineAlgorithm.Internal;

//using System;

//internal class EncodingAlgorithm
//{
//    internal static void EncodeNext(ref readonly CoordinateDifference difference, ref int position, ref char[] buffer) {
//        int length = GetRequiredCharCount(difference.Latitude) + GetRequiredCharCount(difference.Longitude);

//        if (position + length > buffer.Length) {
//            throw new Exception($"Position: {position}, Length: {length}, Buffer Length: {buffer.Length}");
//        }

//        Encode(difference.Latitude, ref position, ref buffer);
//        Encode(difference.Longitude, ref position, ref buffer);
//    }

//    private static void Encode(int difference, ref int index, ref char[] buffer) {
//        int rem = difference << 1;

//        if (difference < 0) {
//            rem = ~rem;
//        }

//        while (rem >= Defaults.Algorithm.Space) {
//            buffer[index++] = Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
//            rem >>= Defaults.Algorithm.ShiftLength;
//        }

//        buffer[index++] = Convert.ToChar(rem + Defaults.Algorithm.QuestionMark);
//    }

//    public static int GetRequiredCharCount(int difference) => difference switch {
//        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
//        >= -16 and <= +15 => 1,
//        >= -512 and <= +511 => 2,
//        >= -16384 and <= +16383 => 3,
//        >= -524288 and <= +524287 => 4,
//        >= -16777216 and <= +16777215 => 5,
//        _ => 6,
//    };
//}
