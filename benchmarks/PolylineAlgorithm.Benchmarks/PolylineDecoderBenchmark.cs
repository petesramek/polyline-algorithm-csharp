//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using PolylineAlgorithm;
using PolylineAlgorithm.Benchmarks.Internal;

[Orderer(SummaryOrderPolicy.Declared)]
[BenchmarkCategory(BenchmarkCategory.PublicApi)]
//[CategoriesColumn]
//[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();
    public static Polyline Polyline { get; } = (Polyline)"}adrJh}}cVazlw@uykyNhaqeE`vfzG_~kY}~`eTsr{~Cwn~aOty_g@thapJvvoqKxt{sStfahDmtvmIfmiqBhjq|HujpgComs{Z}dhdKcidPymnvBqmquE~qrfI`x{lPf|ftGn~}d_@q}saAurjmu@bwr_DxrfaK~{rO~bidPwfduXwlioFlpum@twvfFpmi~VzxcsOqyejYhh|i@pbnr[twvfF_ueUujvbSa_d~ZkcnjZla~f[pmquEebxo[j}nr@xnn|H{gyiKbh{yH`oenn@y{mpIrbd~EmipgH}fuov@hjqtTp|flAttvkFrym_d@|eyCwn~aOfvdNmeawM??{yxdUcidPca{}D_atqGenzcAlra{@trgWhn{aZ??tluqOgu~sH";

    [Benchmark]
    public void PolylineDecoder_Decode() {
        Polyline polyline = Polyline;

        var decoder = new PolylineDecoder();

        decoder
            .Decode(in polyline)
            .Consume(_consumer);
    }
}
