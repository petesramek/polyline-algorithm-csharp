namespace PolylineAlgorithm.Internal;

using System.Buffers;

internal class PolylineSegment : ReadOnlySequenceSegment<char> {
    public PolylineSegment(ReadOnlyMemory<char> memory, long runningIndex = 0) {
        Memory = memory;
        RunningIndex = runningIndex;
    }

    public void Append(ReadOnlyMemory<char> memory) {
        Append(new PolylineSegment(memory));
    }

    public void Append(PolylineSegment next) {
        next.RunningIndex = RunningIndex + Memory.Length;

        Next = next;
    }
}
