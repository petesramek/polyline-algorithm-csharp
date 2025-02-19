//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Validation;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a range within coordinate value, latitude or longitude, is considered valid.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
public readonly struct CoordinateRange : IEquatable<CoordinateRange> {
    public CoordinateRange() {
        Min = double.MinValue;
        Max = double.MaxValue;
    }

    public CoordinateRange(double min, double max) {
        if (min >= max) {
            throw new ArgumentOutOfRangeException(nameof(min), string.Format(ExceptionMessageResource.ArgumentMinCannotBeGreaterOfEqualThanMaxArgumentMessageFormat, min, max));
        }

        Min = min;
        Max = max;
    }

    /// <summary>
    /// Represents inclusive minimal value of the range.
    /// </summary>
    public readonly double Min { get; }

    /// <summary>
    /// Represents inclusive maximal value of the range.
    /// </summary>
    public readonly double Max { get; }

    /// <summary>
    /// Indicates whether the <paramref name="value"/> is within the range.
    /// </summary>
    /// <param name="value">A value to be validated is in range.</param>
    /// <returns><see langword="true" /> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool IsInRange(double value) => value >= Min && value <= Max;

    /// <summary>
    /// Returns the formatted string respresentation of this instance.
    /// </summary>
    /// <returns>The formatted string respresentation of this instance.</returns>
    /// <remarks>{ Min: [double], Max: [double] }</remarks>
    [ExcludeFromCodeCoverage]
    public override string ToString() {
        return $"{{ {nameof(Min)}: {Min.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Max)}: {Max.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) {
        return obj is CoordinateRange range && Equals(range);
    }

    /// <inheritdoc />
    public bool Equals(CoordinateRange other) {
        return Min == other.Min &&
               Max == other.Max;
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() {
        return HashCode.Combine(Min, Max);
    }

    [ExcludeFromCodeCoverage]
    public static bool operator ==(CoordinateRange left, CoordinateRange right) {
        return left.Equals(right);
    }

    [ExcludeFromCodeCoverage]
    public static bool operator !=(CoordinateRange left, CoordinateRange right) {
        return !(left == right);
    }
}