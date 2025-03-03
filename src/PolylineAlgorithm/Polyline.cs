//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Represents a readonly encoded polyline string.
/// </summary>
[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay("Value: {ToString()}, IsEmpty: {IsEmpty}, Length: {Length}")]
public readonly struct Polyline : IEquatable<Polyline> {
    private readonly ReadOnlySequence<char> _value;

    private Polyline(ReadOnlySequence<char> value) {
        _value = value;
    }

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

        _value = _value = new ReadOnlySequence<char>(value.AsMemory());
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
    public readonly bool IsEmpty => _value.IsEmpty;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline" /> is empty.
    /// </summary>
    public readonly long Length => _value.Length;

    /// <summary>
    /// Copies the characters in this instance to a Unicode character array.
    /// </summary>
    /// <returns>A Unicode character array.</returns>
    public char[] ToCharArray() => _value.ToArray();

    /// <summary>
    /// Returns the underlying <see cref="ReadOnlySequence{T}" /> this instance represents.
    /// </summary>
    /// <returns>The underlying <see cref="ReadOnlySequence{T}"/>.</returns>
    public ReadOnlySequence<char> AsSequence() => _value;

    /// <summary>
    /// Returns a string representation of the value of this instance.
    /// </summary>
    /// <returns>The string value of this <see cref="Polyline"/> object.</returns>
    public override string ToString() => Value.ToString();

    public Polyline Append(Polyline other) {
        if (other.IsEmpty) {
            return this;
        }

        var last = GetSegments(out PolylineSegment initial);

        var end = other.GetSegments(out PolylineSegment continuation);

        last.Append(continuation);

        var sequence = new ReadOnlySequence<char>(initial, 0, end, end.Memory.Length);

        return new Polyline(sequence);
    }

    private PolylineSegment GetSegments(out PolylineSegment initial) {
        initial = new PolylineSegment(Value.First);

        if (Value.IsSingleSegment) {
            return initial;
        }

        PolylineSegment? current = null;

        foreach (var segment in Value) {
            if (current is null) {
                current = initial;
            } else {
                current = current.Append(segment);
            }
        }

        // Current will never be null in case we return from the method early using Value.IsSingleSegment property check
        return current!;
    }

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
    public bool Equals(Polyline other) {
        if(IsEmpty && other.IsEmpty) {
            return true;
        }

        if(Length != other.Length) {
            return false;
        }

        long start = 0;
        bool result = true;
        int chunkSize = 256;
        long max = Math.DivRem(Length, chunkSize, out long remainder) + (remainder > 0 ? 1 : 0);
        Span<char> @this = stackalloc char[chunkSize];
        Span<char> that = stackalloc char[chunkSize];

        for (int i = 0; i < max; i++) {
            start = i * chunkSize;

            if(max - i == 1) {
                @this.Clear();
                that.Clear();
                chunkSize = (int)remainder;
            }

            Value.Slice(start, chunkSize).CopyTo(@this);
            other.Value.Slice(start, chunkSize).CopyTo(that);

            if(!@this.SequenceEqual(that)) {
                result = false;
                break;
            }
        }

        return result;
    }

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

    internal void Append(IEnumerable<char> latitude, IEnumerable<char> longitude) {
        throw new NotImplementedException();
    }

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