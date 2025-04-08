namespace PolylineAlgorithm.Internal;

using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Auto)]
internal struct PolylineBuilder {
    private PolylineSegment? _initial;
    private PolylineSegment? _last;

    public void Append(ReadOnlyMemory<char> value) {
        var current = new PolylineSegment(value);

        _initial ??= current;

        _last?.Append(current);
        _last = current;
    }

    public readonly Polyline Build() {
        if (_initial is null) {
            return Polyline.FromMemory(ReadOnlyMemory<char>.Empty);
        }

        return Polyline.FromSequence(new(_initial, 0, _last, _last!.Memory.Length));
    }
}