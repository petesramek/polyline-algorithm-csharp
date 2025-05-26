//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

/// <summary>
/// Represents an inclusive range of valid values for a coordinate, such as latitude or longitude.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
internal readonly struct CoordinateRange : IEquatable<CoordinateRange> {
    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateRange"/> struct with the minimum and maximum values set to <see cref="double.MinValue"/> and <see cref="double.MaxValue"/>, respectively.
    /// </summary>
    public CoordinateRange() {
        Min = double.MinValue;
        Max = double.MaxValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateRange"/> struct with the specified minimum and maximum values.
    /// </summary>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="min"/> is greater than or equal to <paramref name="max"/>.
    /// </exception>
    public CoordinateRange(double min, double max) {
        if (min >= max) {
            throw new ArgumentOutOfRangeException(nameof(min), string.Format(ExceptionMessageResource.ArgumentMinCannotBeGreaterOfEqualThanMaxArgumentMessageFormat, min, max));
        }

        Min = min;
        Max = max;
    }

    /// <summary>
    /// Gets the inclusive lower bound of the range.
    /// </summary>
    public readonly double Min { get; }

    /// <summary>
    /// Gets the inclusive upper bound of the range.
    /// </summary>
    public readonly double Max { get; }

    /// <summary>
    /// Determines whether the specified value falls within the range, inclusive of the minimum and maximum bounds.
    /// </summary>
    /// <param name="value">The value to test for inclusion in the range.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than or equal to <see cref="Min"/> and less than or equal to <see cref="Max"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsInRange(double value) => value >= Min && value <= Max;

    /// <summary>
    /// Returns a string that represents the current <see cref="CoordinateRange"/> instance.
    /// </summary>
    /// <returns>
    /// A string in the format <c>{ Min: [double], Max: [double] }</c> representing the range.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public override string ToString() {
        return $"{{ {nameof(Min)}: {Min.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Max)}: {Max.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) => obj is CoordinateRange range && Equals(range);

    /// <summary>
    /// Indicates whether the current <see cref="CoordinateRange"/> is equal to another <see cref="CoordinateRange"/> instance.
    /// </summary>
    /// <param name="other">The other <see cref="CoordinateRange"/> to compare with this instance.</param>
    /// <returns>
    /// <see langword="true"/> if both <see cref="Min"/> and <see cref="Max"/> are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(CoordinateRange other) {
        return Min == other.Min && Max == other.Max;
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Min, Max);

    /// <summary>
    /// Determines whether two <see cref="CoordinateRange"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="CoordinateRange"/> to compare.</param>
    /// <param name="right">The second <see cref="CoordinateRange"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if both instances have equal <see cref="Min"/> and <see cref="Max"/> values; otherwise, <see langword="false"/>.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public static bool operator ==(CoordinateRange left, CoordinateRange right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="CoordinateRange"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="CoordinateRange"/> to compare.</param>
    /// <param name="right">The second <see cref="CoordinateRange"/> to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the instances differ in either <see cref="Min"/> or <see cref="Max"/>; otherwise, <see langword="false"/>.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(CoordinateRange left, CoordinateRange right) => !(left == right);
}
