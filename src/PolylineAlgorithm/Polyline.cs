//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    /// Creates a new <see cref="Polyline"/> structure that is empty.
    /// </summary>
    public Polyline() {
        _value = ReadOnlySequence<char>.Empty;
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified string value.
    /// </summary>
    /// <param name="value">The string value to initialize the polyline with.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public Polyline(string value) {
        if (value is null) {
            throw new ArgumentNullException(nameof(value));
        }

        _value = new ReadOnlySequence<char>(value.AsMemory());
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified Unicode character array.
    /// </summary>
    /// <param name="value">The Unicode character array to initialize the polyline with.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public Polyline(char[] value) {
        if (value is null) {
            throw new ArgumentNullException(nameof(value));
        }

        _value = new ReadOnlySequence<char>(value.AsMemory());
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified readonly memory region.
    /// </summary>
    /// <param name="value">The readonly memory region to initialize the polyline with.</param>
    public Polyline(ReadOnlyMemory<char> value) {
        _value = new ReadOnlySequence<char>(value);
    }

    /// <summary>
    /// Gets the span of characters in the polyline.
    /// </summary>
    internal readonly ReadOnlySequence<char> Value => _value;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline" /> is empty.
    /// </summary>
    public readonly bool IsEmpty => Value.IsEmpty;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline" /> is empty.
    /// </summary>
    public readonly long Length => Value.Length;

    /// <summary>
    /// Copies the characters in this instance to a Unicode character array.
    /// </summary>
    /// <returns>A Unicode character array.</returns>
    public char[] CopyTo(char[] destination) {
        if (destination is null) {
            throw new ArgumentNullException(nameof(destination));
        }

        if(Length != destination.Length) {
            throw new ArgumentException("", paramName: nameof(destination));
        }

        _value.CopyTo(destination);

        return destination;
    }

    public ReadOnlySequence<char> AsSequence() => Value;

    public override string ToString() {
        if (IsEmpty) {
            return string.Empty;
        }

        if (Value.IsSingleSegment) {
            return Value.FirstSpan.ToString();
        }
        
        var sb = Value.Length <= int.MaxValue ? new StringBuilder(Convert.ToInt32(Value.Length)) : new StringBuilder();
        var enumerator = Value.GetEnumerator();

        while (true) {
            if (!enumerator.MoveNext()) {
                break;
            }

            if (enumerator.Current.IsEmpty) {
                continue;
            }

            sb.Append(enumerator.Current);
        }

        return sb.ToString();
    }

    public bool Equals(Polyline other) {
        if ((IsEmpty != other.IsEmpty) || (Length != other.Length)) {
            return false;
        }

        long position = 0;
        var enumerator = Value.GetEnumerator();
        var right = new SequenceReader<char>(other.Value);

        while (true) {
            if (!enumerator.MoveNext()) {
                break;
            }

            if (enumerator.Current.IsEmpty) {
                continue;
            }

            if (!right.IsNext(enumerator.Current.Span, true)) {
                break;
            }

            position += enumerator.Current.Length;
        }

        return position == right.Length;
    }

    public override bool Equals(object obj) {
        return obj is Polyline other && Equals(other);
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    public static bool operator ==(Polyline left, Polyline right) {
        return left.Equals(right);
    }

    public static bool operator !=(Polyline left, Polyline right) {
        return !(left == right);
    }

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
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = $"Provided alternative {nameof(Polyline)}.{nameof(FromMemory)}.")]
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