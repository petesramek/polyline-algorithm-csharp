namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods for encoding and decoding geographic coordinates into polyline strings.
/// </summary>
public class PolylineEncoding {
    /// <summary>
    /// Gets the default instance of the <see cref="PolylineEncoding"/> class.
    /// </summary>
    public static readonly PolylineEncoding Default = new();

    /// <summary>
    /// Attempts to read an encoded value from the buffer and update the variance.
    /// </summary>
    /// <param name="variance">The current variance to update with the decoded value.</param>
    /// <param name="buffer">The buffer containing the encoded polyline data.</param>
    /// <param name="position">The current position in the buffer. This will be updated as the value is read.</param>
    /// <returns>
    /// <see langword="true"/> if the value was successfully read; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReadValue(ref int variance, ref ReadOnlyMemory<char> buffer, ref int position) {
        if (position == buffer.Length) {
            return false;
        }

        int chunk = 0;
        int sum = 0;
        int shifter = 0;
        ReadOnlySpan<char> span = buffer.Span;

        while (position < buffer.Length) {
            chunk = span[position++] - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;

            if (chunk < Defaults.Algorithm.Space) {
                break;
            }
        }

        variance += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        return chunk < Defaults.Algorithm.Space;
    }

    /// <summary>
    /// Converts a normalized integer value back to its original double representation.
    /// </summary>
    /// <param name="value">The normalized integer value.</param>
    /// <returns>The denormalized double value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Denormalize(int value) => (double)value / Defaults.Algorithm.Precision;

    /// <summary>
    /// Attempts to write an encoded value to the buffer.
    /// </summary>
    /// <param name="variance">The variance to encode.</param>
    /// <param name="buffer">The buffer to write the encoded value to.</param>
    /// <param name="position">The current position in the buffer. This will be updated as the value is written.</param>
    /// <returns>
    /// <see langword="true"/> if the value was successfully written; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryWriteValue(int variance, ref Span<char> buffer, ref int position) {
        if (buffer.Length < position + GetCharCount(variance)) {
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

    /// <summary>
    /// Normalizes a double value into an integer representation for encoding.
    /// </summary>
    /// <param name="value">The double value to normalize.</param>
    /// <returns>The normalized integer value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Normalize(double value) => (int)Math.Truncate(value * Defaults.Algorithm.Precision);

    /// <summary>
    /// Calculates the number of characters required to encode a given variance.
    /// </summary>
    /// <param name="variance">The variance to encode.</param>
    /// <returns>The number of characters required to encode the variance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetCharCount(int variance) => variance switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };
}
