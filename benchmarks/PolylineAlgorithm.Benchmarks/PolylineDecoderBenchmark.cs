//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks {
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using BenchmarkDotNet.Order;
    using PolylineAlgorithm.Internal;

    [MemoryDiagnoser]
    [MarkdownExporter]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class PolylineDecoderBenchmark {
        private readonly Consumer _consumer = new();

        public PolylineDecoder Decoder { get; set; }

        public static IEnumerable<string> GetPolylines() {
            yield return "mz}lHssngJj`gqSnx~lEcovfTnms{Zdy~qQj_deI";
            yield return "}vwdGjafcRsvjKi}pxUhsrtCngtcAjjgzEdqvtLrscbKj}nr@wetlUc`nq]}_kfCyrfaK~wluUl`u}|@wa{lUmmuap@va{lU~oihCu||bF`|era@wsnnIjny{DxamaScqxza@dklDf{}kb@mtpeCavfzGqhx`Wyzzkm@jm`d@dba~Pppkg@h}pxU|rtnHp|flA|~xaPuykyN}fhv[h}pxUx~p}Ymx`sZih~iB{edwB";
            yield return "}adrJh}}cVazlw@uykyNhaqeE`vfzG_~kY}~`eTsr{~Cwn~aOty_g@thapJvvoqKxt{sStfahDmtvmIfmiqBhjq|HujpgComs{Z}dhdKcidPymnvBqmquE~qrfI`x{lPf|ftGn~}d_@q}saAurjmu@bwr_DxrfaK~{rO~bidPwfduXwlioFlpum@twvfFpmi~VzxcsOqyejYhh|i@pbnr[twvfF_ueUujvbSa_d~ZkcnjZla~f[pmquEebxo[j}nr@xnn|H{gyiKbh{yH`oenn@y{mpIrbd~EmipgH}fuov@hjqtTp|flAttvkFrym_d@|eyCwn~aOfvdNmeawM??{yxdUcidPca{}D_atqGenzcAlra{@trgWhn{aZ??tluqOgu~sH";
        }

        [GlobalSetup]
        public void Setup() {
            Decoder = new PolylineDecoder();
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetPolylines))]
        public void Decode(string polyline) => Decoder
            .Decode(polyline)
            .Consume(_consumer);
    }
}
