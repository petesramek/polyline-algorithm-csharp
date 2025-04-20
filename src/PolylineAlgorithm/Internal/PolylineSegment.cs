namespace PolylineAlgorithm.Internal;

using System.Buffers;

/// <summary>
/// Represents a segment of a polyline, implemented as a linked list of memory segments.
/// This class extends <see cref="ReadOnlySequenceSegment{T}"/> to support efficient handling of polyline data.
/// </summary>
internal class PolylineSegment : ReadOnlySequenceSegment<char> {
    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineSegment"/> class with the specified memory and running index.
    /// </summary>
    /// <param name="memory">The memory segment to associate with this polyline segment.</param>
    /// <param name="runningIndex">
    /// The cumulative index of this segment within the polyline. Defaults to 0.
    /// </param>
    public PolylineSegment(ReadOnlyMemory<char> memory, long runningIndex = 0) {
        Memory = memory;
        RunningIndex = runningIndex;
    }

    /// <summary>
    /// Appends a new memory segment to the current polyline segment.
    /// </summary>
    /// <param name="memory">The memory segment to append.</param>
    public void Append(ReadOnlyMemory<char> memory) {
        Append(new PolylineSegment(memory));
    }

    /// <summary>
    /// Appends another <see cref="PolylineSegment"/> to the current segment, forming a linked list.
    /// </summary>
    /// <param name="next">The next <see cref="PolylineSegment"/> to append.</param>
    public void Append(PolylineSegment next) {
        next.RunningIndex = RunningIndex + Memory.Length;
        Next = next;
    }
}
