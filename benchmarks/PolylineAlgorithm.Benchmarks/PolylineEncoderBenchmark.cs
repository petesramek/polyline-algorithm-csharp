//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm;
using System.Collections.Generic;

[RankColumn]
public class PolylineEncoderBenchmark {
    private static readonly Random R = new();

    public static IEnumerable<Coordinate>? Enumeration { get; private set; }

    public static List<Coordinate>? List { get; private set; }

    public PolylineEncoder Encoder = new();

    [GlobalSetup]
    public void SetupData() {
        Enumeration = Enumerable.Range(0, 100).Select(i => new Coordinate(R.Next(-90, 90) + R.NextDouble(), R.Next(-180, 180) + R.NextDouble()));
        List = [.. Enumeration];
    }

    [Benchmark]
    public Polyline PolylineEncoder_Encode_List() {
        return Encoder
            .Encode(List!);
    }

    [Benchmark]
    public Polyline PolylineEncoder_Encode_Enumerator() {
        return Encoder
            .Encode(Enumeration!);
    }
}
