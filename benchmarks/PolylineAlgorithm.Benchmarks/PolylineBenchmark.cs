//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PolylineAlgorithm;

[RankColumn]
[MemoryDiagnoser]
[MarkdownExporter]
[Orderer(SummaryOrderPolicy.Declared)]
public class PolylineBenchmark {
    private static class Input {
        public static string String { get; } = "}adrJh}}cVazlw@uykyNhaqeE`vfzG_~kY}~`eTsr{~Cwn~aOty_g@thapJvvoqKxt{sStfahDmtvmIfmiqBhjq|HujpgComs{Z}dhdKcidPymnvBqmquE~qrfI`x{lPf|ftGn~}d_@q}saAurjmu@bwr_DxrfaK~{rO~bidPwfduXwlioFlpum@twvfFpmi~VzxcsOqyejYhh|i@pbnr[twvfF_ueUujvbSa_d~ZkcnjZla~f[pmquEebxo[j}nr@xnn|H{gyiKbh{yH`oenn@y{mpIrbd~EmipgH}fuov@hjqtTp|flAttvkFrym_d@|eyCwn~aOfvdNmeawM??{yxdUcidPca{}D_atqGenzcAlra{@trgWhn{aZ??tluqOgu~sH";
        public static char[] CharArray { get; } = String.ToCharArray();
        public static ReadOnlyMemory<char> Memory { get; } = new Memory<char>(CharArray);
    }

    [Benchmark(Baseline = true)]
    public Polyline Polyline_Constructor_Empty() {
        var polyline = new Polyline();
        return polyline;
    }


    [Benchmark]
    public Polyline Polyline_Constructor_String() {
        var value = Input.String;
        var polyline = new Polyline(value);
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_Constructor_CharArray() {
        var value = Input.CharArray;
        var polyline = new Polyline(value);
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_Constructor_Memory() {
        var value = Input.Memory;
        var polyline = new Polyline(value);
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_Implicit_Conversion_String() {
        Polyline polyline = Input.String;
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_Implicit_Conversion_CharArray() {
        Polyline polyline = Input.CharArray;
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_Implicit_Conversion_Memory() {
        Polyline polyline = Input.Memory;
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_FromString() {
        var value = Input.String;
        Polyline polyline = Polyline.FromString(in value);
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_FromCharArray() {
        var value = Input.CharArray;
        Polyline polyline = Polyline.FromCharArray(in value);
        return polyline;
    }

    [Benchmark]
    public Polyline Polyline_FromMemory() {
        var value = Input.Memory;
        Polyline polyline = Polyline.FromMemory(in value);
        return polyline;
    }
}
