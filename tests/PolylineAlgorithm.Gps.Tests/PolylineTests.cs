//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Gps.Tests;

using PolylineAlgorithm.Gps;
using PolylineAlgorithm.Gps.Tests.Properties;
using System;

/// <summary>
/// Tests for <see cref="Polyline"/>.
/// </summary>
[TestClass]
public sealed class PolylineTests {
    /// <summary>
    /// Tests that default constructor creates an empty polyline.
    /// </summary>
    [TestMethod]

    public void Polyline_DefaultConstructor_CreatesEmptyPolyline() {
        // Arrange & Act
        Polyline polyline = new Polyline();

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that IsEmpty returns true for default constructed polyline.
    /// </summary>
    [TestMethod]

    public void IsEmpty_DefaultConstructedPolyline_ReturnsTrue() {
        // Arrange
        Polyline polyline = new Polyline();

        // Act
        bool isEmpty = polyline.IsEmpty;

        // Assert
        Assert.IsTrue(isEmpty);
    }

    /// <summary>
    /// Tests that IsEmpty returns false for non-empty polyline.
    /// </summary>
    [TestMethod]

    public void IsEmpty_NonEmptyPolyline_ReturnsFalse() {
        // Arrange
        Polyline polyline = Polyline.FromString("test");

        // Act
        bool isEmpty = polyline.IsEmpty;

        // Assert
        Assert.IsFalse(isEmpty);
    }

    /// <summary>
    /// Tests that Length returns zero for default constructed polyline.
    /// </summary>
    [TestMethod]

    public void Length_DefaultConstructedPolyline_ReturnsZero() {
        // Arrange
        Polyline polyline = new Polyline();

        // Act
        int length = polyline.Length;

        // Assert
        Assert.AreEqual(0, length);
    }

    /// <summary>
    /// Tests that Length returns correct value for non-empty polyline.
    /// </summary>
    [TestMethod]

    public void Length_NonEmptyPolyline_ReturnsCorrectLength() {
        // Arrange
        const string testString = "test";
        Polyline polyline = Polyline.FromString(testString);

        // Act
        int length = polyline.Length;

        // Assert
        Assert.AreEqual(testString.Length, length);
    }

    /// <summary>
    /// Tests that CopyTo throws ArgumentNullException when destination is null.
    /// </summary>
    [TestMethod]

    public void CopyTo_NullDestination_ThrowsArgumentNullException() {
        // Arrange
        Polyline polyline = Polyline.FromString("test");

        // Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => polyline.CopyTo(null!));
        Assert.AreEqual("destination", exception.ParamName);
    }

    /// <summary>
    /// Tests that CopyTo throws ArgumentException when destination is too small.
    /// </summary>
    [TestMethod]

    public void CopyTo_DestinationTooSmall_ThrowsArgumentException() {
        // Arrange
        Polyline polyline = Polyline.FromString("test");
        char[] destination = new char[2];

        // Act & Assert
        ArgumentException exception = Assert.ThrowsExactly<ArgumentException>(() => polyline.CopyTo(destination));
        Assert.AreEqual("destination", exception.ParamName);
    }

    /// <summary>
    /// Tests that CopyTo copies characters to destination array of exact length.
    /// </summary>
    [TestMethod]

    public void CopyTo_ExactLengthDestination_CopiesCharacters() {
        // Arrange
        const string testString = "test";
        Polyline polyline = Polyline.FromString(testString);
        char[] destination = new char[testString.Length];

        // Act
        polyline.CopyTo(destination);

        // Assert
        Assert.AreEqual(testString, new string(destination));
    }

    /// <summary>
    /// Tests that CopyTo copies characters to destination array larger than polyline length.
    /// </summary>
    [TestMethod]

    public void CopyTo_LargerDestination_CopiesCharacters() {
        // Arrange
        const string testString = "test";
        Polyline polyline = Polyline.FromString(testString);
        char[] destination = new char[testString.Length + 5];

        // Act
        polyline.CopyTo(destination);

        // Assert
        for (int i = 0; i < testString.Length; i++) {
            Assert.AreEqual(testString[i], destination[i]);
        }
    }

    /// <summary>
    /// Tests that CopyTo handles empty polyline.
    /// </summary>
    [TestMethod]

    public void CopyTo_EmptyPolyline_SucceedsWithEmptyOrLargerArray() {
        // Arrange
        Polyline polyline = new Polyline();
        char[] destination = Array.Empty<char>();

        // Act
        polyline.CopyTo(destination);

        // Assert
        Assert.AreEqual(0, destination.Length);
    }

    /// <summary>
    /// Tests that ToString returns empty string for empty polyline.
    /// </summary>
    [TestMethod]

    public void ToString_EmptyPolyline_ReturnsEmptyString() {
        // Arrange
        Polyline polyline = new Polyline();

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    /// <summary>
    /// Tests that ToString returns correct string for non-empty polyline.
    /// </summary>
    [TestMethod]

    public void ToString_NonEmptyPolyline_ReturnsCorrectString() {
        // Arrange
        const string testString = "test";
        Polyline polyline = Polyline.FromString(testString);

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(testString, result);
    }

    /// <summary>
    /// Tests that ToString returns correct string for polyline with special characters.
    /// </summary>
    [TestMethod]

    public void ToString_PolylineWithSpecialCharacters_ReturnsCorrectString() {
        // Arrange
        const string testString = "~`@!#$%";
        Polyline polyline = Polyline.FromString(testString);

        // Act
        string result = polyline.ToString();

        // Assert
        Assert.AreEqual(testString, result);
    }

    /// <summary>
    /// Tests that Equals returns true when comparing two identical polylines.
    /// </summary>
    [TestMethod]

    public void Equals_IdenticalPolylines_ReturnsTrue() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test");
        Polyline polyline2 = Polyline.FromString("test");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing two different polylines.
    /// </summary>
    [TestMethod]

    public void Equals_DifferentPolylines_ReturnsFalse() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test1");
        Polyline polyline2 = Polyline.FromString("test2");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns true when comparing two empty polylines.
    /// </summary>
    [TestMethod]

    public void Equals_TwoEmptyPolylines_ReturnsTrue() {
        // Arrange
        Polyline polyline1 = new Polyline();
        Polyline polyline2 = new Polyline();

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing empty and non-empty polylines.
    /// </summary>
    [TestMethod]

    public void Equals_EmptyAndNonEmptyPolylines_ReturnsFalse() {
        // Arrange
        Polyline polyline1 = new Polyline();
        Polyline polyline2 = Polyline.FromString("test");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing polylines with different lengths.
    /// </summary>
    [TestMethod]

    public void Equals_PolylinesWithDifferentLengths_ReturnsFalse() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test");
        Polyline polyline2 = Polyline.FromString("testing");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals returns false when comparing polylines with same length but different content.
    /// </summary>
    [TestMethod]

    public void Equals_SameLengthDifferentContent_ReturnsFalse() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("abcd");
        Polyline polyline2 = Polyline.FromString("efgh");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals object overload returns true when comparing identical polylines.
    /// </summary>
    [TestMethod]

    public void EqualsObject_IdenticalPolylines_ReturnsTrue() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test");
        object polyline2 = Polyline.FromString("test");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsTrue(result);
    }

    /// <summary>
    /// Tests that Equals object overload returns false when comparing different polylines.
    /// </summary>
    [TestMethod]

    public void EqualsObject_DifferentPolylines_ReturnsFalse() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test1");
        object polyline2 = Polyline.FromString("test2");

        // Act
        bool result = polyline1.Equals(polyline2);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals object overload returns false when comparing with null.
    /// </summary>
    [TestMethod]

    public void EqualsObject_NullObject_ReturnsFalse() {
        // Arrange
        Polyline polyline = Polyline.FromString("test");

        // Act
        bool result = polyline.Equals(null);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that Equals object overload returns false when comparing with a different type.
    /// </summary>
    [TestMethod]

    public void EqualsObject_DifferentType_ReturnsFalse() {
        // Arrange
        Polyline polyline = Polyline.FromString("test");
        object other = "test";

        // Act
        bool result = polyline.Equals(other);

        // Assert
        Assert.IsFalse(result);
    }

    /// <summary>
    /// Tests that GetHashCode returns zero for empty polyline.
    /// </summary>
    [TestMethod]

    public void GetHashCode_EmptyPolyline_ReturnsZero() {
        // Arrange
        Polyline polyline = new Polyline();

        // Act
        int hashCode = polyline.GetHashCode();

        // Assert
        Assert.AreEqual(0, hashCode);
    }

    /// <summary>
    /// Tests that GetHashCode returns consistent value for same polyline.
    /// </summary>
    [TestMethod]

    public void GetHashCode_SamePolyline_ReturnsConsistentValue() {
        // Arrange
        Polyline polyline = Polyline.FromString("test");

        // Act
        int hashCode1 = polyline.GetHashCode();
        int hashCode2 = polyline.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode returns same value for equal polylines.
    /// </summary>
    [TestMethod]

    public void GetHashCode_EqualPolylines_ReturnsSameHashCode() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test");
        Polyline polyline2 = Polyline.FromString("test");

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

    public void GetHashCode_DifferentPolylines_ReturnsDifferentHashCodes() {
        // Arrange
        Polyline polyline1 = Polyline.FromString("test1");
        Polyline polyline2 = Polyline.FromString("test2");

        // Act
        int hashCode1 = polyline1.GetHashCode();
        int hashCode2 = polyline2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    /// <summary>
    /// Tests that GetHashCode handles single character polyline.
    /// </summary>
    [TestMethod]

    public void GetHashCode_SingleCharacterPolyline_ReturnsNonZeroHashCode() {
        // Arrange
        Polyline polyline = Polyline.FromString("a");

        // Act
        int hashCode = polyline.GetHashCode();

        // Assert
        Assert.AreNotEqual(0, hashCode);
    }

    /// <summary>
    /// Tests that FromCharArray creates a polyline from a valid character array.
    /// </summary>
    [TestMethod]

    public void FromCharArray_ValidCharArray_CreatesPolyline() {
        // Arrange
        char[] chars = { 't', 'e', 's', 't' };

        // Act
        Polyline polyline = Polyline.FromCharArray(chars);

        // Assert
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual("test", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromCharArray creates an empty polyline from an empty character array.
    /// </summary>
    [TestMethod]

    public void FromCharArray_EmptyCharArray_CreatesEmptyPolyline() {
        // Arrange
        char[] chars = Array.Empty<char>();

        // Act
        Polyline polyline = Polyline.FromCharArray(chars);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromCharArray throws ArgumentNullException when character array is null.
    /// </summary>
    [TestMethod]

    public void FromCharArray_NullCharArray_ThrowsArgumentNullException() {
        // Arrange & Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => Polyline.FromCharArray(null!));
        Assert.AreEqual("polyline", exception.ParamName);
    }

    /// <summary>
    /// Tests that FromString creates a polyline from a valid string.
    /// </summary>
    [TestMethod]

    public void FromString_ValidString_CreatesPolyline() {
        // Arrange
        const string testString = "test";

        // Act
        Polyline polyline = Polyline.FromString(testString);

        // Assert
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual(testString, polyline.ToString());
    }

    /// <summary>
    /// Tests that FromString creates an empty polyline from an empty string.
    /// </summary>
    [TestMethod]

    public void FromString_EmptyString_CreatesEmptyPolyline() {
        // Arrange
        string testString = string.Empty;

        // Act
        Polyline polyline = Polyline.FromString(testString);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromString throws ArgumentNullException when string is null.
    /// </summary>
    [TestMethod]

    public void FromString_NullString_ThrowsArgumentNullException() {
        // Arrange & Act & Assert
        ArgumentNullException exception = Assert.ThrowsExactly<ArgumentNullException>(() => Polyline.FromString(null!));
        Assert.AreEqual("polyline", exception.ParamName);
    }

    /// <summary>
    /// Tests that FromString handles special characters correctly.
    /// </summary>
    [TestMethod]

    public void FromString_SpecialCharacters_CreatesPolyline() {
        // Arrange
        const string testString = "~`@!#$%";

        // Act
        Polyline polyline = Polyline.FromString(testString);

        // Assert
        Assert.AreEqual(testString.Length, polyline.Length);
        Assert.AreEqual(testString, polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates a polyline from a valid memory region.
    /// </summary>
    [TestMethod]

    public void FromMemory_ValidMemory_CreatesPolyline() {
        // Arrange
        char[] chars = { 't', 'e', 's', 't' };
        ReadOnlyMemory<char> memory = new ReadOnlyMemory<char>(chars);

        // Act
        Polyline polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual("test", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates an empty polyline from an empty memory region.
    /// </summary>
    [TestMethod]

    public void FromMemory_EmptyMemory_CreatesEmptyPolyline() {
        // Arrange
        ReadOnlyMemory<char> memory = ReadOnlyMemory<char>.Empty;

        // Act
        Polyline polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromMemory creates an empty polyline from a default memory region.
    /// </summary>
    [TestMethod]

    public void FromMemory_DefaultMemory_CreatesEmptyPolyline() {
        // Arrange
        ReadOnlyMemory<char> memory = default;

        // Act
        Polyline polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.IsTrue(polyline.IsEmpty);
        Assert.AreEqual(0, polyline.Length);
    }

    /// <summary>
    /// Tests that FromMemory handles special characters correctly.
    /// </summary>
    [TestMethod]

    public void FromMemory_SpecialCharacters_CreatesPolyline() {
        // Arrange
        const string testString = "~`@!#$%";
        ReadOnlyMemory<char> memory = testString.AsMemory();

        // Act
        Polyline polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.AreEqual(testString.Length, polyline.Length);
        Assert.AreEqual(testString, polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates a polyline from a single character memory.
    /// </summary>
    [TestMethod]

    public void FromMemory_SingleCharacter_CreatesPolyline() {
        // Arrange
        char[] chars = { 'a' };
        ReadOnlyMemory<char> memory = new ReadOnlyMemory<char>(chars);

        // Act
        Polyline polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.AreEqual(1, polyline.Length);
        Assert.AreEqual("a", polyline.ToString());
    }

    /// <summary>
    /// Tests that FromMemory creates a polyline from a memory slice.
    /// </summary>
    [TestMethod]

    public void FromMemory_MemorySlice_CreatesPolyline() {
        // Arrange
        const string testString = "testing123";
        ReadOnlyMemory<char> memory = testString.AsMemory(0, 4);

        // Act
        Polyline polyline = Polyline.FromMemory(memory);

        // Assert
        Assert.AreEqual(4, polyline.Length);
        Assert.AreEqual("test", polyline.ToString());
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void EqualityOperator_WithIdenticalPolylines_ReturnsTrue() {
        // Arrange
        var polyline1 = Polyline.FromString("abcde");
        var polyline2 = Polyline.FromString("abcde");

        // Act
        bool result = polyline1 == polyline2;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void EqualityOperator_WithDifferentPolylines_ReturnsFalse() {
        // Arrange
        var polyline1 = Polyline.FromString("abcde");
        var polyline2 = Polyline.FromString("xyz");

        // Act
        bool result = polyline1 == polyline2;

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void InequalityOperator_WithIdenticalPolylines_ReturnsFalse() {
        // Arrange
        var polyline1 = Polyline.FromString("abcde");
        var polyline2 = Polyline.FromString("abcde");

        // Act
        bool result = polyline1 != polyline2;

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void InequalityOperator_WithDifferentPolylines_ReturnsTrue() {
        // Arrange
        var polyline1 = Polyline.FromString("abcde");
        var polyline2 = Polyline.FromString("xyz");

        // Act
        bool result = polyline1 != polyline2;

        // Assert
        Assert.IsTrue(result);
    }
}
