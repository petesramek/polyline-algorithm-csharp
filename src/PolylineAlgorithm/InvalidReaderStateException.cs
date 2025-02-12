//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that is caused by invalid reader state.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Main purpose is to report position in which failure occurs, thus we have to have only one constructor.")]
public sealed class InvalidReaderStateException(string message)
    : Exception(message) {

    public static void ThrowIfCannotRead(bool canRead, int readerPosition, int polylineLength) {
        if(canRead) {
            return;
        }

        if (polylineLength == 0) {
            throw new InvalidReaderStateException(ExceptionMessageResource.PolylineStringIsEmptyMessage);
        } else {
            throw new InvalidReaderStateException(string.Format(ExceptionMessageResource.UnableToReadPolylineAtPositionMessageFormat, readerPosition, polylineLength));
        }
    }
}
