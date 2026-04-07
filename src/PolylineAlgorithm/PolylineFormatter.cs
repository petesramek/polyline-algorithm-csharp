//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Abstraction;
using PolylineAlgorithm.Internal;
using System;

/// <summary>
/// Provides static factory methods and ready-made instances of <see cref="IPolylineFormatter{TPolyline}"/>
/// for the most common polyline surface types.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="ForString"/> or <see cref="ForMemory"/> for the two most common cases.
/// Call <see cref="Create{T}"/> to build a custom formatter from a pair of delegates.
/// </para>
/// </remarks>
public static class PolylineFormatter {
    /// <summary>
    /// Gets a formatter that produces a <see cref="string"/> from the encoded char buffer and reads
    /// the buffer back via <see cref="string.AsMemory()"/>.
    /// </summary>
    public static IPolylineFormatter<string> ForString { get; } =
        new DelegatePolylineFormatter<string>(
            static mem => new string(mem.Span),
            static s => s.AsMemory());

    /// <summary>
    /// Gets a pass-through formatter for <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/>.
    /// Both <c>Write</c> and <c>Read</c> are identity operations.
    /// </summary>
    public static IPolylineFormatter<ReadOnlyMemory<char>> ForMemory { get; } =
        new DelegatePolylineFormatter<ReadOnlyMemory<char>>(
            static mem => mem,
            static mem => mem);

    /// <summary>
    /// Creates a custom <see cref="IPolylineFormatter{TPolyline}"/> from a pair of delegates.
    /// </summary>
    /// <typeparam name="T">The polyline surface type.</typeparam>
    /// <param name="write">
    /// Converts the encoded <see cref="ReadOnlyMemory{T}"/> of <see cref="char"/> produced by the encoder
    /// into a <typeparamref name="T"/>.
    /// </param>
    /// <param name="read">
    /// Extracts the encoded character buffer from a <typeparamref name="T"/> for the decoder to consume.
    /// </param>
    /// <returns>A sealed <see cref="IPolylineFormatter{TPolyline}"/> backed by the supplied delegates.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="write"/> or <paramref name="read"/> is <see langword="null"/>.
    /// </exception>
    public static IPolylineFormatter<T> Create<T>(
        Func<ReadOnlyMemory<char>, T> write,
        Func<T, ReadOnlyMemory<char>> read) {
        if (write is null) {
            throw new ArgumentNullException(nameof(write));
        }

        if (read is null) {
            throw new ArgumentNullException(nameof(read));
        }

        return new DelegatePolylineFormatter<T>(write, read);
    }
}
