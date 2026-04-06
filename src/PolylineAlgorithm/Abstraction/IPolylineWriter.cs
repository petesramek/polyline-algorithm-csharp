//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

/// <summary>
/// Defines a write-only cursor for emitting individual field values into the polyline encoding pipeline.
/// </summary>
/// <remarks>
/// <para>
/// Instances of <see cref="IPolylineWriter"/> are created and owned by the encoding engine. Formatter implementations
/// must not create writers directly.
/// </para>
/// <para>
/// During encoding, the engine creates one writer per encoding session and calls
/// <see cref="Write(double)"/> once for every field of every item, in the order determined by the formatter.
/// Delta accumulation, zigzag encoding, and ASCII shifting are performed internally; the formatter
/// only sees raw floating-point field values.
/// </para>
/// <para>
/// The writer is shared across all items in the sequence so that delta state carries correctly
/// across item boundaries.
/// </para>
/// </remarks>
public interface IPolylineWriter {
    /// <summary>
    /// Emits one field value into the polyline encoding pipeline.
    /// </summary>
    /// <param name="value">
    /// The field value to emit. The engine normalises the value, computes the delta against the
    /// same-position field of the previous item, applies zigzag encoding, and writes the result
    /// to the output buffer.
    /// </param>
    void Write(double value);
}
