namespace PolylineAlgorithm.Internal;

using System.Buffers;

internal class PolylineSegment : ReadOnlySequenceSegment<char> {
    public PolylineSegment(ReadOnlyMemory<char> memory, long runningIndex = 0) {
        Memory = memory;
        RunningIndex = runningIndex;
    }

    public PolylineSegment Append(ReadOnlyMemory<char> memory) {
        var segment = new PolylineSegment(memory, RunningIndex + memory.Length);

        Next = segment;

        return segment;
    }

    public PolylineSegment Append(PolylineSegment next) {
        Next = next;

        return next;
    }
}
