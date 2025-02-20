//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

/// <summary>
/// Represents a readonly encoded polyline string.
/// </summary>
[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay("Value: {ToString()}, IsEmpty: {IsEmpty}, Length: {Length}")]
public readonly struct Polyline : IEquatable<Polyline> {
    private readonly ReadOnlyMemory<char> _value;

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that is empty.
    /// </summary>
    public Polyline() {
        _value = ReadOnlyMemory<char>.Empty;
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified string value.
    /// </summary>
    /// <param name="value">The string value to initialize the polyline with.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public Polyline(string value) {
        _value = value?.AsMemory() ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified Unicode character array.
    /// </summary>
    /// <param name="value">The Unicode character array to initialize the polyline with.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public Polyline(char[] value) {
        _value = value?.AsMemory() ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified readonly memory region.
    /// </summary>
    /// <param name="value">The readonly memory region to initialize the polyline with.</param>
    public Polyline(ReadOnlyMemory<char> value) {
        _value = value;
    }

    /// <summary>
    /// Gets the span of characters in the polyline.
    /// </summary>
    internal readonly ReadOnlySpan<char> Span => _value.Span;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline" /> is empty.
    /// </summary>
    public readonly bool IsEmpty => _value.IsEmpty;

    /// <summary>
    /// Gets the number of characters in the current <see cref="Polyline" /> object.
    /// </summary>
    public readonly int Length => _value.Length;

    /// <summary>
    /// Copies the characters in this instance to a Unicode character array.
    /// </summary>
    /// <returns>A Unicode character array.</returns>
    public char[] ToCharArray() => _value.ToArray();

    /// <summary>
    /// Returns the underlying <see cref="ReadOnlyMemory{T}" /> this instance represents.
    /// </summary>
    /// <returns>The underlying <see cref="ReadOnlyMemory{T}"/>.</returns>
    public ReadOnlyMemory<char> AsMemory() => _value;

    /// <summary>
    /// Returns a string representation of the value of this instance.
    /// </summary>
    /// <returns>The string value of this <see cref="Polyline"/> object.</returns>
    public override string ToString() => _value.ToString();

    #region Overrides

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj) => obj is Polyline polyline && Equals(polyline);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(_value);

    #endregion

    #region IEquatable<Polyline> implementation

    /// <inheritdoc />
    public bool Equals(Polyline other) => _value.Span.SequenceEqual(other._value.Span);

    #endregion

    #region Equality operators

    /// <summary>
    /// Indicates whether the values of two specified <see cref="Polyline" /> objects are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator ==(Polyline left, Polyline right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the values of two specified <see cref="Polyline" /> objects are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    [ExcludeFromCodeCoverage]
    public static bool operator !=(Polyline left, Polyline right) => !(left == right);

    #endregion

    #region Factory methods

    /// <summary>
    /// Creates an instance of the current type from a Unicode character array.
    /// </summary>
    /// <param name="polyline">A Unicode character array representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified Unicode character array.</returns>
    public static Polyline FromCharArray(char[] polyline) => new(polyline);

    /// <summary>
    /// Creates an instance of the current type from a readonly memory region.
    /// </summary>
    /// <param name="polyline">A readonly memory region representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified readonly memory region.</returns>
    public static Polyline FromMemory(ReadOnlyMemory<char> polyline) => new(polyline);

    /// <summary>
    /// Creates an instance of the current type from a string.
    /// </summary>
    /// <param name="polyline">A string representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified string value.</returns>
    public static Polyline FromString(string polyline) => new(polyline);

    #endregion

    #region Explicit conversions

    /// <summary>
    /// Defines an explicit conversion of a Unicode character array to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The Unicode character array to convert.</param>
    /// <returns>The converted Unicode character array.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(char[] polyline) => FromCharArray(polyline);

    /// <summary>
    /// Defines an explicit conversion of a readonly memory region to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The readonly memory region to convert.</param>
    /// <returns>The converted readonly memory region.</returns>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = $"Provided alternative {nameof(Polyline)}.{nameof(FromMemory)} to follow {nameof(String)}.{nameof(AsMemory)} naming pattern.")]
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(ReadOnlyMemory<char> polyline) => FromMemory(polyline);

    /// <summary>
    /// Defines an explicit conversion of a string to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The string to convert.</param>
    /// <returns>The converted string.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(string polyline) => FromString(polyline);

    #endregion
}

