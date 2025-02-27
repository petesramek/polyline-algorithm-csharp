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
/// Represents an error that is caused by an invalid reader state.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidReaderStateException)}: {{ToString()}}")]
public sealed class InvalidReaderStateException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidReaderStateException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    private InvalidReaderStateException(string message)
        : base(message) { }

    /// <summary>
    /// Throws an <see cref="InvalidReaderStateException"/> if the reader cannot read from the polyline.
    /// </summary>
    /// <param name="canRead">A value indicating whether the reader can read from the polyline.</param>
    /// <param name="readerPosition">The current position of the reader.</param>
    /// <param name="polylineLength">The length of the polyline.</param>
    /// <exception cref="InvalidReaderStateException">
    /// Thrown when the reader cannot read from the polyline because the polyline is either empty or the reader has reached the end of the polyline.
    /// </exception>
    internal static void ThrowIfCannotRead(bool canRead, long readerPosition, long polylineLength) {
        if (canRead) {
            return;
        }

        if (polylineLength == 0) {
            throw new InvalidReaderStateException(ExceptionMessageResource.PolylineStringIsEmptyMessage);
        } else {
            throw new InvalidReaderStateException(string.Format(ExceptionMessageResource.UnableToReadPolylineAtPositionMessageFormat, readerPosition, polylineLength));
        }
    }
}