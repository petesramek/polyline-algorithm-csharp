//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

/// <summary>
/// The main entry point for the benchmark application.
/// </summary>
internal class Program {
    /// <summary>
    /// The main method that runs the benchmarks.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    static void Main(string[] args) {
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, DefaultConfig.Instance);
    }
}