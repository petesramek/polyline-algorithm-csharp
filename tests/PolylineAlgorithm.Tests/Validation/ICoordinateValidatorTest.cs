//  
// Copyright (c) Pete Sramek. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.  
//

namespace PolylineAlgorithm.Tests.Validation;

using PolylineAlgorithm.Validation;

/// <summary>
/// Tests <see cref="Polyline"/> type.
/// </summary>
[TestClass]
public class ICoordinateValidatorTest {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private static ICoordinateValidator _original;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [ClassInitialize]
    public static void ClassInitialize(TestContext context) {
        _original = ICoordinateValidator.Global;
    }

    [TestMethod]
    public void SetDefault_Validator_Ok() {
        // Arrange
        CoordinateRange range = new(double.MinValue, double.MaxValue);
        CoordinateValidator validator = new(range, range);

        // Act
        ICoordinateValidator
            .SetGlobal(validator);

        ICoordinateValidator @new = ICoordinateValidator.Global;

        // Assert
        Assert.AreEqual(validator, @new);
        Assert.AreNotEqual(_original, @new);
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void ClassCleanup() {
        ICoordinateValidator
            .SetGlobal(_original);
    }
}
