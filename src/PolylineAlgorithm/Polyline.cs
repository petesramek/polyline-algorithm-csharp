namespace PolylineAlgorithm;

using System;

public readonly struct Polyline : IEquatable<Polyline> {
    public Polyline() {
        Value = Memory<char>.Empty;
    }

    public Polyline(string polyline)
        : this(polyline.AsMemory()) {
        if (string.IsNullOrWhiteSpace(polyline)) {
            throw new ArgumentException($"'{nameof(polyline)}' cannot be null or whitespace.", nameof(polyline));
        }
    }

    public Polyline(char[] polyline)
        : this(polyline.AsMemory()) {
        if (polyline is null) {
            throw new ArgumentNullException(nameof(polyline));
        }
    }

    internal Polyline(ReadOnlyMemory<char> polyline) {
        Value = polyline;
    }

    public ReadOnlyMemory<char> Value { get; }

    public bool IsEmpty => Value.Length == 0;

    public int Length => Value.Length;

    public char this[int i] {
        get => Value.Span[i];
    }

    public override bool Equals(object? obj) {
        return obj is Polyline polyline && Equals(polyline);
    }

    public bool Equals(Polyline other) {
        return Value.Span == other.Value.Span;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Value);
    }

    public static bool operator ==(Polyline left, Polyline right) {
        return left.Equals(right);
    }

    public static bool operator !=(Polyline left, Polyline right) {
        return !(left == right);
    }

    public override string ToString() {
        return Value.ToString();
    }
}
