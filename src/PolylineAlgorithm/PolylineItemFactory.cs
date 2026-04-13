//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using System;

/// <summary>
/// Represents a factory method that reconstructs a <typeparamref name="T"/> item from denormalized
/// values decoded from a polyline.
/// </summary>
/// <typeparam name="T">The value or item type to create.</typeparam>
/// <param name="values">
/// The denormalized values reconstructed by the polyline decoder. Each element corresponds to the
/// original <see cref="double"/> value that was encoded, with the precision factor divided out and any
/// baseline added back. The span length equals the number of columns defined via
/// <see cref="FormatterBuilder{TValue, TPolyline}.AddValue"/>.
/// </param>
/// <returns>A <typeparamref name="T"/> instance reconstructed from <paramref name="values"/>.</returns>
public delegate T PolylineItemFactory<T>(ReadOnlySpan<double> values);
