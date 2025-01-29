//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Cloudikka.PolylineAlgorithm.Encoding;
using PolylineAlgorithm;
using PolylinerNet;
using System.Collections.Generic;

[RankColumn]
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[Orderer(SummaryOrderPolicy.Default)]
public class EncodeBenchmark {
    public static IEnumerable<Coordinate> PolylineAlgorithm_Coordinates { get; } = [new(60.81071, -121.40005), new(70.05664, -38.43130), new(37.52379, -84.83755), new(41.85003, 26.25620), new(68.04709, 110.63120), new(61.48922, 50.16245), new(-4.46018, -58.11880), new(-32.16061, -3.27505), new(-50.89185, -55.30630), new(-28.52070, 90.94370), new(35.26009, 93.75620), new(54.83622, 128.91245), new(1.16022, 37.50620), new(-44.26398, -131.24380), new(-33.34325, 154.22495), new(-59.65879, 90.94370), new(-62.38215, 0.94370), new(72.32117, 40.31870), new(64.66910, 2.34995), new(-61.04971, -84.83755), new(77.10238, -91.86880), new(-72.88859, -129.83755), new(-69.24987, -24.36880), new(77.41254, 119.06870), new(-70.69409, 83.91245), new(78.85650, 75.47495), new(26.83989, 140.16245), new(-24.75069, -108.74380), new(30.53968, -145.30630), new(79.12503, 145.78745), new(-34.51006, 133.13120), new(-73.29753, -60.93130), new(-74.08712, 23.44370), new(-76.57404, 100.78745), new(-76.57404, 100.78745), new(39.72082, 103.59995), new(70.99412, 148.59995), new(82.27591, 138.75620), new(78.29964, -3.27505), new(78.29964, -3.27505), new(-8.65039, 47.34995)];
    public static List<PolylinePoint> PolylinerNet_PolylinePoint { get; } = [new(60.81071, -121.40005), new(70.05664, -38.43130), new(37.52379, -84.83755), new(41.85003, 26.25620), new(68.04709, 110.63120), new(61.48922, 50.16245), new(-4.46018, -58.11880), new(-32.16061, -3.27505), new(-50.89185, -55.30630), new(-28.52070, 90.94370), new(35.26009, 93.75620), new(54.83622, 128.91245), new(1.16022, 37.50620), new(-44.26398, -131.24380), new(-33.34325, 154.22495), new(-59.65879, 90.94370), new(-62.38215, 0.94370), new(72.32117, 40.31870), new(64.66910, 2.34995), new(-61.04971, -84.83755), new(77.10238, -91.86880), new(-72.88859, -129.83755), new(-69.24987, -24.36880), new(77.41254, 119.06870), new(-70.69409, 83.91245), new(78.85650, 75.47495), new(26.83989, 140.16245), new(-24.75069, -108.74380), new(30.53968, -145.30630), new(79.12503, 145.78745), new(-34.51006, 133.13120), new(-73.29753, -60.93130), new(-74.08712, 23.44370), new(-76.57404, 100.78745), new(-76.57404, 100.78745), new(39.72082, 103.59995), new(70.99412, 148.59995), new(82.27591, 138.75620), new(78.29964, -3.27505), new(78.29964, -3.27505), new(-8.65039, 47.34995)];
    public static IEnumerable<(double, double)> Cloudikka_PolylineEncoding_Tuple { get; } = [new(60.81071, -121.40005), new(70.05664, -38.43130), new(37.52379, -84.83755), new(41.85003, 26.25620), new(68.04709, 110.63120), new(61.48922, 50.16245), new(-4.46018, -58.11880), new(-32.16061, -3.27505), new(-50.89185, -55.30630), new(-28.52070, 90.94370), new(35.26009, 93.75620), new(54.83622, 128.91245), new(1.16022, 37.50620), new(-44.26398, -131.24380), new(-33.34325, 154.22495), new(-59.65879, 90.94370), new(-62.38215, 0.94370), new(72.32117, 40.31870), new(64.66910, 2.34995), new(-61.04971, -84.83755), new(77.10238, -91.86880), new(-72.88859, -129.83755), new(-69.24987, -24.36880), new(77.41254, 119.06870), new(-70.69409, 83.91245), new(78.85650, 75.47495), new(26.83989, 140.16245), new(-24.75069, -108.74380), new(30.53968, -145.30630), new(79.12503, 145.78745), new(-34.51006, 133.13120), new(-73.29753, -60.93130), new(-74.08712, 23.44370), new(-76.57404, 100.78745), new(-76.57404, 100.78745), new(39.72082, 103.59995), new(70.99412, 148.59995), new(82.27591, 138.75620), new(78.29964, -3.27505), new(78.29964, -3.27505), new(-8.65039, 47.34995)];


    [Benchmark(Baseline = true)]
    public ReadOnlySpan<char> PolylineEncoder_Encode() {
        var encoder = new PolylineEncoder();
        return encoder.Encode(PolylineAlgorithm_Coordinates);
    }

    [Benchmark]
    public string Polyliner_Encode() {
        var polyliner = new Polyliner();
        return polyliner.Encode(PolylinerNet_PolylinePoint);
    }

    [Benchmark]
    public string Cloudikka_PolylineEncoding_Encode() {
        var polyliner = new PolylineEncoding();
        return polyliner.Encode(Cloudikka_PolylineEncoding_Tuple);
    }
}
