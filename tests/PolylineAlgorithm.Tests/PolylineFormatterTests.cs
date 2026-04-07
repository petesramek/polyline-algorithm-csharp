//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests;

using PolylineAlgorithm;
using System;

/// <summary>
/// Tests for <see cref="FormatterBuilder{T}"/>, <see cref="PolylineValueFormatter{T}"/>,
/// and <see cref="PolylineOptions{TValue,TPolyline}"/>.
/// </summary>
[TestClass]
public sealed class PolylineFormatterTests {
    // ---------------------------------------------------------------------------
    // FormatterBuilder<T>.Create
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void Create_Returns_New_Builder() {
        // Act
        FormatterBuilder<(double X, double Y)> result = FormatterBuilder<(double X, double Y)>.Create();

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Create_With_Multiple_Invocations_Returns_Different_Instances() {
        // Act
        FormatterBuilder<(double X, double Y)> first = FormatterBuilder<(double X, double Y)>.Create();
        FormatterBuilder<(double X, double Y)> second = FormatterBuilder<(double X, double Y)>.Create();

        // Assert
        Assert.AreNotSame(first, second);
    }

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T>.AddValue — argument validation
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void AddValue_With_Null_Name_Throws_ArgumentNullException() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => builder.AddValue(null!, static t => t.X));
        Assert.AreEqual("name", ex.ParamName);
    }

    [TestMethod]
    public void AddValue_With_Empty_Name_Throws_ArgumentException() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();

        // Act & Assert
        ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(
            () => builder.AddValue(string.Empty, static t => t.X));
        Assert.AreEqual("name", ex.ParamName);
    }

    [TestMethod]
    public void AddValue_With_Null_Selector_Throws_ArgumentNullException() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => builder.AddValue("X", null!));
        Assert.AreEqual("selector", ex.ParamName);
    }

    [TestMethod]
    public void AddValue_With_Duplicate_Name_Throws_ArgumentException() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();
        builder.AddValue("X", static t => t.X);

        // Act & Assert
        ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(
            () => builder.AddValue("X", static t => t.Y));
        Assert.AreEqual("name", ex.ParamName);
    }

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T>.AddValue — happy path & chaining
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void AddValue_Returns_Same_Builder_For_Method_Chaining() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();

        // Act
        FormatterBuilder<(double X, double Y)> result = builder.AddValue("X", static t => t.X);

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void AddValue_With_Different_Names_Succeeds() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X)
            .AddValue("Y", static t => t.Y)
            .Build();

        // Assert
        Assert.AreEqual(2, formatter.Width);
    }

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T>.SetBaseline — argument validation
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void SetBaseline_With_No_Rules_Throws_InvalidOperationException() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(() => builder.SetBaseline(1000L));
    }

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T>.SetBaseline — happy path
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void SetBaseline_Returns_Same_Builder_For_Method_Chaining() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X);

        // Act
        FormatterBuilder<(double X, double Y)> result = builder.SetBaseline(100L);

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void SetBaseline_Applies_Only_To_Last_Added_Rule() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X)
            .AddValue("Y", static t => t.Y)
            .SetBaseline(500L)
            .Build();

        // Assert — only Y (index 1) has a baseline; X (index 0) returns 0
        Assert.AreEqual(0L, formatter.GetBaseline(0));
        Assert.AreEqual(500L, formatter.GetBaseline(1));
    }

    [TestMethod]
    public void SetBaseline_Can_Be_Called_On_Each_Rule() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X).SetBaseline(100L)
            .AddValue("Y", static t => t.Y).SetBaseline(200L)
            .Build();

        // Assert
        Assert.AreEqual(100L, formatter.GetBaseline(0));
        Assert.AreEqual(200L, formatter.GetBaseline(1));
    }

    // ---------------------------------------------------------------------------
    // FormatterBuilder<T>.Build — validation
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void Build_With_No_Rules_Throws_InvalidOperationException() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create();

        // Act & Assert
        Assert.ThrowsExactly<InvalidOperationException>(() => builder.Build());
    }

    [TestMethod]
    public void Build_With_Multiple_Invocations_Returns_Different_Instances() {
        // Arrange
        FormatterBuilder<(double X, double Y)> builder = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X);

        // Act
        PolylineValueFormatter<(double X, double Y)> first = builder.Build();
        PolylineValueFormatter<(double X, double Y)> second = builder.Build();

        // Assert
        Assert.AreNotSame(first, second);
    }

    // ---------------------------------------------------------------------------
    // PolylineValueFormatter<T>.Width
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void Width_Equals_Number_Of_Added_Rules() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y, double Z)> formatter = FormatterBuilder<(double X, double Y, double Z)>.Create()
            .AddValue("X", static t => t.X)
            .AddValue("Y", static t => t.Y)
            .AddValue("Z", static t => t.Z)
            .Build();

        // Assert
        Assert.AreEqual(3, formatter.Width);
    }

    [TestMethod]
    public void Width_Is_One_For_Single_Rule() {
        // Arrange & Act
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        // Assert
        Assert.AreEqual(1, formatter.Width);
    }

    // ---------------------------------------------------------------------------
    // PolylineValueFormatter<T>.HasBaselines
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void HasBaselines_Is_False_When_No_Baselines_Are_Set() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X)
            .AddValue("Y", static t => t.Y)
            .Build();

        // Assert
        Assert.IsFalse(formatter.HasBaselines);
    }

    [TestMethod]
    public void HasBaselines_Is_True_When_Any_Baseline_Is_Set() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X)
            .AddValue("Y", static t => t.Y).SetBaseline(100L)
            .Build();

        // Assert
        Assert.IsTrue(formatter.HasBaselines);
    }

    [TestMethod]
    public void HasBaselines_Is_True_When_All_Baselines_Are_Set() {
        // Arrange & Act
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X).SetBaseline(10L)
            .AddValue("Y", static t => t.Y).SetBaseline(20L)
            .Build();

        // Assert
        Assert.IsTrue(formatter.HasBaselines);
    }

    // ---------------------------------------------------------------------------
    // PolylineValueFormatter<T>.GetBaseline
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void GetBaseline_Returns_Zero_When_No_Baseline_Configured() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        // Act
        long result = formatter.GetBaseline(0);

        // Assert
        Assert.AreEqual(0L, result);
    }

    [TestMethod]
    public void GetBaseline_Returns_Configured_Baseline() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .SetBaseline(42L)
            .Build();

        // Act
        long result = formatter.GetBaseline(0);

        // Assert
        Assert.AreEqual(42L, result);
    }

    [TestMethod]
    public void GetBaseline_Returns_Negative_Baseline() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .SetBaseline(-1000L)
            .Build();

        // Act
        long result = formatter.GetBaseline(0);

        // Assert
        Assert.AreEqual(-1000L, result);
    }

    // ---------------------------------------------------------------------------
    // PolylineValueFormatter<T>.GetValues
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void GetValues_Scales_Single_Column_By_Factor() {
        // Arrange — precision 5 → factor = 100000; use a value exact in double arithmetic
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v, precision: 5)
            .Build();

        Span<long> output = stackalloc long[1];

        // Act — 38.5 * 100000 = 3850000 (exactly representable)
        formatter.GetValues(38.5, output);

        // Assert
        Assert.AreEqual(3850000L, output[0]);
    }

    [TestMethod]
    public void GetValues_Scales_Multiple_Columns_Independently() {
        // Arrange
        PolylineValueFormatter<(double Lat, double Lon)> formatter =
            FormatterBuilder<(double Lat, double Lon)>.Create()
                .AddValue("Lat", static t => t.Lat, precision: 5)
                .AddValue("Lon", static t => t.Lon, precision: 5)
                .Build();

        Span<long> output = stackalloc long[2];

        // Act — 38.5 * 100000 = 3850000;  -120.25 * 100000 = -12025000 (both exact in double)
        formatter.GetValues((38.5, -120.25), output);

        // Assert
        Assert.AreEqual(3850000L, output[0]);
        Assert.AreEqual(-12025000L, output[1]);
    }

    [TestMethod]
    public void GetValues_With_Wrong_Buffer_Length_Throws_ArgumentException() {
        // Arrange — formatter has Width = 2 but buffer has length 1
        PolylineValueFormatter<(double X, double Y)> formatter = FormatterBuilder<(double X, double Y)>.Create()
            .AddValue("X", static t => t.X)
            .AddValue("Y", static t => t.Y)
            .Build();

        long[] tooShort = new long[1];

        // Act & Assert
        ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(
            () => formatter.GetValues((1.0, 2.0), tooShort.AsSpan()));
        Assert.AreEqual("values", ex.ParamName);
    }

    [TestMethod]
    public void GetValues_With_Oversized_Buffer_Throws_ArgumentException() {
        // Arrange — formatter has Width = 1 but buffer has length 3
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        long[] tooLong = new long[3];

        // Act & Assert
        ArgumentException ex = Assert.ThrowsExactly<ArgumentException>(
            () => formatter.GetValues(1.0, tooLong.AsSpan()));
        Assert.AreEqual("values", ex.ParamName);
    }

    [TestMethod]
    public void GetValues_With_Zero_Returns_Zero() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v, precision: 5)
            .Build();

        Span<long> output = stackalloc long[1];

        // Act
        formatter.GetValues(0.0, output);

        // Assert
        Assert.AreEqual(0L, output[0]);
    }

    [TestMethod]
    public void GetValues_With_Negative_Value_Returns_Negative_Scaled_Long() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v, precision: 5)
            .Build();

        Span<long> output = stackalloc long[1];

        // Act
        formatter.GetValues(-90.0, output);

        // Assert
        Assert.AreEqual(-9000000L, output[0]);
    }

    [TestMethod]
    public void GetValues_With_Custom_Precision_Scales_Correctly() {
        // Arrange — precision 3 → factor = 1000
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v, precision: 3)
            .Build();

        Span<long> output = stackalloc long[1];

        // Act
        formatter.GetValues(1.5, output);

        // Assert — 1.5 * 1000 = 1500
        Assert.AreEqual(1500L, output[0]);
    }

    // ---------------------------------------------------------------------------
    // PolylineOptions<TValue,TPolyline> constructor validation
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void PolylineOptions_With_Null_ValueFormatter_Throws_ArgumentNullException() {
        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => _ = new PolylineOptions<double, string>(null!, PolylineFormatter.ForString));
        Assert.AreEqual("valueFormatter", ex.ParamName);
    }

    [TestMethod]
    public void PolylineOptions_With_Null_PolylineFormatter_Throws_ArgumentNullException() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        // Act & Assert
        ArgumentNullException ex = Assert.ThrowsExactly<ArgumentNullException>(
            () => _ = new PolylineOptions<double, string>(formatter, null!));
        Assert.AreEqual("polylineFormatter", ex.ParamName);
    }

    // ---------------------------------------------------------------------------
    // PolylineOptions<TValue,TPolyline> properties
    // ---------------------------------------------------------------------------

    [TestMethod]
    public void PolylineOptions_Stores_ValueFormatter() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        // Act
        PolylineOptions<double, string> options = new(formatter, PolylineFormatter.ForString);

        // Assert
        Assert.AreSame(formatter, options.ValueFormatter);
    }

    [TestMethod]
    public void PolylineOptions_Stores_PolylineFormatter() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        // Act
        PolylineOptions<double, string> options = new(formatter, PolylineFormatter.ForString);

        // Assert
        Assert.AreSame(PolylineFormatter.ForString, options.PolylineFormatter);
    }

    [TestMethod]
    public void PolylineOptions_With_Null_Encoding_Uses_Default_Options() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();

        // Act
        PolylineOptions<double, string> options = new(formatter, PolylineFormatter.ForString, null);

        // Assert
        Assert.IsNotNull(options.Encoding);
        Assert.AreEqual(5u, options.Encoding.Precision);
        Assert.AreEqual(512, options.Encoding.StackAllocLimit);
    }

    [TestMethod]
    public void PolylineOptions_Stores_Custom_Encoding_Options() {
        // Arrange
        PolylineValueFormatter<double> formatter = FormatterBuilder<double>.Create()
            .AddValue("Value", static v => v)
            .Build();
        PolylineEncodingOptions encoding = PolylineEncodingOptionsBuilder.Create()
            .WithPrecision(7)
            .WithStackAllocLimit(1024)
            .Build();

        // Act
        PolylineOptions<double, string> options = new(formatter, PolylineFormatter.ForString, encoding);

        // Assert
        Assert.AreSame(encoding, options.Encoding);
        Assert.AreEqual(7u, options.Encoding.Precision);
        Assert.AreEqual(1024, options.Encoding.StackAllocLimit);
    }
}
