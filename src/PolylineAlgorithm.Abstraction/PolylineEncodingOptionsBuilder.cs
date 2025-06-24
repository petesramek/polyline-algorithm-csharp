//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

/// <summary>
/// Provides a static factory for creating <see cref="IPolylineEncodingOptionsBuilder{TCoordinate}"/> instances.
/// </summary>
public class PolylineEncodingOptionsBuilder {
    /// <summary>
    /// Creates a new <see cref="IPolylineEncodingOptionsBuilder{TCoordinate}"/> instance for the specified coordinate type.
    /// </summary>
    /// <typeparam name="TCoordinate">The type representing a coordinate.</typeparam>
    /// <returns>An <see cref="IPolylineEncodingOptionsBuilder{TCoordinate}"/> instance for configuring polyline encoding options.</returns>
    public static IPolylineEncodingOptionsBuilder<TCoordinate> Create<TCoordinate>() {
        return new PolylineEncodingOptionsBuilder<TCoordinate>();
    }
}