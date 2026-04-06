//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

/// <summary>
/// Holds the per-field delta accumulation state for polyline encoding and decoding.
/// </summary>
/// <remarks>
/// Instances are owned by caller-provided encoder/decoder subclasses as private fields — one per
/// encoded field (e.g. latitude, longitude). The engine mutates the state on every
/// <see cref="PolylineWriter.Write"/> or <see cref="PolylineReader.Read"/> call via an
/// <see langword="internal"/> reference, while the public <see cref="Value"/> property lets callers
/// read the current accumulated value (e.g. for diagnostics or logging) without being able to write it.
/// </remarks>
public struct PolylineValueState {
    internal int _value;

    /// <summary>
    /// Gets the current accumulated normalized integer value.
    /// </summary>
    public readonly int Value => _value;
}
