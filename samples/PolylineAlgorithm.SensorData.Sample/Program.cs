//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using PolylineAlgorithm.SensorData.Sample;

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

var encoder = new SensorDataEncoder();
var decoder = new SensorDataDecoder();

// Encode
string encoded = encoder.Encode(readings);

Console.WriteLine("=== Sensor Data Polyline Sample ===");
Console.WriteLine();
Console.WriteLine("Input readings:");
foreach (SensorReading r in readings)
{
    Console.WriteLine($"  [{r.Timestamp:HH:mm:ss}]  {r.Temperature:F1} °C");
}
Console.WriteLine();
Console.WriteLine($"Encoded polyline: {encoded}");
Console.WriteLine();

// Decode (timestamps and temperatures are both recovered)
IEnumerable<SensorReading> decoded = decoder.Decode(encoded);

Console.WriteLine("Decoded readings:");
foreach (SensorReading r in decoded)
{
    Console.WriteLine($"  [{r.Timestamp:HH:mm:ss}]  {r.Temperature:F1} °C");
}
