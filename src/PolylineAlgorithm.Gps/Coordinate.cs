//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Gps;

using PolylineAlgorithm.Diagnostics;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a geographic coordinate as a pair of latitude and longitude values.
/// </summary>
/// <remarks>
/// This struct is designed to be immutable and lightweight, providing a simple way to represent
/// geographic coordinates in degrees. It includes validation for latitude and longitude ranges
/// and provides methods for equality comparison and string representation.
///
/// <para>Note: The value (0, 0) is a valid coordinate (Gulf of Guinea), but is also treated as the "default" value by <see cref="IsDefault"/>.</para>
/// </remarks>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Auto)]
public readonly struct Coordinate : IEquatable<Coordinate> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct with default values (0) for <see cref="Latitude"/> and <see cref="Longitude"/>.
    /// </summary>
    /// <remarks>
    /// The default value (0, 0) is a valid coordinate (Gulf of Guinea), but is also treated as the "default" value by <see cref="IsDefault"/>.
    /// </remarks>
    public Coordinate() {
        Latitude = default;
        Longitude = default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct with the specified latitude and longitude values.
    /// </summary>
    /// <param name="latitude">
    /// The latitude component of the coordinate, in degrees. Must be between -90 and 90.
    /// </param>
    /// <param name="longitude">
    /// The longitude component of the coordinate, in degrees. Must be between -180 and 180.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="latitude"/> is less than -90 or greater than 90,
    /// or when <paramref name="longitude"/> is less than -180 or greater than 180.
    /// </exception>
    public Coordinate(double latitude, double longitude) {
        Validator.Validate(latitude, longitude);

        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets the latitude component of the coordinate, in degrees.
    /// </summary>
    public double Latitude { get; }

    /// <summary>
    /// Gets the longitude component of the coordinate, in degrees.
    /// </summary>
    public double Longitude { get; }

    /// <summary>
    /// Determines whether this coordinate is the default value (both <see cref="Latitude"/> and <see cref="Longitude"/> are 0).
    /// </summary>
    /// <remarks>
    /// The value (0, 0) is a valid coordinate (Gulf of Guinea), but is also treated as the "default" value by this method.
    /// </remarks>
    /// <returns>
    /// <see langword="true"/> if both latitude and longitude are 0; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsDefault()
        => Latitude.Equals(default)
        && Longitude.Equals(default);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Coordinate coordinate && Equals(coordinate);

    /// <inheritdoc />
    public bool Equals(Coordinate other) {
        return Latitude.Equals(other.Latitude) &&
               Longitude.Equals(other.Longitude);
    }

    /// <summary>
    /// Determines whether two <see cref="Coordinate"/> instances are approximately equal within a specified tolerance.
    /// </summary>
    /// <param name="other">The other coordinate to compare.</param>
    /// <param name="tolerance">The maximum allowed difference for latitude and longitude.</param>
    /// <returns>
    /// <see langword="true"/> if both latitude and longitude differ by less than <paramref name="tolerance"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(Coordinate other, double tolerance)
        => Math.Abs(Latitude - other.Latitude) < tolerance
        && Math.Abs(Longitude - other.Longitude) < tolerance;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    /// <summary>
    /// Returns a string representation of this coordinate in the format: <c>{ Latitude: [double], Longitude: [double] }</c>.
    /// </summary>
    /// <returns>
    /// A string representation of the coordinate.
    /// </returns>
    public override string ToString() {
        return $"{{ {nameof(Latitude)}: {Latitude.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Longitude)}: {Longitude.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    /// <summary>
    /// Determines whether two <see cref="Coordinate"/> instances are equal.
    /// </summary>
    /// <param name="left">The first coordinate to compare.</param>
    /// <param name="right">The second coordinate to compare.</param>
    /// <returns>
    /// <see langword="true"/> if both coordinates are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Coordinate"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first coordinate to compare.</param>
    /// <param name="right">The second coordinate to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the coordinates are not equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Coordinate left, Coordinate right) => !(left == right);

    /// <summary>
    /// Provides static methods for validating latitude and longitude values used in <see cref="Coordinate"/>.
    /// </summary>
    /// <remarks>
    /// The <c>Validator</c> class ensures that latitude and longitude values are within their valid ranges:
    /// <list type="bullet">
    /// <item>
    /// <description>Latitude must be between -90 and 90 degrees.</description>
    /// </item>
    /// <item>
    /// <description>Longitude must be between -180 and 180 degrees.</description>
    /// </item>
    /// </list>
    /// If a value is out of range or not finite, an exception is thrown via <see cref="ExceptionGuard.ThrowCoordinateValueOutOfRange"/>.
    /// </remarks>
    internal static class Validator {
        /// <summary>
        /// Validates that the specified latitude is within the valid range of -90 to 90 degrees.
        /// </summary>
        /// <param name="latitude">The latitude value to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="latitude"/> is less than -90, greater than 90, or not a finite number.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateLatitude(double latitude) {
            const double min = -90;
            const double max = 90;

            ValidateValue(latitude, min, max, nameof(latitude));
        }

        /// <summary>
        /// Validates that the specified longitude is within the valid range of -180 to 180 degrees.
        /// </summary>
        /// <param name="longitude">The longitude value to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="longitude"/> is less than -180, greater than 180, or not a finite number.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateLongitude(double longitude) {
            const double min = -180;
            const double max = 180;

            ValidateValue(longitude, min, max, nameof(longitude));
        }

        /// <summary>
        /// Validates that the specified value is finite and within the specified range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="min">The minimum allowed value (inclusive).</param>
        /// <param name="max">The maximum allowed value (inclusive).</param>
        /// <param name="paramName">The name of the parameter being validated.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="value"/> is not finite, less than <paramref name="min"/>, or greater than <paramref name="max"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateValue(double value, double min, double max, string paramName) {
            if (!double.IsFinite(value) || value < min || value > max) {
                ExceptionGuard.ThrowCoordinateValueOutOfRange(value, min, max, paramName);
            }
        }

        /// <summary>
        /// Validates both latitude and longitude values for a coordinate.
        /// </summary>
        /// <param name="latitude">The latitude value to validate.</param>
        /// <param name="longitude">The longitude value to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if either <paramref name="latitude"/> or <paramref name="longitude"/> is out of range or not finite.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Validate(double latitude, double longitude) {
            ValidateLatitude(latitude);
            ValidateLongitude(longitude);
        }
    }
}
