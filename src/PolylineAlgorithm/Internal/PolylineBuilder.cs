namespace PolylineAlgorithm.Internal;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Provides functionality to build a polyline from multiple segments of characters.
/// This struct is used to efficiently construct a <see cref="Polyline"/> instance.
/// </summary>
[StructLayout(LayoutKind.Auto)]
internal struct PolylineBuilder {
    private PolylineSegment? _initial;
    private PolylineSegment? _last;

    /// <summary>
    /// Appends a new segment of characters to the polyline being built.
    /// </summary>
    /// <param name="value">The segment of characters to append.</param>
    /// <remarks>
    /// This method creates a new <see cref="PolylineSegment"/> for the provided memory
    /// and links it to the existing chain of segments.
    /// </remarks>
    public void Append(ReadOnlyMemory<char> value) {
        var current = new PolylineSegment(value);

        _initial ??= current;

        _last?.Append(current);
        _last = current;
    }

    /// <summary>
    /// Builds the final <see cref="Polyline"/> instance from the appended segments.
    /// </summary>
    /// <returns>
    /// A <see cref="Polyline"/> instance representing the concatenated segments.
    /// If no segments were appended, an empty <see cref="Polyline"/> is returned.
    /// </returns>
    public readonly Polyline Build() {
        if (_initial is null) {
            return Polyline.FromMemory(ReadOnlyMemory<char>.Empty);
        }

        return Polyline.FromSequence(new(_initial, 0, _last, _last!.Memory.Length));
    }
}
