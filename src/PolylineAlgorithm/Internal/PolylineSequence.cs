namespace PolylineAlgorithm.Internal;

using System.Buffers;

internal class PolylineSegment : ReadOnlySequenceSegment<char> {
    public PolylineSegment(ReadOnlyMemory<char> memory, long runningIndex = 0) {
        Memory = memory;
        RunningIndex = runningIndex;
    }

    public PolylineSegment Append(ReadOnlyMemory<char> memory) {
        return Append(new PolylineSegment(memory));
    }

    public PolylineSegment Append(PolylineSegment next) {
        next.RunningIndex = RunningIndex + Memory.Length;

        Next = next;

        return next;
    }
}
