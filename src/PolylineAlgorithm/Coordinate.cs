//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Validation;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
/// <summary>
/// Represents a geographic coordinate as a pair of latitude and longitude values.
/// </summary>
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
    /// <param name="latitude">The latitude component, in degrees.</param>
    /// <param name="longitude">The longitude component, in degrees.</param>
    public Coordinate(double latitude, double longitude) {
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

    /// <summary>
    /// Determines whether this coordinate is valid by checking if both <see cref="Latitude"/> and <see cref="Longitude"/>
    /// are within their respective valid ranges as defined by the default <see cref="ICoordinateValidator"/>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the coordinate is within valid latitude and longitude ranges; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid() => ICoordinateValidator.Default.IsValid(this);

    #region Overrides

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) => obj is Coordinate coordinate && Equals(coordinate);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    /// <summary>
    /// Returns a string representation of this coordinate in the format: <c>{ Latitude: [double], Longitude: [double] }</c>.
    /// </summary>
    /// <returns>
    /// A string representation of the coordinate.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public override string ToString() {
        return $"{{ {nameof(Latitude)}: {Latitude.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Longitude)}: {Longitude.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    #endregion

    #region IEquatable<Coordinate> implementation

    /// <summary>
    /// Indicates whether this coordinate is equal to another <see cref="Coordinate"/> instance.
    /// </summary>
    /// <param name="other">The coordinate to compare with this instance.</param>
    /// <returns>
    /// <see langword="true"/> if both latitude and longitude are equal; otherwise, <see langword="false"/>.
    /// </returns>
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
    [ExcludeFromCodeCoverage]
    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Coordinate"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first coordinate to compare.</param>
    /// <param name="right">The second coordinate to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the coordinates are not equal; otherwise, <see langword="false"/>.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Coordinate left, Coordinate right) => !(left == right);

    #endregion
}
