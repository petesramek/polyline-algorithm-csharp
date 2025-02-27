namespace PolylineAlgorithm.Internal;

using System;
using System.Runtime.CompilerServices;

internal static class EncodingAlgorithm
{
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlyMemory<char> EncodeNext(int latitudeDiff, int longitudeDiff) {
        int capacity = GetRequiredCharCount(latitudeDiff) + GetRequiredCharCount(longitudeDiff);

        int index = 0;
        Span<char> buffer = stackalloc char[capacity];

        index = EncodeDifference(ref latitudeDiff, ref index, ref buffer);
        index = EncodeDifference(ref longitudeDiff, ref index, ref buffer);

        return buffer.ToArray();
    }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref int EncodeDifference(ref int difference, ref int index, ref Span<char> buffer) {
        int rem = difference << 1;

        if (difference < 0) {
            rem = ~rem;
        }

        while (rem >= Defaults.Algorithm.Space) {
            buffer[index++] = Convert.ToChar((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        buffer[index++] = Convert.ToChar(rem + Defaults.Algorithm.QuestionMark);

        return ref index;
    }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetRequiredCharCount(int difference) => difference switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };
}
