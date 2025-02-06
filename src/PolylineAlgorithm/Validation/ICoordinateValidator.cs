//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Validation;

public interface ICoordinateValidator {
    CoordinateRange Latitude { get; }
    CoordinateRange Longitude { get; }

    bool IsValid(Coordinate coordinate);
}