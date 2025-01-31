//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using Cloudikka.PolylineAlgorithm.Encoding;
using PolylineAlgorithm;
using PolylinerNet;

[RankColumn]
[MemoryDiagnoser]
[MarkdownExporter]
[Orderer(SummaryOrderPolicy.Declared)]
public class DecodeBenchmark {
    private readonly Consumer _consumer = new();
    public static string String_Polyline { get; } = "}adrJh}}cVazlw@uykyNhaqeE`vfzG_~kY}~`eTsr{~Cwn~aOty_g@thapJvvoqKxt{sStfahDmtvmIfmiqBhjq|HujpgComs{Z}dhdKcidPymnvBqmquE~qrfI`x{lPf|ftGn~}d_@q}saAurjmu@bwr_DxrfaK~{rO~bidPwfduXwlioFlpum@twvfFpmi~VzxcsOqyejYhh|i@pbnr[twvfF_ueUujvbSa_d~ZkcnjZla~f[pmquEebxo[j}nr@xnn|H{gyiKbh{yH`oenn@y{mpIrbd~EmipgH}fuov@hjqtTp|flAttvkFrym_d@|eyCwn~aOfvdNmeawM??{yxdUcidPca{}D_atqGenzcAlra{@trgWhn{aZ??tluqOgu~sH";
    public static ReadOnlyMemory<char> Memory_Polyline { get; } = String_Polyline.AsMemory();


    [Benchmark(Baseline = true)]
    public void PolylineDecoder_Decode() {
        ReadOnlySpan<char> polyline = Memory_Polyline.Span;
        var decoder = new PolylineDecoder();
        decoder.Decode(in polyline).Consume(_consumer);
    }


    [Benchmark]
    public void Cloudikka_PolylineEncoding_Decode() {
        string polyline = String_Polyline;
        var polyliner = new PolylineEncoding();
        polyliner.Decode(polyline).Consume(_consumer);
    }

    [Benchmark]
    public void Polyliner_Decode() {
        string polyline = String_Polyline;
        var polyliner = new Polyliner();
        polyliner.Decode(polyline).Consume(_consumer);
    }
}
