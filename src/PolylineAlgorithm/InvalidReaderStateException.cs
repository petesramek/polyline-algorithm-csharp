//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm;

using PolylineAlgorithm.Internal;
using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents error that is caused by invalid reader state.
/// </summary>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Main purpose is to report position in which failure occurs, thus we have to have only one constructor.")]
public class InvalidReaderStateException : InvalidOperationException {
    public InvalidReaderStateException(int position, int length)
        : base(GetErrorMessage(position, length)) {
    }

    private static string GetErrorMessage(int readerPosition, int polylineLength) {
        if (polylineLength == 0) {
            return ExceptionMessageResource.PolylineStringIsEmptyMessage;
        } else {
            return string.Format(ExceptionMessageResource.UnableToReadPolylineAtPositionMessageFormat, readerPosition, polylineLength);
        }
    }
}
