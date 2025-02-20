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
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Coordinate : IEquatable<Coordinate> {
    /// <summary>
    /// Creates a new <see cref="Coordinate"/> structure that contains <see cref="Latitude" /> and <see cref="Longitude" /> set to <see langword="default"/> value.
    /// </summary>
    public Coordinate() {
        Latitude = default;
        Longitude = default;
    }

    /// <summary>
    /// Creates a new <see cref="Coordinate"/> structure that contains <see cref="Latitude" /> and <see cref="Longitude" /> set to specified values.
    /// </summary>
    /// <param name="latitude">A latitude value.</param>
    /// <param name="longitude">A latitude value.</param>
    public Coordinate(double latitude, double longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets the latitude value as a double.
    /// </summary>
    public readonly double Latitude { get; }

    /// <summary>
    /// Gets the longitude value as a double.
    /// </summary>
    public readonly double Longitude { get; }

    /// <summary>
    /// Gets a value that indicates whether both, the <see cref="Latitude" /> and <see cref="Longitude"/> values, are equal to <see langword="default" />.
    /// </summary>
    public bool IsDefault
        => Latitude == default
        && Longitude == default;

    /// <summary>
    /// Gets a value that indicates whether both, the <see cref="Latitude" /> and <see cref="Longitude"/> values, are in the valid range.
    /// </summary>
    public bool IsValid
        => ICoordinateValidator.Default.Latitude.IsInRange(Latitude)
        && ICoordinateValidator.Default.Longitude.IsInRange(Longitude);

    #region Overrides

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) {
        return obj is Coordinate coordinate && Equals(coordinate);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() {
        return HashCode.Combine(Latitude, Longitude);
    }

    /// <summary>
    /// Returns the formatted string respresentation of this instance.
    /// </summary>
    /// <returns>The formatted string respresentation of this instance.</returns>
    /// <remarks>{ Latitude: [double], Longitude: [double] }</remarks>
    [ExcludeFromCodeCoverage]
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
    /// Indicates whether the values of two specified <see cref="Coordinate" /> objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>

    [ExcludeFromCodeCoverage]
    public static bool operator ==(Coordinate left, Coordinate right) {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates whether the values of two specified <see cref="CoordinateRange" /> objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Coordinate left, Coordinate right) {
        return !(left == right);
    }

    #endregion
}