//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that is caused by invalid coordinate.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidCoordinateException)}: {{ToString()}}")]
public sealed class InvalidCoordinateException : Exception {
    private InvalidCoordinateException(Coordinate coordinate, string message)
        : base(message) {
        Coordinate = coordinate;
    }

    /// <summary>
    /// Coordinate that caused the exception.
    /// </summary>
    public Coordinate Coordinate { get; }

    internal static void ThrowIfNotValid(Coordinate coordinate) {
        if(coordinate.IsValid) {
            return;
        }

        throw new InvalidCoordinateException(coordinate, string.Format(ExceptionMessageResource.CoordinateIsOutOfRangeMessageFormat, coordinate.Latitude, coordinate.Longitude));
    }
}