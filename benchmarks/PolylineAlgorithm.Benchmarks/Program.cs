//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

internal class Program {
    static void Main(string[] args) {
        var config = DefaultConfig.Instance
            //.AddJob(Job.Default.WithRuntime(CoreRuntime.Core20))
            //.AddJob(Job.Default.WithRuntime(CoreRuntime.Core21))
            //.AddJob(Job.Default.WithRuntime(CoreRuntime.Core22))
            //.AddJob(Job.Default.WithRuntime(CoreRuntime.Core30))
            //.AddJob(Job.Default.WithRuntime(CoreRuntime.Core31))
            .AddJob(Job.Default.WithRuntime(CoreRuntime.Core50))
            .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60))
            .AddJob(Job.Default.WithRuntime(CoreRuntime.Core70))
            .AddJob(Job.Default.WithRuntime(CoreRuntime.Core80))
            .AddJob(Job.Default.WithRuntime(CoreRuntime.Core90));

        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, config);
    }
}
