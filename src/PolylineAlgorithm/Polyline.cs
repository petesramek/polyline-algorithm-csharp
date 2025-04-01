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
    private readonly ReadOnlySequence<char> _value;

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that is empty.
    /// </summary>
    public Polyline() {
        _value = ReadOnlySequence<char>.Empty;
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified Unicode character array.
    /// </summary>
    /// <param name="value">The readonly memory region to initialize the polyline with.</param>
    private Polyline(ReadOnlySequence<char> value) {
        _value = value;
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

    //public long GetCoordinateCount() {
    //    long count = 0;

    //    var enumerator = GetEnumerator();

    //    while (enumerator.MoveNext()) {
    //        count++;
    //    }

    //    return count;
    //}

    public ReadOnlySequence<char>.Enumerator GetEnumerator() {
        return _value.GetEnumerator();
    }

    /// <summary>
    /// Copies the characters in this instance to a Unicode character array.
    /// </summary>
    /// <returns>A Unicode character array.</returns>
    public void CopyTo(char[] destination) {
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

            sb.Append(enumerator.Current.Span);
        }

        return sb.ToString();
    }

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

        if (reader.Remaining > 0) {
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
    public static Polyline FromCharArray(char[] polyline) {
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

        return FromMemory(polyline.AsMemory());
    }

    /// <summary>
    /// Creates an instance of the current type from a Unicode character array.
    /// </summary>
    /// <param name="polyline">A Unicode character array representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified Unicode character array.</returns>
    public static Polyline FromMemory(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return new();
        }

        return FromSequence(new ReadOnlySequence<char>(polyline));
    }

    internal static Polyline FromSequence(ReadOnlySequence<char> value) {
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
    public static explicit operator Polyline(char[] polyline) => FromCharArray(polyline);

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
    public static explicit operator Polyline(ReadOnlyMemory<char> polyline) => FromMemory(polyline);

    #endregion

    [StructLayout(LayoutKind.Auto)]
    internal struct PolylineBuilder {
        private PolylineSegment? _initial;
        private PolylineSegment? _last;

        public void Append(in ReadOnlyMemory<char> value) {
            var current = new PolylineSegment(value);

            _initial ??= current;

            _last?.Append(current);
            _last = current;
        }

        public Polyline Build() {
            if (_initial is null) {
                return FromMemory(ReadOnlyMemory<char>.Empty);
            }

            return FromSequence(new(_initial, 0, _last, _last!.Memory.Length));
        }
    }
}