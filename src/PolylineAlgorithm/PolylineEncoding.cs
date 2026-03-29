//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Diagnostics;
using PolylineAlgorithm.Internal;

using System;
using System.Numerics;
using System.Runtime.InteropServices;

/// <summary>
/// Static helper API for working with Google-style polyline encoded data and for converting geographic coordinate values
/// between their floating-point representation and the integer "normalized" representation used by the encoding algorithm.
/// </summary>
/// <remarks>
/// The <see cref="PolylineEncoding"/> type contains three groups of capabilities:
/// <list type="bullet">
/// <item><description>Normalization/denormalization: Convert between double coordinates and the integer representation used by polyline encoding.</description></item>
/// <item><description>Low-level read/write primitives: Encode and decode individual integer delta values to/from polyline character buffers.</description></item>
/// <item><description>Validation helpers: Verify that a polyline character sequence contains only valid characters and that each encoded block is well-formed.</description></item>
/// </list>
/// The implementation uses zig-zag encoding for signed values and 5-bit chunk output for ASCII-safe characters. Validation
/// methods are provided to fail early when input contains invalid characters or invalid block structure.
/// </remarks>
public static class PolylineEncoding {
    /// <summary>
    /// Convert a floating-point geographic coordinate (latitude or longitude) into the integer "normalized" form used by
    /// polyline encoding.
    /// </summary>
    /// <remarks>
    /// The method multiplies <paramref name="value"/> by 10^<paramref name="precision"/> and then truncates toward zero to
    /// produce the integer representation. A precision of 5 is the common default for polyline encodings (for example, 37.78903 -> 3778903).
    ///
    /// The method does not accept non-finite values (NaN or Infinity) and will invoke the library's validation helpers if such a value is passed.
    /// If <paramref name="precision"/> is zero, the value is truncated to an integer without scaling.
    ///
    /// Implementation note: <see cref="Math.Truncate(double)"/> is used to produce a deterministic integer result.
    /// </remarks>
    /// <param name="value">Floating-point coordinate value to normalize. Must be finite (not NaN or Infinity).</param>
    /// <param name="precision">Number of decimal places to preserve. Default is 5.</param>
    /// <returns>Integer representation of the coordinate at the requested precision.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown via internal validation if <paramref name="value"/> is not a finite number.</exception>
    /// <exception cref="OverflowException">Thrown when the checked conversion to <see cref="int"/> overflows.</exception>
    public static int Normalize(double value, uint precision = 5) {
        // Fast return if the value is zero, return 0 as the normalized value.
        if (value.Equals(default)) {
            return 0;
        }

        // Validate that the value is finite and not NaN or Infinity.
        if (!double.IsFinite(value)) {
            ExceptionGuard.ThrowNotFiniteNumber(nameof(value));
        }

        // Fast return if precision is zero, return current value converted to Int32.
        if (precision == default) {
            return (int)Math.Truncate(value);
        }

        uint factor = Pow10.GetFactor(precision);

        checked {
            return (int)(Math.Truncate(value * 10 * factor) / 10);
        }

    }

    /// <summary>
    /// Convert an integer normalized coordinate back into its floating-point representation using the given precision.
    /// </summary>
    /// <remarks>
    /// This is the inverse of <see cref="Normalize"/>. The method divides <paramref name="value"/> by 10^<paramref name="precision"/>
    /// and returns a <see cref="double"/>. If <paramref name="precision"/> is zero the integer value is returned as a double unchanged.
    ///
    /// Arithmetic is performed in a <c>checked</c> context so that any overflow is surfaced as an <see cref="OverflowException"/>.
    /// </remarks>
    /// <param name="value">Integer normalized coordinate (typically produced by <see cref="Normalize"/>).</param>
    /// <param name="precision">Number of decimal places that were used when normalizing. Default is 5.</param>
    /// <returns>Denormalized floating-point coordinate.</returns>
    /// <exception cref="OverflowException">If arithmetic overflows during conversion.</exception>
    public static double Denormalize(int value, uint precision = 5) {
        if (value.Equals(default)) {
            return default;
        }

        // Fast return if precision is zero, return current value converted to double.
        if (precision == default) {
            return value;
        }

        uint factor = Pow10.GetFactor(precision);

        checked {

            return value / (double)factor;
        }
    }

    /// <summary>
    /// Decode a single polyline-encoded integer from <paramref name="buffer"/> starting at <paramref name="position"/>.
    /// </summary>
    /// <remarks>
    /// The method reads one encoded integer (a delta) from the polyline character buffer using the polyline decoding algorithm:
    /// it accumulates 5-bit chunks from successive characters, stops when a chunk with the continuation bit cleared is seen,
    /// and then reconstructs the signed integer using zig-zag decoding. The decoded value is added to <paramref name="delta"/>.
    ///
    /// The <paramref name="position"/> argument is advanced to the character after the last character consumed for the value.
    /// If the buffer ends before a complete encoded value is available the method returns <c>false</c> and <paramref name="position"/>
    /// will point to the buffer end.
    /// </remarks>
    /// <param name="delta">Reference to the accumulator; the decoded value will be added to this parameter.</param>
    /// <param name="buffer">Read-only memory containing polyline-encoded characters.</param>
    /// <param name="position">Reference to the current index in <paramref name="buffer"/>; advanced as characters are read.</param>
    /// <returns><c>true</c> if a full value was decoded; <c>false</c> if the buffer ended before the value was complete.</returns>
    public static bool TryReadValue(ref int delta, ReadOnlyMemory<char> buffer, ref int position) {
        // Validate that the position is within the bounds of the buffer.
        if (position >= buffer.Length) {
            return false;
        }

        // Initialize variables for reading the value.
        int chunk = 0;
        int sum = 0;
        int shifter = 0;
        ReadOnlySpan<char> span = buffer.Span;

        // Read characters from the buffer until a termination condition is met or the end of the buffer is reached.
        while (position < buffer.Length) {
            chunk = span[position++] - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;

            // If the chunk is less than the space character, it indicates the end of the value.
            if (chunk < Defaults.Algorithm.Space) {
                break;
            }
        }

        delta += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        // If the end of the buffer was reached without reading a complete value, return false.
        return chunk < Defaults.Algorithm.Space;
    }


    /// <summary>
    /// Encode a single integer delta value into polyline character format and write it into <paramref name="buffer"/>.
    /// </summary>
    /// <remarks>
    /// The method applies zig-zag encoding (value &lt;&lt; 1, invert if negative) and then writes the value as a sequence of 5-bit
    /// chunks. Each output character encodes 5 bits plus a continuation-bit; characters are offset by the configured question-mark
    /// base value to make them ASCII-safe.
    ///
    /// Before writing the method checks that there is sufficient space in the destination span by calling
    /// <see cref="GetRequiredBufferSize(int)"/>. If there is not enough space the method returns <c>false</c> without modifying
    /// the buffer or position.
    /// </remarks>
    /// <param name="delta">The integer value to encode (typically a coordinate delta).</param>
    /// <param name="buffer">Destination character span to write the encoded characters into.</param>
    /// <param name="position">Reference to the current write index in <paramref name="buffer"/>; advanced as characters are written.</param>
    /// <returns><c>true</c> if the value was encoded and written; <c>false</c> if the buffer did not have sufficient space.</returns>
    public static bool TryWriteValue(int delta, Span<char> buffer, ref int position) {
        // Validate that the position and required space for write is within the bounds of the buffer.
        if (buffer[position..].Length < GetRequiredBufferSize(delta)) {
            return false;
        }

        int rem = delta << 1;

        // If the delta is negative, we need to invert the bits to get the correct representation.
        if (delta < 0) {
            rem = ~rem;
        }

        // Write the value to the buffer in a way that encodes it using the specified algorithm.
        while (rem >= Defaults.Algorithm.Space) {
            buffer[position++] =
                (char)((Defaults.Algorithm.Space
                | (rem & Defaults.Algorithm.UnitSeparator))
                + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        // Write the final character, which is less than the space character.
        buffer[position++] = (char)(rem + Defaults.Algorithm.QuestionMark);

        return true;
    }

    /// <summary>
    /// Compute the number of characters required to encode the given integer delta in polyline format.
    /// </summary>
    /// <remarks>
    /// The method performs the same zig-zag transformation used by <see cref="TryWriteValue(int, Span{char}, ref int)"/>
    /// and then counts how many 5-bit output chunks are needed. Each chunk maps to one character; the minimum required size is 1.
    /// </remarks>
    /// <param name="delta">Integer delta value.</param>
    /// <returns>Number of characters required to encode <paramref name="delta"/>.</returns>
    /// <seealso cref="TryWriteValue(int, Span{char}, ref int)"/>
    public static int GetRequiredBufferSize(int delta) {
        long rem = (long)delta << 1;

        if (delta < 0) {
            rem = ~rem;
        }

        int size = 1;

        while (rem >= Defaults.Algorithm.Space) {
            rem >>= Defaults.Algorithm.ShiftLength;
            size++;
        }

        return size;
    }

    #region Validation

    /// <summary>
    /// Minimum base character value used by the algorithm (ASCII code of the configured question-mark offset).
    /// </summary>
    private const ushort Min = Defaults.Algorithm.QuestionMark;

    /// <summary>
    /// Maximum character value accepted by the character-range validation logic (computed from the configured constants).
    /// </summary>
    /// <remarks>
    /// This value is derived from the library's algorithm constants; it represents the upper inclusive bound that
    /// <see cref="ValidateCharRange(ReadOnlySpan{char})"/> will accept. The concrete numeric value depends on the
    /// <see cref="Defaults.Algorithm.QuestionMark"/> constant used by the project.
    /// </remarks>
    private const ushort Max = (Defaults.Algorithm.QuestionMark + Defaults.Algorithm.QuestionMark);

    /// <summary>
    /// The numeric threshold used to identify a block terminator (characters with value lower than this mark end an encoded block).
    /// </summary>
    private const ushort End = (Defaults.Algorithm.QuestionMark + Defaults.Algorithm.Space);

    /// <summary>
    /// SIMD vector populated with the minimum allowed character value to accelerate vectorized range checks.
    /// </summary>
    private static readonly Vector<ushort> MinVector = new(Min);

    /// <summary>
    /// SIMD vector populated with the maximum allowed character value to accelerate vectorized range checks.
    /// </summary>
    private static readonly Vector<ushort> MaxVector = new(Max);

    /// <summary>
    /// Validate the overall format of a polyline character sequence.
    /// </summary>
    /// <remarks>
    /// This validation performs two checks:
    /// <list type="number">
    /// <item><description>Character range check: ensure every character's numeric code is inside the allowed range (between <see cref="Min"/> and <see cref="Max"/>, inclusive).</description></item>
    /// <item><description>Block structure check: ensure each encoded value (block) terminates within the configured maximum block length and that the sequence ends on a block terminator.</description></item>
    /// </list>
    /// If a problem is detected an <see cref="ArgumentException"/> (via internal exception helpers) is thrown describing the violation.
    /// </remarks>
    /// <param name="polyline">Span of characters representing the encoded polyline segment to validate.</param>
    public static void ValidateFormat(ReadOnlySpan<char> polyline) {
        // 1. SIMD character check (reuse existing method)
        ValidateCharRange(polyline);
        // 2. Block structure check
        ValidateBlockLength(polyline);
    }

    /// <summary>
    /// Validates that all characters in the polyline segment are within the allowed ASCII range for polyline encoding.
    /// </summary>
    /// <remarks>
    /// Uses SIMD vectorization for efficient validation of large spans. Falls back to scalar checks for any block where an invalid character is detected.
    /// The valid numeric range is from '?' (configured question-mark offset) up to the computed <see cref="Max"/> inclusive.
    /// </remarks>
    /// <param name="polyline">A span representing the polyline segment to validate.</param>
    /// <exception cref="ArgumentException">Thrown via internal helpers when an invalid character is found in the polyline segment.</exception>
    public static void ValidateCharRange(ReadOnlySpan<char> polyline) {
        int length = polyline.Length;
        int vectorSize = Vector<ushort>.Count;

        int i = 0;
        for (; i <= length - vectorSize; i += vectorSize) {
            var span = MemoryMarshal.Cast<char, ushort>(polyline.Slice(i, vectorSize));
#if NET5_0_OR_GREATER
            var chars = new Vector<ushort>(span);
#else
            var chars = new Vector<ushort>(span.ToArray());
#endif
            var belowMin = Vector.LessThan(chars, MinVector);
            var aboveMax = Vector.GreaterThan(chars, MaxVector);
            if (Vector.BitwiseOr(belowMin, aboveMax) != Vector<ushort>.Zero) {
                // Fallback to scalar check for this block
                for (int j = 0; j < vectorSize; j++) {
                    char character = polyline[i + j];
                    if (character < Min || character > Max) {
                        ExceptionGuard.ThrowInvalidPolylineCharacter(character, i + j);
                    }
                }
            }
        }

        for (; i < length; i++) {
            char character = polyline[i];
            if (character < Min || character > Max) {
                ExceptionGuard.ThrowInvalidPolylineCharacter(character, i);
            }
        }
    }

    /// <summary>
    /// Validates the block structure of a polyline segment, ensuring each encoded value does not exceed the configured maximum length and the polyline ends correctly.
    /// </summary>
    /// <remarks>
    /// Iterates through the polyline, counting the length of each block (a sequence of characters representing an encoded value).
    /// Throws an <see cref="ArgumentException"/> (via internal helpers) if any block exceeds the configured maximum number of characters
    /// or if the polyline does not end with a block terminator.
    /// </remarks>
    /// <param name="polyline">A span representing the polyline segment to validate.</param>
    /// <exception cref="ArgumentException">Thrown via internal helpers when a block exceeds the maximum length or when a block terminator is missing.</exception>
    public static void ValidateBlockLength(ReadOnlySpan<char> polyline) {
        int blockLen = 0;
        bool foundBlockEnd = false;

        for (int i = 0; i < polyline.Length; i++) {
            blockLen++;

            if (polyline[i] < End) {
                foundBlockEnd = true;
                if (blockLen > Defaults.Polyline.Block.Length.Max) {
                    ExceptionGuard.ThrowPolylineBlockTooLong(i - blockLen + 1);
                }
                blockLen = 0;
            } else {
                foundBlockEnd = false;
            }
        }

        if (!foundBlockEnd) {
            ExceptionGuard.ThrowInvalidPolylineBlockTerminator();
        }
    }

    #endregion
}