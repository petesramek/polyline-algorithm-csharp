//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests;

using Newtonsoft.Json.Linq;
using PolylineAlgorithm.Tests.Internal;
using System;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class PolylineTest {
    public static IEnumerable<object[]> MemoryParamaters => [
        [Defaults.Polyline.Empty.AsMemory()],
        [Defaults.Polyline.Valid.AsMemory()]
    ];

    public static IEnumerable<object[]> StringParamaters => [
        [Defaults.Polyline.Empty],
        [Defaults.Polyline.Valid]
    ];

    public static IEnumerable<object[]> CharArrayParamaters => [
        [Defaults.Polyline.Empty.ToCharArray()],
        [Defaults.Polyline.Valid.ToCharArray()]
    ];

    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        bool isEmpty = true;
        int length = 0;
        ReadOnlySpan<char> span = [];

        // Act
        Polyline polyline = new();

        // Assert
        Assert.AreEqual(isEmpty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    public void Constructor_Null_String_ArgumentNullException() {
        // Arrange
        string value = null!;

        // Act
        Polyline New(string value) => new(value);

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => New(value));
    }

    [TestMethod]
    [DynamicData(nameof(StringParamaters))]
    public void Constructor_Empty_String_Ok(string value) {
        // Arrange
        bool isEmpty = value.Length == 0;
        int length = value.Length;
        ReadOnlySpan<char> span = value.AsSpan();

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.AreEqual(isEmpty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    public void Constructor_Null_CHarArray_ArgumentNullException() {
        // Arrange
        char[] value = null!;

        // Act
        Polyline New(char[] value) => new(value);

        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => New(value));
    }


    [TestMethod]
    [DynamicData(nameof(CharArrayParamaters))]
    public void Constructor_Empty_CharArray_Ok(char[] value) {
        // Arrange
        bool isEmpty = value.Length == 0;
        int length = value.Length;
        ReadOnlySpan<char> span = value.AsSpan();

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.AreEqual(isEmpty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    [DynamicData(nameof(MemoryParamaters))]
    public void Constructor_Memory_Paramater_Ok(ReadOnlyMemory<char> value) {
        // Arrange
        bool isEmpty = value.IsEmpty;
        int length = value.Length;
        ReadOnlySpan<char> span = value.Span;

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.AreEqual(isEmpty, polyline.IsEmpty);
        Assert.AreEqual(length, polyline.Length);
        Assert.IsTrue(span.SequenceEqual(polyline.Span));
    }

    [TestMethod]
    [DynamicData(nameof(StringParamaters))]
    public void FromString_Equals_New(string value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        Polyline result = Polyline.FromString(in value);

        // Assert
        Assert.AreEqual(polyline, result);
    }

    [TestMethod]
    [DynamicData(nameof(CharArrayParamaters))]
    public void FromCharArray_Equals_New(char[] value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        Polyline result = Polyline.FromCharArray(in value);

        // Assert
        Assert.AreEqual(polyline, result);
    }

    [TestMethod]
    [DynamicData(nameof(MemoryParamaters))]
    public void FromMemory_Equals_New(ReadOnlyMemory<char> value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        Polyline result = Polyline.FromMemory(in value);

        // Assert
        Assert.AreEqual(polyline, result);
    }


    [TestMethod]
    [DynamicData(nameof(StringParamaters))]
    public void ToString_Equals_Constructor_Parameter(string value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    [DynamicData(nameof(CharArrayParamaters))]
    public void ToCharArray_Equals_Constructor_Parameter(char[] value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        char[] result = polyline.ToCharArray();

        // Assert
        CollectionAssert.AreEqual(value, result);
    }

    [TestMethod]
    [DynamicData(nameof(MemoryParamaters))]
    public void AsMemory_Equals_Constructor_Parameter(ReadOnlyMemory<char> value) {
        // Arrange
        Polyline polyline = new(value);

        // Act
        ReadOnlyMemory<char> result = polyline.AsMemory();

        // Assert
        Assert.AreEqual(value, result);
    }
}
