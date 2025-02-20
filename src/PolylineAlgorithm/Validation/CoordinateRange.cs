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
    /// <summary>
    /// Creates a new <see cref="CoordinateRange"/> structure that contains <see cref="Min"/>
    /// set to <see cref="double.MinValue"/> and <see cref="Max"/> to <see cref="double.MaxValue"/>.
    /// </summary>
    public CoordinateRange() {
        Min = double.MinValue;
        Max = double.MaxValue;
    }

    /// <summary>
    /// Creates a new <see cref="CoordinateRange"/> structure that contains <see cref="Min"/> and <see cref="Max"/> set to specified values.
    /// </summary>
    /// <param name="min">A minimal allowed value.</param>
    /// <param name="max">A maximal allowed value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="min" /> argument is greater or equal to <paramref name="max" /> argument.</exception>
    public CoordinateRange(double min, double max) {
        if (min >= max) {
            throw new ArgumentOutOfRangeException(nameof(min), string.Format(ExceptionMessageResource.ArgumentMinCannotBeGreaterOfEqualThanMaxArgumentMessageFormat, min, max));
        }

        Min = min;
        Max = max;
    }

    /// <summary>
    /// Gets minimal allowed value.
    /// </summary>
    public readonly double Min { get; }

    /// <summary>
    /// Gets maximal allowed value.
    /// </summary>
    public readonly double Max { get; }

    /// <summary>
    /// Returns a value indicating whether <see cref="double" /> value is within the allowed range.
    /// </summary>
    /// <param name="value">The value to be evaluated.</param>
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

    /// <summary>
    /// Indicates whether the values of two specified <see cref="CoordinateRange" /> objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator ==(CoordinateRange left, CoordinateRange right) {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates whether the values of two specified <see cref="CoordinateRange" /> objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(CoordinateRange left, CoordinateRange right) {
        return !(left == right);
    }
}