//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
#if NET8_0_OR_GREATER
using System.Text;
#endif

/// <summary>
/// Exception thrown when a polyline is determined to be malformed or invalid during processing.
/// </summary>
/// <remarks>
/// This exception is used internally to indicate that a polyline string does not conform to the expected format or contains errors.
/// </remarks>
[DebuggerDisplay($"{nameof(InvalidPolylineException)}: {{ToString()}}")]
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "Internal use only.")]
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
public sealed class InvalidPolylineException : Exception {
#if NET8_0_OR_GREATER
    private static readonly CompositeFormat _polylineStringIsMalformedMessage = CompositeFormat.Parse(ExceptionMessageResource.PolylineStringIsMalformedMessage);
#else
    private static readonly string _polylineStringIsMalformedMessage = ExceptionMessageResource.PolylineStringIsMalformedMessage;
#endif
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
        Debug.Assert(position >= 0, "Position must be a non-negative value.");

        throw new InvalidPolylineException(string.Format(CultureInfo.InvariantCulture, _polylineStringIsMalformedMessage, position));
    }
}