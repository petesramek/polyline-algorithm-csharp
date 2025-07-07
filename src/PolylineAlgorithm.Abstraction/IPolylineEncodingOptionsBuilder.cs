//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

using Microsoft.Extensions.Logging;

public interface IPolylineEncodingOptionsBuilder {
    IPolylineEncodingOptionsBuilder WithBufferSize(int maxBufferSize);

    IPolylineEncodingOptionsBuilder WithLoggerFactory(ILoggerFactory loggerFactory);

    PolylineEncodingOptions Build();
}
