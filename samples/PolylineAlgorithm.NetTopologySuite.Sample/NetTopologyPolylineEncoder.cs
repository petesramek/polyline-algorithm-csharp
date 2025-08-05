//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.NetTopologySuite.Sample;

using global::NetTopologySuite.Geometries;
using PolylineAlgorithm.Abstraction;

/// <summary>
/// Encodes a collection of geographic coordinates into an encoded polyline string using NetTopologySuite's Point type.
/// </summary>
public sealed class NetTopologyPolylineEncoder : AbstractPolylineEncoder<Point, string> {
    /// <summary>
    /// Creates a string representation of the provided polyline.
    /// </summary>
    /// <param name="polyline">
    /// The polyline to encode as a string.
    /// </param>
    /// <returns>
    /// An encoded polyline string representation of the provided polyline.
    /// </returns>
    protected override string CreatePolyline(ReadOnlyMemory<char> polyline) {
        if (polyline.IsEmpty) {
            return string.Empty;
        }

        return polyline.ToString();
    }

    /// <summary>
    /// Extracts the latitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">
    /// The coordinate from which to extract the latitude value. This should be a not null <see cref="Point"/> instance.
    /// </param>
    /// <returns>
    /// The latitude value as a <see cref="double"/>.
    /// </returns>
    protected override double GetLatitude(Point current) {
        // Validate parameter

        return current.X;
    }

    /// <summary>
    /// Extracts the longitude value from the specified coordinate.
    /// </summary>
    /// <param name="current">
    /// The coordinate from which to extract the longitude value. This should be a not null <see cref="Point"/> instance.
    /// </param>
    /// <returns>
    /// The longitude value as a <see cref="double"/>.
    /// </returns>
    protected override double GetLongitude(Point current) {
        // Validate parameter

        return current.Y;
    }
}