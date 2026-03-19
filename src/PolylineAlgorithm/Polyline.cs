//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal.Diagnostics;
using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

/// <summary>
/// Represents an immutable, read-only encoded polyline string.
/// Provides methods for creation, inspection, and conversion of polyline data from various character sources.
/// </summary>
/// <remarks>
/// This struct is designed to be lightweight and efficient, allowing for quick comparisons and memory-safe operations.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay("Value: {ToDebugString()}, IsEmpty: {IsEmpty}, Length: {Length}")]
public readonly struct Polyline : IEquatable<Polyline> {
    /// <summary>
    /// Initializes a new, empty instance of the <see cref="Polyline"/> struct.
    /// </summary>
    public Polyline() {
        Value = ReadOnlyMemory<char>.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Polyline"/> struct with the specified character sequence.
    /// </summary>
    /// <param name="value">
    /// A read-only memory region of characters representing an encoded polyline.
    /// </param>
    private Polyline(ReadOnlyMemory<char> value) {
        Value = value;
    }

    /// <summary>
    /// Gets the underlying read-only sequence of characters representing the polyline.
    /// </summary>
    internal readonly ReadOnlyMemory<char> Value { get; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline"/> is empty.
    /// </summary>
    public readonly bool IsEmpty => Value.IsEmpty;

    /// <summary>
    /// Gets the length of the polyline in characters as an <see cref="int"/>.
    /// </summary>
    public readonly int Length => Value.Length;

    /// <summary>
    /// Copies the characters of this polyline to the specified destination array.
    /// </summary>
    /// <param name="destination">
    /// The destination array to copy the characters to.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="destination"/> is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the length of <paramref name="destination"/> does not match the polyline's length.
    /// </exception>
    public void CopyTo(char[] destination) {
        if (destination is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(destination));
        }

        if (destination.Length < Length) {
            ExceptionGuard.ThrowDestinationArrayLengthMustBeEqualOrGreaterThanPolylineLength(destination.Length, Length, nameof(destination));
        }

        Value.CopyTo(destination);
    }

    /// <summary>
    /// Returns a string representation of the polyline.
    /// </summary>
    /// <returns>
    /// A string containing the characters of the polyline, or an empty string if the polyline is empty.
    /// </returns>
    public override string ToString() {
        if (IsEmpty) {
            return string.Empty;
        }

        return new string(Value.Span);
    }

    /// <summary>
    /// Returns a debug-friendly string representation of the polyline.
    /// </summary>
    /// <returns>
    /// A string that includes the polyline value, truncated if necessary, for debugging purposes.
    /// </returns>
    [ExcludeFromCodeCoverage(
#if NET5_0_OR_GREATER
        Justification = "Only used for debugging."
#endif
    )]
    private string ToDebugString() {
        if (IsEmpty) {
            return string.Empty;
        }

        var span = Value.Span;
        if (span.Length <= 32)
            return $"\"{new string(span)}\"";
        return $"\"{new string(span.Slice(0, 10))}...{new string(span.Slice(span.Length - 10))}\"";
    }

    /// <summary>
    /// Determines whether the current <see cref="Polyline"/> instance is equal to another <see cref="Polyline"/> instance.
    /// </summary>
    /// <param name="other">The other <see cref="Polyline"/> to compare with.</param>
    /// <returns>
    /// <see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(Polyline other) {
        if ((IsEmpty != other.IsEmpty) || (Length != other.Length)) {
            return false;
        }

        return Value.Span.SequenceEqual(other.Value.Span);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        return obj is Polyline other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() {
        // Content-based hash code for value equality
        var span = Value.Span;
        if (span.IsEmpty)
            return 0;
        unchecked {
            int hash = 17;
            for (int i = 0; i < span.Length; i++)
                hash = hash * 31 + span[i];
            return hash;
        }
    }

    /// <summary>
    /// Determines whether two <see cref="Polyline"/> instances are equal.
    /// </summary>
    /// <param name="left">
    /// The first polyline to compare.
    /// </param>
    /// <param name="right">
    /// The second polyline to compare.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the polylines are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Polyline left, Polyline right) {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="Polyline"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first polyline to compare.</param>
    /// <param name="right">The second polyline to compare.</param>
    /// <returns><see langword="true"/> if the polylines are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Polyline left, Polyline right) {
        return !(left == right);
    }

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a Unicode character array.
    /// </summary>
    /// <param name="polyline">
    /// A character array representing an encoded polyline.
    /// </param>
    /// <returns>
    /// The <see cref="Polyline"/> instance corresponding to the specified character array.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    public static Polyline FromCharArray(char[] polyline) {
        if (polyline is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(polyline));
        }

        return FromMemory(polyline.AsMemory());
    }

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a string.
    /// </summary>
    /// <param name="polyline">
    /// A string representing an encoded polyline.
    /// </param>
    /// <returns>
    /// The <see cref="Polyline"/> instance corresponding to the specified string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="polyline"/> is <see langword="null"/>.
    /// </exception>
    public static Polyline FromString(string polyline) {
        if (polyline is null) {
            ExceptionGuard.ThrowArgumentNull(nameof(polyline));
        }

        return FromMemory(polyline.AsMemory());
    }

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a read-only memory region of characters.
    /// </summary>
    /// <param name="polyline">
    /// A read-only memory region representing an encoded polyline.
    /// </param>
    /// <returns>
    /// The <see cref="Polyline"/> instance corresponding to the specified memory region.
    /// </returns>
    public static Polyline FromMemory(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return new();
        }

        return new Polyline(polyline);
    }
}
