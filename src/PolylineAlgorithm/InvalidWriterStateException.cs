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
public sealed class InvalidWriterStateException(string message)
    : Exception(message) {

    public static void ThrowIfCannotWrite(bool canWrite, int readerPosition, int bufferSize) {
        if (canWrite) {
            return;
        }

        if (bufferSize == 0) {
            throw new InvalidWriterStateException(ExceptionMessageResource.PolylineBufferIsEmptyMessage);
        } else {
            throw new InvalidWriterStateException(string.Format(ExceptionMessageResource.UnableToWritePolylineBufferAtPositionMessageFormat, readerPosition, bufferSize));
        }
    }
}
