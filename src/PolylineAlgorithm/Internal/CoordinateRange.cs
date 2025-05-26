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
/// Represents a range within which a coordinate value, either latitude or longitude, is considered valid.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential, Pack = 8, Size = 16)]
internal readonly struct CoordinateRange : IEquatable<CoordinateRange> {
    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateRange"/> struct with <see cref="Min"/>
    /// set to <see cref="double.MinValue"/> and <see cref="Max"/> set to <see cref="double.MaxValue"/>.
    /// </summary>
    public CoordinateRange() {
        Min = double.MinValue;
        Max = double.MaxValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinateRange"/> struct with specified minimum and maximum values.
    /// </summary>
    /// <param name="min">The minimum allowed value for the range.</param>
    /// <param name="max">The maximum allowed value for the range.</param>
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
    /// Gets the minimum allowed value for the range.
    /// </summary>
    public readonly double Min { get; }

    /// <summary>
    /// Gets the maximum allowed value for the range.
    /// </summary>
    public readonly double Max { get; }

    /// <summary>
    /// Determines whether a specified value is within the range.
    /// </summary>
    /// <param name="value">The value to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than or equal to <see cref="Min"/> 
    /// and less than or equal to <see cref="Max"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsInRange(double value) => value >= Min && value <= Max;

    /// <summary>
    /// Returns a string representation of this instance.
    /// </summary>
    /// <returns>
    /// A string representation of this instance in the format:
    /// <c>{ Min: [double], Max: [double] }</c>.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public override string ToString() {
        return $"{{ {nameof(Min)}: {Min.ToString("G", CultureInfo.InvariantCulture)}, {nameof(Max)}: {Max.ToString("G", CultureInfo.InvariantCulture)} }}";
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) => obj is CoordinateRange range && Equals(range);

    /// <summary>
    /// Determines whether the current <see cref="CoordinateRange"/> instance is equal to another instance.
    /// </summary>
    /// <param name="other">The other <see cref="CoordinateRange"/> instance to compare with.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="Min"/> and <see cref="Max"/> values of both instances are equal;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(CoordinateRange other) {
        return Min == other.Min && Max == other.Max;
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Min, Max);

    /// <summary>
    /// Determines whether two specified <see cref="CoordinateRange"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public static bool operator ==(CoordinateRange left, CoordinateRange right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified <see cref="CoordinateRange"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(CoordinateRange left, CoordinateRange right) => !(left == right);
}
