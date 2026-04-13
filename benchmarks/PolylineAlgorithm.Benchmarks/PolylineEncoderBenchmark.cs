namespace PolylineAlgorithm.Benchmarks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PolylineAlgorithm;
using PolylineAlgorithm.Utility;

/// <summary>
/// Benchmarks for <see cref="PolylineEncoder{TValue, TPolyline}"/>.
/// </summary>
public class PolylineEncoderBenchmark {
    private readonly Consumer _consumer = new();

    /// <summary>
    /// Number of coordinates for benchmarks.
    /// </summary>
    [Params(1, 100, 1_000)]
    public int CoordinatesCount { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    /// <summary>
    /// Coordinates as array.
    /// </summary>
    public (double Latitude, double Longitude)[] Array { get; private set; }

    /// <summary>
    /// Coordinates as read-only memory.
    /// </summary>
    public ReadOnlyMemory<(double Latitude, double Longitude)> Memory { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Polyline encoder instance.
    /// </summary>
    private readonly PolylineEncoder<(double Latitude, double Longitude), string> _encoder = CreateEncoder();

    private static PolylineEncoder<(double Latitude, double Longitude), string> CreateEncoder() {
        PolylineFormatter<(double Latitude, double Longitude), string> formatter =
            FormatterBuilder<(double Latitude, double Longitude), string>.Create()
                .AddValue("lat", static c => c.Latitude)
                .AddValue("lon", static c => c.Longitude)
                .WithReaderWriter(static m => new string(m.Span), static s => s.AsMemory())
                .Build();

        return new PolylineEncoder<(double Latitude, double Longitude), string>(
            new PolylineOptions<(double Latitude, double Longitude), string>(formatter));
    }

    /// <summary>
    /// Sets up benchmark data.
    /// </summary>
    [GlobalSetup]
    public void SetupData() {
        Array = [.. RandomValueProvider.GetCoordinates(CoordinatesCount)];
        Memory = Array.AsMemory();
    }

    /// <summary>
    /// Benchmark: encode coordinates from span.
    /// </summary>
    [Benchmark]
    public void PolylineEncoder_Encode_Span() {
        var polyline = _encoder.Encode(Memory.Span);
        _consumer.Consume(polyline);
    }

    /// <summary>
    /// Benchmark: encode coordinates from array.
    /// </summary>
    [Benchmark]
    public void PolylineEncoder_Encode_Array() {
        var polyline = _encoder.Encode(Array);
        _consumer.Consume(polyline);
    }
}
