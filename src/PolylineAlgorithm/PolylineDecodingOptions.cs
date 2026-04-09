//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

/// <summary>
/// Per-call options for a chunked decoding operation.
/// </summary>
/// <typeparam name="TCoordinate">The coordinate type understood by the formatter.</typeparam>
/// <remarks>
/// Pass an instance of this class to the chunked
/// <see cref="Abstraction.IChunkedPolylineDecoder{TPolyline, TValue}.Decode"/> overload to control
/// the accumulated-delta seed used at the start of each chunk. When <see cref="HasPrevious"/> is
/// <see langword="false"/> zero-initialisation is used, which is the existing default behaviour.
/// </remarks>
public sealed class PolylineDecodingOptions<TCoordinate> {
    private readonly TCoordinate _previous;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineDecodingOptions{TCoordinate}"/> with no
    /// previous coordinate (zero-initialised baseline will be used).
    /// </summary>
    public PolylineDecodingOptions() { }

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineDecodingOptions{TCoordinate}"/> with the
    /// specified previous coordinate used to seed the accumulated-delta state.
    /// </summary>
    /// <param name="previous">
    /// The last coordinate of the previous chunk, used to seed the accumulated-delta state.
    /// </param>
    public PolylineDecodingOptions(TCoordinate previous) {
        _previous = previous;
        HasPrevious = true;
    }

    /// <summary>
    /// Gets a value indicating whether a previous coordinate has been supplied to seed the
    /// accumulated-delta state. When <see langword="false"/> zero-initialisation is used, which is
    /// the existing default.
    /// </summary>
    public bool HasPrevious { get; }

    /// <summary>
    /// Gets the last coordinate of the previous chunk, used to seed the accumulated-delta state.
    /// Only meaningful when <see cref="HasPrevious"/> is <see langword="true"/>.
    /// </summary>
    public TCoordinate Previous => _previous;
}
