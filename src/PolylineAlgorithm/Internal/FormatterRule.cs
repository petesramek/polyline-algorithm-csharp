//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Internal;

using System;

/// <summary>
/// Represents a single column rule baked into a <see cref="PolylineFormatter{TCoordinate, TPolyline}"/>.
/// Stores the pre-calculated factor and an optional baseline alongside the user-supplied value selector.
/// </summary>
/// <typeparam name="T">The source object type from which the column value is extracted.</typeparam>
internal sealed class FormatterRule<T> {
    /// <summary>
    /// Initializes a new instance of <see cref="FormatterRule{T}"/>.
    /// </summary>
    /// <param name="name">The column name used for diagnostics.</param>
    /// <param name="factor">The pre-calculated scaling factor (10^precision).</param>
    /// <param name="select">The delegate that extracts the raw value from an item.</param>
    /// <param name="baseline">The optional baseline value applied to the first item only.</param>
    internal FormatterRule(string name, long factor, Func<T, double> select, long? baseline = null) {
        Name = name;
        Factor = factor;
        Select = select;
        Baseline = baseline;
    }

    /// <summary>
    /// Gets the column name. Used for diagnostics and duplicate-name detection only.
    /// </summary>
    internal string Name { get; }

    /// <summary>
    /// Gets the pre-calculated scaling factor (10^precision).
    /// Stored as <see cref="long"/> so that <c>(long)(value * Factor)</c> stays in 64-bit arithmetic
    /// throughout the encoding hot loop without additional casting.
    /// </summary>
    internal long Factor { get; }

    /// <summary>
    /// Gets the optional reference value used as the baseline for the first encoded point.
    /// When set, the encoder subtracts this value from the first item's scaled column value so that
    /// the initial delta is <c>scaled_first_value − baseline</c> rather than <c>scaled_first_value</c>.
    /// </summary>
    internal long? Baseline { get; }

    /// <summary>
    /// Gets the delegate that extracts the column's raw <see cref="double"/> value from an item of type
    /// <typeparamref name="T"/>. Stored as a concrete delegate so the JIT can inline the call site.
    /// </summary>
    internal Func<T, double> Select { get; }
}
