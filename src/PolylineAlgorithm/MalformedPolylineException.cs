//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that occurs during polyline encoding. 
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(MalformedPolylineException)}: {{ToString()}}")]
public sealed class MalformedPolylineException : Exception {
    /// <summary>
    /// Initializes an instance
    /// </summary>
    /// <param name="position"></param>
    /// <param name="innerException"></param>
    private MalformedPolylineException(string message)
        : base(message) { }

    internal static void Throw(int position) {
        throw new MalformedPolylineException(string.Format(ExceptionMessageResource.PolylineStringIsMalformedMessage, position));
    }
}