//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents an error that is caused by an invalid writer state.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidWriterStateException)}: {{ToString()}}")]
public sealed class InvalidWriterStateException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidWriterStateException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    private InvalidWriterStateException(string message)
        : base(message) { }

    /// <summary>
    /// Throws an <see cref="InvalidWriterStateException"/> if the writer cannot write to the buffer.
    /// </summary>
    /// <param name="canWrite">A value indicating whether the writer can write to the buffer.</param>
    /// <param name="writerPosition">The current position of the writer.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    /// <exception cref="InvalidWriterStateException">
    /// Thrown when the writer cannot write to the buffer because the buffer is either empty or the writer has reached the end of the buffer.
    /// </exception>
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