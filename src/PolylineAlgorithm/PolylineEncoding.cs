//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Buffers;
using System.IO.Pipelines;

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
    /// Attempts to read a value from the specified buffer and updates the variance.
    /// </summary>
    /// <remarks>This method processes the buffer starting at the specified position and attempts to decode a value.
    /// The decoded value is used to update the <paramref name="variance"/> parameter. The method stops reading when a
    /// termination condition is met or the end of the buffer is reached.</remarks>
    /// <param name="variance">
    /// A reference to the integer that will be updated based on the value read from the buffer.
    /// </param>
    /// <param name="buffer">
    /// A reference to the read-only memory buffer containing the data to be processed.
    /// </param>
    /// <param name="position">
    /// A reference to the current position within the buffer. The position is incremented as the method reads data.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a value was successfully read and the end of the buffer was not reached; otherwise, <see
    /// langword="false"/>.
    /// </returns>

    public static bool TryReadValue(ref int variance, ref ReadOnlyMemory<char> buffer, ref int position) {
        // Validate that the position is within the bounds of the buffer.
        if (position == buffer.Length) {
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

        variance += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;

        // If the end of the buffer was reached without reading a complete value, return false.
        return chunk < Defaults.Algorithm.Space;
    }

    /// <summary>
    /// Converts a normalized integer value to its denormalized double representation based on the specified type.
    /// </summary>
    /// <remarks>The denormalization process divides the input value by a predefined precision factor to
    /// produce the resulting double. Ensure that <paramref name="value"/> is validated against the specified <paramref
    /// name="type"/> before calling this method.</remarks>
    /// <param name="value">
    /// The normalized integer value to be denormalized. Must be within the valid range for the specified <paramref
    /// name="type"/>.
    /// </param>
    /// <param name="type">
    /// The type that defines the valid range for <paramref name="value"/>.
    /// </param>
    /// <returns>
    /// The denormalized double representation of the input value. Returns <see langword="0.0"/> if <paramref
    /// name="value"/> is <see langword="0"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is outside the valid range for the specified <paramref name="type"/>.
    /// </exception>

    public static double Denormalize(int value, CoordinateValueType type) {
        // Validate that the type is not None, as it does not represent a valid coordinate value type.
        if (type == CoordinateValueType.None) {
            throw new ArgumentOutOfRangeException(nameof(type), string.Format(ExceptionMessageResource.ArgumentCannotBeCoordinateValueTypeMessageFormat, type.ToString()));
        }

        // Validate that the value is finite and within the acceptable range for the specified type.
        if (!ValidateValue(value, type)) {
            throw new ArgumentOutOfRangeException(nameof(value), value, string.Format(ExceptionMessageResource.ArgumentOutOfRangeForSpecifiedCoordinateValueTypeMessageFormat, type.ToString().ToLowerInvariant()));
        }

        // Return fast if the value is zero, return 0.0 as the denormalized value.
        if (value == 0) {
            return 0.0;
        }

        return Math.Truncate((double)value) / Defaults.Algorithm.Precision;
    }

    /// <summary>
    /// Attempts to write a value derived from the specified <paramref name="variance"/> into the provided <paramref
    /// name="buffer"/> at the given <paramref name="position"/>.
    /// </summary>
    /// <remarks>
    /// This method performs bounds checking to ensure that the buffer has sufficient space to
    /// accommodate the calculated value. If the buffer does not have enough space, the method returns <see
    /// langword="false"/> without modifying the buffer or position.
    /// </remarks>
    /// <param name="variance">
    /// The integer value used to calculate the output to be written into the buffer.
    /// </param>
    /// <param name="buffer">
    /// A reference to the span of characters where the value will be written.
    /// </param>
    /// <param name="position">
    /// A reference to the current position in the buffer where writing begins. This value is updated to reflect the new
    /// position after writing.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the value was successfully written to the buffer; otherwise, <see langword="false"/>.
    /// </returns>

    public static bool TryWriteValue(int variance, ref Span<char> buffer, ref int position) {
        // Validate that the position and required space for write is within the bounds of the buffer.
        if (buffer.Length < position + GetCharCount(variance)) {
            return false;
        }

        int rem = variance << 1;

        // If the variance is negative, we need to invert the bits to get the correct representation.
        if (variance < 0) {
            rem = ~rem;
        }

        // Write the value to the buffer in a way that encodes it using the specified algorithm.
        while (rem >= Defaults.Algorithm.Space) {
            buffer[position++] = (char)((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        // Write the final character, which is less than the space character.
        buffer[position++] = (char)(rem + Defaults.Algorithm.QuestionMark);

        return true;
    }

    /// <summary>
    /// Normalizes a given numeric value based on the specified type and precision settings.
    /// </summary>
    /// <remarks>
    /// This method validates the input value to ensure it is finite and within the acceptable range
    /// for the specified type. If the value is valid, it applies a normalization algorithm using a predefined precision
    /// factor.
    /// </remarks>
    /// <param name="value">
    /// The numeric value to normalize. Must be a finite number.
    /// </param>
    /// <param name="type">
    /// The type against which the value is validated. Determines the acceptable range for the value.
    /// </param>
    /// <returns>
    /// An integer representing the normalized value. Returns <c>0</c> if the input value is <c>0.0</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is not a finite number or is outside the valid range for the specified
    /// <paramref name="type"/>.
    /// </exception>

    public static int Normalize(double value, CoordinateValueType type) {
        // Validate that the type is not None, as it does not represent a valid coordinate value type.
        if (type == CoordinateValueType.None) {
            throw new ArgumentOutOfRangeException(nameof(type), string.Format(ExceptionMessageResource.ArgumentCannotBeCoordinateValueTypeMessageFormat, type.ToString()));
        }

        // Validate that the value is finite and not NaN or Infinity.
        if (double.IsNaN(value) || double.IsInfinity(value)) {
            throw new ArgumentOutOfRangeException(nameof(value), ExceptionMessageResource.ArgumentValueMustBeFiniteNumber);
        }

        // Validate that the value is within the acceptable range for the specified type.
        if (!ValidateValue(value, type)) {
            throw new ArgumentOutOfRangeException(nameof(value), value, string.Format(ExceptionMessageResource.ArgumentOutOfRangeForSpecifiedCoordinateValueTypeMessageFormat, type.ToString().ToLowerInvariant()));
        }

        // Fast return if the value is zero, return 0 as the normalized value.
        if (value == 0.0) {
            return 0;
        }

        return (int)Math.Round(value * Defaults.Algorithm.Precision);
    }

    /// <summary>
    /// Determines the number of characters required to represent the specified integer value within predefined
    /// variance ranges.
    /// </summary>
    /// <remarks>
    /// The method uses predefined ranges to efficiently determine the character count.  Smaller
    /// values require fewer characters, while larger values require more.  This method is optimized for performance
    /// using a switch expression.
    /// </remarks>
    /// <param name="variance">
    /// The integer value for which the character count is calculated. Must be within the range  of a 32-bit signed
    /// integer.
    /// </param>
    /// <returns>
    /// The number of characters required to represent the <paramref name="variance"/> value,  based on its magnitude.
    /// Returns a value between 1 and 6 inclusive.
    /// </returns>

    public static int GetCharCount(int variance) => variance switch {
        // DO NOT CHANGE THE ORDER. We are skipping inside exclusive ranges as those are covered by previous statements.
        >= -16 and <= +15 => 1,
        >= -512 and <= +511 => 2,
        >= -16384 and <= +16383 => 3,
        >= -524288 and <= +524287 => 4,
        >= -16777216 and <= +16777215 => 5,
        _ => 6,
    };

    /// <summary>
    /// Validates whether the specified denormalized value falls within the acceptable range for the given value type.
    /// </summary>
    /// <param name="value">
    /// The denormalized value to validate.
    /// </param>
    /// <param name="type">
    /// The type of value to validate, such as latitude or longitude.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="value"/> is within the valid range for the specified <paramref
    /// name="type"/>; otherwise, <see langword="false"/>.
    /// </returns>
    private static bool ValidateValue<T>(T value, CoordinateValueType type) => (type, value) switch {
        (CoordinateValueType.Latitude, int normalized) when normalized >= Defaults.Coordinate.Latitude.Normalized.Min && normalized <= Defaults.Coordinate.Latitude.Normalized.Max => true,
        (CoordinateValueType.Longitude, int normalized) when normalized >= Defaults.Coordinate.Longitude.Normalized.Min && normalized <= Defaults.Coordinate.Longitude.Normalized.Max => true,
        (CoordinateValueType.Latitude, double denormalized) when denormalized >= Defaults.Coordinate.Latitude.Min && denormalized <= Defaults.Coordinate.Latitude.Max => true,
        (CoordinateValueType.Longitude, double denormalized) when denormalized >= Defaults.Coordinate.Longitude.Min && denormalized <= Defaults.Coordinate.Longitude.Max => true,
        _ => false,
    };

    /// <summary>
    /// Attempts to read a single encoded value from a <see cref="SequenceReader{T}"/> of bytes and updates the
    /// variance. This overload enables zero-allocation decoding when the source is a pipe buffer.
    /// </summary>
    /// <remarks>
    /// The polyline encoding characters are all in the ASCII range, so each encoded byte maps directly to the
    /// equivalent <see cref="char"/> value used by <see cref="TryReadValue(ref int, ref ReadOnlyMemory{char}, ref int)"/>.
    /// If the reader reaches the end of the buffer before a complete value has been decoded, the variance is left
    /// unchanged and the caller should wait for more data.
    /// </remarks>
    /// <param name="variance">
    /// A reference to the integer that will be updated based on the value read from the buffer.
    /// </param>
    /// <param name="reader">
    /// A reference to the <see cref="SequenceReader{T}"/> over the pipe buffer. The reader's position is advanced
    /// only for each byte that belongs to a completely decoded value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a complete value was successfully decoded; <see langword="false"/> if the buffer
    /// ended before the value was fully read (partial data — the caller should supply more bytes).
    /// </returns>
    internal static bool TryReadValue(ref int variance, ref SequenceReader<byte> reader) {
        int chunk = 0;
        int sum = 0;
        int shifter = 0;

        while (reader.TryRead(out byte b)) {
            chunk = b - Defaults.Algorithm.QuestionMark;
            sum |= (chunk & Defaults.Algorithm.UnitSeparator) << shifter;
            shifter += Defaults.Algorithm.ShiftLength;

            if (chunk < Defaults.Algorithm.Space) {
                variance += (sum & 1) == 1 ? ~(sum >> 1) : sum >> 1;
                return true;
            }
        }

        // Buffer ended before a terminating byte was found — incomplete value.
        return false;
    }

    /// <summary>
    /// Writes a single encoded value derived from <paramref name="variance"/> directly into an
    /// <see cref="IBufferWriter{T}"/> of bytes. This overload enables zero-allocation encoding when the
    /// destination is a pipe writer.
    /// </summary>
    /// <remarks>
    /// The method requests a span of up to 6 bytes from the writer (the maximum size of one encoded value),
    /// writes the encoded bytes, and then advances the writer by the number of bytes actually used.
    /// </remarks>
    /// <param name="variance">
    /// The integer value used to calculate the output bytes to be written.
    /// </param>
    /// <param name="writer">
    /// The <see cref="IBufferWriter{T}"/> to which the encoded bytes are written.
    /// </param>
    internal static void WriteValue(int variance, IBufferWriter<byte> writer) {
        // Maximum encoded size per value is 6 bytes.
        Span<byte> span = writer.GetSpan(6);

        int rem = variance << 1;

        if (variance < 0) {
            rem = ~rem;
        }

        int written = 0;

        while (rem >= Defaults.Algorithm.Space) {
            span[written++] = (byte)((Defaults.Algorithm.Space | rem & Defaults.Algorithm.UnitSeparator) + Defaults.Algorithm.QuestionMark);
            rem >>= Defaults.Algorithm.ShiftLength;
        }

        span[written++] = (byte)(rem + Defaults.Algorithm.QuestionMark);

        writer.Advance(written);
    }
}
