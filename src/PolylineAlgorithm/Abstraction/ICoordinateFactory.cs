//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

public interface ICoordinateFactory<T> {
    public T Create(double latitude, double longitude);
}