//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

public static class CoordinateValidator {
    public static CoordinateRange Latitude { get; } = new CoordinateRange(-90d, 90d);
    public static CoordinateRange Longitude { get; } = new CoordinateRange(-180d, 180d);
}
