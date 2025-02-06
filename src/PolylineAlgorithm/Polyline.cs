namespace PolylineAlgorithm;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Auto)]
public readonly struct Polyline : IEquatable<Polyline> {
    public readonly ReadOnlyMemory<char> _value;

    public Polyline() {
        _value = ReadOnlyMemory<char>.Empty;
    }

    public Polyline(ref readonly string value) {
        if (value is null) {
            throw new ArgumentNullException(nameof(value));
        }

        _value = value.AsMemory();
    }

    public Polyline(ref readonly char[] value) {
        if (value is null) {
            throw new ArgumentNullException(nameof(value));
        }

        _value = value.AsMemory();
    }

    public Polyline(ref readonly ReadOnlyMemory<char> value) => _value = value;

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    internal ReadOnlySpan<char> Span => _value.Span;

    public override string ToString() => _value.ToString();

    public char[] ToCharArray() => _value.ToArray();

    public ReadOnlyMemory<char> AsMemory() => _value;

    public ReadOnlySpan<char> AsSpan() => _value.Span;

    public override bool Equals(object? obj) => obj is Polyline polyline && Equals(polyline);

    public bool Equals(Polyline other) => _value.Equals(other._value);

    public override int GetHashCode() => HashCode.Combine(_value);

    public static bool operator ==(Polyline left, Polyline right) => left.Equals(right);

    public static bool operator !=(Polyline left, Polyline right) =>  !(left == right);

    public static implicit operator Polyline(string polyline) => FromString(in polyline);

    public static implicit operator Polyline(char[] polyline) => FromCharArray(in polyline);

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = $"Provided alternative {nameof(Polyline)}.{nameof(FromMemory)} to follow {nameof(String)}.{nameof(AsMemory)} naming pattern.")]
    public static implicit operator Polyline(ReadOnlyMemory<char> polyline) => FromMemory(in polyline);

    public static Polyline FromString(ref readonly string polyline) => new(in polyline);

    public static Polyline FromCharArray(ref readonly char[] polyline) => new(in polyline);

    public static Polyline FromMemory(ref readonly ReadOnlyMemory<char> polyline) => new(in polyline);
}
