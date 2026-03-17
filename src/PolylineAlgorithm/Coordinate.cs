//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
#if NET8_0_OR_GREATER
using System.Text;
#endif

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
public readonly struct Coordinate : IEquatable<Coordinate>
{
#if NET8_0_OR_GREATER
    private static readonly CompositeFormat _coordinateValueMustBeBetweenValuesMessageFormat = CompositeFormat.Parse(ExceptionMessageResource.CoordinateValueMustBeBetweenValuesMessageFormat);
#else
    private static readonly string _coordinateValueMustBeBetweenValuesMessageFormat = ExceptionMessageResource.CoordinateValueMustBeBetweenValuesMessageFormat;
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct with default values (0) for <see cref="Latitude"/> and <see cref="Longitude"/>.
    /// </summary>
    /// <remarks>
    /// The default value (0, 0) is a valid coordinate (Gulf of Guinea), but is also treated as the "default" value by <see cref="IsDefault"/>.
    /// </remarks>
    public Coordinate()
    {
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
    public Coordinate(double latitude, double longitude)
    {
        Validator.ValidateLatitude(latitude);
        Validator.ValidateLongitude(longitude);

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
    public bool Equals(Coordinate other)
    {
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
    public override string ToString()
    {
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
    /// Provides validation methods for latitude and longitude values used in <see cref="Coordinate"/>.
    /// </summary>
    internal static class Validator
    {
        /// <summary>
        /// Validates that the specified latitude is within the valid range of -90 to 90 degrees and is a finite value.
        /// </summary>
        /// <param name="latitude">The latitude value to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="latitude"/> is less than -90, greater than 90, or not a finite value.
        /// </exception>
        internal static void ValidateLatitude(double latitude)
        {
            if (latitude < -90 || latitude > 90 || !double.IsFinite(latitude))
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), string.Format(CultureInfo.InvariantCulture, _coordinateValueMustBeBetweenValuesMessageFormat, "Latitude", -90, 90));
            }
        }

        /// <summary>
        /// Validates that the specified longitude is within the valid range of -180 to 180 degrees and is a finite value.
        /// </summary>
        /// <param name="longitude">The longitude value to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="longitude"/> is less than -180, greater than 180, or not a finite value.
        /// </exception>
        internal static void ValidateLongitude(double longitude)
        {
            if (longitude < -180 || longitude > 180 || !double.IsFinite(longitude))
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), string.Format(CultureInfo.InvariantCulture, _coordinateValueMustBeBetweenValuesMessageFormat, "Longitude", -180, 180));
            }
        }
    }
}
