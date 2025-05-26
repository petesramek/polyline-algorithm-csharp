//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Represents a readonly encoded polyline string.
/// </summary>
[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay("Value: {ToString()}, IsEmpty: {IsEmpty}, Length: {Length}")]
public readonly struct Polyline : IEquatable<Polyline> {
    private readonly ReadOnlySequence<char> _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Polyline"/> struct that is empty.
    /// </summary>
    public Polyline() {
        _value = ReadOnlySequence<char>.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Polyline"/> struct with the specified character sequence.
    /// </summary>
    /// <param name="value">The readonly sequence of characters to initialize the polyline with.</param>
    private Polyline(ReadOnlySequence<char> value) {
        _value = value;
    }

    /// <summary>
    /// Gets the readonly sequence of characters representing the polyline.
    /// </summary>
    internal readonly ReadOnlySequence<char> Value => _value;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline"/> is empty.
    /// </summary>
    public readonly bool IsEmpty => Value.IsEmpty;

    /// <summary>
    /// Gets the length of the polyline in characters.
    /// </summary>
    public readonly long Length => Value.Length;

    /// <summary>
    /// Returns an enumerator for iterating through the characters in the polyline.
    /// </summary>
    /// <returns>An enumerator for the polyline's character sequence.</returns>
    public ReadOnlySequence<char>.Enumerator GetEnumerator() {
        return _value.GetEnumerator();
    }

    /// <summary>
    /// Copies the characters in this instance to the specified destination array.
    /// </summary>
    /// <param name="destination">The destination array to copy the characters to.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="destination"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the length of <paramref name="destination"/> does not match the polyline's length.</exception>
    public void CopyTo(char[] destination) {
        if (destination is null) {
            throw new ArgumentNullException(nameof(destination));
        }

        if (Length != destination.Length) {
            throw new ArgumentException("Destination array length must match the polyline's length.", nameof(destination));
        }

        _value.CopyTo(destination);
    }

    /// <summary>
    /// Returns a string representation of the polyline.
    /// </summary>
    /// <returns>A string containing the characters of the polyline.</returns>
    public override string ToString() {
        if (IsEmpty) {
            return string.Empty;
        }

        if (Value.IsSingleSegment) {
            return Value.FirstSpan.ToString();
        }

        var sb = Value.Length <= int.MaxValue ? new StringBuilder((int)Value.Length) : new StringBuilder();
        var enumerator = Value.GetEnumerator();

        while (true) {
            if (!enumerator.MoveNext()) {
                break;
            }

            if (enumerator.Current.IsEmpty) {
                continue;
            }

            sb.Append(enumerator.Current.Span);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Determines whether the current polyline is equal to another polyline.
    /// </summary>
    /// <param name="other">The polyline to compare with the current polyline.</param>
    /// <returns><see langword="true"/> if the polylines are equal; otherwise, <see langword="false"/>.</returns>
    public bool Equals(Polyline other) {
        if ((IsEmpty != other.IsEmpty) || (Length != other.Length)) {
            return false;
        }

        var enumerator = GetEnumerator();
        var reader = new SequenceReader<char>(other.Value);

        while (enumerator.MoveNext()) {
            if (!reader.IsNext(enumerator.Current.Span, true)) {
                return false;
            }
        }

        return reader.Remaining == 0;
    }

    /// <inheritdoc />
    public override bool Equals(object obj) {
        return obj is Polyline other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Determines whether two <see cref="Polyline"/> instances are equal.
    /// </summary>
    /// <param name="left">The first polyline to compare.</param>
    /// <param name="right">The second polyline to compare.</param>
    /// <returns><see langword="true"/> if the polylines are equal; otherwise, <see langword="false"/>.</returns>
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

    #region Factory methods

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a Unicode character array.
    /// </summary>
    /// <param name="polyline">A Unicode character array representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> instance corresponding to the specified character array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="polyline"/> is null.</exception>
    public static Polyline FromCharArray(char[] polyline) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }

        return FromMemory(polyline.AsMemory());
    }

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a string.
    /// </summary>
    /// <param name="polyline">A string representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> instance corresponding to the specified string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="polyline"/> is null.</exception>
    public static Polyline FromString(string polyline) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }

        return FromMemory(polyline.AsMemory());
    }

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a readonly memory region of characters.
    /// </summary>
    /// <param name="polyline">A readonly memory region representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> instance corresponding to the specified memory region.</returns>
    public static Polyline FromMemory(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return new();
        }

        return FromSequence(new ReadOnlySequence<char>(polyline));
    }

    /// <summary>
    /// Creates a <see cref="Polyline"/> from a readonly sequence of characters.
    /// </summary>
    /// <param name="value">A readonly sequence of characters representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> instance corresponding to the specified sequence.</returns>
    internal static Polyline FromSequence(ReadOnlySequence<char> value) {
        return new Polyline(value);
    }

    #endregion

    #region Explicit conversions

    /// <summary>
    /// Defines an explicit conversion of a Unicode character array to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The Unicode character array to convert.</param>
    /// <returns>The converted <see cref="Polyline"/> instance.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(char[] polyline) => FromCharArray(polyline);

    /// <summary>
    /// Defines an explicit conversion of a string to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The string to convert.</param>
    /// <returns>The converted <see cref="Polyline"/> instance.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(string polyline) => FromString(polyline);

    /// <summary>
    /// Defines an explicit conversion of a readonly memory region to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The readonly memory region to convert.</param>
    /// <returns>The converted <see cref="Polyline"/> instance.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(ReadOnlyMemory<char> polyline) => FromMemory(polyline);

    #endregion
}
