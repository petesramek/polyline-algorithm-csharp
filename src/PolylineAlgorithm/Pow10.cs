namespace PolylineAlgorithm;

using System;

/// <summary>
/// Provides optimized calculation of powers of 10 for precision-based operations.
/// </summary>
/// <remarks>
/// This class caches common powers of 10 (10^0 through 10^9) for efficient lookup,
/// falling back to <see cref="Math.Pow"/> for larger exponents.
/// </remarks>
public static class Pow10 {
    /// <summary>
    /// Pre-computed powers of 10 from 10^0 to 10^9.
    /// </summary>
    private static readonly uint[] _pow10 = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

    /// <summary>
    /// Gets the multiplication factor for a given precision level.
    /// </summary>
    /// <param name="precision">The precision level (exponent for base 10).</param>
    /// <returns>
    /// The power of 10 corresponding to the specified precision (10^precision).
    /// </returns>
    /// <remarks>
    /// For precision values 0-9, returns pre-computed values for optimal performance.
    /// For larger values, computes the result using <see cref="Math.Pow"/>.
    /// The operation is performed in a checked context to detect arithmetic overflow.
    /// </remarks>
    /// <exception cref="OverflowException">
    /// Thrown when the computed power of 10 exceeds <see cref="uint.MaxValue"/>.
    /// </exception>
    public static uint GetFactor(uint precision) {
        checked {
            return precision < _pow10.Length ? _pow10[precision] : (uint)Math.Pow(10, precision);
        }
    }
}
