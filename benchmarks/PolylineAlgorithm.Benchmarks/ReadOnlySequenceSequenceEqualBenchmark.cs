//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm;
using System;
using System.Buffers;
using System.Diagnostics;

/// <summary>
/// Benchmarks for the <see cref="PolylineDecoder"/> class.
/// </summary>
[RankColumn]
[ShortRunJob]
public class ReadOnlySequenceSequenceEqualBenchmark {
    [Params(1, 10, 100, 500, 1_000, 10_000, 100_000, 1_000_000, 10_000_000)]
    public int ArrayLength;

    [ParamsAllValues]
    public SequenceStructure? Structure;

    [ParamsAllValues]
    public bool AllowEmptySequence;

    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public ReadOnlySequence<char> S1 { get; private set; }

    /// <summary>
    /// Gets the string value representing the encoded polyline.
    /// </summary>
    public ReadOnlySequence<char> S2 { get; private set; }

    //[ParamsAllValues]
    //public bool AllowEmpty { get; set; }

    /// <summary>
    /// Sets up the data for the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        var a1 = CreateArray(ArrayLength);

        S1 = CreateSequence(a1, AllowEmptySequence);

        if(Structure == SequenceStructure.EqualSegments) {
            S2 = S1;
        } else if (Structure == SequenceStructure.EqualContents) {
            S2 = CreateSequence(a1, AllowEmptySequence);
        } else {
            S2 = CreateSequence(CreateArray(ArrayLength), AllowEmptySequence);
        }

        static ReadOnlySequence<T> CreateSequence<T>(Memory<T> array, bool allowEmpty = false) {
            ReadOnlySegment<T> initial = null!;
            ReadOnlySegment<T> last = null!;

            int consumed = 0;
            while (consumed < array.Length) {
                var length = Random.Shared.Next(allowEmpty ? 0 : 1, array.Length - consumed + 1);
                var slice = length == 0 ? Memory<T>.Empty : array.Slice(consumed, length);
                var segment = new ReadOnlySegment<T>(slice);

                if (initial is null) {
                    initial = segment;
                    last = segment;
                } else {
                    last.Append(segment);
                    last = segment;
                }

                consumed += length;
            }

            var sequence = new ReadOnlySequence<T>(initial, 0, last, last.Memory.Length);

            return sequence;
        }

        static Memory<char> CreateArray(int length) {
            Memory<char> array = new char[length];

            for (int i = 0; i < length; i++) {

                array.Span[i] = Convert.ToChar(Random.Shared.Next(Char.MinValue, Char.MaxValue + 1));
            }

            return array;
        }
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public bool SequenceEqual_SequenceReader_IsNext_V1() {
        var left = new SequenceReader<char>(S1);
        var right = new SequenceReader<char>(S2);

        while (true) {
            if(!right.IsNext(left.CurrentSpan)) {
                break;
            }

            left.Advance(left.CurrentSpan.Length);
            right.Advance(left.CurrentSpan.Length);

            if (left.Remaining == 0) {
                return true;
            }
        }

        Debug.Assert(false);
        return false;
    }

    /// <summary>
    /// Benchmarks the decoding of a polyline from a string.
    /// </summary>
    [Benchmark]
    public bool SequenceEqual_NerdBank_SpanEnumeration() {
        ReadOnlySequence<char>.Enumerator aEnumerator = S1.GetEnumerator();
        ReadOnlySequence<char>.Enumerator bEnumerator = S2.GetEnumerator();

        ReadOnlySpan<char> aCurrent = default;
        ReadOnlySpan<char> bCurrent = default;

        while (true) {
            bool aNext = TryGetNonEmptySpan(ref aEnumerator, ref aCurrent);
            bool bNext = TryGetNonEmptySpan(ref bEnumerator, ref bCurrent);
            if (!aNext && !bNext) {
                // We've reached the end of both sequences at the same time.
                return true;
            } else if (aNext != bNext) {
                // One ran out of bytes before the other.
                // We don't anticipate this, because we already checked the lengths.
                throw new InvalidOperationException();
            }

            int commonLength = Math.Min(aCurrent.Length, bCurrent.Length);
            if (!aCurrent[..commonLength].SequenceEqual(bCurrent[..commonLength])) {
                Debug.Assert(false);

                return false;
            }

            aCurrent = aCurrent.Slice(commonLength);
            bCurrent = bCurrent.Slice(commonLength);
        }

        static bool TryGetNonEmptySpan(ref ReadOnlySequence<char>.Enumerator enumerator, ref ReadOnlySpan<char> span) {
            while (span.Length == 0) {
                if (!enumerator.MoveNext()) {
                    return false;
                }

                span = enumerator.Current.Span;
            }

            return true;
        }
    }

    private class ReadOnlySegment<T> : ReadOnlySequenceSegment<T> {
        public ReadOnlySegment(ReadOnlyMemory<T> memory, long runningIndex = 0) {
            Memory = memory;
            RunningIndex = runningIndex;
        }

        public void Append(ReadOnlyMemory<T> memory) {
            Append(new ReadOnlySegment<T>(memory));
        }

        public void Append(ReadOnlySegment<T> next) {
            next.RunningIndex = RunningIndex + Memory.Length;
            Next = next;
        }
    }

    public enum SequenceStructure {
        EqualSegments = 0,
        EqualContents = 1,
    }
}