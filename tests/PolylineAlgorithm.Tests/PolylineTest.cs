//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Tests.Data;
using System;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class PolylineTest {
    public static IEnumerable<object[]> StringParameters => [
        [Values.Polyline.Empty],
        [Values.Polyline.Valid]
    ];

    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        bool empty = true;
        int length = 0;
        ReadOnlySpan<char> span = [];

        // Act
        Polyline polyline = new();

        // Assert
        Assert.AreEqual(empty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    public void Constructor_Null_String_ArgumentNullException() {
        // Arrange
        string value = null!;

        // Act
        static Polyline New(string value) => new(value);

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => New(value));
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void Constructor_String_Parameter_Ok(string value) {
        // Arrange
        bool empty = value.Length == 0;
        int length = value.Length;
        ReadOnlySpan<char> span = value.AsSpan();

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.AreEqual(empty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    public void Constructor_Null_CharArray_ArgumentNullException() {
        // Arrange
        char[] value = null!;

        // Act
        static Polyline New(char[] value) => new(value);

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => New(value));
    }


    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void Constructor_CharArray_Parameter_Ok(string value) {
        // Arrange
        bool empty = value.Length == 0;
        int length = value.Length;
        ReadOnlySpan<char> span = value.AsSpan();

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.AreEqual(empty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void Constructor_Memory_Parameter_Ok(string value) {
        // Arrange
        bool empty = value.Length == 0;
        int length = value.Length;
        ReadOnlySpan<char> span = value;

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.AreEqual(empty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void FromString_Equals_New(string value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        Polyline result = Polyline.FromString(in value);

        // Assert
        Assert.AreEqual(polyline, result);
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void FromCharArray_Equals_New(string value) {
        // Arrange
        char[] array = [..value];
        Polyline polyline = new(array);

        // Act
        Polyline result = Polyline.FromCharArray(in array);

        // Assert
        Assert.AreEqual(polyline, result);
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void FromMemory_Equals_New(string value) {
        // Arrange
        ReadOnlyMemory<char> memory = value.AsMemory();
        Polyline polyline = new(value);

        // Act
        Polyline result = Polyline.FromMemory(in memory);

        // Assert
        Assert.AreEqual(polyline, result);
    }


    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void ToString_Equals_Constructor_Parameter(string value) {
        // Arrange
        Polyline polyline = new(value);
        string expected = value;

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void ToCharArray_Equals_Constructor_Parameter(string value) {
        // Arrange
        Polyline polyline = new(value);
        char[] expected = value.ToCharArray();

        // Act
        char[] result = polyline.ToCharArray();

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    [DynamicData(nameof(StringParameters))]
    public void AsMemory_Equals_Constructor_Parameter(string value) {
        // Arrange
        Polyline polyline = new(value);
        ReadOnlyMemory<char> expected = value.AsMemory();

        // Act
        ReadOnlyMemory<char> result = polyline.AsMemory();

        // Assert
        Assert.AreEqual(expected, result);
    }
}
