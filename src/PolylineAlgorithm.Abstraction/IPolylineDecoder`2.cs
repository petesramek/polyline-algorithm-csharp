//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Abstraction;

public interface IPolylineDecoder<TSource, TResult> : IPolylineDecoder {
    public TResult Decode(TSource value);
}
