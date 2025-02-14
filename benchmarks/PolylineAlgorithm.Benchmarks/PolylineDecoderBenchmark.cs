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
[CategoriesColumn]
[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();
    public static Polyline Polyline { get; } = (Polyline)@"sq_|Fptm{UrbxoHinoiEuhmbHctjfc@`rocCt|nfJgiauLvc_uIh`pg_@h_fkVgctZoyqfW{rxgQhhymFpuvvDwqcfTlqbcBiegdTwf{uNngc|z@{vhlDnsi_B~nz`O}d_hNp{n`E}kcoKm`bCiul~\jhg|Hv{qoWshzh\{lf_G`pzMqm|bLzswoQbhcm`@b`}cIgignPgxntR|}vo^f~|}L}|_jBa|ujWuxkjQfj|w[wsz`Pmdb{XohnwA`srxWjitms@c_~`Tava_l@jxxcF`d|zHcpnaMnd{kYzccwPxzpiHfsxlL}jjjQqvdbIikyvj@cjdqTh`zoDzqleHnfmik@tbnoB_t}gFkzx_Ct_eiP`wxrBcd_|w@zlhfPtlxgBkwyyZn|~tFlpj|HxqiwUnddkFoo|nTee}dSfkcg`@py`uQiguom@zkkpEfcgkAntuuDzl~il@ir_gCrd~nI_ryeC_qmmMl_kgCz`qgFzkejBmlchYyp`hZ`_cuYzuc}Onqz}Ew~Gcsmj@lp{Hqj_gz@ne{pJnny~]g{tuNxbno[lfq_Lqhwjt@qn|cCuxnMyivuIh{|tR`ylsQlqbfO}rf`LxghfBg~{nAv`gdNbjh_Fglt|NfxwyBowwhW{bdtNdbkqe@rxtwSy{_fX{btm@va`_LkhwuUyqgzK`xdnKgbwsFigt_Mofdn\h|x[ccoPtbpvNz}skb@pl~xEqascV_wsx[`f_z]zewFs`zjAhturWxayhJqmfaAjmhhHxwuwF_aru@ojemVq|beu@kkucBdmryTevflDcbmdAnp|dHfpbd^io}z@e~}dFzcybQ}`hxOyt|bNl}blLnuspKk|t|k@itjfHt}}aVyzmcF_rgmLct}x@bazdq@loajBxygb@f}krVgnuqOcrx_Daqvp_@ew}yUn}kpU|uwnItashEpe_aHusi{Fsu}_Ewfhv[dzhzKxh_qXucxfXmynkGxuqbW|ppgi@vrsq@clryZk`bt^spkyP";
    public PolylineDecoder Decoder = new();

    [Benchmark]
    public void PolylineDecoder_Decode() {
        Polyline polyline = Polyline;

        Decoder
            .Decode(in polyline)
            .Consume(_consumer);
    }
}
