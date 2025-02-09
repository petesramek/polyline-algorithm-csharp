//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that is caused by invalid coordinate.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Main purpose is to report coordinate that is invalid, thus we have to have only one construtor.")]
public sealed class InvalidCoordinateException : Exception {
    public InvalidCoordinateException(Coordinate coordinate, Exception? innerException = null)
        : base(string.Format(ExceptionMessageResource.CoordinateValidationExceptionCoordinateIsOutOfRangeErrorMessageFormat, coordinate.Latitude, coordinate.Longitude),
               innerException) {
        Coordinate = coordinate;
    }

    /// <summary>
    /// Coordinate that caused the exception.
    /// </summary>
    public Coordinate Coordinate { get; }
}
