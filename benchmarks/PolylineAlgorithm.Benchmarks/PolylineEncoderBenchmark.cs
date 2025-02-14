//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PolylineAlgorithm;
using PolylineAlgorithm.Benchmarks.Internal;
using System.Collections.Generic;

[Orderer(SummaryOrderPolicy.Declared)]
[BenchmarkCategory(BenchmarkCategory.PublicApi)]
[CategoriesColumn]
[RankColumn]
public class PolylineEncoderBenchmark {
    private static readonly Random R = new();

    public IEnumerable<Coordinate> Enumeration { get; private set; }

    public List<Coordinate> List { get; private set; }

    [GlobalSetup]
    public void SetupData() {
        Enumeration = Enumerable.Range(0, 1_000).Select(i => new Coordinate(R.Next(-90, 90) + R.NextDouble(), R.Next(-180, 180) + R.NextDouble()));
        List = [..Enumeration];
    }

    [Benchmark]
    public Polyline PolylineEncoder_Encode_List() {
        var encoder = new PolylineEncoder();

        return encoder
            .Encode(List);
    }

    [Benchmark]
    public Polyline PolylineEncoder_Encode_Enumerator() {
        var encoder = new PolylineEncoder();

        return encoder
            .Encode(Enumeration);
    }

}
