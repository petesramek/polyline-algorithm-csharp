//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolylineAlgorithm;

/// <summary>
/// Tests for the <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class PolylineTests {
    private static readonly char[] ExpectedTestChars = ['t', 'e', 's', 't'];
    private static readonly char[] ExpectedDefaultChars = ['\0', '\0', '\0', '\0', '\0'];

    /// <summary>
    /// Tests that the default constructor creates an empty polyline.
    /// </summary>
    [TestMethod]
    public void Constructor_Default_CreatesEmptyPolyline() {
        // Arrange & Act
        var polyline = new Polyline();

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that IsEmpty returns true for a polyline created with default constructor.
    /// </summary>
    [TestMethod]
    public void IsEmpty_DefaultConstructor_ReturnsTrue() {
        // Arrange
        var polyline = new Polyline();

        // Act
        bool isEmpty = polyline.IsEmpty;

        // Assert
        Assert.IsTrue(isEmpty);
    }

    /// <summary>
    /// Tests that IsEmpty returns false for a polyline with content.
    /// </summary>
    [TestMethod]
    public void IsEmpty_WithContent_ReturnsFalse() {
        // Arrange
        var polyline = Polyline.FromString("abc");

        // Act
        bool isEmpty = polyline.IsEmpty;

        // Assert
        Assert.IsFalse(isEmpty);
    }

    /// <summary>
    /// Tests that Length returns zero for an empty polyline.
    /// </summary>
    [TestMethod]
    public void Length_EmptyPolyline_ReturnsZero() {
        // Arrange
        var polyline = new Polyline();

        // Act
        int length = polyline.Length;

        // Assert
        Assert.AreEqual(0, length);
    }

    /// <summary>
    /// Tests that Length returns correct value for a polyline with content.
    /// </summary>
    [TestMethod]
    public void Length_WithContent_ReturnsCorrectLength() {
        // Arrange
        var polyline = Polyline.FromString("hello");

        // Act
        int length = polyline.Length;

        // Assert
        Assert.AreEqual(5, length);
    }

    /// <summary>
    /// Tests that CopyTo throws ArgumentNullException when destination is null.
    /// </summary>
    [TestMethod]
    public void CopyTo_NullDestination_ThrowsArgumentNullException() {
        // Arrange
        var polyline = Polyline.FromString("test");

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => polyline.CopyTo(null!));
    }

    /// <summary>
    /// Tests that CopyTo throws ArgumentException when destination is smaller than polyline length.
    /// </summary>
    [TestMethod]
    public void CopyTo_DestinationTooSmall_ThrowsArgumentException() {
        // Arrange
        var polyline = Polyline.FromString("hello");
        char[] destination = new char[3];

        // Act & Assert
        _ = Assert.Throws<ArgumentException>(() => polyline.CopyTo(destination));
    }

    /// <summary>
    /// Tests that CopyTo successfully copies characters when destination has exact size.
    /// </summary>
    [TestMethod]
    public void CopyTo_DestinationExactSize_CopiesCharacters() {
        // Arrange
        var polyline = Polyline.FromString("test");
        char[] destination = new char[4];

        // Act
        polyline.CopyTo(destination);

        // Assert
        CollectionAssert.AreEqual(ExpectedTestChars, destination);
    }

    /// <summary>
    /// Tests that CopyTo successfully copies characters when destination is larger than polyline.
    /// </summary>
    [TestMethod]
    public void CopyTo_DestinationLargerThanPolyline_CopiesCharacters() {
        // Arrange
        var polyline = Polyline.FromString("abc");
        char[] destination = new char[10];

        // Act
        polyline.CopyTo(destination);

        // Assert
        Assert.AreEqual('a', destination[0]);
        Assert.AreEqual('b', destination[1]);
        Assert.AreEqual('c', destination[2]);
        Assert.AreEqual('\0', destination[3]); // Rest should be default
    }

    /// <summary>
    /// Tests that CopyTo works correctly with empty polyline.
    /// </summary>
    [TestMethod]
    public void CopyTo_EmptyPolyline_DoesNotThrow() {
        // Arrange
        var polyline = new Polyline();
        char[] destination = new char[5];

        // Act
        polyline.CopyTo(destination);

        // Assert
        CollectionAssert.AreEqual(ExpectedDefaultChars, destination);
    }

    /// <summary>
    /// Tests that CopyTo throws ArgumentNullException for empty polyline with null destination.
    /// </summary>
    [TestMethod]
    public void CopyTo_EmptyPolylineNullDestination_ThrowsArgumentNullException() {
        // Arrange
        var polyline = new Polyline();

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => polyline.CopyTo(null!));
    }

    /// <summary>
    /// Tests that ToString returns empty string for an empty polyline.
    /// </summary>
    [TestMethod]
    public void ToString_EmptyPolyline_ReturnsEmptyString() {
        // Arrange
        var polyline = new Polyline();

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    /// <summary>
    /// Tests that ToString returns correct string for a polyline with content.
    /// </summary>
    [TestMethod]
    public void ToString_WithContent_ReturnsCorrectString() {
        // Arrange
        var polyline = Polyline.FromString("polyline");

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual("polyline", result);
    }

    /// <summary>
    /// Tests that ToString returns correct string for a polyline with special characters.
    /// </summary>
    [TestMethod]
    public void ToString_WithSpecialCharacters_ReturnsCorrectString() {
        // Arrange
        var polyline = Polyline.FromString("_p~iF~ps|U");

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual("_p~iF~ps|U", result);
    }

    /// <summary>
    /// Tests that ToString returns correct string for a single character polyline.
    /// </summary>
    [TestMethod]
    public void ToString_SingleCharacter_ReturnsCorrectString() {
        // Arrange
        var polyline = Polyline.FromString("x");

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual("x", result);
    }

    /// <summary>
    /// Tests that Equals returns true when comparing two identical polylines.
    /// </summary>
    [TestMethod]
    public void Equals_IdenticalPolylines_ReturnsTrue() {
        // Arrange
        var polyline1 = Polyline.FromString("test");
        var polyline2 = Polyline.FromString("test");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing polylines with different content.
    /// </summary>
    [TestMethod]
    public void Equals_DifferentContent_ReturnsFalse() {
        // Arrange
        var polyline1 = Polyline.FromString("test1");
        var polyline2 = Polyline.FromString("test2");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing polylines with different lengths.
    /// </summary>
    [TestMethod]
    public void Equals_DifferentLength_ReturnsFalse() {
        // Arrange
        var polyline1 = Polyline.FromString("test");
        var polyline2 = Polyline.FromString("testlong");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns true when comparing two empty polylines.
    /// </summary>
    [TestMethod]
    public void Equals_BothEmpty_ReturnsTrue() {
        // Arrange
        var polyline1 = new Polyline();
        var polyline2 = new Polyline();

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing empty polyline with non-empty polyline.
    /// </summary>
    [TestMethod]
    public void Equals_EmptyAndNonEmpty_ReturnsFalse() {
        // Arrange
        var polyline1 = new Polyline();
        var polyline2 = Polyline.FromString("test");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing non-empty polyline with empty polyline.
    /// </summary>
    [TestMethod]
    public void Equals_NonEmptyAndEmpty_ReturnsFalse() {
        // Arrange
        var polyline1 = Polyline.FromString("test");
        var polyline2 = new Polyline();

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that object Equals returns true when comparing identical polylines.
    /// </summary>
    [TestMethod]
    public void EqualsObject_IdenticalPolylines_ReturnsTrue() {
        // Arrange
        var polyline1 = Polyline.FromString("test");
        object polyline2 = Polyline.FromString("test");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that object Equals returns false when comparing different polylines.
    /// </summary>
    [TestMethod]
    public void EqualsObject_DifferentPolylines_ReturnsFalse() {
        // Arrange
        var polyline1 = Polyline.FromString("test1");
        object polyline2 = Polyline.FromString("test2");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that object Equals returns false when comparing with null.
    /// </summary>
    [TestMethod]
    public void EqualsObject_Null_ReturnsFalse() {
        // Arrange
        var polyline = Polyline.FromString("test");

        // Act
#pragma warning disable CA1508 // Avoid dead conditional code
        bool result = polyline.Equals(null);
#pragma warning restore CA1508 // Avoid dead conditional code

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that object Equals returns false when comparing with different type.
    /// </summary>
    [TestMethod]
    public void EqualsObject_DifferentType_ReturnsFalse() {
        // Arrange
        var polyline = Polyline.FromString("test");
        object otherType = "test";

        // Act
        bool result = polyline.Equals(otherType);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that GetHashCode returns zero for empty polyline.
    /// </summary>
    [TestMethod]
    public void GetHashCode_EmptyPolyline_ReturnsZero() {
        // Arrange
        var polyline = new Polyline();

        // Act
        int hashCode = polyline.GetHashCode();

        // Assert
        Assert.AreEqual(0, hashCode);
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for identical polylines.
    /// </summary>
    [TestMethod]
    public void GetHashCode_IdenticalPolylines_ReturnsSameHashCode() {
        // Arrange
        var polyline1 = Polyline.FromString("test");
        var polyline2 = Polyline.FromString("test");

        // Act
        int hashCode1 = polyline1.GetHashCode();
        int hashCode2 = polyline2.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns different values for different polylines.
    /// </summary>
    [TestMethod]
    public void GetHashCode_DifferentPolylines_ReturnsDifferentHashCode() {
        // Arrange
        var polyline1 = Polyline.FromString("test1");
        var polyline2 = Polyline.FromString("test2");

        // Act
        int hashCode1 = polyline1.GetHashCode();
        int hashCode2 = polyline2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns non-zero value for non-empty polyline.
    /// </summary>
    [TestMethod]
    public void GetHashCode_NonEmptyPolyline_ReturnsNonZero() {
        // Arrange
        var polyline = Polyline.FromString("a");

        // Act
        int hashCode = polyline.GetHashCode();

        // Assert
        Assert.AreNotEqual(0, hashCode);
    }

    /// <summary>
    /// Tests that GetHashCode is consistent across multiple calls.
    /// </summary>
    [TestMethod]
    public void GetHashCode_MultipleCalls_ReturnsConsistentValue() {
        // Arrange
        var polyline = Polyline.FromString("test");

        // Act
        int hashCode1 = polyline.GetHashCode();
        int hashCode2 = polyline.GetHashCode();
        int hashCode3 = polyline.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
        Assert.AreEqual(hashCode2, hashCode3);
    }

    /// <summary>
    /// Tests that FromCharArray throws ArgumentNullException when passed null.
    /// </summary>
    [TestMethod]
    public void FromCharArray_Null_ThrowsArgumentNullException() {
        // Arrange
        char[]? nullArray = null;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => Polyline.FromCharArray(nullArray!));
    }

    /// <summary>
    /// Tests that FromCharArray creates polyline from valid char array.
    /// </summary>
    [TestMethod]
    public void FromCharArray_ValidArray_CreatesPolyline() {
        // Arrange
        char[] chars = ['t', 'e', 's', 't'];

        // Act
        var polyline = Polyline.FromCharArray(chars);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual("test", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromCharArray creates empty polyline from empty array.
    /// </summary>
    [TestMethod]
    public void FromCharArray_EmptyArray_CreatesEmptyPolyline() {
        // Arrange
        char[] chars = [];

        // Act
        var polyline = Polyline.FromCharArray(chars);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromCharArray creates polyline with single character.
    /// </summary>
    [TestMethod]
    public void FromCharArray_SingleCharacter_CreatesPolyline() {
        // Arrange
        char[] chars = ['x'];

        // Act
        var polyline = Polyline.FromCharArray(chars);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(1, polyline.Length);
        Assert.AreEqual("x", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromCharArray creates polyline with special characters.
    /// </summary>
    [TestMethod]
    public void FromCharArray_SpecialCharacters_CreatesPolyline() {
        // Arrange
        char[] chars = ['_', 'p', '~', 'i', 'F'];

        // Act
        var polyline = Polyline.FromCharArray(chars);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(5, polyline.Length);
        Assert.AreEqual("_p~iF", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromString throws ArgumentNullException when passed null.
    /// </summary>
    [TestMethod]
    public void FromString_Null_ThrowsArgumentNullException() {
        // Arrange
        string? nullString = null;

        // Act & Assert
        _ = Assert.Throws<ArgumentNullException>(() => Polyline.FromString(nullString!));
    }

    /// <summary>
    /// Tests that FromString creates polyline from valid string.
    /// </summary>
    [TestMethod]
    public void FromString_ValidString_CreatesPolyline() {
        // Arrange
        string str = "test";

        // Act
        var polyline = Polyline.FromString(str);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual("test", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromString creates empty polyline from empty string.
    /// </summary>
    [TestMethod]
    public void FromString_EmptyString_CreatesEmptyPolyline() {
        // Arrange
        string str = string.Empty;

        // Act
        var polyline = Polyline.FromString(str);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromString creates polyline with single character.
    /// </summary>
    [TestMethod]
    public void FromString_SingleCharacter_CreatesPolyline() {
        // Arrange
        string str = "x";

        // Act
        var polyline = Polyline.FromString(str);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(1, polyline.Length);
        Assert.AreEqual("x", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromString creates polyline with special characters.
    /// </summary>
    [TestMethod]
    public void FromString_SpecialCharacters_CreatesPolyline() {
        // Arrange
        string str = "_p~iF~ps|U";

        // Act
        var polyline = Polyline.FromString(str);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(10, polyline.Length);
        Assert.AreEqual("_p~iF~ps|U", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates empty polyline from empty memory.
    /// </summary>
    [TestMethod]
    public void FromMemory_EmptyMemory_CreatesEmptyPolyline() {
        // Arrange
        ReadOnlyMemory<char> memory = ReadOnlyMemory<char>.Empty;

        // Act
        var polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromMemory creates polyline from valid memory.
    /// </summary>
    [TestMethod]
    public void FromMemory_ValidMemory_CreatesPolyline() {
        // Arrange
        char[] chars = ['t', 'e', 's', 't'];
        ReadOnlyMemory<char> memory = new(chars);

        // Act
        var polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual("test", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates polyline with single character.
    /// </summary>
    [TestMethod]
    public void FromMemory_SingleCharacter_CreatesPolyline() {
        // Arrange
        char[] chars = ['x'];
        ReadOnlyMemory<char> memory = new(chars);

        // Act
        var polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(1, polyline.Length);
        Assert.AreEqual("x", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates polyline with special characters.
    /// </summary>
    [TestMethod]
    public void FromMemory_SpecialCharacters_CreatesPolyline() {
        // Arrange
        char[] chars = ['_', 'p', '~', 'i', 'F', '~', 'p', 's', '|', 'U'];
        ReadOnlyMemory<char> memory = new(chars);

        // Act
        var polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(10, polyline.Length);
        Assert.AreEqual("_p~iF~ps|U", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates polyline from memory slice.
    /// </summary>
    [TestMethod]
    public void FromMemory_MemorySlice_CreatesPolyline() {
        // Arrange
        char[] chars = ['a', 'b', 'c', 'd', 'e', 'f'];
        ReadOnlyMemory<char> memory = new ReadOnlyMemory<char>(chars).Slice(1, 3);

        // Act
        var polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsFalse(polyline.IsEmpty);
        Assert.AreEqual(3, polyline.Length);
        Assert.AreEqual("bcd", polyline.ToString());
    }


    [TestMethod]
    public void EqualityOperator_IdenticalPolylines_ReturnsTrue() {
        var polyline1 = Polyline.FromString("abcdef");
        var polyline2 = Polyline.FromString("abcdef");

        Assert.IsTrue(polyline1 == polyline2);
        Assert.IsFalse(polyline1 != polyline2);
    }

    [TestMethod]
    public void EqualityOperator_DifferentPolylines_ReturnsFalse() {
        var polyline1 = Polyline.FromString("abcdef");
        var polyline2 = Polyline.FromString("ghijkl");

        Assert.IsFalse(polyline1 == polyline2);
        Assert.IsTrue(polyline1 != polyline2);
    }

    [TestMethod]
    public void EqualityOperator_EmptyPolylines_ReturnsTrue() {
        var polyline1 = new Polyline();
        var polyline2 = new Polyline();

        Assert.IsTrue(polyline1 == polyline2);
        Assert.IsFalse(polyline1 != polyline2);
    }

    [TestMethod]
    public void EqualityOperator_EmptyAndNonEmptyPolylines_ReturnsFalse() {
        var polyline1 = new Polyline();
        var polyline2 = Polyline.FromString("abcdef");

        Assert.IsFalse(polyline1 == polyline2);
        Assert.IsTrue(polyline1 != polyline2);
    }
}
