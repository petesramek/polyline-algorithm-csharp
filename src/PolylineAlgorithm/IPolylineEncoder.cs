//  
// Copyright (c) Petr Šrámek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using System.Collections.Generic;

public interface IPolylineEncoder {
    string Encode(IEnumerable<(double Latitude, double Longitude)> coordinates);
}