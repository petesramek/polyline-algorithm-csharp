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
    public static readonly Coordinate Default = new();
    internal static long Size { get; } = Marshal.SizeOf<Coordinate>();

    /// <summary>
    /// Creates a new <see cref="Coordinate"/> structure with <see cref="Latitude"/> and <see cref="Longitude"/> set to their default values.
    /// </summary>
    public Coordinate() {
        Latitude = default;
        Longitude = default;
    }

    /// <summary>
    /// Creates a new <see cref="Coordinate"/> structure with specified latitude and longitude values.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    public Coordinate(double latitude, double longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Gets the latitude value as a double.
    /// </summary>
    public double Latitude { get; }

    /// <summary>
    /// Gets the longitude value as a double.
    /// </summary>
    public double Longitude { get; }

    /// <summary>
    /// Gets a value indicating whether both the <see cref="Latitude"/> and <see cref="Longitude"/> values are equal to their default values.
    /// </summary>
    public bool IsDefault()
        => Latitude == default
        && Longitude == default;

    /// <summary>
    /// Gets a value indicating whether both the <see cref="Latitude"/> and <see cref="Longitude"/> values are within the valid range.
    /// </summary>
    public bool IsValid()
        => ICoordinateValidator.Default.Latitude.IsInRange(Latitude)
        && ICoordinateValidator.Default.Longitude.IsInRange(Longitude);

    /// <summary>
    /// Deconstructs this instance into its latitude and longitude components.
    /// </summary>
    /// <param name="latitude">The latitude component.</param>
    /// <param name="longitude">The longitude component.</param>
    public readonly void Deconstruct(out double latitude, out double longitude) {
        latitude = Latitude;
        longitude = Longitude;
    }

    #region Overrides

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) => obj is Coordinate coordinate && Equals(coordinate);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    /// <summary>
    /// Returns the formatted string representation of this instance.
    /// </summary>
    /// <returns>The formatted string representation of this instance.</returns>
    /// <remarks>The format is: { Latitude: [double], Longitude: [double] }</remarks>
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
    /// Indicates whether the values of two specified <see cref="Coordinate"/> objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the values of two specified <see cref="Coordinate"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Coordinate left, Coordinate right) => !(left == right);

    #endregion
}