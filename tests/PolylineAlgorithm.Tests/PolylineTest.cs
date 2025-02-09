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
    public static IEnumerable<object[]> MemoryConstructorParamaters => [
        [Defaults.Polyline.Empty.AsMemory()],
        [Defaults.Polyline.Valid.AsMemory()]
    ];

    public static IEnumerable<object[]> StringConstructorParamaters => [
        [Defaults.Polyline.Empty],
        [Defaults.Polyline.Valid]
    ];

    public static IEnumerable<object[]> CharArrayConstructorParamaters => [
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
    [DynamicData(nameof(StringConstructorParamaters))]
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
    [DynamicData(nameof(CharArrayConstructorParamaters))]
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
    [DynamicData(nameof(MemoryConstructorParamaters))]
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
    public void Constructor_Valid_String_Ok() {
        // Arrange
        string value = Defaults.Polyline.Valid;
        int expectedLength = value.Length;
        ReadOnlySpan<char> expectedSpan = value.AsSpan();

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.IsTrue(expectedSpan.SequenceEqual(polyline.Span));
        Assert.AreEqual(expectedLength, polyline.Length);
    }

    [TestMethod]
    public void Constructor_Valid_CharArray_Ok() {
        // Arrange
        char[] value = Defaults.Polyline.Valid.ToCharArray();
        int expectedLength = value.Length;
        ReadOnlySpan<char> expectedSpan = value.AsSpan();

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.IsTrue(expectedSpan.SequenceEqual(polyline.Span));
        Assert.AreEqual(expectedLength, polyline.Length);
    }


    [TestMethod]
    public void Constructor_Valid_Memory_Ok() {
        // Arrange
        ReadOnlyMemory<char> value = Defaults.Polyline.Valid.AsMemory();
        int expectedLength = value.Length;
        ReadOnlySpan<char> expectedSpan = value.Span;

        // Act
        Polyline polyline = new(value);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.IsTrue(expectedSpan.SequenceEqual(polyline.Span));
        Assert.AreEqual(expectedLength, polyline.Length);
    }

    [TestMethod]
    public void FromString_Equals_New() {
        // Arrange
        string value = Defaults.Polyline.Valid;
        Polyline expectedPolyline = new(value);

        // Act
        Polyline polyline = Polyline.FromString(in value);

        // Assert
        Assert.AreEqual(expectedPolyline, polyline);
    }

    [TestMethod]
    public void FromCharArray_Equals_New() {
        // Arrange
        char[] value = Defaults.Polyline.Valid.ToCharArray();
        Polyline expectedPolyline = new(value);

        // Act
        Polyline polyline = Polyline.FromCharArray(in value);

        // Assert
        Assert.AreEqual(expectedPolyline, polyline);
    }

    [TestMethod]
    public void FromMemory_Equals_New() {
        // Arrange
        ReadOnlyMemory<char> value = Defaults.Polyline.Valid.AsMemory();
        Polyline expectedPolyline = new(value);

        // Act
        Polyline polyline = Polyline.FromMemory(in value);

        // Assert
        Assert.AreEqual(expectedPolyline, polyline);
    }


    [TestMethod]
    public void ToString_Equals_Constructor_Parameter() {
        // Arrange
        Polyline polyline = new(Defaults.Polyline.Valid);
        string expectedResult = Defaults.Polyline.Valid;

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void ToCharArray_Equals_Constructor_Parameter() {
        // Arrange
        Polyline polyline = new(Defaults.Polyline.Valid);
        char[] expectedResult = Defaults.Polyline.Valid.ToCharArray();

        // Act
        char[] result = polyline.ToCharArray();

        // Assert
        CollectionAssert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void AsMemory_Equals_Constructor_Parameter() {
        // Arrange
        Polyline polyline = new(Defaults.Polyline.Valid);
        ReadOnlyMemory<char> expectedResult = Defaults.Polyline.Valid.AsMemory();

        // Act
        ReadOnlyMemory<char> result = polyline.AsMemory();

        // Assert
        Assert.AreEqual(expectedResult, result);
    }
}
