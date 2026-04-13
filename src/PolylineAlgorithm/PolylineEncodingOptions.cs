//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

/// <summary>
/// Per-call options for a chunked encoding operation.
/// </summary>
/// <typeparam name="TValue">The coordinate type understood by the formatter.</typeparam>
/// <remarks>
/// Pass an instance of this class to the chunked
/// <see cref="Abstraction.IPolylineEncoder{TValue, TPolyline}.Encode"/> overload to control
/// the delta baseline used at the start of each chunk. When <see cref="HasPrevious"/> is
/// <see langword="false"/> the formatter's built-in baseline (or zero) is used, which is equivalent
/// to the existing default behaviour.
/// </remarks>
public sealed class PolylineEncodingOptions<TValue> {
    private readonly TValue _previous;

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineEncodingOptions{TValue}"/> with no
    /// previous coordinate (formatter default baseline will be used).
    /// </summary>
    public PolylineEncodingOptions() { }

    /// <summary>
    /// Initializes a new instance of <see cref="PolylineEncodingOptions{TValue}"/> with the
    /// specified previous coordinate used to seed the delta baseline.
    /// </summary>
    /// <param name="previous">
    /// The last coordinate of the previous chunk, used to seed the delta baseline.
    /// </param>
    public PolylineEncodingOptions(TValue previous) {
        _previous = previous;
        HasPrevious = true;
    }

    /// <summary>
    /// Gets a value indicating whether a previous coordinate has been supplied to seed the delta
    /// baseline. When <see langword="false"/> the formatter's built-in baseline is used as the
    /// starting point (which defaults to zero when no baseline has been configured), equivalent to
    /// the existing default behaviour.
    /// </summary>
    public bool HasPrevious { get; }

    /// <summary>
    /// Gets the last coordinate of the previous chunk, used to seed the delta baseline.
    /// Only meaningful when <see cref="HasPrevious"/> is <see langword="true"/>.
    /// </summary>
    public TValue Previous => _previous;
}
