namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;
using CommandLine;
using Perfolizer.Mathematics.Sequences;
using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Utility;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static PolylineAlgorithm.Internal.Defaults.Polyline.Block;

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

    [Benchmark]
    public void Foreach_Structure_Validation() => ValidateStructureForeach(polyline);

    [Benchmark]
    public void Vectorized_Structure_Validation_3() => ValidateStructureVectorized(polyline);

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
    public static void ValidateStructureForeach(ReadOnlySpan<char> polyline) {
        // 1. SIMD character check (reuse existing method)
        ValidateVectorized(polyline);

        // 2. Block structure check
        int blockLen = 0;
        bool foundBlockEnd = false;

        for (int i = 0; i < polyline.Length; i++) {
            blockLen++;

            if (polyline[i] < End) {
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

    public static void ValidateStructureVectorized(ReadOnlySpan<char> polyline) {
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
                    if (character < Min || character > Max) {
                        throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
                    }
                }
            }

            for (int j = 0; j < vectorSize; j++) {
                if (slice[j] != 0) {
                    int globalIndex = i + j;
                    if (globalIndex - blockEndIndex > 7) {
                        throw new ArgumentException($"Block at position {blockEndIndex + 1} exceeds 7 characters.", nameof(polyline));
                    }
                    blockEndIndex = globalIndex;
                }
            }
        }

        for (; i < length; i++) {
            char character = polyline[i];
            if (character < Min || character > Max) {
                throw new ArgumentException($"Polyline contains invalid character '{character}'.", nameof(polyline));
            }
            if (character < End) {
                if (i - blockEndIndex > 7) {
                    throw new ArgumentException($"Block at position {blockEndIndex + 1} exceeds 7 characters.", nameof(polyline));
                }
                blockEndIndex = i;
            }
        }

        if (blockEndIndex != polyline.Length - 1) {
            throw new ArgumentException("Polyline does not end with a valid block terminator.", nameof(polyline));
        }
    }

    private static readonly ushort Min = Defaults.Algorithm.QuestionMark;
    private static readonly ushort Max = (ushort)(Defaults.Algorithm.QuestionMark * 2);
    private static readonly ushort End = (ushort)(Defaults.Algorithm.QuestionMark + Defaults.Algorithm.Space);

    private static readonly Vector<ushort> MinVector = new Vector<ushort>(Min);
    private static readonly Vector<ushort> MaxVector = new Vector<ushort>(Max);
}