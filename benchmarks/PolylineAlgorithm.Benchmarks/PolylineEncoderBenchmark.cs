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
    public static List<Coordinate> GetList() {
        return [..GetEnumeration()];
    }

    public static IEnumerable<Coordinate> GetEnumeration() {
        for (int i = 0; i < 1_000_000; i++) {
            yield return new Coordinate(R.Next(-90, 90) + R.NextDouble(), R.Next(-180, 180) + R.NextDouble());
        }
    }

    [Benchmark]
    public Polyline PolylineEncoder_Encode_List() {
        var encoder = new PolylineEncoder();

        return encoder
            .Encode(GetList());
    }

    [Benchmark]
    public Polyline PolylineEncoder_Encode_Enumerator() {
        var encoder = new PolylineEncoder();

        return encoder
            .Encode(GetEnumeration());
    }

}
