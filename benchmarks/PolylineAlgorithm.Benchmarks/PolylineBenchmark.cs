//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;

[RankColumn]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net50, baseline: true)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Declared)]
public class PolylineBenchmark {
    private readonly Consumer _consumer = new();

    public DefaultPolylineDecoder Decoder { get; } = new DefaultPolylineDecoder();
    public Polyline Polyline { get; } = new Polyline("}adrJh}}cVazlw@uykyNhaqeE`vfzG_~kY}~`eTsr{~Cwn~aOty_g@thapJvvoqKxt{sStfahDmtvmIfmiqBhjq|HujpgComs{Z}dhdKcidPymnvBqmquE~qrfI`x{lPf|ftGn~}d_@q}saAurjmu@bwr_DxrfaK~{rO~bidPwfduXwlioFlpum@twvfFpmi~VzxcsOqyejYhh|i@pbnr[twvfF_ueUujvbSa_d~ZkcnjZla~f[pmquEebxo[j}nr@xnn|H{gyiKbh{yH`oenn@y{mpIrbd~EmipgH}fuov@hjqtTp|flAttvkFrym_d@|eyCwn~aOfvdNmeawM??{yxdUcidPca{}D_atqGenzcAlra{@trgWhn{aZ??tluqOgu~sH");

    [Benchmark]
    public Span<Coordinate> Decode()
        => Decoder
        .Decode(Polyline);

    public DefaultPolylineEncoder Encoder { get; } = new DefaultPolylineEncoder();

    public IEnumerable<Coordinate> Coordinates { get; } = [Coordinate.Create(60.81071, -121.40005), Coordinate.Create(70.05664, -38.43130), Coordinate.Create(37.52379, -84.83755), Coordinate.Create(41.85003, 26.25620), Coordinate.Create(68.04709, 110.63120), Coordinate.Create(61.48922, 50.16245), Coordinate.Create(-4.46018, -58.11880), Coordinate.Create(-32.16061, -3.27505), Coordinate.Create(-50.89185, -55.30630), Coordinate.Create(-28.52070, 90.94370), Coordinate.Create(35.26009, 93.75620), Coordinate.Create(54.83622, 128.91245), Coordinate.Create(1.16022, 37.50620), Coordinate.Create(-44.26398, -131.24380), Coordinate.Create(-33.34325, 154.22495), Coordinate.Create(-59.65879, 90.94370), Coordinate.Create(-62.38215, 0.94370), Coordinate.Create(72.32117, 40.31870), Coordinate.Create(64.66910, 2.34995), Coordinate.Create(-61.04971, -84.83755), Coordinate.Create(77.10238, -91.86880), Coordinate.Create(-72.88859, -129.83755), Coordinate.Create(-69.24987, -24.36880), Coordinate.Create(77.41254, 119.06870), Coordinate.Create(-70.69409, 83.91245), Coordinate.Create(78.85650, 75.47495), Coordinate.Create(26.83989, 140.16245), Coordinate.Create(-24.75069, -108.74380), Coordinate.Create(30.53968, -145.30630), Coordinate.Create(79.12503, 145.78745), Coordinate.Create(-34.51006, 133.13120), Coordinate.Create(-73.29753, -60.93130), Coordinate.Create(-74.08712, 23.44370), Coordinate.Create(-76.57404, 100.78745), Coordinate.Create(-76.57404, 100.78745), Coordinate.Create(39.72082, 103.59995), Coordinate.Create(70.99412, 148.59995), Coordinate.Create(82.27591, 138.75620), Coordinate.Create(78.29964, -3.27505), Coordinate.Create(78.29964, -3.27505), Coordinate.Create(-8.65039, 47.34995)];


    [Benchmark]
    public Polyline Encode()
        => Encoder
        .Encode(Coordinates);
}
