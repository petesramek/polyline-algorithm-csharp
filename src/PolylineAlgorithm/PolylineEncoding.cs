//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;

using System;
using System.Numerics;
using System.Runtime.InteropServices;

/// <summary>
/// Provides methods for encoding and decoding polyline data, as well as utilities for normalizing and de-normalizing
/// geographic coordinate values.
/// </summary>
/// <remarks>The <see cref="PolylineEncoding"/> class includes functionality for working with encoded polyline
/// data, such as reading and writing encoded values, as well as methods for normalizing and de-normalizing geographic
/// coordinates. It also provides validation utilities to ensure values conform to expected ranges for latitude and
/// longitude.</remarks>
public static class PolylineEncoding {
    /// <summary>
    /// Normalizes a geographic coordinate value to an integer representation based on the specified precision.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method converts a floating-point coordinate value into a normalized integer by multiplying it by 10 raised
    /// to the power of the specified precision, then rounding the result using the specified <paramref name="rounding"/> strategy.
    /// </para>
    /// <para>
    /// For example, with the default precision of 5:
    /// <list type="bullet">
    /// <item><description>A value of 37.78903 becomes 3778903</description></item>
    /// <item><description>A value of -122.4123 becomes -12241230</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// The method validates that the input value is finite (not NaN or infinity) before performing normalization.
    /// If the precision is 0, the value is rounded without multiplication.
    /// </para>
    /// </remarks>
    /// <param name="value">
    /// The numeric value to normalize. Must be a finite number (not NaN or infinity).
    /// </param>
    /// <param name="precision">
    /// The number of decimal places of precision to preserve in the normalized value. 
    /// The value is multiplied by 10^<paramref name="precision"/> before rounding. 
    /// Default is 5, which is standard for polyline encoding.
    /// </param>
    /// <param name="rounding">
    /// The rounding strategy to use when converting the scaled value to an integer.
    /// Default is <see cref="MidpointRounding.AwayFromZero"/>, which rounds midpoint values to the nearest number away from zero.
    /// </param>
    /// <returns>
    /// An integer representing the normalized value. Returns <c>0</c> if the input <paramref name="value"/> is <c>0.0</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is not a finite number (NaN or infinity).
    /// </exception>
    /// <exception cref="OverflowException">
    /// Thrown when the normalized result exceeds the range of a 32-bit signed integer during the conversion from double to int.
    /// </exception>
    public static int Normalize(double value, uint precision = 5) {
        // Fast return if the value is zero, return 0 as the normalized value.
        if (value.Equals(default)) {
            return 0;
        }

        // Validate that the value is finite and not NaN or Infinity.
        if (!double.IsFinite(value)) {
            throw new ArgumentOutOfRangeException(nameof(value), ExceptionMessageResource.ArgumentValueMustBeFiniteNumber);
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
    /// Converts a normalized integer coordinate value back to its floating-point representation based on the specified precision.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method reverses the normalization performed by <see cref="Normalize"/>. It takes an integer value and converts it
    /// to a double by dividing it by 10 raised to the power of the specified precision. If <paramref name="precision"/> is 0,
    /// the value is returned as a double without division.
    /// </para>
    /// <para>
    /// The calculation is performed inside a <see langword="checked"/> block to ensure that any arithmetic overflow is detected
    /// and an <see cref="OverflowException"/> is thrown.
    /// </para>
    /// <para>
    /// For example, with a precision of 5:
    /// <list type="bullet">
    /// <item><description>A value of 3778903 becomes 37.78903</description></item>
    /// <item><description>A value of -12241230 becomes -122.4123</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// If the input <paramref name="value"/> is <c>0</c>, the method returns <c>0.0</c> immediately.
    /// </para>
    /// </remarks>
    /// <param name="value">
    /// The integer value to denormalize. Typically produced by the <see cref="Normalize"/> method.
    /// </param>
    /// <param name="precision">
    /// The number of decimal places used during normalization. Default is 5, matching standard polyline encoding precision.
    /// </param>
    /// <param name="rounding">
    /// The rounding strategy to use when converting the result to a double. Default is <see cref="MidpointRounding.AwayFromZero"/>.
    /// </param>
    /// <returns>
    /// The denormalized floating-point coordinate value.
    /// </returns>
    /// <exception cref="OverflowException">
    /// Thrown if the arithmetic operation overflows during conversion.
    /// </exception>
    public static double Denormalize(int value, uint precision = 5) {
        if (value.Equals(default)) {
            return default;
        }

        // Fast return if precision is zero, return current value converted to Int32.
        if (precision == default) {
            return value;
        }

        uint factor = Pow10.GetFactor(precision);

        checked {

            return value / (double)factor;
        }
    }

    /// <summary>
    /// Attempts to read an encoded integer value from a polyline buffer, updating the specified delta and position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method decodes a value from a polyline-encoded character buffer, starting at the given position. It reads
    /// characters sequentially, applying the polyline decoding algorithm, and updates the <paramref name="delta"/> with
    /// the decoded value. The position is advanced as characters are processed.
    /// </para>
    /// <para>
    /// The decoding process continues until a character with a value less than the algorithm's space constant is encountered,
    /// which signals the end of the encoded value. If the buffer is exhausted before a complete value is read, the method returns <see langword="false"/>.
    /// </para>
    /// <para>
    /// The decoded value is added to <paramref name="delta"/> using zigzag decoding, which handles both positive and negative values.
    /// </para>
    /// </remarks>
    /// <param name="delta">
    /// Reference to the integer accumulator that will be updated with the decoded value.
    /// </param>
    /// <param name="buffer">
    /// The buffer containing polyline-encoded characters.
    /// </param>
    /// <param name="position">
    /// Reference to the current position in the buffer. This value is updated as characters are read.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a value was successfully read and decoded; <see langword="false"/> if the buffer ended before a complete value was read.
    /// </returns>
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
    /// Attempts to write an encoded integer value to a polyline buffer, updating the specified position.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method encodes an integer delta value into a polyline-encoded format and writes it to the provided character buffer,
    /// starting at the given position. It applies zigzag encoding followed by the polyline encoding algorithm to represent
    /// both positive and negative values efficiently.
    /// </para>
    /// <para>
    /// The encoding process first converts the value using zigzag encoding (left shift by 1, with bitwise inversion for negative values),
    /// then writes it as a sequence of characters. Each character encodes 5 bits of data, with continuation bits indicating whether
    /// more characters follow. The position is advanced as characters are written.
    /// </para>
    /// <para>
    /// Before writing, the method validates that sufficient space is available in the buffer by calling <see cref="GetCharCount"/>.
    /// If the buffer does not have enough remaining capacity, the method returns <see langword="false"/> without modifying the buffer or position.
    /// </para>
    /// <para>
    /// This method is the inverse of <see cref="TryReadValue"/> and can be used to encode coordinate deltas for polyline serialization.
    /// </para>
    /// </remarks>
    /// <param name="delta">
    /// The integer value to encode and write to the buffer. This value typically represents the difference between consecutive
    /// coordinate values in polyline encoding.
    /// </param>
    /// <param name="buffer">
    /// The destination buffer where the encoded characters will be written. Must have sufficient capacity to hold the encoded value.
    /// </param>
    /// <param name="position">
    /// Reference to the current position in the buffer. This value is updated as characters are written to reflect the new position
    /// after encoding is complete.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the value was successfully encoded and written to the buffer; <see langword="false"/> if the buffer
    /// does not have sufficient remaining capacity to hold the encoded value.
    /// </returns>
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
    /// Calculates the number of characters required to encode a delta value in polyline format.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method determines how many characters will be needed to represent an integer delta value when encoded
    /// using the polyline encoding algorithm. It performs the same zigzag encoding transformation as <see cref="TryWriteValue"/>
    /// but only calculates the required buffer size without actually writing any data.
    /// </para>
    /// <para>
    /// The calculation process:
    /// <list type="number">
    /// <item><description>Applies zigzag encoding: left-shifts the value by 1 bit, then inverts all bits if the original value was negative</description></item>
    /// <item><description>Counts how many 5-bit chunks are needed to represent the encoded value</description></item>
    /// <item><description>Each chunk requires one character, with a minimum of 1 character for any value</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// This method is useful for pre-allocating buffers of the correct size before encoding polyline data, helping to avoid
    /// buffer overflow checks during the actual encoding process.
    /// </para>
    /// <para>
    /// The method uses a <see langword="long"/> internally to prevent overflow during the left-shift operation on large negative values.
    /// </para>
    /// </remarks>
    /// <param name="delta">
    /// The integer delta value to calculate the encoded size for. This value typically represents the difference between
    /// consecutive coordinate values in polyline encoding.
    /// </param>
    /// <returns>
    /// The number of characters required to encode the specified delta value. The minimum return value is 1.
    /// </returns>
    /// <seealso cref="TryWriteValue"/>
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

    public static class Validation {
        /// <summary>
        /// Validates the format of a polyline segment, ensuring all characters are valid and block structure is correct.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs two levels of validation on the provided polyline segment:
        /// </para>
        /// <list type="number">
        ///   <item>
        ///     <description>
        ///       <b>Character Range Validation:</b> Checks that every character in the polyline is within the valid ASCII range for polyline encoding ('?' [63] to '_' [95], inclusive).
        ///       Uses SIMD acceleration for efficient validation of large segments.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       <b>Block Structure Validation:</b> Ensures that each encoded value (block) does not exceed 7 characters and that the polyline ends with a valid block terminator.
        ///     </description>
        ///   </item>
        /// </list>
        /// <para>
        /// If an invalid character or block structure is detected, an <see cref="ArgumentException"/> is thrown with details about the error.
        /// </para>
        /// </remarks>
        /// <param name="polyline">A span representing the polyline segment to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when an invalid character is found or the block structure is invalid.
        /// </exception>
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
        /// <para>
        /// Uses SIMD vectorization for efficient validation of large spans. Falls back to scalar checks for any block where an invalid character is detected.
        /// </para>
        /// <para>
        /// The valid range is from '?' (63) to '_' (95), inclusive. If an invalid character is found, an <see cref="ArgumentException"/> is thrown.
        /// </para>
        /// </remarks>
        /// <param name="polyline">A span representing the polyline segment to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when an invalid character is found in the polyline segment.
        /// </exception>
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
                            throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
                        }
                    }
                }
            }

            for (; i < length; i++) {
                char character = polyline[i];
                if (character < Min || character > Max) {
                    throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
                }
            }
        }

        /// <summary>
        /// Validates the block structure of a polyline segment, ensuring each encoded value does not exceed 7 characters and the polyline ends correctly.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Iterates through the polyline, counting the length of each block (a sequence of characters representing an encoded value).
        /// Throws an <see cref="ArgumentException"/> if any block exceeds 7 characters or if the polyline does not end with a valid block terminator.
        /// </para>
        /// </remarks>
        /// <param name="polyline">A span representing the polyline segment to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when a block exceeds 7 characters or the polyline does not end with a valid block terminator.
        /// </exception>
        public static void ValidateBlockLength(ReadOnlySpan<char> polyline) {
            int blockLen = 0;
            bool foundBlockEnd = false;

            for (int i = 0; i < polyline.Length; i++) {
                blockLen++;

                if (polyline[i] < End) {
                    foundBlockEnd = true;
                    if (blockLen > 7) {
                        throw new ArgumentException($"Block at position {i - blockLen + 1} exceeds 7 characters.", nameof(polyline));
                    }
                    blockLen = 0;
                } else {
                    foundBlockEnd = false;
                }
            }

            if (!foundBlockEnd) {
                throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
            }
        }

        /// <summary>
        /// The minimum valid character value for polyline encoding, corresponding to the ASCII value of '?' (63).
        /// </summary>
        private const ushort Min = Defaults.Algorithm.QuestionMark;

        /// <summary>
        /// The maximum valid character value for polyline encoding, calculated as the sum of two question mark values ('?' + '?', or 63 + 63 = 126).
        /// </summary>
        private const ushort Max = (Defaults.Algorithm.QuestionMark + Defaults.Algorithm.QuestionMark);

        /// <summary>
        /// The character value that marks the end of a polyline block, calculated as the sum of the question mark value and the space value ('?' + ' ', or 63 + 32 = 95).
        /// </summary>
        private const ushort End = (Defaults.Algorithm.QuestionMark + Defaults.Algorithm.Space);

        /// <summary>
        /// SIMD vector containing the minimum valid character value for efficient range validation of polyline segments.
        /// </summary>
        private static readonly Vector<ushort> MinVector = new(Min);

        /// <summary>
        /// SIMD vector containing the maximum valid character value for efficient range validation of polyline segments.
        /// </summary>
        private static readonly Vector<ushort> MaxVector = new(Max);
    }
}
