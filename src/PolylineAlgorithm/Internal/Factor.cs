namespace PolylineAlgorithm.Internal;

using System;

/// <summary>
/// Provides optimized calculation of powers of 10 for precision-based operations.
/// </summary>
/// <remarks>
/// This class caches common powers of 10 (10^0 through 10^9) for efficient lookup,
/// falling back to <see cref="Math.Pow"/> for larger exponents.
/// </remarks>
internal static class Factor {
    /// <summary>
    /// Pre-computed powers of 10 from 10^0 to 10^9.
    /// </summary>
    private static readonly uint[] _factors = [1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000];

    /// <summary>
    /// Returns the power of 10 for the specified precision level.
    /// </summary>
    /// <param name="precision">
    /// The exponent for the base 10 (i.e., the number of decimal places).
    /// </param>
    /// <returns>
    /// The value of 10 raised to the power of <paramref name="precision"/> as a <see cref="uint"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="precision"/> is between 0 and 9 (inclusive), the method returns a cached value
    /// for optimal performance. For other values, the result is computed using <see cref="Math.Pow"/>.
    /// The calculation is performed in a checked context to ensure that arithmetic overflow is detected.
    /// </remarks>
    /// <exception cref="OverflowException">
    /// Thrown if the computed value exceeds <see cref="uint.MaxValue"/>.
    /// </exception>
    public static uint Get(uint precision) {
        checked {
            return precision < _factors.Length ? _factors[precision] : (uint)Math.Pow(10, precision);
        }
    }
}