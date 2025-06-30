//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using PolylineAlgorithm.Abstraction.Internal;
using PolylineAlgorithm.Abstraction.Properties;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides methods for encoding and decoding polyline data, as well as utilities for normalizing and denormalizing
/// geographic coordinate values.
/// </summary>
/// <remarks>The <see cref="PolylineEncoding"/> class includes functionality for working with encoded polyline
/// data, such as reading and writing encoded values, as well as methods for normalizing and denormalizing geographic
/// coordinates. It also provides validation utilities to ensure values conform to expected ranges for latitude and
/// longitude.</remarks>
public static class PolylineEncoding {
    /// <summary>
    /// Attempts to read a value from the specified buffer and updates the variance.
    /// </summary>
    /// <remarks>This method processes the buffer starting at the specified position and attempts to decode a value.
    /// The decoded value is used to update the <paramref name="variance"/> parameter. The method stops reading when a
    /// termination condition is met or the end of the buffer is reached.</remarks>
    /// <param name="variance">A reference to the integer that will be updated based on the value read from the buffer.</param>
    /// <param name="buffer">A reference to the read-only memory buffer containing the data to be processed.</param>
    /// <param name="position">A reference to the current position within the buffer. The position is incremented as the method reads data.</param>
    /// <returns><see langword="true"/> if a value was successfully read and the end of the buffer was not reached; otherwise, <see
    /// langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryReadValue(ref int variance, ref ReadOnlyMemory<char> buffer, ref int position) {
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
    /// Converts a normalized integer value to its denormalized double representation based on the specified type.
    /// </summary>
    /// <remarks>The denormalization process divides the input value by a predefined precision factor to
    /// produce the resulting double. Ensure that <paramref name="value"/> is validated against the specified <paramref
    /// name="type"/> before calling this method.</remarks>
    /// <param name="value">The normalized integer value to be denormalized. Must be within the valid range for the specified <paramref
    /// name="type"/>.</param>
    /// <param name="type">The type that defines the valid range for <paramref name="value"/>.</param>
    /// <returns>The denormalized double representation of the input value. Returns <see langword="0.0"/> if <paramref
    /// name="value"/> is <see langword="0"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is outside the valid range for the specified <paramref name="type"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Denormalize(int value, ValueType type) {
        if (!ValidateNormalizedValue(value, type)) {
            throw new ArgumentOutOfRangeException(nameof(value), value, string.Format(ExceptionMessageResource.ArgumentIsOutOfRangeForSpecifiedType, type.ToString().ToLowerInvariant()));
        }

        if (value == 0) {
            return 0.0;
        }

        return Math.Truncate((double)value) / Defaults.Algorithm.Precision;
    }

    /// <summary>
    /// Validates whether the specified normalized value falls within the acceptable range for the given value type.
    /// </summary>
    /// <remarks>The valid range for normalized values depends on the specified <paramref name="type"/>: <list
    /// type="bullet"> <item> <description> For <see cref="ValueType.Latitude"/>, the value must be between
    /// <c>Defaults.Coordinate.Latitude.Normalized.Min</c> and <c>Defaults.Coordinate.Latitude.Normalized.Max</c>,
    /// inclusive. </description> </item> <item> <description> For <see cref="ValueType.Longitude"/>, the value must be
    /// between <c>Defaults.Coordinate.Longitude.Normalized.Min</c> and
    /// <c>Defaults.Coordinate.Longitude.Normalized.Max</c>, inclusive. </description> </item> </list> Any other
    /// <paramref name="type"/> will result in the method returning <see langword="false"/>.</remarks>
    /// <param name="value">The normalized value to validate. Must be an integer.</param>
    /// <param name="type">The type of value to validate, such as <see cref="ValueType.Latitude"/> or <see cref="ValueType.Longitude"/>.</param>
    /// <returns><see langword="true"/> if the normalized value is within the valid range for the specified value type;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool ValidateNormalizedValue(int value, ValueType type) => (type, value) switch {
        (ValueType.Latitude, int normalized) when normalized >= Defaults.Coordinate.Latitude.Normalized.Min && normalized <= Defaults.Coordinate.Latitude.Normalized.Max => true,
        (ValueType.Longitude, int normalized) when normalized >= Defaults.Coordinate.Longitude.Normalized.Min && normalized <= Defaults.Coordinate.Longitude.Normalized.Max => true,
        _ => false,
    };

    /// <summary>
    /// Validates whether the specified denormalized value falls within the acceptable range for the given value type.
    /// </summary>
    /// <remarks>The valid ranges for latitude and longitude are defined by <see
    /// cref="Defaults.Coordinate.Latitude.Min"/>, <see cref="Defaults.Coordinate.Latitude.Max"/>, <see
    /// cref="Defaults.Coordinate.Longitude.Min"/>, and <see cref="Defaults.Coordinate.Longitude.Max"/>.</remarks>
    /// <param name="value">The denormalized value to validate.</param>
    /// <param name="type">The type of value to validate, such as latitude or longitude.</param>
    /// <returns><see langword="true"/> if the <paramref name="value"/> is within the valid range for the specified <paramref
    /// name="type"/>; otherwise, <see langword="false"/>.</returns>
    public static bool ValidateDenormalizedValue(double value, ValueType type) => (type, value) switch {
        (ValueType.Latitude, double denormalized) when denormalized >= Defaults.Coordinate.Latitude.Min && denormalized <= Defaults.Coordinate.Latitude.Max => true,
        (ValueType.Longitude, double denormalized) when denormalized >= Defaults.Coordinate.Longitude.Min && denormalized <= Defaults.Coordinate.Longitude.Max => true,
        _ => false,
    };

    /// <summary>
    /// Attempts to write a value derived from the specified <paramref name="variance"/> into the provided <paramref
    /// name="buffer"/> at the given <paramref name="position"/>.
    /// </summary>
    /// <remarks>This method performs bounds checking to ensure that the buffer has sufficient space to
    /// accommodate the calculated value. If the buffer does not have enough space, the method returns <see
    /// langword="false"/> without modifying the buffer or position.</remarks>
    /// <param name="variance">The integer value used to calculate the output to be written into the buffer.</param>
    /// <param name="buffer">A reference to the span of characters where the value will be written.</param>
    /// <param name="position">A reference to the current position in the buffer where writing begins. This value is updated to reflect the new
    /// position after writing.</param>
    /// <returns><see langword="true"/> if the value was successfully written to the buffer; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryWriteValue(int variance, ref Span<char> buffer, ref int position) {
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
    /// Normalizes a given numeric value based on the specified type and precision settings.
    /// </summary>
    /// <remarks>This method validates the input value to ensure it is finite and within the acceptable range
    /// for the specified type. If the value is valid, it applies a normalization algorithm using a predefined precision
    /// factor.</remarks>
    /// <param name="value">The numeric value to normalize. Must be a finite number.</param>
    /// <param name="type">The type against which the value is validated. Determines the acceptable range for the value.</param>
    /// <returns>An integer representing the normalized value. Returns <c>0</c> if the input value is <c>0.0</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not a finite number or is outside the valid range for the specified
    /// <paramref name="type"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Normalize(double value, ValueType type) {
        if (double.IsNaN(value) || double.IsInfinity(value)) {
            throw new ArgumentOutOfRangeException(nameof(value), ExceptionMessageResource.ArgumentValueMustBeFiniteNumber);
        }

        if (!ValidateDenormalizedValue(value, type)) {
            throw new ArgumentOutOfRangeException(nameof(value), value, string.Format(ExceptionMessageResource.ArgumentIsOutOfRangeForSpecifiedType, type.ToString().ToLowerInvariant()));
        }

        if (value == 0.0) {
            return 0;
        }

        return (int)Math.Round(value * Defaults.Algorithm.Precision);
    }

    /// <summary>
    /// Determines the number of characters required to represent the specified integer value within predefined
    /// variance ranges.
    /// </summary>
    /// <remarks>The method uses predefined ranges to efficiently determine the character count.  Smaller
    /// values require fewer characters, while larger values require more.  This method is optimized for performance
    /// using a switch expression.</remarks>
    /// <param name="variance">The integer value for which the character count is calculated. Must be within the range  of a 32-bit signed
    /// integer.</param>
    /// <returns>The number of characters required to represent the <paramref name="variance"/> value,  based on its magnitude.
    /// Returns a value between 1 and 6 inclusive.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    /// Represents the type of a geographic coordinate value.
    /// </summary>
    /// <remarks>This enumeration is used to specify whether a coordinate value represents latitude or
    /// longitude. Latitude values indicate the north-south position, while longitude values indicate the east-west
    /// position.</remarks>
    public enum ValueType {
        Latitude,
        Longitude
    }
}
