namespace PolylineAlgorithm.Internal;

using System.Buffers;

internal class PolylineSegment : ReadOnlySequenceSegment<byte> {
    public PolylineSegment(ReadOnlyMemory<byte> memory, long runningIndex = 0) {
        Memory = memory;
        RunningIndex = runningIndex;
    }

    public void Append(ReadOnlyMemory<byte> memory) {
        Append(new PolylineSegment(memory));
    }

    public void Append(PolylineSegment next) {
        next.RunningIndex = RunningIndex + Memory.Length;
        Next = next;
    }
}
