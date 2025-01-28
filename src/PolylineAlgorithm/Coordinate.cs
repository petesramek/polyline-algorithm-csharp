//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

[DebuggerDisplay(@"Latitude: {Latitude}, Longitude: {Longitude}")]
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
public readonly struct Coordinate : IEquatable<Coordinate> {
    /// <summary>
    /// Initialized default instance of <see cref="Coordinate"/>.
    /// </summary>
    public Coordinate() {
        Latitude = 0d;
        Longitude = 0d;
    }

    public Coordinate(double latitude, double longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    public readonly double Latitude { get; }
    public readonly double Longitude { get; }

    public bool IsDefault
        => Latitude == default
        && Longitude == default;

    public bool IsValid
        => CoordinateValidator.Latitude.IsInRange(Latitude)
        && CoordinateValidator.Longitude.IsInRange(Longitude);


    public override bool Equals(object? obj) {
        return obj is Coordinate coordinate && Equals(coordinate);
    }

    public bool Equals(Coordinate other) {
        return Latitude == other.Latitude &&
               Longitude == other.Longitude;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Latitude, Longitude);
    }

    public static bool operator ==(Coordinate left, Coordinate right) {
        return left.Equals(right);
    }

    public static bool operator !=(Coordinate left, Coordinate right) {
        return !(left == right);
    }
}
