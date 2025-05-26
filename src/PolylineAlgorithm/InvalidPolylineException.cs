//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Exception thrown when a polyline is determined to be malformed or invalid during processing.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidPolylineException)}: {{ToString()}}")]
public sealed class InvalidPolylineException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPolylineException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">
    /// The error message that describes the reason for the exception.
    /// </param>
    private InvalidPolylineException(string message)
        : base(message) { }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> with a message indicating the position in the polyline where the error was detected.
    /// </summary>
    /// <param name="position">
    /// The zero-based index in the polyline string where the error occurred.
    /// </param>
    /// <exception cref="InvalidPolylineException">
    /// Always thrown to indicate that the polyline is malformed at the specified position.
    /// </exception>
    internal static void Throw(long position) {
        throw new InvalidPolylineException(string.Format(ExceptionMessageResource.PolylineStringIsMalformedMessage, position.ToString()));
    }
}