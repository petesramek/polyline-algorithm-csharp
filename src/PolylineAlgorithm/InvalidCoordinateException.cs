//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Properties;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents an error that is caused by an invalid coordinate.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidCoordinateException)}: {{ToString()}}")]
public sealed class InvalidCoordinateException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCoordinateException"/> class with the specified coordinate and error message.
    /// </summary>
    /// <param name="coordinate">The coordinate that caused the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    private InvalidCoordinateException(Coordinate coordinate, string message)
        : base(message) {
        Coordinate = coordinate;
    }

    /// <summary>
    /// Gets the coordinate that caused the exception.
    /// </summary>
    public Coordinate Coordinate { get; }

    /// <summary>
    /// Throws an <see cref="InvalidCoordinateException"/> if the specified coordinate is not valid.
    /// </summary>
    /// <param name="coordinate">The coordinate to validate.</param>
    /// <exception cref="InvalidCoordinateException">Thrown when the coordinate is invalid.</exception>
    internal static void ThrowIfNotValid(Coordinate coordinate) {
        if (coordinate.IsValid) {
            return;
        }

        throw new InvalidCoordinateException(coordinate, string.Format(ExceptionMessageResource.CoordinateIsOutOfRangeMessageFormat, coordinate.Latitude, coordinate.Longitude));
    }
}