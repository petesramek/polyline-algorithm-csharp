//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
public readonly struct CoordinateRange : IEquatable<CoordinateRange> {
    public CoordinateRange(double min, double max) {
        if (min >= max) {
            throw new ArgumentException();
        }

        Min = min;
        Max = max;
    }
    public readonly double Min { get; }
    public readonly double Max { get; }

    public override bool Equals(object? obj) {
        return obj is CoordinateRange range && Equals(range);
    }

    public bool Equals(CoordinateRange other) {
        return Min == other.Min &&
               Max == other.Max;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Min, Max);
    }

    public bool IsInRange(double value) => value >= Min && value <= Max;

    public static bool operator ==(CoordinateRange left, CoordinateRange right) {
        return left.Equals(right);
    }

    public static bool operator !=(CoordinateRange left, CoordinateRange right) {
        return !(left == right);
    }
}
