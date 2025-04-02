//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

public interface ICoordinateFactory<T> {
    public T Construct(double latitude, double longitude);

    public (double Latitude, double Longitude) Deconstruct(T coordinate);
}