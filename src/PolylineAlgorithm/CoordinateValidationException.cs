//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The exception that is thrown when coordinate is not valid.
/// </summary>
/// <param name="coordinate">An invalid coordinate that caused the exception.</param>
[DebuggerDisplay($"{{{nameof(GetFormattedMessage)}(),nq}}")]
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "There is no reason to have standard constructors.")]
public sealed class CoordinateValidationException((double Latitude, double Longitude) coordinate)
    : Exception(GetFormattedMessage(coordinate)) {

    /// <summary>
    /// An invalid coordinate that caused the exception.
    /// </summary>
    public (double Latitude, double Longitude) Coordinate { get; }

    /// <summary>
    /// Returns a formatted exception message.
    /// </summary>
    /// <param name="coordinate">An invalid coordinate that caused the exception.</param>
    /// <returns>A formatted exception message.</returns>
    private static string GetFormattedMessage((double Latitude, double Longitude) coordinate)
        => string.Format(
            ExceptionMessageResource.CoordinateValidationExceptionCoordinateIsOutOfRangeErrorMessageFormat,
            coordinate.Latitude,
            coordinate.Longitude
        );
}
