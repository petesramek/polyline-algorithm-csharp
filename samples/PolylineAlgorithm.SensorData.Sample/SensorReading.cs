//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.SensorData.Sample;

/// <summary>
/// Represents a single temperature reading captured by a sensor.
/// </summary>
/// <param name="Timestamp">The UTC time at which the reading was captured.</param>
/// <param name="Temperature">The temperature value of the reading, in degrees Celsius.</param>
public readonly record struct SensorReading(DateTimeOffset Timestamp, double Temperature);
