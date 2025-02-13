//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that is caused by invalid reader state.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidWriterStateException)}: {{ToString()}}")]
public sealed class InvalidWriterStateException : Exception {
    private InvalidWriterStateException(string message)
        : base(message) { }

    internal static void ThrowIfCannotWrite(bool canWrite, int writerPosition, int bufferSize) {
        if (canWrite) {
            return;
        }

        if (bufferSize == 0) {
            throw new InvalidWriterStateException(ExceptionMessageResource.PolylineBufferIsEmptyMessage);
        } else {
            throw new InvalidWriterStateException(string.Format(ExceptionMessageResource.UnableToWritePolylineBufferAtPositionMessageFormat, writerPosition, bufferSize));
        }
    }
}
