﻿//  
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
    public class PolylineEncoderBenchmark {
        private readonly Consumer _consumer = new();

        public PolylineEncoder Encoder { get; set; }

        public IEnumerable<IEnumerable<(double, double)>> GetCoordinates() {
            yield return new[] { (49.47383, 59.06250), (-58.37407, 25.31250), (52.99363, -120.93750), (-44.49024, -174.37500) };
            yield return new[] { (42.88895, -100.30630), (44.91513, 19.22495), (20.40244, 7.97495), (-15.52130, -63.74380), (-78.95116, -72.18130), (38.63072, 88.13120), (60.81071, 151.41245), (-58.20769, -173.43130), (59.40939, 83.91245), (-58.20769, 61.41245), (-20.86278, -119.99380), (34.10374, -150.93130), (-71.15367, 31.88120), (-72.04138, -153.74380), (-49.99635, -107.33755), (76.12614, 135.94370), (70.05664, 41.72495), (63.43879, -77.80630), (13.68456, -90.46255), (-75.90519, -7.49380), (74.71112, -127.02505), (-66.61109, 17.81870), (-49.08384, 37.50620) };
            yield return new[] { (60.81071, -121.40005), (70.05664, -38.43130), (37.52379, -84.83755), (41.85003, 26.25620), (68.04709, 110.63120), (61.48922, 50.16245), (-4.46018, -58.11880), (-32.16061, -3.27505), (-50.89185, -55.30630), (-28.52070, 90.94370), (35.26009, 93.75620), (54.83622, 128.91245), (1.16022, 37.50620), (-44.26398, -131.24380), (-33.34325, 154.22495), (-59.65879, 90.94370), (-62.38215, 0.94370), (72.32117, 40.31870), (64.66910, 2.34995), (-61.04971, -84.83755), (77.10238, -91.86880), (-72.88859, -129.83755), (-69.24987, -24.36880), (77.41254, 119.06870), (-70.69409, 83.91245), (78.85650, 75.47495), (26.83989, 140.16245), (-24.75069, -108.74380), (30.53968, -145.30630), (79.12503, 145.78745), (-34.51006, 133.13120), (-73.29753, -60.93130), (-74.08712, 23.44370), (-76.57404, 100.78745), (-76.57404, 100.78745), (39.72082, 103.59995), (70.99412, 148.59995), (82.27591, 138.75620), (78.29964, -3.27505), (78.29964, -3.27505), (-8.65039, 47.34995) };
        }

        [GlobalSetup]
        public void Setup() {
            Encoder = new PolylineEncoder();
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetCoordinates))]
        public void Encode(IEnumerable<(double, double)> coordinates) => Encoder
            .Encode(coordinates)
            .Consume(_consumer);
    }
}
