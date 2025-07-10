//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a geographic coordinate as a pair of latitude and longitude values.
/// </summary>
/// <remarks>
/// This struct is designed to be immutable and lightweight, providing a simple way to represent
/// geographic coordinates in degrees. It includes validation for latitude and longitude ranges
/// and provides methods for equality comparison and string representation.
/// </remarks>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
public readonly struct Coordinate : IEquatable<Coordinate> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct with default values (0) for <see cref="Latitude"/> and <see cref="Longitude"/>.
    /// </summary>
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
        if (latitude < -90 || latitude > 90) {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");
        }

        if (longitude < -180 || longitude > 180) {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");
        }

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
    /// <returns>
    /// <see langword="true"/> if both latitude and longitude are 0; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsDefault()
        => Latitude == default
        && Longitude == default;

    #region Overrides

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Coordinate coordinate && Equals(coordinate);

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

    #endregion

    #region IEquatable<Coordinate> implementation

    /// <inheritdoc />
    public bool Equals(Coordinate other) {
        return Latitude == other.Latitude &&
               Longitude == other.Longitude;
    }

    #endregion

    #region Equality operators

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

    #endregion
}
