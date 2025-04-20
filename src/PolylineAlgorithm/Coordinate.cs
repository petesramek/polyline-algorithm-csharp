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
/// Represents a latitude and longitude coordinate pair.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
public readonly struct Coordinate : IEquatable<Coordinate> {
    public static readonly Coordinate Default;
    internal static long Size { get; } = Marshal.SizeOf<Coordinate>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct with default values for <see cref="Latitude"/> and <see cref="Longitude"/>.
    /// </summary>
    public Coordinate() {
        Latitude = default;
        Longitude = default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct with specified latitude and longitude values.
    /// </summary>
    /// <param name="latitude">The latitude value, in degrees.</param>
    /// <param name="longitude">The longitude value, in degrees.</param>
    public Coordinate(double latitude, double longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets the latitude value of the coordinate, in degrees.
    /// </summary>
    public double Latitude { get; }

    /// <summary>
    /// Gets the longitude value of the coordinate, in degrees.
    /// </summary>
    public double Longitude { get; }

    /// <summary>
    /// Determines whether the coordinate is the default value (both <see cref="Latitude"/> and <see cref="Longitude"/> are 0).
    /// </summary>
    /// <returns><see langword="true"/> if the coordinate is the default value; otherwise, <see langword="false"/>.</returns>
    public bool IsDefault()
        => Latitude == default
        && Longitude == default;

    /// <summary>
    /// Determines whether the coordinate is valid by checking if both <see cref="Latitude"/> and <see cref="Longitude"/> are within their respective valid ranges.
    /// </summary>
    /// <returns><see langword="true"/> if the coordinate is valid; otherwise, <see langword="false"/>.</returns>
    public bool IsValid()
        => ICoordinateValidator.Default.Latitude.IsInRange(Latitude)
        && ICoordinateValidator.Default.Longitude.IsInRange(Longitude);

    #region Overrides

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) => obj is Coordinate coordinate && Equals(coordinate);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    /// <summary>
    /// Returns a string representation of the coordinate in the format: { Latitude: [double], Longitude: [double] }.
    /// </summary>
    /// <returns>A string representation of the coordinate.</returns>
    [ExcludeFromCodeCoverage]
    public override string ToString() {
        return $"{{ {nameof(Latitude)}: {Latitude.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Longitude)}: {Longitude.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    #endregion

    #region IEquatable<Coordinate> implementation

    /// <summary>
    /// Determines whether the current coordinate is equal to another coordinate.
    /// </summary>
    /// <param name="other">The coordinate to compare with the current coordinate.</param>
    /// <returns><see langword="true"/> if the coordinates are equal; otherwise, <see langword="false"/>.</returns>
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
    /// <returns><see langword="true"/> if the coordinates are equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Coordinate"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first coordinate to compare.</param>
    /// <param name="right">The second coordinate to compare.</param>
    /// <returns><see langword="true"/> if the coordinates are not equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Coordinate left, Coordinate right) => !(left == right);

    #endregion
}
