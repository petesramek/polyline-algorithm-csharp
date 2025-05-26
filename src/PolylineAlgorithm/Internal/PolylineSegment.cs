//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System.Buffers;

/// <summary>
/// Represents a segment of a polyline as part of a linked list of character memory segments.
/// Inherits from <see cref="ReadOnlySequenceSegment{char}"/> to enable efficient manipulation and traversal of polyline data.
/// </summary>
internal class PolylineSegment : ReadOnlySequenceSegment<char> {
    /// <summary>
    /// Initializes a new instance of the <see cref="PolylineSegment"/> class with the specified memory segment and optional running index.
    /// </summary>
    /// <param name="memory">
    /// The <see cref="ReadOnlyMemory{char}"/> segment to associate with this polyline segment.
    /// </param>
    /// <param name="runningIndex">
    /// The cumulative index of this segment within the polyline sequence. Defaults to 0.
    /// </param>
    public PolylineSegment(ReadOnlyMemory<char> memory, long runningIndex = 0) {
        Memory = memory;
        RunningIndex = runningIndex;
    }

    /// <summary>
    /// Appends a new memory segment to the end of the current polyline segment chain.
    /// </summary>
    /// <param name="memory">
    /// The <see cref="ReadOnlyMemory{char}"/> segment to append as a new <see cref="PolylineSegment"/>.
    /// </param>
    public void Append(ReadOnlyMemory<char> memory) {
        Append(new PolylineSegment(memory));
    }

    /// <summary>
    /// Appends the specified <see cref="PolylineSegment"/> to the end of the current segment chain,
    /// updating the running index and linking the segments.
    /// </summary>
    /// <param name="next">
    /// The <see cref="PolylineSegment"/> to append as the next segment in the chain.
    /// </param>
    public void Append(PolylineSegment next) {
        next.RunningIndex = RunningIndex + Memory.Length;
        Next = next;
    }
}
