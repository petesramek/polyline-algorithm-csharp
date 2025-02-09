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
public class InvalidWriterStateException : Exception {
    public InvalidWriterStateException(int position, int bufferSize)
        : base(GetErrorMessage(position, bufferSize)) {
    }

    private static string GetErrorMessage(int readerPosition, int bufferSize) {
        if (bufferSize == 0) {
            return ExceptionMessageResource.PolylineBufferIsEmptyMessage;
        } else {
            return string.Format(ExceptionMessageResource.UnableToWritePolylineBufferAtPositionMessageFormat, readerPosition, bufferSize);
        }
    }
}
