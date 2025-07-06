//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm.Utility;
using System;

/// <summary>
/// Tests for the <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class PolylineTest {
    /// <summary>
    /// Provides test data for the string parameter tests.
    /// </summary>
    public static IEnumerable<object[]> LengthParameters => [
        [1],
        [10],
        [100],
        [1_000]
    ];

    /// <summary>
    /// Tests the parameterless constructor of the <see cref="Polyline"/> class.
    /// </summary>
    [TestMethod]
    public void Constructor_Parameterless_Ok() {
        // Arrange
        int expectedLength = 0;

        // Act
        Polyline polyline = new();

        // Assert
        Assert.AreEqual(expectedLength, polyline.Length);
        Assert.IsTrue(polyline.IsEmpty);
        Assert.IsTrue(polyline.Value.IsEmpty);
    }

    /// <summary>
    /// Tests the <see cref="Polyline"/> constructor with a null string, expecting an <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void Constructor_Null_String_ArgumentNullException() {
        // Arrange
        string value = null!;

        // Act
        static Polyline New(string value) => Polyline.FromString(value);

        // Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => New(value));
    }

    /// <summary>
    /// Tests the <see cref="Polyline"/> constructor with a string parameter.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void Constructor_String_Parameter_Ok(int length) {
        // Arrange
        var value = RandomValueProvider.GetPolyline(length);

        // Act
        Polyline result = Polyline.FromString(value);

        // Assert
        Assert.AreEqual(value.Length, result.Length);
        Assert.AreEqual(value.Length == 0, result.IsEmpty);
        Assert.AreEqual(value, result.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Polyline"/> constructor with a null character array, expecting an <see cref="ArgumentNullException"/>.
    /// </summary>
    [TestMethod]
    public void FromCharArray_Null_CharArray_ArgumentNullException() {
        // Arrange
        char[] value = null!;
        static Polyline New(char[] value) => Polyline.FromCharArray(value);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() => New(value));

        // Assert

    }

    /// <summary>
    /// Tests the <see cref="Polyline"/> constructor with a character array parameter.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void FromCharArray_CharArray_Parameter_Ok(int size) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(size).ToCharArray();
        bool isEmpty = polyline.Length == 0;
        long length = polyline.Length;

        // Act
        Polyline result = Polyline.FromCharArray(polyline);

        // Assert
        Assert.AreEqual(isEmpty, result.IsEmpty);
        Assert.AreEqual(length, result.Length);
        Assert.AreEqual(new string(polyline), result.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Polyline"/> constructor with a memory parameter.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void FromMemory_Memory_Parameter_Ok(int size) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(size).AsMemory();
        bool isEmpty = polyline.Length == 0;
        long length = polyline.Length;

        // Act
        Polyline result = Polyline.FromMemory(polyline);

        // Assert
        Assert.AreEqual(isEmpty, result.IsEmpty);
        Assert.AreEqual(length, result.Length);
        Assert.AreEqual(polyline.ToString(), result.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Polyline.FromString(string)"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void FromString_Equals_New(int size) {
        // Arrange
        var polyline = RandomValueProvider.GetPolyline(size);
        bool isEmpty = polyline.Length == 0;
        long length = polyline.Length;

        // Act
        Polyline result = Polyline.FromString(polyline);

        // Assert
        Assert.AreEqual(polyline, result.ToString());
    }

    /// <summary>
    /// Tests the <see cref="Polyline.FromCharArray(char[])"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void FromCharArray_Equals_New(int size) {
        // Arrange
        char[] array = RandomValueProvider.GetPolyline(size).ToCharArray();
        Polyline polyline = Polyline.FromCharArray(array);

        // Act
        Polyline result = Polyline.FromCharArray(array);

        // Assert
        Assert.IsTrue(polyline.Equals(result));
    }

    /// <summary>
    /// Tests the <see cref="Polyline.FromMemory(ReadOnlyMemory{char})"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void FromMemory_Equals_New(int size) {
        // Arrange
        ReadOnlyMemory<char> memory = RandomValueProvider.GetPolyline(size).AsMemory();
        Polyline polyline = Polyline.FromMemory(memory);

        // Act
        Polyline result = Polyline.FromMemory(memory);

        // Assert
        Assert.IsTrue(polyline.Equals(result));
    }

    /// <summary>
    /// Tests the <see cref="Polyline.ToString"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void ToString_Equals_Constructor_Parameter(int size) {
        // Arrange
        Polyline polyline = Polyline.FromString(RandomValueProvider.GetPolyline(size));
        string expected = RandomValueProvider.GetPolyline(size);

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests the <see cref="Polyline.ToCharArray"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void CopyTo_Equals_Expected_Value(int size) {
        // Arrange
        Polyline polyline = Polyline.FromString(RandomValueProvider.GetPolyline(size));
        char[] expected = RandomValueProvider.GetPolyline(size).ToCharArray();
        char[] result = new char[polyline.Length];

        // Act
        polyline.CopyTo(result);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    /// <summary>
    /// Tests the <see cref="Polyline.ToCharArray"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void CopyTo_Smaller_Array_Destination_Parameter_Throws_ArgumentException(int size) {
        // Arrange
        Polyline polyline = Polyline.FromString(RandomValueProvider.GetPolyline(size));
        char[] destination = new char[polyline.Length - 1];
        void CopyTo() => polyline.CopyTo(destination);

        // Act
       var exception = Assert.ThrowsExactly<ArgumentException>(CopyTo);

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }

    /// <summary>
    /// Tests the <see cref="Polyline.ToCharArray"/> method.
    /// </summary>
    /// <param name="value">The string value.</param>
    [TestMethod]
    [DynamicData(nameof(LengthParameters))]
    public void CopyTo_Null_Destination_Parameter_Throws_ArgumentNullException(int size) {
        // Arrange
        Polyline polyline = Polyline.FromString(RandomValueProvider.GetPolyline(size));
        char[] destination = null!;
        void CopyTo() => polyline.CopyTo(destination);

        // Act
        var exception = Assert.ThrowsExactly<ArgumentNullException>(CopyTo);

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
    }
}