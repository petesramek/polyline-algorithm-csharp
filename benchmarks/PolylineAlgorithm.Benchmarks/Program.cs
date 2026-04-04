//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Running;

/// <summary>
/// Main entry point for benchmarks.
/// </summary>
internal static class Program {
    /// <summary>
    /// Runs the benchmarks.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    static void Main(string[] args) {
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args);
    }
}