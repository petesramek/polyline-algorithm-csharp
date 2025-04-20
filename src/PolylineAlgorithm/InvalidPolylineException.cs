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
/// Represents an exception that is thrown when a polyline is malformed or invalid.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidPolylineException)}: {{ToString()}}")]
public sealed class InvalidPolylineException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidPolylineException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    private InvalidPolylineException(string message)
        : base(message) { }

    /// <summary>
    /// Throws an <see cref="InvalidPolylineException"/> with a message indicating the position of the error in the polyline.
    /// </summary>
    /// <param name="position">The zero-based position in the polyline where the error occurred.</param>
    /// <exception cref="InvalidPolylineException">Always thrown to indicate a malformed polyline.</exception>
    internal static void Throw(long position) {
        throw new InvalidPolylineException(string.Format(ExceptionMessageResource.PolylineStringIsMalformedMessage, position.ToString()));
    }
}