//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using PolylineAlgorithm;
using PolylineAlgorithm.SensorData.Sample;
using System;

public static class Program {
    // 2020-01-01 00:00:00 UTC in Unix seconds. Used as the delta-encoding baseline for timestamps
    // so that the first absolute delta stays within the int32 safe range of the polyline algorithm.
    private const long TimestampBaseEpoch = 1_577_836_800L;

    public static void Main(string[] args) {
        // Build a formatter for SensorReading: timestamp (Unix seconds, precision 0) + temperature.
        // SetBaseline keeps the first timestamp delta small; the formatter adds it back when decoding.
        PolylineFormatter<SensorReading, string> formatter =
            FormatterBuilder<SensorReading, string>.Create()
                .AddValue("timestamp", static r => (double)r.Timestamp.ToUnixTimeSeconds(), precision: 0)
                .SetBaseline(TimestampBaseEpoch)
                .AddValue("temperature", static r => r.Temperature, precision: 5)
                // The formatter automatically denormalizes: v[0] = Unix seconds, v[1] = temperature.
                .WithCreate(static v => new SensorReading(
                    DateTimeOffset.FromUnixTimeSeconds((long)v[0]),
                    v[1]))
                .ForPolyline(static m => new string(m.Span), static s => s.AsMemory())
                .Build();

        PolylineOptions<SensorReading, string> options = new(formatter);
        PolylineEncoder<SensorReading, string> encoder = new(options);
        PolylineDecoder<string, SensorReading> decoder = new(options);

        // Sample temperature readings from a sensor over six seconds
        var readings = new SensorReading[]
        {
            new(DateTimeOffset.UtcNow,                23.5),
            new(DateTimeOffset.UtcNow.AddSeconds(1),  23.7),
            new(DateTimeOffset.UtcNow.AddSeconds(2),  24.1),
            new(DateTimeOffset.UtcNow.AddSeconds(3),  25.0),
            new(DateTimeOffset.UtcNow.AddSeconds(4),  24.8),
            new(DateTimeOffset.UtcNow.AddSeconds(5),  24.8),
            new(DateTimeOffset.UtcNow.AddSeconds(6),  22.3),
        };

        // Encode
        string encoded = encoder.Encode(readings);

        Console.WriteLine("=== Sensor Data Polyline Sample ===");
        Console.WriteLine();
        Console.WriteLine("Input readings:");

        foreach (SensorReading r in readings) {
            Console.WriteLine($"  [{r.Timestamp:HH:mm:ss}]  {r.Temperature:F1} °C");
        }

        Console.WriteLine();
        Console.WriteLine($"Encoded polyline: {encoded}");
        Console.WriteLine();

        // Decode (timestamps and temperatures are both recovered)
        IEnumerable<SensorReading> decoded = decoder.Decode(encoded);

        Console.WriteLine("Decoded readings:");

        foreach (SensorReading r in decoded) {
            Console.WriteLine($"  [{r.Timestamp:HH:mm:ss}]  {r.Temperature:F1} °C");
        }
    }
}