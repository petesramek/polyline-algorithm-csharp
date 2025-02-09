//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

internal class Program {
    static void Main(string[] args) {
        var config = DefaultConfig.Instance
            .AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig(true)))
            .WithOptions(ConfigOptions.JoinSummary);

        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, DefaultConfig.Instance);
    }
}
