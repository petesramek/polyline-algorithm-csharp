//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that occurs during polyline encoding. 
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Main purpose is to report position in which failure occurs, thus we have to have only one constructor.")]
public sealed class PolylineMalformedException : Exception {
    /// <summary>
    /// Initializes an instance
    /// </summary>
    /// <param name="position"></param>
    /// <param name="innerException"></param>
    private PolylineMalformedException(int position, string message)
        : base(message) {
        Position = position;
    }

    /// <summary>
    /// Position in polyline string at which error occurs.
    /// </summary>
    public int Position { get; }

    public static void Throw(int position) {
        throw new PolylineMalformedException(position, string.Format(ExceptionMessageResource.PolylineStringIsMalformedMessage, position));
    }
}