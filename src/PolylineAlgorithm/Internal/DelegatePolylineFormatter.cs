//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using PolylineAlgorithm.Abstraction;
using System;

/// <summary>
/// A sealed delegate-backed implementation of <see cref="IPolylineFormatter{TPolyline}"/>.
/// Produced by <see cref="PolylineFormatter.Create{T}"/> and the built-in factory properties.
/// </summary>
/// <typeparam name="T">The polyline surface type.</typeparam>
internal sealed class DelegatePolylineFormatter<T> : IPolylineFormatter<T> {
    private readonly Func<ReadOnlyMemory<char>, T> _write;
    private readonly Func<T, ReadOnlyMemory<char>> _read;

    internal DelegatePolylineFormatter(Func<ReadOnlyMemory<char>, T> write, Func<T, ReadOnlyMemory<char>> read) {
        _write = write;
        _read = read;
    }

    /// <inheritdoc/>
    public T Write(ReadOnlyMemory<char> encoded) => _write(encoded);

    /// <inheritdoc/>
    public ReadOnlyMemory<char> Read(T polyline) => _read(polyline);
}
