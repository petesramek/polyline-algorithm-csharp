//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using System;

public readonly struct Coordinate : IEquatable<Coordinate> {
    private static CoordinateRange ValidLatitude { get; } = new CoordinateRange(-90d, 90d);
    private static CoordinateRange ValidLongitude { get; } = new CoordinateRange(-180d, 180d);

    public static Coordinate Create(double latitude, double longitude) {
        ValidateCoordinate(ref latitude, ref longitude, out var exceptions);

        if (exceptions.Count == 0) {
            return new Coordinate(latitude, longitude);
        } else if (exceptions.Count == 1) {
            throw exceptions[0];
        } else {
            throw new AggregateException(exceptions);
        }

        static void ValidateCoordinate(ref double latitude, ref double longitude, out List<Exception> exceptions) {
            exceptions = new List<Exception>(2);

            if (!ValidLatitude.IsInRange(latitude)) {
                exceptions.Add(new ArgumentException());
            }

            if (!ValidLongitude.IsInRange(longitude)) {
                exceptions.Add(new ArgumentException());
            }
        }
    }

    /// <summary>
    /// Initialized default instance of <see cref="Coordinate"/>.
    /// </summary>
    /// <remarks>Use <see cref="Coordinate.Create(double, double)"/> to initialize non-default instance.</remarks>
    public Coordinate() {
        Latitude = 0d;
        Longitude = 0d;
    }

    private Coordinate(double latitude, double longitude) {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }

    public bool IsDefault
        => Latitude == default
        && Longitude == default;

    public override bool Equals(object? obj)
        => obj is Coordinate coordinate && Equals(coordinate);

    public bool Equals(Coordinate other)
        => Latitude == other.Latitude
            && Longitude == other.Longitude;

    public override int GetHashCode()
        => HashCode.Combine(Latitude, Longitude);

    public static bool operator ==(Coordinate left, Coordinate right)
        => left.Equals(right);

    public static bool operator !=(Coordinate left, Coordinate right)
        => !(left == right);

    private readonly struct CoordinateRange {
        public CoordinateRange(double min, double max) {
            if (min >= max) {
                throw new ArgumentException();
            }

            Min = min;
            Max = max;
        }
        public double Min { get; }
        public double Max { get; }

        public bool IsInRange(double value) => value >= Min && value <= Max;
    }
}
