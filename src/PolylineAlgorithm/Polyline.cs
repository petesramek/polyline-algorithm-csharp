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
[StructLayout(LayoutKind.Explicit)]
[DebuggerDisplay("Value: {ToString()}, IsEmpty: {IsEmpty}, Length: {Length}")]
public readonly struct Polyline {
    [FieldOffset(0)]
    private readonly ReadOnlySequence<byte> _value;

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that is empty.
    /// </summary>
    public Polyline() {
        _value = ReadOnlySequence<byte>.Empty;
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

        _value = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(value));
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

        _value = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(value));
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified Unicode character array.
    /// </summary>
    /// <param name="value">The Unicode character array to initialize the polyline with.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public Polyline(ReadOnlyMemory<byte> value) {
        _value = new ReadOnlySequence<byte>(value);
    }

    /// <summary>
    /// Creates a new <see cref="Polyline"/> structure that contains the specified Unicode character array.
    /// </summary>
    /// <param name="value">The Unicode character array to initialize the polyline with.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is null.</exception>
    public Polyline(ReadOnlySequence<byte> value) {
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


    public ReadOnlySequence<byte> AsSequence() {
        return Value;
    }

    public bool SequenceEquals(Polyline other) {
        if (IsEmpty != other.IsEmpty || Length != other.Length) {
            return false;
        }

        if (Value.IsSingleSegment == other.Value.IsSingleSegment) {
            return Value.FirstSpan.SequenceEqual(other.Value.FirstSpan);
        }

        Stopwatch sw = new();
        sw.Start();

        var result1 = ReadByOneEquality(Value, other.Value);

        sw.Stop();
        var v1 = sw.Elapsed;

        sw.Restart();

        var result2 = TryCopyEquality(Value, other.Value);

        sw.Stop();
        var v2 = sw.Elapsed;

        sw.Restart();

        var result3 = IsNextEquality(Value, other.Value);

        sw.Stop();
        var v3 = sw.Elapsed;

        sw.Restart();

        var result4 = NerdBankEquality(Value, other.Value);

        sw.Stop();
        var v4 = sw.Elapsed;

        return result1 & result2 & result3 & result4;

        static bool ReadByOneEquality(ReadOnlySequence<byte> left, ReadOnlySequence<byte> right) {
            var @this = new SequenceReader<byte>(left);
            var that = new SequenceReader<byte>(right);

            while (@this.TryRead(out byte one) && that.TryRead(out byte two) && one == two) {
                if (@this.Remaining == 0) {
                    return true;
                }
            }

            return false;
        }

        static bool TryCopyEquality(ReadOnlySequence<byte> left, ReadOnlySequence<byte> right) {
            var @this = new SequenceReader<byte>(left);
            var that = new SequenceReader<byte>(right);

            Span<byte> buffer = left.Length < 512_000 ? stackalloc byte[(int)left.Length] : stackalloc byte[512_000];

            unsafe {
                while (true) {
                    if (@this.TryCopyTo(buffer) || that.IsNext(buffer, true)) {
                        @this.Advance(buffer.Length);

                        if (@this.Remaining < buffer.Length) {
                            buffer = buffer[..(int)@this.Remaining];
                        } else if (@this.Remaining == 0) {
                            return true;
                        }
                    } else {
                        return false;
                    }
                }
            }
        }

        static bool IsNextEquality(ReadOnlySequence<byte> left, ReadOnlySequence<byte> right) {
            var first = new SequenceReader<byte>(left);
            var second = new SequenceReader<byte>(right);

            while (true) {
                if (!second.IsNext(first.CurrentSpan)) {
                    break;
                }

                first.Advance(first.CurrentSpan.Length);
                second.Advance(first.CurrentSpan.Length);

                if (first.Remaining == 0) {
                    return true;
                }
            }
            return false;
        }

        static bool NerdBankEquality(ReadOnlySequence<byte> left, ReadOnlySequence<byte> right) {
            ReadOnlySequence<byte>.Enumerator aEnumerator = left.GetEnumerator();
            ReadOnlySequence<byte>.Enumerator bEnumerator = right.GetEnumerator();

            ReadOnlySpan<byte> aCurrent = default;
            ReadOnlySpan<byte> bCurrent = default;

            while (true) {
                bool aNext = TryGetNonEmptySpan(ref aEnumerator, ref aCurrent);
                bool bNext = TryGetNonEmptySpan(ref bEnumerator, ref bCurrent);
                if (!aNext && !bNext) {
                    // We've reached the end of both sequences at the same time.
                    return true;
                } else if (aNext != bNext) {
                    // One ran out of bytes before the other.
                    // We don't anticipate this, because we already checked the lengths.
                    throw new InvalidOperationException();
                }

                int commonLength = Math.Min(aCurrent.Length, bCurrent.Length);
                if (!aCurrent[..commonLength].SequenceEqual(bCurrent[..commonLength])) {
                    return false;
                }

                aCurrent = aCurrent.Slice(commonLength);
                bCurrent = bCurrent.Slice(commonLength);
            }

            static bool TryGetNonEmptySpan(ref ReadOnlySequence<byte>.Enumerator enumerator, ref ReadOnlySpan<byte> span) {
                while (span.Length == 0) {
                    if (!enumerator.MoveNext()) {
                        return false;
                    }

                    span = enumerator.Current.Span;
                }

                return true;
            }
        }

    }

    #region Factory methods

    /// <summary>
    /// Creates an instance of the current type from a Unicode character array.
    /// </summary>
    /// <param name="polyline">A Unicode character array representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified Unicode character array.</returns>
    public static Polyline FromCharArray(char[] polyline) => new(polyline);


    /// <summary>
    /// Creates an instance of the current type from a Unicode character array.
    /// </summary>
    /// <param name="polyline">A Unicode character array representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified Unicode character array.</returns>
    public static Polyline FromByteArray(byte[] polyline) => new(polyline);

    /// <summary>
    /// Creates an instance of the current type from a string.
    /// </summary>
    /// <param name="polyline">A string representing an encoded polyline.</param>
    /// <returns>The <see cref="Polyline"/> value that corresponds to the specified string value.</returns>
    public static Polyline FromString(string polyline) => new(polyline);


    public static Polyline FromMemory(ReadOnlyMemory<byte> polyline) => new(polyline);

    public static Polyline FromSequence(ReadOnlySequence<byte> polyline) => new(polyline);

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
    public static explicit operator Polyline(Memory<byte> polyline) => FromMemory(polyline);

    /// <summary>
    /// Defines an explicit conversion of a string to a <see cref="Polyline"/>.
    /// </summary>
    /// <param name="polyline">The string to convert.</param>
    /// <returns>The converted string.</returns>
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(ReadOnlySequence<byte> polyline) => FromSequence(polyline);

    #endregion
}