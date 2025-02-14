namespace PolylineAlgorithm.Benchmarks;
using BenchmarkDotNet.Attributes;

using BenchmarkDotNet.Order;

[Orderer(SummaryOrderPolicy.Declared)]
[CategoriesColumn]
[RankColumn]
public class CoordinateBenchmark {
    private static class Input {
        public static double Latitude { get; } = 90;

        public static double Longitude { get; } = 180;
    }

    public IEnumerable<Coordinate> Coordinates { get; } = [new(), new(Input.Latitude, Input.Longitude), new(Input.Latitude + 1, Input.Longitude + 1)];

    [Benchmark]
    public Coordinate Coordinate_Constructor_Empty() {
        Coordinate coordinate = new();
        return coordinate;
    }

    [Benchmark]
    public Coordinate Coordinate_Constructor_Latitude_Longitude() {
        Coordinate coordinate = new(Input.Latitude, Input.Longitude);
        return coordinate;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Coordinates))]
    public bool Coordinate_IsDefault(Coordinate coordinate) {
        return coordinate.IsDefault;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Coordinates))]
    public bool Coordinate_IsValid(Coordinate coordinate) {
        return coordinate.IsValid;
    }
}
