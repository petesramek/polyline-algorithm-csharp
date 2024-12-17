//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks {
    using BenchmarkDotNet.Running;

    internal class Program {
        static void Main(string[] _) {
            BenchmarkRunner
                .Run<PolylineEncoderBenchmark>();
        }
    }
}
