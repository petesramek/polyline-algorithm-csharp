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
/// Represents a latitude and longitude pair.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Coordinate : IEquatable<Coordinate>
{
    /// <summary>
    /// Initializes default instance of <see cref="Coordinate"/> with <see cref="Latitude" /> and <see cref="Longitude" /> equal to 0.
    /// </summary>
    public Coordinate()
    {
        Latitude = 0d;
        Longitude = 0d;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Coordinate"/> with <paramref name="latitude"/> and <paramref name="longitude"/> values.
    /// </summary>
    /// <param name="latitude">A latitude value.</param>
    /// <param name="longitude">A latitude value.</param>
    public Coordinate(double latitude, double longitude)
    {
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
    /// Gets a value that indicates whether both, the <see cref="Latitude" /> and <see cref="Longitude"/> values, are equal to 0.
    /// </summary>
    public bool IsDefault
        => Latitude == default
        && Longitude == default;

    /// <summary>
    /// Gets a value that indicates whether both, the <see cref="Latitude" /> and <see cref="Longitude"/> values, are in valid range.
    /// </summary>
    public bool IsValid
        => ICoordinateValidator.Default.Latitude.IsInRange(Latitude)
        && ICoordinateValidator.Default.Longitude.IsInRange(Longitude);

    #region Overrides

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj)
    {
        return obj is Coordinate coordinate && Equals(coordinate);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }

    /// <summary>
    /// Returns the formatted string respresentation of this instance.
    /// </summary>
    /// <returns>The formatted string respresentation of this instance.</returns>
    /// <remarks>{ Latitude: [double], Longitude: [double] }</remarks>
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        return $"{{ {nameof(Latitude)}: {Latitude.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Longitude)}: {Longitude.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    #endregion

    #region IEquatable<Coordinate> implementation

    /// <inheritdoc />
    public bool Equals(Coordinate other)
    {
        return Latitude == other.Latitude &&
               Longitude == other.Longitude;
    }

    #endregion

    #region Equality operators

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public static bool operator ==(Coordinate left, Coordinate right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Coordinate left, Coordinate right)
    {
        return !(left == right);
    }

    #endregion
}
