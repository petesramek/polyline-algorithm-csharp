//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using System;

/// <summary>
/// Tests for <see cref="InvalidPolylineException"/>.
/// </summary>
[TestClass]
public sealed class InvalidPolylineExceptionTests {
    /// <summary>
    /// Tests that the default constructor creates an instance with a null message.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Default_Creates_Instance() {
        // Act
        InvalidPolylineException ex = new();

        // Assert
        Assert.IsNotNull(ex);
        Assert.IsNull(ex.InnerException);
    }

    /// <summary>
    /// Tests that the message and inner exception constructor stores both values.
    /// </summary>
    [TestMethod]
    public void Constructor_With_Message_And_Inner_Exception_Stores_Both() {
        // Arrange
        const string message = "polyline is malformed";
        Exception inner = new InvalidOperationException("inner");

        // Act
        InvalidPolylineException ex = new(message, inner);

        // Assert
        Assert.AreEqual(message, ex.Message);
        Assert.AreSame(inner, ex.InnerException);
    }
}
