namespace PolylineAlgorithm;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

[StructLayout(LayoutKind.Auto)]
[DebuggerDisplay("Value: {ToString()}, IsEmpty: {IsEmpty}, Length: {Length}")]
public readonly struct Polyline : IEquatable<Polyline> {
    public readonly ReadOnlyMemory<char> _value;

    public Polyline() {
        _value = ReadOnlyMemory<char>.Empty;
    }

    public Polyline(string value) {
        _value = value?.AsMemory() ?? throw new ArgumentNullException(nameof(value));
    }

    public Polyline(char[] value) {
        _value = value?.AsMemory() ?? throw new ArgumentNullException(nameof(value));
    }

    public Polyline(ReadOnlyMemory<char> value) {
        _value = value;
    }

    internal readonly ReadOnlySpan<char> Span => _value.Span;

    public readonly bool IsEmpty => _value.IsEmpty;

    public readonly int Length => _value.Length;

    public char[] ToCharArray() => _value.ToArray();

    public ReadOnlyMemory<char> AsMemory() => _value;

    /// <inheritdoc />
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

    [ExcludeFromCodeCoverage]
    public static bool operator ==(Polyline left, Polyline right) => left.Equals(right);

    [ExcludeFromCodeCoverage]
    public static bool operator !=(Polyline left, Polyline right) => !(left == right);

    #endregion

    #region Factory methods

    public static Polyline FromCharArray(ref readonly char[] polyline) => new(polyline);

    public static Polyline FromMemory(ref readonly ReadOnlyMemory<char> polyline) => new(polyline);

    public static Polyline FromString(ref readonly string polyline) => new(polyline);


    #endregion

    #region Explicit conversions

    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(char[] polyline) => FromCharArray(in polyline);

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = $"Provided alternative {nameof(Polyline)}.{nameof(FromMemory)} to follow {nameof(String)}.{nameof(AsMemory)} naming pattern.")]
    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(ReadOnlyMemory<char> polyline) => FromMemory(in polyline);

    [ExcludeFromCodeCoverage]
    public static explicit operator Polyline(string polyline) => FromString(in polyline);

    #endregion
}
