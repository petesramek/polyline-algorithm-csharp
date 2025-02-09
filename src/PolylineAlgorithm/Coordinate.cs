//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Validation;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
[DebuggerDisplay("Latitude: {Latitude}, Longitude: {Longitude}")]
public readonly struct Coordinate : IEquatable<Coordinate> {
    /// <summary>
    /// Initializes default instance of <see cref="Coordinate"/> with latitude and longitude equal to 0.
    /// </summary>
    public Coordinate() {
        Latitude = 0d;
        Longitude = 0d;
    }

    /// <summary>
    /// Initializes instance of <see cref="Coordinate"/> with <paramref name="latitude"/> and <paramref name="longitude"/> values.
    /// </summary>
    /// <param name="latitude">A latitude value.</param>
    /// <param name="longitude">A latitude value.</param>
    public Coordinate(double latitude, double longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    public readonly double Latitude { get; }

    public readonly double Longitude { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDefault
        => Latitude == default
        && Longitude == default;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Uses <see cref="ICoordinateValidator.Default"/>.
    /// </remarks>
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

    #endregion

    #region IEquatable<Coordinate> implementation

    /// <inheritdoc />
    public bool Equals(Coordinate other) {
        return Latitude == other.Latitude &&
               Longitude == other.Longitude;
    }

    #endregion

    #region Equality operators

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public static bool operator ==(Coordinate left, Coordinate right) {
        return left.Equals(right);
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Coordinate left, Coordinate right) {
        return !(left == right);
    }

    #endregion
}
