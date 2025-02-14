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
    public static Polyline Polyline { get; } = (Polyline)"|koyD_tmn[|sfDltuii@qjarFkvifi@uepdF~bwdSb{cfSf|y{Nhc}Gfp}~OabanXo{p_t@kfx[rb`yDaiwkAncxm]beulDitjhYqsloAiigeM`uzPhtkxDqiaT`pplw@jzfvXx~i|B_dxuC_sk{[uqktKstyl@pra`BuwasUmvjr@rvs{j@x`dcM}tqbe@mkbiJr_zbFlpc}K~rt{BetlmQw`wnQj|ckKobv{Icbw~Bf}p|b@}mcnDa_usb@zkc|G`pgg\\\\ifuhBhbe~Z~ul`NilxbHunrxZvo}qJ`}efQvfaaAty{yDx_kE{vf_Hwns`r@ucq`NzfziVxznnEz`vaD|yzrMkkjf`@{sewCxrv`i@pbrg@cpfeUgulnBsmapKhdbnHsrusCqt}sHvajfl@mygsRl{aoF`|{eQn`hFjhi_Esj_tk@lsfk@}rpgAtggHtgzQprpjBphmn]wacxWdpiyDdtxdAeqtrBjkjAscknk@_egt@s}qiBxp}eUhypv^onrxSnyndPvazxViswuX_efmTvy`gApigfTb`|uB_bacSnxjbHvihiRmrxsGsy{qBfihyIcrexM{{cle@jcjpDzbx`\\\\u~jzEyjseJcw`pEw`rlUpew|JlqqcKj}~uFlmk`GmjudUqj}~Jz|_rAb~bgH|lstUfh|gj@vwygCmlyhWwhbmCc}uzTwvklKam|dDasi{KrmenSpi_Bzkznb@xsdaKcvzsq@~~f{Fz_}}Pq}nrA{qe_\\\\lmw}BxvzwSaqbiBkqnyLumkhF`nnic@mntzEsz_oc@tikmIffhob@rnauKccmp]jmdkDbcorUqkerQ~gby\\\\lfytIml`sSbiyyC_njae@gvrnIjsahTg|}xGuw~pWywyyCjgag|@dphjZgmh~]k}zxYc~ciNdmzwHp}rhB`rkhJrafaIghfvVxdzp`@ffctJetj~b@bz|rFd_oaGqslzMo{djUlwtaSrowsb@czmtQenbnWlkqjS~hhwEwxqc@vnzyP";

    [Benchmark]
    public void PolylineDecoder_Decode() {
        Polyline polyline = Polyline;

        var decoder = new PolylineDecoder();

        decoder
            .Decode(in polyline)
            .Consume(_consumer);
    }
}
