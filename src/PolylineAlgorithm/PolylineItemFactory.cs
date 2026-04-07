//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;

/// <summary>
/// Represents a factory method that reconstructs a <typeparamref name="T"/> item from an array of
/// scaled integer values decoded from a polyline.
/// </summary>
/// <typeparam name="T">The coordinate or item type to create.</typeparam>
/// <param name="values">
/// The scaled integer values accumulated from the polyline decoder. Each element corresponds to one
/// column as defined by the <see cref="FormatterBuilder{T}"/> that built the associated formatter.
/// </param>
/// <returns>A <typeparamref name="T"/> instance reconstructed from <paramref name="values"/>.</returns>
public delegate T PolylineItemFactory<T>(ReadOnlySpan<long> values);
