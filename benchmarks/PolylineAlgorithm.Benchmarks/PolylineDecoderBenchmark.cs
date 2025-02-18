//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;

[RankColumn]
public class PolylineDecoderBenchmark {
    private readonly Consumer _consumer = new();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string StringValue { get; private set; }
    public char[] CharArray { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public ReadOnlyMemory<char> Memory { get; private set; }


    public PolylineDecoder Decoder = new();

    [GlobalSetup]
    public void SetupData() {
        StringValue = @"sq_|Fptm{UrbxoHinoiEuhmbHctjfc@`rocCt|nfJgiauLvc_uIh`pg_@h_fkVgctZoyqfW{rxgQhhymFpuvvDwqcfTlqbcBiegdTwf{uNngc|z@{vhlDnsi_B~nz`O}d_hNp{n`E}kcoKm`bCiul~\jhg|Hv{qoWshzh\{lf_G`pzMqm|bLzswoQbhcm`@b`}cIgignPgxntR|}vo^f~|}L}|_jBa|ujWuxkjQfj|w[wsz`Pmdb{XohnwA`srxWjitms@c_~`Tava_l@jxxcF`d|zHcpnaMnd{kYzccwPxzpiHfsxlL}jjjQqvdbIikyvj@cjdqTh`zoDzqleHnfmik@tbnoB_t}gFkzx_Ct_eiP`wxrBcd_|w@zlhfPtlxgBkwyyZn|~tFlpj|HxqiwUnddkFoo|nTee}dSfkcg`@py`uQiguom@zkkpEfcgkAntuuDzl~il@ir_gCrd~nI_ryeC_qmmMl_kgCz`qgFzkejBmlchYyp`hZ`_cuYzuc}Onqz}Ew~Gcsmj@lp{Hqj_gz@ne{pJnny~]g{tuNxbno[lfq_Lqhwjt@qn|cCuxnMyivuIh{|tR`ylsQlqbfO}rf`LxghfBg~{nAv`gdNbjh_Fglt|NfxwyBowwhW{bdtNdbkqe@rxtwSy{_fX{btm@va`_LkhwuUyqgzK`xdnKgbwsFigt_Mofdn\h|x[ccoPtbpvNz}skb@pl~xEqascV_wsx[`f_z]zewFs`zjAhturWxayhJqmfaAjmhhHxwuwF_aru@ojemVq|beu@kkucBdmryTevflDcbmdAnp|dHfpbd^io}z@e~}dFzcybQ}`hxOyt|bNl}blLnuspKk|t|k@itjfHt}}aVyzmcF_rgmLct}x@bazdq@loajBxygb@f}krVgnuqOcrx_Daqvp_@ew}yUn}kpU|uwnItashEpe_aHusi{Fsu}_Ewfhv[dzhzKxh_qXucxfXmynkGxuqbW|ppgi@vrsq@clryZk`bt^spkyP";
        CharArray = StringValue.ToCharArray();
        Memory = StringValue.AsMemory();
    }

    [Benchmark]
    public void PolylineDecoder_Decode_FromString() {
        Polyline polyline = Polyline.FromString(StringValue);

        Decoder
            .Decode(in polyline)
            .Consume(_consumer);
    }

    [Benchmark]
    public void PolylineDecoder_Decode_FromCharArray() {
        Polyline polyline = Polyline.FromCharArray(CharArray);

        Decoder
            .Decode(in polyline)
            .Consume(_consumer);
    }

    [Benchmark]
    public void PolylineDecoder_Decode_FromMemory() {
        Polyline polyline = Polyline.FromMemory(Memory);

        Decoder
            .Decode(in polyline)
            .Consume(_consumer);
    }
}
