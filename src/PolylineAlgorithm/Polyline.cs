//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
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
    private readonly ReadOnlySequence<byte> _value;

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that is empty.
    /// </summary>
    public Polyline() {
        _value = ReadOnlySequence<byte>.Empty;
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified Unicode character array.
    /// </summary>
    /// <param name="value">The readonly memory region to initialize the polyline with.</param>
    private Polyline(ReadOnlySequence<byte> value) {
        _value = value;
    }

    /// <summary>
    /// Gets the span of characters in the polyline.
    /// </summary>
    internal readonly ReadOnlySequence<byte> Value => _value;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline" /> is empty.
    /// </summary>
    public readonly bool IsEmpty => Value.IsEmpty;

    /// <summary>
    /// Gets a value indicating whether this <see cref="Polyline" /> is empty.
    /// </summary>
    public readonly long Length => Value.Length;

    public long GetCoordinateCount() {
        long count = 0;

        var enumerator = GetEnumerator();

        while (enumerator.MoveNext()) {
            count++;
        }

        return count;
    }

    public ReadOnlySequence<byte>.Enumerator GetEnumerator() {
        return _value.GetEnumerator();
    }

    /// <summary>
    /// Copies the characters in this instance to a Unicode character array.
    /// </summary>
    /// <returns>A Unicode character array.</returns>
    public void CopyTo(byte[] destination) {
        if (destination is null) {
            throw new ArgumentNullException(nameof(destination));
        }

        if (Length != destination.Length) {
            throw new ArgumentException("", paramName: nameof(destination));
        }

        _value.CopyTo(destination);
    }

    public override string ToString() {
        if (IsEmpty) {
            return string.Empty;
        }

        if (Value.IsSingleSegment) {
            return Encoding.UTF8.GetString(Value.FirstSpan);
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

            sb.Append(Encoding.UTF8.GetString(enumerator.Current.Span));
        }

        return sb.ToString();
    }

    public bool Equals(Polyline other) {
        if ((IsEmpty != other.IsEmpty) || (Length != other.Length)) {
            return false;
        }

        var first = GetEnumerator();
        var second = other.GetEnumerator();

        while (first.MoveNext()) {
            if (!second.MoveNext() || !first.Current.Span.SequenceEqual(second.Current.Span)) {
                return false;
            }
        }

        if (second.MoveNext()) {
            return false;
        }

        return true;
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
    public static Polyline FromByteArray(byte[] polyline) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }

        return FromMemory(polyline.AsMemory());
    }

    /// <summary>
    /// Creates an instance of the current type from a string.
    /// </summary>
    /// <param name="polyline">A string representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified string value.</returns>
    public static Polyline FromString(string polyline) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }

        return FromMemory(Encoding.UTF8.GetBytes(polyline));
    }

    /// <summary>
    /// Creates an instance of the current type from a Unicode character array.
    /// </summary>
    /// <param name="polyline">A Unicode character array representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified Unicode character array.</returns>
    public static Polyline FromMemory(ReadOnlyMemory<byte> polyline) {
        if (polyline.IsEmpty) {
            return new();
        }

        var builder = new PolylineBuilder();

        builder
            .Append(polyline);

        return builder.Build();
    }

    internal static Polyline FromSequence(ReadOnlySequence<byte> value) {
        return new Polyline(value);
    }

    #endregion

    #region Explicit conversions

    /// <summary>
    /// Defines an explicit conversion of a Unicode character array to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The Unicode character array to convert.</param>
    /// <returns>The converted Unicode character array.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(byte[] polyline) => FromByteArray(polyline);

    /// <summary>
    /// Defines an explicit conversion of a string to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The string to convert.</param>
    /// <returns>The converted string.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(string polyline) => FromString(polyline);

    /// <summary>
    /// Defines an explicit conversion of a string to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The string to convert.</param>
    /// <returns>The converted string.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(Memory<byte> polyline) => FromMemory(polyline);

    #endregion

    internal struct PolylineBuilder {
        private PolylineSegment? _initial;
        private PolylineSegment? _last;

        public void Append(ReadOnlyMemory<byte> value) {
                var current = new PolylineSegment(value);

                _initial ??= current;

                _last?.Append(current);
                _last = current;
        }

        public Polyline Build() {
            if (_initial is null) {
                return FromMemory(ReadOnlyMemory<byte>.Empty);
            }

            return FromSequence(new(_initial, 0, _last, _last!.Memory.Length));
        }

        //private long GetCount(ReadOnlyMemory<char> memory) {
        //    int position = 0;
        //    int index;
        //    long count = 0;

        //    while (position < memory.Length) {
        //        index = memory.Span[position..].IndexOfAny(Defaults.Polyline.Delimiters) + 1;
        //        index += memory.Span[(position + index)..].IndexOfAny(Defaults.Polyline.Delimiters) + 1;

        //        position += index;

        //        if (position == memory.Length) {
        //            break;
        //        }

        //        count++;
        //    }

        //    return count;
        //}
    }
}