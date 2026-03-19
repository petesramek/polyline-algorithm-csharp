namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using PolylineAlgorithm.Utility;

public class PolylineEncodingValidationBenchmark {
    private string polyline;

    /// <summary>
    /// Number of coordinates for benchmarks. Set by BenchmarkDotNet.
    /// </summary>
    [Params(8, 64, 128, 1024, 4096, 20480, 102400)]
    public int CoordinatesCount { get; set; }

    [GlobalSetup]
    public void Setup() {
        polyline = RandomValueProvider.GetPolyline(CoordinatesCount);
    }

    [Benchmark(Baseline = true)]
    public void ValidateCharRange() => PolylineEncoding.ValidateCharRange(polyline);

    [Benchmark]
    public void ValidateBlockLength() => PolylineEncoding.ValidateBlockLength(polyline);


    [Benchmark]
    public void ValidateFormat() => PolylineEncoding.ValidateFormat(polyline);
}