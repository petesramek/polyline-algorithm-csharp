//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using PolylineAlgorithm.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that is caused by malformed polyline.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Internal use only.")]
[DebuggerDisplay($"{nameof(InvalidPolylineException)}: {{ToString()}}")]
public sealed class InvalidPolylineException : Exception
    
    
    {
    private InvalidPolylineException(string message)
        : base(message) { }

    internal static void Throw(int position) {
        throw new InvalidPolylineException(string.Format(ExceptionMessageResource.PolylineStringIsMalformedMessage, position));
    }
}