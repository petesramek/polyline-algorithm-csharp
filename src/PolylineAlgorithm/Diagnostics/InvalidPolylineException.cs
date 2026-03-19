//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Diagnostics;

using System;
using System.Diagnostics;

/// <summary>
/// Exception thrown when a polyline is determined to be malformed or invalid during processing.
/// </summary>
/// <remarks>
/// This exception is used internally to indicate that a polyline string does not conform to the expected format or contains errors.
/// </remarks>
[DebuggerDisplay($"{nameof(InvalidPolylineException)}: {{ToString()}}")]
public sealed class InvalidPolylineException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPolylineException"/> class.
    /// </summary>
    public InvalidPolylineException()
        : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPolylineException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">
    /// The error message that describes the reason for the exception.
    /// </param>
    internal InvalidPolylineException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPolylineException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
    /// </param>
    public InvalidPolylineException(string message, Exception innerException)
        : base(message, innerException) { }
}