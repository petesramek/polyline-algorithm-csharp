namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Utility;
using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.InteropServices;

[MemoryDiagnoser]
public class PolylineValidationBenchmark {
    private string polyline;

    /// <summary>
    /// Number of coordinates for benchmarks. Set by BenchmarkDotNet.
    /// </summary>
    [Params(128, 1024, 4096, 20480)]
    public int CoordinatesCount { get; set; }

    [GlobalSetup]
    public void Setup() {
        polyline = RandomValueProvider.GetPolyline(CoordinatesCount);
    }

    //[Benchmark]
    //public void Foreach_Char_Validation() => ValidateForeach(polyline);

    //[Benchmark]
    //public void Vectorized_Char_Validation() => ValidateVectorized(polyline);

    [Benchmark]
    public void Foreach_Structure_Validation() => ValidateStructure(polyline);

    [Benchmark]
    public void Foreach_Structure_Validation_2() => ValidateStructure_5(polyline);

    //[Benchmark]
    //public void Vectorized_Structure_Validation_1() => ValidateStructureVectorized_1(polyline);

    //[Benchmark]
    //public void Vectorized_Structure_Validation_2() => ValidateStructureVectorized_2(polyline);

    [Benchmark]
    public void Vectorized_Structure_Validation_3() => ValidateStructureVectorized_3(polyline);

    //private void ValidateForeach(string polyline) {
    //    foreach (char character in polyline) {
    //        if (character < Defaults.Algorithm.QuestionMark || character > Defaults.Algorithm.QuestionMark * 2)
    //            throw new ArgumentException("Invalid character");
    //    }
    //}

    public static void ValidateVectorized(ReadOnlySpan<char> polyline) {
        int length = polyline.Length;
        int vectorSize = Vector<ushort>.Count;

        const ushort min = Defaults.Algorithm.QuestionMark;
        const ushort max = min * 2; // 126 the upper limit.

        // Create constant vectors once
        var minVector = new Vector<ushort>(min);
        var maxVector = new Vector<ushort>(max);

        int i = 0;
        for (; i <= length - vectorSize; i += vectorSize) {
            var span = MemoryMarshal.Cast<char, ushort>(polyline.Slice(i, vectorSize));
#if NET5_0_OR_GREATER
            var chars = new Vector<ushort>(span);
#else
                var chars = new Vector<ushort>(span.ToArray());
#endif
            var belowMin = Vector.LessThan(chars, minVector);
            var aboveMax = Vector.GreaterThan(chars, maxVector);
            if (Vector.BitwiseOr(belowMin, aboveMax) != Vector<ushort>.Zero) {
                // Fallback to scalar check for this block
                for (int j = 0; j < vectorSize; j++) {
                    char character = polyline[i + j];
                    if (character < min || character > max)
                        throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
                }
            }
        }

        for (; i < length; i++) {
            char character = polyline[i];
            if (character < min || character > max)
                throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
        }
    }

    /// <summary>
    /// Validates that a polyline is structurally correct:
    /// - All characters are in the allowed range
    /// - No block exceeds 7 characters
    /// - Polyline ends with a block terminator
    /// </summary>
    /// <param name="polyline">The polyline to validate.</param>
    /// <exception cref="ArgumentException">Thrown if the polyline is structurally invalid.</exception>
    public static void ValidateStructure(ReadOnlySpan<char> polyline) {
        // 1. SIMD character check (reuse existing method)
        ValidateVectorized(polyline);

        // 2. Block structure check
        int blockLen = 0;
        bool foundBlockEnd = false;

        for (int i = 0; i < polyline.Length; i++) {
            int value = polyline[i] - Defaults.Algorithm.QuestionMark;
            blockLen++;

            if (value < Defaults.Algorithm.Space) {
                foundBlockEnd = true;
                if (blockLen > 7)
                    throw new ArgumentException($"Block at position {i - blockLen + 1} exceeds 7 characters.", nameof(polyline));
                blockLen = 0;
            } else {
                foundBlockEnd = false;
            }
        }

        if (!foundBlockEnd)
            throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
    }

    //    public static void ValidateStructureVectorized_1(ReadOnlySpan<char> polyline) {
    //        // 1. SIMD character check (reuse existing method)
    //        ValidateVectorized(polyline);

    //        // 2. Vectorized block end search
    //        int length = polyline.Length;
    //        int vectorSize = Vector<ushort>.Count;
    //        int blockLen = 0;
    //        bool foundBlockEnd = false;

    //        var space = new Vector<ushort>(Defaults.Algorithm.Space);
    //        var qmark = Defaults.Algorithm.QuestionMark;

    //        int i = 0;
    //        for (; i <= length - vectorSize; i += vectorSize) {
    //            var span = MemoryMarshal.Cast<char, ushort>(polyline.Slice(i, vectorSize));
    //#if NET5_0_OR_GREATER
    //            var chars = new Vector<ushort>(span);
    //#else
    //        var chars = new Vector<ushort>(span.ToArray());
    //#endif
    //            // Subtract question mark from all
    //            var adjusted = chars - new Vector<ushort>(qmark);
    //            // Find block ends: adjusted < space
    //            var isBlockEnd = Vector.LessThan(adjusted, space);

    //            for (int j = 0; j < vectorSize; j++) {
    //                blockLen++;
    //                if (isBlockEnd[j] != 0) {
    //                    foundBlockEnd = true;
    //                    if (blockLen > 7)
    //                        throw new ArgumentException($"Block at position {i + j - blockLen + 1} exceeds 7 characters.", nameof(polyline));
    //                    blockLen = 0;
    //                } else {
    //                    foundBlockEnd = false;
    //                }
    //            }
    //        }

    //        // Scalar tail
    //        for (; i < length; i++) {
    //            int value = polyline[i] - qmark;
    //            blockLen++;
    //            if (value < Defaults.Algorithm.Space) {
    //                foundBlockEnd = true;
    //                if (blockLen > 7)
    //                    throw new ArgumentException($"Block at position {i - blockLen + 1} exceeds 7 characters.", nameof(polyline));
    //                blockLen = 0;
    //            } else {
    //                foundBlockEnd = false;
    //            }
    //        }

    //        if (!foundBlockEnd)
    //            throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
    //    }

    //    public static void ValidateStructureVectorized_2(ReadOnlySpan<char> polyline) {
    //        // 1. SIMD character check (reuse existing method)
    //        ValidateVectorized(polyline);

    //        // 2. SIMD block end detection
    //        int length = polyline.Length;
    //        int vectorSize = Vector<ushort>.Count;
    //        var space = new Vector<ushort>(Defaults.Algorithm.Space);
    //        var qmark = Defaults.Algorithm.QuestionMark;

    //        // Over-allocate for worst case (every char is a block end)
    //        int[] blockEnds = new int[length];
    //        int blockEndCount = 0;

    //        int i = 0;
    //        for (; i <= length - vectorSize; i += vectorSize) {
    //            var span = MemoryMarshal.Cast<char, ushort>(polyline.Slice(i, vectorSize));
    //#if NET5_0_OR_GREATER
    //            var chars = new Vector<ushort>(span);
    //#else
    //        var chars = new Vector<ushort>(span.ToArray());
    //#endif
    //            var adjusted = chars - new Vector<ushort>(qmark);
    //            var isBlockEnd = Vector.LessThan(adjusted, space);

    //            for (int j = 0; j < vectorSize; j++) {
    //                if (isBlockEnd[j] != 0) {
    //                    blockEnds[blockEndCount++] = i + j;
    //                }
    //            }
    //        }
    //        // Scalar tail
    //        for (; i < length; i++) {
    //            int value = polyline[i] - qmark;
    //            if (value < Defaults.Algorithm.Space) {
    //                blockEnds[blockEndCount++] = i;
    //            }
    //        }

    //        // 3. Validate block lengths using block end positions
    //        int lastEnd = -1;
    //        for (int idx = 0; idx < blockEndCount; idx++) {
    //            int end = blockEnds[idx];
    //            int blockLen = end - lastEnd;
    //            if (blockLen > 7)
    //                throw new ArgumentException($"Block at position {lastEnd + 1} exceeds 7 characters.", nameof(polyline));
    //            lastEnd = end;
    //        }
    //        if (lastEnd != polyline.Length - 1)
    //            throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
    //    }

    public static void ValidateStructureVectorized_3(ReadOnlySpan<char> polyline) {
        int length = polyline.Length;
        int vectorSize = Vector<ushort>.Count;
        int blockEndIndex = -1;
        var span = MemoryMarshal.Cast<char, ushort>(polyline);
        int i = 0;
        for (; i <= length - vectorSize; i += vectorSize) {
            var slice = span.Slice(i, vectorSize);

#if NET5_0_OR_GREATER
            var chars = new Vector<ushort>(slice);
#else
            var chars = new Vector<ushort>(slice.ToArray());
#endif
            var belowMin = Vector.LessThan(chars, MinVector);
            var aboveMax = Vector.GreaterThan(chars, MaxVector);
            if (Vector.BitwiseOr(belowMin, aboveMax) != Vector<ushort>.Zero) {
                for (int j = 0; j < vectorSize; j++) {
                    char character = polyline[i + j];
                    if (character < Min || character > Max)
                        throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
                }
            }

            var isBlockEnd = Vector.LessThan(chars, BlockEndVector);
            for (int j = 0; j < vectorSize; j++) {
                if (isBlockEnd[j] != 0) {
                    int globalIndex = i + j;
                    if (globalIndex - blockEndIndex > 7)
                        throw new ArgumentException($"Block at position {blockEndIndex + 1} exceeds 7 characters.", nameof(polyline));
                    blockEndIndex = globalIndex;
                }
            }
        }

        for (; i < length; i++) {
            char character = polyline[i];
            if (character < Min || character > Max)
                throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
            int value = character - Min;
            if (value < Defaults.Algorithm.Space) {
                if (i - blockEndIndex > 7)
                    throw new ArgumentException($"Block at position {blockEndIndex + 1} exceeds 7 characters.", nameof(polyline));
                blockEndIndex = i;
            }
        }

        if (blockEndIndex != polyline.Length - 1)
            throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
    }

    public static void ValidateStructure_5(ReadOnlySpan<char> polyline) {
        int length = polyline.Length;
        int vectorSize = Vector<ushort>.Count;
        int blockLen = 0;
        bool foundBlockEnd = false;
        int lastVectorized = 0;

        for (int i = 0; i < length; i++) {
            int value = polyline[i] - Defaults.Algorithm.QuestionMark;
            blockLen++;

            // Every time we reach a vector boundary, process that chunk
            if ((i + 1 - lastVectorized) == vectorSize) {
                ValidateVectorized(polyline.Slice(lastVectorized, vectorSize));
                lastVectorized = i + 1;
            }

            if (polyline[i] < Min || polyline[i] > Max)
                throw new ArgumentException($"Polyline contains invalid character '{polyline[i]}'.", nameof(polyline));

            if (value < Defaults.Algorithm.Space) {
                foundBlockEnd = true;
                if (blockLen > 7)
                    throw new ArgumentException($"Block at position {i - blockLen + 1} exceeds 7 characters.", nameof(polyline));
                blockLen = 0;
            } else {
                foundBlockEnd = false;
            }
        }

        // Process any remaining unprocessed tail (less than vectorSize)
        int remaining = length - lastVectorized;
        if (remaining > 0) {
            for (int i = lastVectorized; i < length; i++) {
                char character = polyline[i];
                if (character < Min || character > Max)
                    throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
            }
        }

        if (!foundBlockEnd)
            throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
    }

    private static readonly ushort Min = Defaults.Algorithm.QuestionMark;
    private static readonly ushort Max = (ushort)(Defaults.Algorithm.QuestionMark * 2);
    private static readonly Vector<ushort> MinVector = new Vector<ushort>(Min);
    private static readonly Vector<ushort> MaxVector = new Vector<ushort>(Max);
    private static readonly Vector<ushort> BlockEndVector = MinVector + new Vector<ushort>(Defaults.Algorithm.Space);
}