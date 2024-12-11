//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks {
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using PolylineAlgorithm.Encoding;

    [MemoryDiagnoser]
    [MarkdownExporter]
    public class PolylineEncodingBenchmark {
        private Consumer _consumer = new Consumer();

        [Params(1_000, 100_000, 10_000_000, Priority = 2)]
        public int N;

        public required IEnumerable<(double, double)> Coordinates { get; set; }

        public required DefaultPolylineEncoding Encoder { get; set; }

        public required string Polyline { get; set; }

        [GlobalSetup]
        public void Setup() {
            Encoder = new DefaultPolylineEncoding();
            Coordinates = [(42.88895, -100.30630), (44.91513, 19.22495), (20.40244, 7.97495), (-15.52130, -63.74380), (-78.95116, -72.18130), (38.63072, 88.13120), (60.81071, 151.41245), (-58.20769, -173.43130), (59.40939, 83.91245), (-58.20769, 61.41245), (-20.86278, -119.99380), (34.10374, -150.93130), (-71.15367, 31.88120), (-72.04138, -153.74380), (-49.99635, -107.33755), (76.12614, 135.94370), (70.05664, 41.72495), (63.43879, -77.80630), (13.68456, -90.46255), (-75.90519, -7.49380), (74.71112, -127.02505), (-66.61109, 17.81870), (-49.08384, 37.50620)];
            Polyline = "}vwdGjafcRsvjKi}pxUhsrtCngtcAjjgzEdqvtLrscbKj}nr@wetlUc`nq]}_kfCyrfaK~wluUl`u}|@wa{lUmmuap@va{lU~oihCu||bF`|era@wsnnIjny{DxamaScqxza@dklDf{}kb@mtpeCavfzGqhx`Wyzzkm@jm`d@dba~Pppkg@h}pxU|rtnHp|flA|~xaPuykyN}fhv[h}pxUx~p}Ymx`sZih~iB{edwB";
        }

        [Benchmark]
        public void Decode() => Encoder
            .Decode(Polyline)
            .Consume(_consumer);

        [Benchmark]
        public void Encode() => Encoder
            .Encode(Coordinates)
            .Consume(_consumer);
    }
}
