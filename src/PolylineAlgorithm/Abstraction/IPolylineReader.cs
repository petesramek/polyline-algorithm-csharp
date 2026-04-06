//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

/// <summary>
/// Defines a read-only cursor for consuming individual field values from the polyline decoding pipeline.
/// </summary>
/// <remarks>
/// <para>
/// Instances of <see cref="IPolylineReader"/> are created and owned by the decoding engine. Formatter implementations
/// must not create readers directly.
/// </para>
/// <para>
/// During decoding, the engine reverses the ASCII shift, un-zigzags, and accumulates deltas for each
/// encoded chunk. The reader exposes the resulting absolute floating-point values one at a time.
/// Each call to <see cref="Read"/> returns the next value and advances the cursor.
/// </para>
/// <para>
/// The reader is shared across all items in the sequence, so the formatter's implementation of
/// <see cref="Read"/> must consume exactly as many values per item as the corresponding
/// encoder's formatter writes.
/// </para>
/// </remarks>
public interface IPolylineReader {
    /// <summary>
    /// Reads and returns the next field value from the polyline decoding pipeline.
    /// </summary>
    /// <returns>
    /// The next decoded floating-point field value.
    /// </returns>
    /// <exception cref="InvalidPolylineException">
    /// Thrown when the polyline data is exhausted or malformed before the expected number of fields
    /// for the current item has been read.
    /// </exception>
    double Read();
}
